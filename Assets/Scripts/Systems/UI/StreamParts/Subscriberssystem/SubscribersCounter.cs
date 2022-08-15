using UnityEngine;
using UnityEngine.UI;

public class SubscribersCounter : MonoBehaviour
{
    [SerializeField] private Text subscriberCountText;

    private void Awake()
    {
        StreamerPack.SubscribersCounter = this;
    }

    private void Start()
    {
        subscriberCountText.text = StatsHolder.subscribers.ToString();
    }

    public void AddSubscribers(int count)
    {
        LevelProggress.currentLevelSubscribersCount += count;
        UIPack.LevelProgressPanel.UpdateSubscribers();
        StreamerPack.CheckSubscribersSumByDonate(LevelProggress.currentLevelSubscribersCount);
        subscriberCountText.text = LevelProggress.currentLevelSubscribersCount.ToString();
    }
}
