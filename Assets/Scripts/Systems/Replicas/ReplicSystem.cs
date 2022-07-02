using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ReplicSystem : MonoBehaviour
{
    [SerializeField] private AudioSource voiceAudioSource;
    [SerializeField] private SubsPanel subsPanel;
    [SerializeField] private KeyCode skipButton;

    private List<ReplicaPack> replicaPacks;
    private bool useMain;

    private void Awake()
    {
        useMain = false;
        replicaPacks = new List<ReplicaPack>();
        GameTools.SetUpReplicsystem(OnDrawSubsChanged);
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
                if (useMain)
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

    private void Skip()
    {
        if(useMain)
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
        subsPanel.SetSkipTip(skipButton);
        useMain = true;
        while (replicaPacks[0].mainList.Count > 0)
        {
            ReplicaItem item = replicaPacks[0].mainList[0];
            voiceAudioSource.PlayOneShot(item.characterVoice);

            if(GameTools.DrawSubs)
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
            useMain = false;
            subsPanel.ClosePanel();
        }
    }

    private IEnumerator TallSkipReplicasCoroutine()
    {
        subsPanel.HideSkipTip();
        useMain = false;

        yield return new WaitForSeconds(2);

        while (replicaPacks[0].skipList.Count > 0)
        {
            ReplicaItem item = replicaPacks[0].skipList[0];
            voiceAudioSource.PlayOneShot(item.characterVoice);

            if (GameTools.DrawSubs)
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

#if UNITY_EDITOR
    [ContextMenu("Отрисовывать субтитры")]
    public void DrawSubs()
    {
        GameTools.DrawSubs = true;
    }
    [ContextMenu("НЕ Отрисовывать субтитры")]
    public void NoDrawSubs()
    {
        GameTools.DrawSubs = false;
    }
#endif
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
    public TMP_Text characterName;
    public TMP_Text characterText;
    public TMP_Text skipTip;

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
