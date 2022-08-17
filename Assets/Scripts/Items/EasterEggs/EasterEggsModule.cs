using UnityEngine;

public class EasterEggsModule : MonoBehaviour
{
    [SerializeField, Min(0)]
    private int easterEggsId = 0;

    void Start()
    {
        LevelProggress.maxEasterEggsCount++;
        UIPack.LevelProgressPanel.UpdateEasterEggs();
    }

    public void UnblockEasterEgg()
    {
        UIPack.EasterEggSystem.UnblockEasterEggWithId(easterEggsId);
        Destroy(gameObject);
    }
}
