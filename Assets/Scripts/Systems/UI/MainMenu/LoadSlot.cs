using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSlot : MonoBehaviour
{
    [SerializeField] Text coinsCounter;
    [SerializeField] Text subscribersCounter;
    [SerializeField] Text recentEvents;
    [SerializeField] Button useSlotButton;

    private int slotIndex;
    private SavedData savedData;

    public void Init(int slot, bool newGameMode)
    {
        slotIndex = slot;
        if(SaveLoadSystem.IsEmptySlot(slot))
        {
            savedData = SaveLoadSystem.GetDefaultData();
        }
        else
        {
            savedData = SaveLoadSystem.LoadFromSlot(slot);
        }

        useSlotButton.onClick = new Button.ButtonClickedEvent();

        if(newGameMode)
        {
            useSlotButton.onClick.AddListener(UseSlotAsNew);
        }
        else
        {
            useSlotButton.onClick.AddListener(UseSlotForLoading);
        }

        DrawSlotData();
    }

    public void UseSlotAsNew()
    {
        savedData = SaveLoadSystem.GetDefaultData();
        StatsHolder.recentEvents = "Вы очень удачно стали подписчиком одного канала." +
            " Мы уже выслали к вам бота, который расскажет правила и научит всему самому важному. Удачи!";
        savedData = SaveLoadSystem.GetDefaultData();
        SaveLoadSystem.SaveToSlot(slotIndex, savedData);
        UseSlotForLoading();
    }

    public void UseSlotForLoading()
    {
        StatsHolder.slotNumber = slotIndex;
        PlayerPrefs.SetInt("LastUsingSlot", StatsHolder.slotNumber);
        SaveLoadSystem.ApplyDataToCurrent(savedData);
        SceneManager.LoadScene(StatsHolder.sceneForLoading);
    }

    private void DrawSlotData()
    {
        coinsCounter.text = savedData.coins.ToString();
        subscribersCounter.text = savedData.subscribers.ToString();
        recentEvents.text = savedData.recentEvets;
    }
}
