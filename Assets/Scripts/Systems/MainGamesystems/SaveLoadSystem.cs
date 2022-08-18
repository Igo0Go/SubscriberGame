using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class SaveLoadSystem
{
    public static bool IsEmptySlot(int slotIndex)
    {
        string SavePath = Path.Combine(Application.streamingAssetsPath, "Saves");

        if(!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }

        SavePath = Path.Combine(SavePath, "Slot" + slotIndex.ToString() + ".save");

        if(File.Exists(SavePath))
        {
            string data = File.ReadAllText(SavePath);
            if(string.IsNullOrEmpty(data))
            {
                return true;
            }
            return false;
        }
        return true;
    }

    public static SavedData GetDataFromCurrent()
    {
        return new SavedData()
        {
            sceneForLoading = StatsHolder.sceneForLoading,
            coins = StatsHolder.coins,
            subscribers = StatsHolder.subscribers,
            currentTargetDonateIndex = StatsHolder.currentTargetDonateIndex,
            unblockedEasterEggsIds = StatsHolder.unblockedEasterEggsIds,
            recentEvets = StatsHolder.recentEvents,

            voices = Settings.VoiceVolume,
            sounds = Settings.SoundsVolume,
            music = Settings.MusicVolume,
            useSubtitles = Settings.UseSubs
        };
    }

    public static SavedData GetDefaultData()
    {
        return new SavedData()
        {
            sceneForLoading = 1,
            coins = 0,
            subscribers = 0,
            currentTargetDonateIndex = 0,
            unblockedEasterEggsIds = new List<int>(),
            recentEvets = "ѕока ничего не случилось. Ёто только первый стрим!",

            voices = 1,
            sounds = 0.75f,
            music = 0.5f,
            useSubtitles = true
        };
    }

    public static void ApplyDataToCurrent(SavedData data)
    {
        StatsHolder.sceneForLoading = data.sceneForLoading;
        StatsHolder.coins = data.coins;
        StatsHolder.subscribers = data.subscribers;
        StatsHolder.currentTargetDonateIndex = data.currentTargetDonateIndex;
        StatsHolder.unblockedEasterEggsIds = data.unblockedEasterEggsIds;
        StatsHolder.recentEvents = data.recentEvets;

        Settings.VoiceVolume = data.voices;
        Settings.SoundsVolume = data.sounds;
        Settings.MusicVolume = data.music;
        Settings.UseSubs = data.useSubtitles;
    }

    public static void SaveToSlot(int slotIndex, SavedData data)
    {
        string SavePath = Path.Combine(Application.streamingAssetsPath, "Saves", "Slot" + slotIndex.ToString() + ".save");

        string dataString = JsonUtility.ToJson(data);

        if(!File.Exists(SavePath))
        {
            File.Create(SavePath);
        }

        File.WriteAllText(SavePath, dataString);
    }

    public static SavedData LoadFromSlot(int slotIndex)
    {
        string FilePath = Path.Combine(Application.streamingAssetsPath, "Saves", "Slot" + slotIndex.ToString() + ".save");

        string json = File.ReadAllText(FilePath);

        return JsonUtility.FromJson<SavedData>(json);
    }

    public static void ClearSlots(int number)
    {
        if (number < 1 || number > 3)
            return;

        if(!IsEmptySlot(number))
        {
            string SavePath = Path.Combine(Application.streamingAssetsPath, "Saves", "Slot" + number.ToString() + ".save");

            string dataString = string.Empty;

            if (!File.Exists(SavePath))
            {
                File.Create(SavePath);
            }

            File.WriteAllText(SavePath, dataString);
        }
    }
}

[System.Serializable]
public class SavedData
{
    public int sceneForLoading;
    public int coins;
    public int subscribers;
    public int currentTargetDonateIndex;
    public List<int> unblockedEasterEggsIds;

    public float voices;
    public float sounds;
    public float music;
    public bool useSubtitles;

    public string recentEvets;
}