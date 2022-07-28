using UnityEngine;
using UnityEngine.UI;

public class SubscribersCounter : MonoBehaviour
{
    [SerializeField] private Text subscriberCountText;

    private void Start()
    {
        subscriberCountText.text = StatsHolder.subscribers.ToString();
    }

    public void AddSubscribers(int count)
    {
        StatsHolder.subscribers += count;
        subscriberCountText.text = StatsHolder.subscribers.ToString();
    }
}
