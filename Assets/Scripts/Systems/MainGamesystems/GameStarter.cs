using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private UnityEvent onStartEvents;

    private void Awake()
    {
        Settings.InitEvents();
        GameCenter.Refresh();
        PlayerPack.SavePoint = transform;
    }

    private IEnumerator Start()
    {
        yield return null;
        GameCenter.SetUpSystems();
        onStartEvents.Invoke();
    }
}
