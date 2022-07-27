using UnityEngine;
using UnityEngine.Events;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private UnityEvent onStartEvents;

    private void Awake()
    {
        GameCenter.SetUp();
        GameCenter.SavePoint = transform;
    }

    private void Start()
    {
        onStartEvents.Invoke();
    }
}
