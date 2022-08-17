using UnityEngine;

public class SubscribersPoint : MonoBehaviour
{
    [SerializeField, Min(1)]
    private int subscribersCount = 1;

    private void Start()
    {
        LevelProggress.maxSubscribersOnLevel += subscribersCount;
        UIPack.LevelProgressPanel.UpdateSubscribers();
    }

    public void AddSubscribers()
    {
        StreamerPack.SubscribersCounter.AddSubscribers(subscribersCount);
        Destroy(gameObject);
    }
}
