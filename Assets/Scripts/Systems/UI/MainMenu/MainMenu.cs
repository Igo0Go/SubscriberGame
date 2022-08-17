using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject easterEggsPanel;
    [SerializeField] private GameObject donatesPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject newGamePanel;
    [SerializeField] private GameObject loadPanel;

    [SerializeField] private List<LoadSlot> newGameSlots;
    [SerializeField] private List<LoadSlot> loadGameSlots;

    private const string defaultSlot = "LastUsingSlot";

    private void Awake()
    {
        if (PlayerPrefs.HasKey(defaultSlot))
        {
            StatsHolder.slotNumber = PlayerPrefs.GetInt(defaultSlot);

            if (!SaveLoadSystem.IsEmptySlot(StatsHolder.slotNumber))
            {
                SaveLoadSystem.ApplyDataToCurrent(SaveLoadSystem.LoadFromSlot(StatsHolder.slotNumber));
            }
        }
        else
        {
            PlayerPrefs.SetInt(defaultSlot, StatsHolder.slotNumber);
        }

        GameCenter.MenuPause = GameCenter.ConsolePause = false;
        GameTools.SetCursorVisible(true);
    }

    public void NewGameToggle()
    {
        newGamePanel.SetActive(!newGamePanel.activeSelf);
        mainButtons.SetActive(!newGamePanel.activeSelf);

        if(newGamePanel.activeSelf)
        {
            for (int i = 0; i < newGameSlots.Count; i++)
            {
                newGameSlots[i].Init(i + 1, true);
            }
        }
    }
    public void MoreNewGame()
    {

    }
    public void LoadGameToggle()
    {
        loadPanel.SetActive(!loadPanel.activeSelf);
        mainButtons.SetActive(!loadPanel.activeSelf);

        if (loadPanel.activeSelf)
        {
            for (int i = 0; i < loadGameSlots.Count; i++)
            {
                if(SaveLoadSystem.IsEmptySlot(i+1))
                {
                    loadGameSlots[i].gameObject.SetActive(false);
                }
                else
                {
                    loadGameSlots[i].gameObject.SetActive(true);
                    loadGameSlots[i].Init(i + 1, false);
                }
            }
        }
    }
    public void EasterEggsPanelToggle()
    {
        easterEggsPanel.SetActive(!easterEggsPanel.activeSelf);
        mainButtons.SetActive(!easterEggsPanel.activeSelf);

        if (easterEggsPanel.activeSelf)
        {
            GetComponent<EasterEggSystem>().ShowAllItemButtons();
        }
    }
    public void DonatesPanelToggle()
    {
        donatesPanel.SetActive(!donatesPanel.activeSelf);
        mainButtons.SetActive(!donatesPanel.activeSelf);

        if (donatesPanel.activeSelf)
        {
            GetComponent<DonateHolderSystem>().ShowAllItemButtons();
        }
    }
    public void SettingsPanelToggle()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        mainButtons.SetActive(!settingsPanel.activeSelf);
    }
    public void ExitGame()
    {
        PlayerPrefs.SetInt(defaultSlot, StatsHolder.slotNumber);
        Application.Quit();
    }
}
