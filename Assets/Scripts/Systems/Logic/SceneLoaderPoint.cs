using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderPoint : MonoBehaviour
{
    [SerializeField] private int sceneForLoading;
    [SerializeField, TextArea] private string newRecentEvent;
    [SerializeField] private bool withNumberSaving;

    public void LoadScene()
    {
        if(withNumberSaving)
        {
            StatsHolder.sceneForLoading = sceneForLoading;
        }
        StatsHolder.recentEvents = newRecentEvent;

        SaveLoadSystem.SaveToSlot(StatsHolder.slotNumber, SaveLoadSystem.GetDataFromCurrent());

        SceneManager.LoadScene(sceneForLoading);
    }
}
