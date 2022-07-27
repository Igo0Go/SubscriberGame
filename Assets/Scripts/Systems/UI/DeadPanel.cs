using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DeadPanel : MonoBehaviour
{
    [SerializeField]
    private Image deadPanel;

    [SerializeField]
    private TMP_Text deadReasonTip;

    [SerializeField]
    private GameObject reasonPanel;

    [SerializeField]
    private Image deadReasonImage;

    [SerializeField]
    [Min(0)]
    private float deadDelay;

    [SerializeField]
    private UnityEvent onEndOfDelay;

    [SerializeField]
    private List<DeadReasonItem> deadReasonItems;

    public void SetReasonByNumber(int number)
    {
        StartCoroutine(DeadPanelCorroutine());
        reasonPanel.SetActive(true);
        DeadReasonItem item = deadReasonItems[number];

        deadReasonImage.sprite = item.icon;
        deadReasonTip.text = item.reasonText;
    }

    private Color activePanelColor;
    private Color disactivePanelColor;

    private void Awake()
    {
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

[System.Serializable]
public class DeadReasonItem
{
    public Sprite icon;
    [TextArea]
    public string reasonText;
}
