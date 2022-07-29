using System.Collections.Generic;
using UnityEngine;

public class DonatePoint : MonoBehaviour
{
    [SerializeField]
    private List<DonateItem> donates;

    public void SendAllDonates()
    {
        foreach (var donate in donates)
        {
            GameCenter.DonateSystem.NewDonate(donate);
        }
        Destroy(gameObject);
    }
}
