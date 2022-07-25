using System.Collections.Generic;
using UnityEngine;

public class InfoMessageModule : MonoBehaviour
{
    [SerializeField, Tooltip("���������, ������� ����� ��������� � �������")]
    private List<MessageQueueItem> messages;

    [SerializeField]
    private MessangerSystem messageDispetcher;

    [ContextMenu("��������� ���������")]
    public void SendMessages()
    {
        foreach (var item in messages)
        {
            messageDispetcher.AddMessage(item);
        }

        Destroy(gameObject, Time.deltaTime);
    }
}