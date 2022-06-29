using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ReplicSystem : MonoBehaviour
{
    [SerializeField] private AudioSource voiceAudioSource;
    [SerializeField] private SubsPanel subsPanel;

    private List<ReplicaPack> replicaPacks;
    private bool useMain;

    private void Awake()
    {
        replicaPacks = new List<ReplicaPack>();
        GameTools.SetUpReplicsystem(OnDrawSubsChanged);
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
                    item = replicaPacks[0].mainList[0];
                }
                subsPanel.SetSubs(item.CharacterName, item.characterText, item.characterColor);
            }
        }
        else
        {
            subsPanel.ClosePanel();
        }
    }

    private IEnumerator TallMainReplicasCoroutine()
    {
        useMain = true;
        while (replicaPacks[0].mainList.Count > 0)
        {
            ReplicaItem item = replicaPacks[0].mainList[0];
            voiceAudioSource.PlayOneShot(item.characterVoice);

            if(GameTools.DrawSubs)
            {
                subsPanel.SetSubs(item.CharacterName, item.characterText, item.characterColor);
            }

            yield return new WaitForSeconds(item.characterVoice.length);
            replicaPacks[0].mainList.RemoveAt(0);
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
    public string characterText;
    public AudioClip characterVoice;
    public UnityEvent onReplicaEnd;
}

public class SubsPanel
{
    public GameObject panelObject;
    public TMP_Text characterName;
    public TMP_Text characterText;

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
}
