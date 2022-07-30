using UnityEngine;
using UnityEngine.UI;

public class CoinsCounter : MonoBehaviour
{
    [SerializeField]
    private Text counterText;
    [SerializeField]
    private AudioSource uiSoundSource;

    private void Awake()
    {
        GameCenter.CoinsCounter = this;
    }

    public void AddCoins(int count, AudioClip clip)
    {
        StatsHolder.coins += count;
        counterText.text = StatsHolder.coins.ToString();
        uiSoundSource.PlayOneShot(clip);
        GameCenter.LevelProgressPanel.UpdateCoins();
    }
}
