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
        LevelProggress.currentLevelCoinsCount += count;
        uiSoundSource.PlayOneShot(clip);
        UpdateCoinsCounter();
    }

    public void UpdateCoinsCounter()
    {
        counterText.text = (LevelProggress.currentLevelCoinsCount + LevelProggress.currentExtraCoinsCount).ToString();
        GameCenter.LevelProgressPanel.UpdateCoins();
    }
}
