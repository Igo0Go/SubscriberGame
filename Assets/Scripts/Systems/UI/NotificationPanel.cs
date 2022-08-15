using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject backgroundPanel;
    [SerializeField]
    private Text notificationField;
    [SerializeField, Min(1)]
    private float notificationViewTime = 3;

    private List<string> notificationQueue;

    private void Awake()
    {
        UIPack.NotificationPanel = this;
        notificationQueue = new List<string>();
        backgroundPanel.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(CheckNotificationCoroutine());
    }

    public void ShowNotification(string text)
    {
        notificationQueue.Add(text);
    }

    private IEnumerator CheckNotificationCoroutine()
    {
        while (true)
        {
            if(notificationQueue.Count > 0)
            {
                backgroundPanel.SetActive(true);
                notificationField.text = notificationQueue[0];
                yield return new WaitForSeconds(notificationViewTime);
                backgroundPanel.SetActive(false);
                notificationQueue.RemoveAt(0);
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(notificationViewTime);
            }
        }
    }
}
