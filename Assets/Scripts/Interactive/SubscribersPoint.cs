using UnityEngine;
using System.Collections;

public class SubscribersPoint : MonoBehaviour
{
    [SerializeField, Min(1)]
    private int subscribersCount = 1;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f); //��������� ��������, ����� LevelProggressPanel �������������
        LevelProggress.maxSubscribersOnLevel += subscribersCount;
        UIPack.LevelProgressPanel.UpdateSubscribers();
    }

    public void AddSubscribers()
    {
        StreamerPack.SubscribersCounter.AddSubscribers(subscribersCount);
        Destroy(gameObject);
    }
}
