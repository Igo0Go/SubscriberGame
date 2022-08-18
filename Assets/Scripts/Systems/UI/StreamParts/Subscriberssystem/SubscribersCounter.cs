using UnityEngine;
using UnityEngine.UI;

public class SubscribersCounter : MonoBehaviour, IGameSystem
{
    [SerializeField] private Text subscriberCountText;

    private void Awake()
    {
        StreamerPack.SubscribersCounter = this;
    }

    public void SetUp()
    {
        subscriberCountText.text = StatsHolder.subscribers.ToString();
    }

    public void AddSubscribers(int count)
    {
        LevelProggress.currentLevelSubscribersCount += count;
        UpdateSubscribers(count);

    }

    public void AddExtraSubscribers(int count)
    {
        LevelProggress.currentExtraSubscribersCount += count;
        UpdateSubscribers(count);
    }

    private void UpdateSubscribers(int count)
    {
        StatsHolder.subscribers += count;
        UIPack.LevelProgressPanel.UpdateSubscribers();
        StreamerPack.CheckSubscribersSumByDonate(StatsHolder.subscribers);
        subscriberCountText.text = StatsHolder.subscribers.ToString();
    }
}
