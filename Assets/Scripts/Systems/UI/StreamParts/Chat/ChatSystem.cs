using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatSystem : MonoBehaviour
{
    [SerializeField]
    private Transform container;
    [SerializeField, Min(0.1f)]
    private float delay = 1;
    [SerializeField, Min(1)]
    private int maxItemsCount = 1;
    [SerializeField]
    private GameObject chatItemPrefab;

    private List<ChatMessageInfo> globalQueue;
    private List<ChatItem> chatItems;

    private void Awake()
    {
        globalQueue = new List<ChatMessageInfo>();
        chatItems = new List<ChatItem>();
    }

    private void Start()
    {
        maxItemsCount = (int)container.GetComponent<RectTransform>().rect.height / 50 - 1;

        StartCoroutine(DelayCoroutine(delay));
    }

    private IEnumerator DelayCoroutine(float delay)
    {
        while(true)
        {
            CheckItems();
            yield return new WaitForSeconds(delay);
        }
    }

    private void CheckItems()
    {
        if(globalQueue.Count > 0)
        {
            if(StatsHolder.subscribers >= globalQueue[0].subscribersCountToActivate)
            {
                ChatItem item = Instantiate(chatItemPrefab, container).GetComponent<ChatItem>();
                item.SetUP(globalQueue[0]);
                chatItems.Add(item);

                if (container.childCount > maxItemsCount)
                {
                    Destroy(chatItems[0].gameObject, 0.05f);
                    chatItems.RemoveAt(0);
                }
            }

            globalQueue.RemoveAt(0);
        }
    }

    public void AddNewItem(List<ChatMessageInfo> info)
    {
        globalQueue.AddRange(info);
    }
    public void AddNewItemWithRemoving(List<ChatMessageInfo> info)
    {
        globalQueue.Clear();
        globalQueue.AddRange(info);
    }
}

[System.Serializable]
public class ChatMessageInfo
{
    public int subscribersCountToActivate = 0;
    public string authorName;
    [TextArea(2,2)]
    public string messageText;
    public Color authorColor;
    public bool useColor = false;
}
