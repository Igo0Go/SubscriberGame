using UnityEngine;
using UnityEngine.Events;

public static class GameCenter
{
    public static Transform SavePoint { get; set; }
    public static PlayerLocomotion PlayerLocomotion { get; set; }
    public static BotController Bot { get; set; }

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

        ConsolePause = MenuPause = false;
        OpportunityToMove = OpportunityToView = true;
    }
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
}

public static class Settings
{
    public static bool UseSubs = true;
}

public static class StatsHolder
{
    public static int subscribers = 0;
}
