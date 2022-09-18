using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DonateSystem : MonoBehaviour, IGameSystem
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
        StreamerPack.DonateSystem = this;
    }

    public void SetUp()
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

    public void ShowDonateFromHolderByIndex(int index)
    {
        DonateItem donate = DonatesHolder.donates[index];
        int counter = FindCastomizeIndexBySum(donate.sum);
        ShowDonate(donate, counter);
    }

    private IEnumerator CheckDonateCoroutine()
    {
        while (true)
        {
            if (donateItems.Count > currentDonate)
            {
                float delay = UseDonate(donateItems[currentDonate]);
                if(delay < donateDelay)
                {
                    delay = donateDelay;
                }
                yield return new WaitForSeconds(delay);
                donateItems[currentDonate].onDonateEnd.Invoke();
                currentDonate++;
                donateVigetContainer.SetActive(false);
                yield return new WaitForSeconds(1);
            }
            else
            {
                donateVigetContainer.SetActive(false);
                yield return new WaitForSeconds(donateDelay);
            }
        }
    }

    private void ShowDonate(DonateItem donate, int castomizeIndex)
    {
        donateVigetContainer.SetActive(true);

        donateSoundSource.PlayOneShot(donateDatabase.castomizeItems[castomizeIndex].sound);
        donateHeader.text = donate.donateName + " " +
            donateDatabase.castomizeItems[castomizeIndex].headerText + " " + donate.sum;
        donateText.text = donate.donateText;
    }
    private float UseDonate(DonateItem donate)
    {
        LevelProggress.currentExtraCoinsCount += donate.sum;
        StatsHolder.coins += donate.sum;
        UIPack.LevelProgressPanel.UpdateCoins();
        StreamerPack.CoinsCounter.UpdateCoinsCounter();
        int castomizeIndex = FindCastomizeIndexBySum(donate.sum);
        ShowDonate(donate, castomizeIndex);

        donate.onDonateStart.Invoke();

        DonatesHolder.donates.Add(donate);

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
    public UnityEvent onDonateStart;
    public UnityEvent onDonateEnd;
}
