using UnityEngine;
using UnityEngine.UI;

public class SubscribersCounter : MonoBehaviour
{
    [SerializeField] private Text subscriberCountText;

    private void Awake()
    {
        GameCenter.SubscribersCounter = this;
    }

    private void Start()
    {
        subscriberCountText.text = StatsHolder.subscribers.ToString();
    }

    public void AddSubscribers(int count)
    {
        LevelProggress.currentLevelSubscribersCount += count;
        GameCenter.LevelProgressPanel.UpdateSubscribers();
        GameCenter.CheckSubscribersSumByDonate(LevelProggress.currentLevelSubscribersCount);
        subscriberCountText.text = LevelProggress.currentLevelSubscribersCount.ToString();
    }
}
