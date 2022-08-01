using UnityEngine;

public class EasterEggsModule : MonoBehaviour
{
    [SerializeField, Min(0)]
    private int easterEggsId = 0;

    void Start()
    {
        LevelProggress.maxEasterEggsCount++;
    }

    public void UnblockEasterEgg()
    {
        GameCenter.EasterEggSystem.UnblockEasterEggWithId(easterEggsId);
        Destroy(gameObject);
    }
}
