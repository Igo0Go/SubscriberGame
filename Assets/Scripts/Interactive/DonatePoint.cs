using System.Collections.Generic;
using UnityEngine;

public class DonatePoint : MonoBehaviour
{
    [SerializeField]
    private List<DonateItem> donates;

    private void Start()
    {
        LevelProggress.maxDonateOnLevel += donates.Count;
        GameCenter.LevelProgressPanel.UpdateDonates();
    }

    public void SendAllDonates()
    {
        foreach (var donate in donates)
        {
            LevelProggress.currentLevelDonateCount++;
            GameCenter.LevelProgressPanel.UpdateDonates();
            GameCenter.DonateSystem.NewDonate(donate);
        }
        Destroy(gameObject);
    }
}
