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
    private int currentItemNumber;

    private void Awake()
    {
        globalQueue = new List<ChatMessageInfo>();
        chatItems = new List<ChatItem>();
    }

    private void Start()
    {
        StartCoroutine(DelayCoroutine(delay));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            container.gameObject.SetActive(!container.gameObject.activeSelf);
        }
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
        if(globalQueue.Count > currentItemNumber)
        {
            ChatItem item = Instantiate(chatItemPrefab, container).GetComponent<ChatItem>();
            item.SetUP(globalQueue[currentItemNumber]);
            chatItems.Add(item);
            currentItemNumber++;
            if(currentItemNumber > maxItemsCount)
            {
                Destroy(chatItems[0].gameObject, 0.05f);
                chatItems.RemoveAt(0);
                globalQueue.RemoveAt(0);
                currentItemNumber--;
            }
        }
    }

    public void AddNewItem(List<ChatMessageInfo> info)
    {
        globalQueue.AddRange(info);
    }
}

[System.Serializable]
public class ChatMessageInfo
{
    public string authorName;
    public string messageText;
    public Color authorColor;
}
