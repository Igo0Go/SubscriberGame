using UnityEngine;
using UnityEngine.UI;

public class CoinsCounter : MonoBehaviour, IGameSystem
{
    [SerializeField]
    private Text counterText;
    [SerializeField]
    private AudioSource uiSoundSource;

    private void Awake()
    {
        StreamerPack.CoinsCounter = this;
    }

    public void SetUp()
    {
        counterText.text = StatsHolder.coins.ToString();
    }

    public void AddCoins(int count, AudioClip clip)
    {
        LevelProggress.currentLevelCoinsCount += count;
        StatsHolder.coins += count;
        uiSoundSource.PlayOneShot(clip);
        UpdateCoinsCounter();
    }

    public void UpdateCoinsCounter()
    {
        counterText.text = StatsHolder.coins.ToString();
        UIPack.LevelProgressPanel.UpdateCoins();
    }
}
