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


    private void Awake()
    {
        GameCenter.LevelProgressPanel = this;
        coinsCompleteIcon.SetActive(false);
    }

    public void UpdateCoins()
    {
        coinsInfoText.text = StatsHolder.coins.ToString() + "/" + LevelProggress.maxCoinsOnLevel.ToString();
        if(StatsHolder.coins >= LevelProggress.maxCoinsOnLevel)
        {
            coinsCompleteIcon.SetActive(true);
            uiSoundsSource.PlayOneShot(coinsCompleteSound);
        }
    }
}

public static class LevelProggress
{
    public static int maxCoinsOnLevel = 0;
}
