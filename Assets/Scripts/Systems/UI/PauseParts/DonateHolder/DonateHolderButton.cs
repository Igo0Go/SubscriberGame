using UnityEngine;
using UnityEngine.UI;

public class DonateHolderButton : MonoBehaviour
{
    [SerializeField]
    private Text titleText;
    [SerializeField]
    private Text donateText;

    private int id;

    public void Init(int id, DonateItem donate)
    {
        this.id = id;
        titleText.text = donate.donateName + "[" + donate.sum + "]";
        donateText.text = donate.donateText;
    }

    public void OnClick()
    {
        GameCenter.DonateSystem.ShowDonateFromHolderByIndex(id);
    }
}
