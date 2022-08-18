using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReplicSystem : MonoBehaviour, IGameSystem
{
    [SerializeField] private AudioSource voiceAudioSource;
    [SerializeField] private SubsPanel subsPanel;
    [SerializeField] private KeyCode skipButton;

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

        Settings.UseSubs = value;
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

            if (replicaPacks.Count > 0)
            {
                StartCoroutine(TallSkipReplicasCoroutine());
            }
        }
    }

    private IEnumerator TallMainReplicasCoroutine()
    {
        if(Settings.UseSubs)
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
            item.onReplicaStart?.Invoke();

            yield return new WaitForSeconds(item.characterVoice.length);
            replicaPacks[0].mainList.RemoveAt(0);
            item.onReplicaEnd?.Invoke();
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
            item.onReplicaStart?.Invoke();
            yield return new WaitForSeconds(item.characterVoice.length);
            replicaPacks[0].skipList.RemoveAt(0);
            item.onReplicaEnd?.Invoke();
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
        }
    }
}

[System.Serializable]
public class ReplicaPack
{
    public List<ReplicaItem> mainList;
    public List<ReplicaItem> skipList;
}

[System.Serializable]
public class ReplicaItem
{
    public string CharacterName;
    public Color characterColor = Color.white;
    [TextArea]
    public string characterText;
    public AudioClip characterVoice;
    public UnityEvent onReplicaStart;
    public UnityEvent onReplicaEnd;
}

[System.Serializable]
public class SubsPanel
{
    public GameObject panelObject;
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
        skipTip.text = skipKey.ToString() + " - заткнуть собеседника";
    }
    public void HideSkipTip()
    {
        skipTip.text = string.Empty;
    }
}
