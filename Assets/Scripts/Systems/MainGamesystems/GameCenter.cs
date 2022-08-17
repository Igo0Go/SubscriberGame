using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

#region GameParts

public static class GameCenter
{
    public static bool GlobalPause => MenuPause || ConsolePause;

    public static bool ConsolePause
    {
        get
        {
            return _consolePause;
        }
        set
        {
            _consolePause = value;
            CheckPause();
        }
    }
    private static bool _consolePause;

    public static bool MenuPause
    {
        get
        {
            return _menuPause;
        }
        set
        {
            _menuPause = value;
            CheckPause();
        }
    }
    private static bool _menuPause;

    public static bool OpportunityToView
    {
        get
        {
            return !GlobalPause && _opportunityToView;
        }
        set
        {
            _opportunityToView = value;
        }
    }
    private static bool _opportunityToView;

    public static bool OpportunityToMove
    {
        get
        {
            return !GlobalPause && _opportunityToMove;
        }
        set
        {
            _opportunityToMove = value;
        }
    }
    private static bool _opportunityToMove;

    private static void CheckPause()
    {
        GameTools.SetCursorVisible(GlobalPause);
        Time.timeScale = GlobalPause ? 0 : 1;
        PauseValueChanged.Invoke(GlobalPause);
    }

    public static UnityEvent<bool> PauseValueChanged;

    public static void SetUp()
    {
        PauseValueChanged = new UnityEvent<bool>();
        Settings.Refresh();

        ConsolePause = MenuPause = false;
        OpportunityToMove = OpportunityToView = true;
    }
}

public static class PlayerPack
{
    public static Transform SavePoint { get; set; }
    public static PlayerLocomotion PlayerLocomotion { get; set; }
    public static BotController Bot { get; set; }
    public static ReplicSystem ReplicSystem { get; set; }
}

public static class UIPack
{
    public static LevelProgressPanel LevelProgressPanel { get; set; }
    public static EasterEggSystem EasterEggSystem { get; set; }
    public static NotificationPanel NotificationPanel { get; set; }
    public static DeadPanel DeadPanel { get; set; }
}

public static class StreamerPack
{
    public static DonateSystem DonateSystem { get; set; }
    public static CoinsCounter CoinsCounter { get; set; }
    public static SubscribersCounter SubscribersCounter { get; set; }

    public static void CheckSubscribersSumByDonate(int currentSubscribersCount)
    {
        for (int i = StatsHolder.currentTargetDonateIndex;
            i < DonateSystem.donateDatabase.donateTargets.Count;
            i++)
        {
            if (DonateSystem.donateDatabase.donateTargets[i].targetSubscriberSumm <= currentSubscribersCount)
            {
                LevelProggress.currentExtraDonateCount++;
                UIPack.LevelProgressPanel.UpdateDonates();
                DonateSystem.NewDonate(DonateSystem.donateDatabase.donateTargets[i].donate);
                StatsHolder.currentTargetDonateIndex = i + 1;
            }
        }
    }

    public static void SendDonate(DonateItem donate)
    {
        DonateSystem.NewDonate(donate);
    }
}

public static class AudioPack
{
    public static AudioModeSystem AudioSystem { get; set; }
    public static EffectsAudioSystem EffectsAudioSystem { get; set; }
    public static MusicSystem MusicSystem { get; set; }
}

public static class TagHolder
{
    public static string TransfromFixator = "TransformFixator";
    public static string Explosion = "Explosion";
    public static string Enemy = "Enemy";
    public static string Interactable = "Interactable";
    public static string Air = "Air";
    public static string DeadZone = "DeadZone";
    public static string Darkness = "Darkness";
    public static string SavePoint = "SavePoint";
}

#endregion

#region DataForSaving

public static class Settings
{
    public static bool UseSubs { get; set; } = true;

    public static float VoiceVolume
    {
        get
        {
            return _voiceVolume;
        }

        set
        {
            _voiceVolume = value;
            VoiceVolumeChanged.Invoke(_voiceVolume);
        }
    }
    private static float _voiceVolume = 1;

    public static float SoundsVolume
    {
        get
        {
            return _soundsVolume;
        }
        set
        {
            _soundsVolume = value;
            SoundsVolumeChanged.Invoke(_soundsVolume);
        }
    }
    private static float _soundsVolume = 1;

    public static float MusicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            _musicVolume = value;
            MusicVolumeChanged.Invoke(_musicVolume);
        }
    }
    private static float _musicVolume = 1;

    public static UnityEvent<float> VoiceVolumeChanged { get; set; }
    public static UnityEvent<float> SoundsVolumeChanged { get; set; }
    public static UnityEvent<float> MusicVolumeChanged { get; set; }

    public static void Refresh()
    {
        VoiceVolumeChanged = new UnityEvent<float>();
        SoundsVolumeChanged = new UnityEvent<float>();
        MusicVolumeChanged = new UnityEvent<float>();
    }
}

public static class StatsHolder
{
    public static int slotNumber = 1;
    public static int sceneForLoading = 1;
    public static int coins = 0;
    public static int subscribers = 0;
    public static int currentTargetDonateIndex = 0;
    public static List<int> unblockedEasterEggsIds = new List<int>();
    public static string recentEvents = "ѕока ничего не случилось. Ёто только первый стрим!";
}

#endregion
