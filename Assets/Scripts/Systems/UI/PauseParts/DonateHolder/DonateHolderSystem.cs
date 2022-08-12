using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonateHolderSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject itemButtonPrefab;
    [SerializeField]
    private Transform itemButtonsContent;

    public void ShowAllItemButtons()
    {
        for (int i = 0; i < itemButtonsContent.childCount; i++)
        {
            Destroy(itemButtonsContent.GetChild(i).gameObject, Time.deltaTime);
        }

        for (int i = 0; i < DonatesHolder.donates.Count; i++)
        {
            Instantiate(itemButtonPrefab, itemButtonsContent).
                GetComponent<DonateHolderButton>().Init(i, DonatesHolder.donates[i]);
        }
    }
}

public static class DonatesHolder
{
    public static List<DonateItem> donates = new List<DonateItem>();
}
