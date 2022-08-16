using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoinsCounter : MonoBehaviour
{
    [SerializeField]
    private Text counterText;
    [SerializeField]
    private AudioSource uiSoundSource;

    private void Awake()
    {
        StreamerPack.CoinsCounter = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
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
