using UnityEngine;
using UnityEngine.Events;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private UnityEvent onStartEvents;

    private void Awake()
    {
        GameCenter.SetUp();
        PlayerPack.SavePoint = transform;
    }

    private void Start()
    {
        onStartEvents.Invoke();
    }
}
