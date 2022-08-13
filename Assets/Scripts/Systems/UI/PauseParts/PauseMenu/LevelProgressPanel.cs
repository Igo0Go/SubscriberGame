using UnityEngine;
using UnityEngine.UI;

public class LevelProgressPanel : MonoBehaviour
{
    [SerializeField]
    private AudioSource uiSoundsSource;

    [SerializeField]
    private Text coinsInfoText;
    [SerializeField]
    private GameObject coinsCompleteIcon;
    [SerializeField]
    private AudioClip coinsCompleteSound;

    [Space(10)]
    [SerializeField]
    private Text donateInfoText;
    [SerializeField]
    private GameObject donateCompleteIcon;
    [SerializeField]
    private AudioClip donateCompleteSound;

    [Space(10)]
    [SerializeField]
    private Text subscribersInfoText;
    [SerializeField]
    private GameObject subscribersCompleteIcon;
    [SerializeField]
    private AudioClip subscribersCompleteSound;

    [Space(10)]
    [SerializeField]
    private Text easterEggsInfoText;
    [SerializeField]
    private GameObject easterEggsCompleteIcon;
    [SerializeField]
    private AudioClip easterEggsCompleteSound;

    private void Awake()
    {
        GameCenter.LevelProgressPanel = this;
        coinsCompleteIcon.SetActive(false);
        donateCompleteIcon.SetActive(false);
        subscribersCompleteIcon.SetActive(false);
        easterEggsCompleteIcon.SetActive(false);
    }

    public void UpdateCoins()
    {
        coinsInfoText.text = LevelProggress.currentLevelCoinsCount.ToString() +
            "/" + LevelProggress.maxCoinsOnLevel.ToString() +
            "<color=red> + " + LevelProggress.currentExtraCoinsCount.ToString() + "</color>";
        if (LevelProggress.currentLevelCoinsCount >= LevelProggress.maxCoinsOnLevel)
        {
            coinsCompleteIcon.SetActive(true);
            uiSoundsSource.PlayOneShot(coinsCompleteSound);
        }
    }

    public void UpdateDonates()
    {
        donateInfoText.text = LevelProggress.currentLevelDonateCount.ToString() +
            "/" + LevelProggress.maxDonateOnLevel.ToString() +
            "<color=red> + " + LevelProggress.currentExtraDonateCount.ToString() + "</color>";
        if(LevelProggress.currentLevelDonateCount >= LevelProggress.maxDonateOnLevel)
        {
            donateCompleteIcon.SetActive(true);
            uiSoundsSource.PlayOneShot(donateCompleteSound);
        }
    }

    public void UpdateSubscribers()
    {
        subscribersInfoText.text = LevelProggress.currentLevelSubscribersCount.ToString()+
            "/" + LevelProggress.maxSubscribersOnLevel.ToString();
        if (LevelProggress.currentLevelSubscribersCount >= LevelProggress.maxSubscribersOnLevel)
        {
            subscribersCompleteIcon.SetActive(true);
            uiSoundsSource.PlayOneShot(subscribersCompleteSound);
        }
    }

    public void UpdateEasterEggs()
    {
        easterEggsInfoText.text = LevelProggress.currentLevelEasterEggsCount.ToString() +
            "/" + LevelProggress.maxEasterEggsCount.ToString();
        if (LevelProggress.currentLevelEasterEggsCount >= LevelProggress.maxEasterEggsCount)
        {
            easterEggsCompleteIcon.SetActive(true);
            uiSoundsSource.PlayOneShot(easterEggsCompleteSound);
        }
    }
}

public static class LevelProggress
{
    public static int currentLevelCoinsCount = 0;
    public static int currentLevelDonateCount = 0;
    public static int currentLevelSubscribersCount = 0;
    public static int currentLevelEasterEggsCount = 0;

    public static int currentExtraCoinsCount = 0;
    public static int currentExtraDonateCount = 0;

    public static int maxCoinsOnLevel = 0;
    public static int maxDonateOnLevel = 0;
    public static int maxSubscribersOnLevel = 0;
    public static int maxEasterEggsCount = 0;

    public static void Reset()
    {
        currentLevelCoinsCount = 0;
        currentLevelDonateCount = 0;
        currentLevelSubscribersCount = 0;
        currentLevelEasterEggsCount = 0;

        currentExtraCoinsCount = 0;
        currentExtraDonateCount = 0;

        maxCoinsOnLevel = 0;
        maxDonateOnLevel = 0;
        maxSubscribersOnLevel = 0;
        maxEasterEggsCount = 0;
    }
}
