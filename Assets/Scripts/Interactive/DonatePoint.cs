using System.Collections.Generic;
using UnityEngine;

public class DonatePoint : MonoBehaviour
{
    [SerializeField]
    private List<DonateItem> donates;

    private void Start()
    {
        LevelProggress.maxDonateOnLevel += donates.Count;
        UIPack.LevelProgressPanel.UpdateDonates();
    }

    public void SendAllDonates()
    {
        foreach (var donate in donates)
        {
            LevelProggress.currentLevelDonateCount++;
            UIPack.LevelProgressPanel.UpdateDonates();
            StreamerPack.DonateSystem.NewDonate(donate);
        }
        Destroy(gameObject);
    }
}
