using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DeadPanel : MonoBehaviour, IGameSystem
{
    [SerializeField]
    private DeadReasonHolder holder;

    [SerializeField]
    private Image deadPanel;

    [SerializeField]
    private Text deadReasonTip;

    [SerializeField]
    private GameObject reasonPanel;

    [SerializeField]
    private Image deadReasonImage;

    [SerializeField]
    private AudioClip deadClip;

    [SerializeField]
    private AudioSource uiAudioSource;

    [SerializeField]
    [Min(0)]
    private float deadDelay;

    [SerializeField]
    private UnityEvent onEndOfDelay;

    public void SetReasonByType(DeadReason type)
    {
        StartCoroutine(DeadPanelCorroutine());
        reasonPanel.SetActive(true);
        uiAudioSource.PlayOneShot(deadClip);
        DeadReasonItem item = holder.FindReasonItemByType(type);

        if(item == null)
        {
            return;
        }

        deadReasonImage.sprite = item.icon;
        deadReasonTip.text = item.reasonTexts[Random.Range(0, item.reasonTexts.Count)];
    }

    private Color activePanelColor;
    private Color disactivePanelColor;

    private void Awake()
    {
        UIPack.DeadPanel = this;
    }

    public void SetUp()
    {
        deadPanel.gameObject.SetActive(true);
        reasonPanel.SetActive(false);
        activePanelColor = deadPanel.color;
        disactivePanelColor = new Color(activePanelColor.r, activePanelColor.g, activePanelColor.b, 0);
        deadPanel.color = disactivePanelColor;
    }

    private IEnumerator DeadPanelCorroutine()
    {
        deadPanel.color = activePanelColor;

        yield return new WaitForSeconds(deadDelay);
        reasonPanel.SetActive(false);
        onEndOfDelay?.Invoke();
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime * 2;
            deadPanel.color = Color.Lerp(disactivePanelColor, activePanelColor, t);
            yield return null;
        }

    }
}
