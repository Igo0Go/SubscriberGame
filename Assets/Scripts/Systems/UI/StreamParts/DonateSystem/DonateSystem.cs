using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonateSystem : MonoBehaviour
{
    public DonateDatabase donateDatabase;

    [SerializeField] private GameObject donateVigetContainer;
    [SerializeField] private Text donateHeader;
    [SerializeField] private Text donateText;
    [SerializeField, Min(1)] private int donateDelay;
    [SerializeField] private AudioSource donateSoundSource;

    private List<DonateItem> donateItems;
    private int currentDonate;

    private void Awake()
    {
        GameCenter.DonateSystem = this;
    }

    private void Start()
    {
        donateItems = new List<DonateItem>();
        currentDonate = 0;
        donateVigetContainer.SetActive(false);
        StartCoroutine(CheckDonateCoroutine());
    }

    public void NewDonate(DonateItem donate)
    {
        donateItems.Add(donate);
    }

    private IEnumerator CheckDonateCoroutine()
    {
        while (true)
        {
            if (donateItems.Count > currentDonate)
            {
                float delay = ShowDonate(donateItems[currentDonate]);
                if(delay < donateDelay)
                {
                    delay = donateDelay;
                }
                yield return new WaitForSeconds(delay);
                currentDonate++;
                donateVigetContainer.SetActive(false);
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(donateDelay);
            }
        }
    }

    private float ShowDonate(DonateItem donate)
    {
        donateVigetContainer.SetActive(true);
        int castomizeIndex = FindCastomizeIndexBySum(donate.sum);
        donateSoundSource.PlayOneShot(donateDatabase.castomizeItems[castomizeIndex].sound);
        donateHeader.text = donate.donateName + " " + 
            donateDatabase.castomizeItems[castomizeIndex].headerText + " " + donate.sum;
        donateText.text = donate.donateText;
        return donateDatabase.castomizeItems[castomizeIndex].sound.length;
    }

    private int FindCastomizeIndexBySum(int sum)
    {
        int i;
        for (i = 1; i < donateDatabase.castomizeItems.Count; i++)
        {
            if(sum >= donateDatabase.castomizeItems[i-1].sum && sum < donateDatabase.castomizeItems[i].sum)
            {
                break;
            }
        }
        return i-1;
    }
}

[System.Serializable]
public class DonateItem
{
    public string donateName;
    [TextArea(0,2)]
    public string donateText;
    public int sum;
}