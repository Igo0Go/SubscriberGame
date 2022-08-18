using UnityEngine;
using System.Collections;

public class EasterEggsModule : MonoBehaviour
{
    [SerializeField, Min(0)]
    private int easterEggsId = 0;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f); //Требуется задержка, чтобы LevelProggressPanel подготовилась
        LevelProggress.maxEasterEggsCount++;
        UIPack.LevelProgressPanel.UpdateEasterEggs();
    }

    public void UnblockEasterEgg()
    {
        UIPack.EasterEggSystem.UnblockEasterEggWithId(easterEggsId);
        Destroy(gameObject);
    }
}
