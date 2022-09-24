using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReplicSystem : MonoBehaviour, IGameSystem
{
    [SerializeField] private AudioSource voiceAudioSource;
    [SerializeField] private SubsPanel subsPanel;
    [SerializeField] private KeyCode skipButton = KeyCode.Q;
    [SerializeField] private KeyCode skipAllButton = KeyCode.P;

    private List<ReplicaPack> replicaPacks;
    private bool useMainReplicPack;

    private void Awake()
    {
        PlayerPack.ReplicSystem = this;
        useMainReplicPack = false;
        replicaPacks = new List<ReplicaPack>();
        subsPanel.ClosePanel();
    }

    public void SetUp()
    {
        GameCenter.PauseValueChanged.AddListener(OnChangePause);
        Settings.VoiceVolumeChanged.AddListener(OnChangeVolume);
        Settings.UseSubsChanged.AddListener(OnDrawSubsChanged);
    }

    private void Update()
    {
        if(Input.GetKeyDown(skipButton))
        {
            Skip();
        }
        else if (Input.GetKeyDown(skipAllButton))
        {
            SkipAll();
        }
    }

    public void AddNewReplicaPack(ReplicaPack pack)
    {
        replicaPacks.Add(pack);
        if(replicaPacks.Count == 1)
        {
            StartCoroutine(TallMainReplicasCoroutine());
        }
    }

    public void OnDrawSubsChanged(bool value)
    {
        if(value)
        {
            if(replicaPacks.Count > 0)
            {
                ReplicaItem item = replicaPacks[0].skipList[0];
                if (useMainReplicPack)
                {
                    subsPanel.SetSkipTip(skipButton);
                    item = replicaPacks[0].mainList[0];
                }
                else
                {
                    subsPanel.ClosePanel();
                }
                subsPanel.SetSubs(item.CharacterName, item.characterText, item.characterColor);
            }
        }
        else
        {
            subsPanel.ClosePanel();
        }
    }

    public void OnChangePause(bool value)
    {
        if(value)
        {
            voiceAudioSource.Pause();
        }
        else
        {
            voiceAudioSource.UnPause();
        }
    }

    private void OnChangeVolume(float newVolume)
    {
        voiceAudioSource.volume = newVolume;
    }

    private void Skip()
    {
        if(useMainReplicPack)
        {
            StopAllCoroutines();

            if (voiceAudioSource.isPlaying)
                voiceAudioSource.Stop();
            subsPanel.ClosePanel();
            subsPanel.HideSkipTip();

            if (replicaPacks.Count > 0)
            {
                StartCoroutine(TallSkipReplicasCoroutine());
            }
        }
    }

    private void SkipAll()
    {
        if(replicaPacks.Count <= 0)
        {
            return;
        }

        StopAllCoroutines();

        if (voiceAudioSource.isPlaying)
            voiceAudioSource.Stop();
        subsPanel.ClosePanel();
        subsPanel.HideSkipTip();

        if (useMainReplicPack)
        {
            foreach (var item in replicaPacks[0].mainList)
            {
                if (!item.onReplicaStart.isDecorEvent)
                {
                    item.onReplicaStart.action.Invoke();
                }
                if (!item.onReplicaEnd.isDecorEvent)
                {
                    item.onReplicaEnd.action.Invoke();
                }
            }
        }
        else
        {
            foreach (var item in replicaPacks[0].skipList)
            {
                if (!item.onReplicaStart.isDecorEvent)
                {
                    item.onReplicaStart.action.Invoke();
                }
                if (!item.onReplicaEnd.isDecorEvent)
                {
                    item.onReplicaEnd.action.Invoke();
                }
            }
        }

        replicaPacks[0].onSkipAll.Invoke();

        replicaPacks.RemoveAt(0);

        if (replicaPacks.Count > 0)
        {
            StartCoroutine(TallMainReplicasCoroutine());
        }
    }

    private IEnumerator TallMainReplicasCoroutine()
    {
        subsPanel.SetSkipTip(skipButton);
        useMainReplicPack = true;
        while (replicaPacks[0].mainList.Count > 0)
        {
            ReplicaItem item = replicaPacks[0].mainList[0];
            voiceAudioSource.PlayOneShot(item.characterVoice);

            if(Settings.UseSubs)
            {
                subsPanel.SetSubs(item.CharacterName, item.characterText, item.characterColor);
            }
            item.onReplicaStart.action.Invoke();

            yield return new WaitForSeconds(item.characterVoice.length);
            replicaPacks[0].mainList.RemoveAt(0);
            item.onReplicaEnd.action.Invoke();
            yield return new WaitForSeconds(0.3f);
        }

        replicaPacks.RemoveAt(0);

        yield return null;

        if(replicaPacks.Count > 0)
        {
            StartCoroutine(TallMainReplicasCoroutine());
        }
        else
        {
            useMainReplicPack = false;
            subsPanel.ClosePanel();
            subsPanel.HideSkipTip();
        }
    }

    private IEnumerator TallSkipReplicasCoroutine()
    {
        subsPanel.HideSkipTip();
        useMainReplicPack = false;

        yield return new WaitForSeconds(2);

        while (replicaPacks[0].skipList.Count > 0)
        {
            ReplicaItem item = replicaPacks[0].skipList[0];
            voiceAudioSource.PlayOneShot(item.characterVoice);

            if (Settings.UseSubs)
            {
                subsPanel.SetSubs(item.CharacterName, item.characterText, item.characterColor);
            }
            item.onReplicaStart.action.Invoke();
            yield return new WaitForSeconds(item.characterVoice.length);
            replicaPacks[0].skipList.RemoveAt(0);
            item.onReplicaEnd.action.Invoke();
            yield return new WaitForSeconds(0.3f);
        }

        replicaPacks.RemoveAt(0);

        yield return null;

        if (replicaPacks.Count > 0)
        {
            StartCoroutine(TallMainReplicasCoroutine());
        }
        else
        {
            subsPanel.ClosePanel();
            subsPanel.HideSkipTip();
        }
    }
}

[System.Serializable]
public class ReplicaPack
{
    public List<ReplicaItem> mainList;
    public List<ReplicaItem> skipList;
    public UnityEvent onSkipAll;
}

[System.Serializable]
public class ReplicaItem
{
    public string CharacterName;
    public Color characterColor = Color.white;
    [TextArea]
    public string characterText;
    public AudioClip characterVoice;
    public UnityEventPack onReplicaStart;
    public UnityEventPack onReplicaEnd;
}

[System.Serializable]
public class UnityEventPack
{
    public bool isDecorEvent = false;
    public UnityEvent action;
}

[System.Serializable]
public class SubsPanel
{
    public GameObject panelObject;
    public GameObject skipTipObject;
    public Text characterName;
    public Text characterText;
    public Text skipTip;

    public void SetSubs(string name, string text, Color color)
    {
        panelObject.SetActive(true);
        characterName.color = color;
        characterName.text = name;
        characterText.text = text;
    }

    public void ClosePanel()
    {
        characterName.text = characterText.text = string.Empty;
        panelObject.SetActive(false);
    }
    public void SetSkipTip(KeyCode skipKey)
    {
        skipTipObject.SetActive(true);
        skipTip.text = skipKey.ToString() + " - заткнуть собеседника";
    }
    public void HideSkipTip()
    {
        skipTipObject.SetActive(false);
    }
}
