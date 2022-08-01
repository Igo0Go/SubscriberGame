using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EasterEggSystem : MonoBehaviour
{
    [SerializeField]
    private EasterEggDatabase easterEggDatabase;
    [SerializeField]
    private GameObject itemButtonPrefab;
    [SerializeField]
    private Transform itemButtonsContent;

    [SerializeField]
    private GameObject itemInfoPanel;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Sprite defaultIcon;
    [SerializeField]
    private Button showInBrowserButton;
    [SerializeField]
    private Text nameField;
    [SerializeField]
    private Text descriptionField;

    private int currentItemId;

    private void Awake()
    {
        ShowItemWithId(-1);
        GameCenter.EasterEggSystem = this;
        easterEggDatabase.RefreshIds();
    }

    private void OnEnable()
    {
        ShowAllItemButtons();
    }

    public void UnblockEasterEggWithId(int id)
    {
        LevelProggress.currentLevelEasterEggsCount++;
        GameCenter.LevelProgressPanel.UpdateEasterEggs();
        StatsHolder.unblockedEasterEggsIds.Add(id);
        GameCenter.NotificationPanel.ShowNotification("������� ��������!\r\n" +
            easterEggDatabase.EasterEggItems[id].name +
            "\r\n �������� ���� ��������, ����� ������ ������");
    }

    public void ShowItemWithId(int id)
    {
        currentItemId = id;
        if(currentItemId < 0)
        {
            itemInfoPanel.SetActive(false);
        }
        else
        {
            itemInfoPanel.SetActive(true);

            var item = easterEggDatabase.EasterEggItems[id];

            icon.sprite = defaultIcon;
            if (item.icon != null)
            {
                icon.sprite = item.icon;
            }
            showInBrowserButton.interactable = false;
            if(!string.IsNullOrEmpty(item.url))
            {
                showInBrowserButton.interactable = true;
            }

            nameField.text = item.name;
            descriptionField.text = item.description;
        }
    }

    public void ShowInBrowser()
    {
        Application.OpenURL(easterEggDatabase.EasterEggItems[currentItemId].url);
    }

    public void ShowAllItemButtons()
    {
        for (int i = 0; i < itemButtonsContent.childCount; i++)
        {
            Destroy(itemButtonsContent.GetChild(i).gameObject, Time.deltaTime);
        }

        for (int i = 0; i < StatsHolder.unblockedEasterEggsIds.Count; i++)
        {
            var item = easterEggDatabase.EasterEggItems[StatsHolder.unblockedEasterEggsIds[i]];

            Instantiate(itemButtonPrefab, itemButtonsContent).
                GetComponent<EasterEggItemButton>().Init(item.Id, item.name);
        }
    }
}