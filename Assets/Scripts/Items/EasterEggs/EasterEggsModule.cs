using UnityEngine;
using System.Collections;

public class EasterEggsModule : MonoBehaviour
{
    [SerializeField, Min(0)]
    private int easterEggsId = 0;

    [SerializeField]
    private bool hiden = false;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f); //Требуется задержка, чтобы LevelProggressPanel подготовилась
        if(!hiden)
        {
            LevelProggress.maxEasterEggsCount++;
            UIPack.LevelProgressPanel.UpdateEasterEggs();
        }
    }

    public void UnblockEasterEgg()
    {
        if(hiden)
        {
            UIPack.EasterEggSystem.UnblockEasterEggWithId(easterEggsId);
        }
        else
        {
            UIPack.EasterEggSystem.UnblockEasterEggWithId_WithUIUpdate(easterEggsId);
        }
        Destroy(gameObject);
    }
}
