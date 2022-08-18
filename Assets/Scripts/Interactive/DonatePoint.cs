using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class DonatePoint : MonoBehaviour
{
    [SerializeField]
    private List<DonateItem> donates;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f); //Требуется задержка, чтобы LevelProggressPanel подготовилась
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
