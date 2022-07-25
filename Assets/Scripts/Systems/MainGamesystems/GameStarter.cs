using UnityEngine;
using UnityEngine.Events;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private UnityEvent onStartEvents;

    private void Awake()
    {
        GameCenter.SetUp();
    }

    private void Start()
    {
        onStartEvents.Invoke();
    }
}
