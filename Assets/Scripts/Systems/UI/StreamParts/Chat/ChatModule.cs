using System.Collections.Generic;
using UnityEngine;

public class ChatModule : MonoBehaviour
{
    [SerializeField] private ChatSystem chatSystem;
    [SerializeField] private List<ChatMessageInfo> messages;

    public void AddAllMessages()
    {
        chatSystem.AddNewItem(messages);
        Destroy(gameObject);
    }

    public void AddAllMessagesAsNew()
    {
        chatSystem.AddNewItemWithRemoving(messages);
        Destroy(gameObject);
    }
}
