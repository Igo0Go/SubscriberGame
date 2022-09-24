using UnityEngine;
using UnityEngine.UI;

public class EasterEggSystem : MonoBehaviour, IGameSystem
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
        UIPack.EasterEggSystem = this;
    }

    public void SetUp()
    {
        ShowItemWithId(-1);
        easterEggDatabase.RefreshIds();
    }

    public void UnblockEasterEggWithId_WithUIUpdate(int id)
    {
        LevelProggress.currentLevelEasterEggsCount++;
        UIPack.LevelProgressPanel.UpdateEasterEggs();
        UnblockEasterEggWithId(id);
        UIPack.NotificationPanel.ShowNotification("Найдена пасхалка!\r\n" +
            easterEggDatabase.EasterEggItems[id].name +
            "\r\n Откройте меню пасхалок, чтобы узнать больше");
    }
    public void UnblockEasterEggWithId(int id)
    {
        StatsHolder.unblockedEasterEggsIds.Add(id);
        StatsHolder.unblockedEasterEggsIds.Sort();
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