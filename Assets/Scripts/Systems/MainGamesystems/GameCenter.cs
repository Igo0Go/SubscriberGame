using UnityEngine;

public static class GameCenter
{
    public static Transform SavePoint { get; set; }
    public static PlayerLocomotion PlayerLocomotion { get; set; }

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

            GameTools.SetCursorVisible(GlobalPause);
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

            GameTools.SetCursorVisible(GlobalPause);
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

    public static void SetUp()
    {
        ConsolePause = MenuPause = false;
        OpportunityToMove = OpportunityToView = true;
    }
}
