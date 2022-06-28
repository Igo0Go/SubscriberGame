using System.Collections.Generic;
using UnityEngine;

public class InfoMessageModule : MonoBehaviour
{
    [SerializeField, Tooltip("Сообщения, которые будут добавлены в очередь")]
    private List<MessageQueueItem> messages;

    [SerializeField]
    private MessangerSystem messageDispetcher;

    [ContextMenu("Отправить сообщения")]
    public void SendMessages()
    {
        foreach (var item in messages)
        {
            messageDispetcher.AddMessage(item);
        }

        Destroy(gameObject, Time.deltaTime);
    }
}