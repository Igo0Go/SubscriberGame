using UnityEngine;

public class SubscribersPoint : MonoBehaviour
{
    [SerializeField, Min(1)]
    private int subscribersCount = 1;

    private void Start()
    {
        LevelProggress.maxSubscribersOnLevel += subscribersCount;
    }

    public void AddSubscribers()
    {
        GameCenter.SubscribersCounter.AddSubscribers(subscribersCount);
        Destroy(gameObject);
    }
}