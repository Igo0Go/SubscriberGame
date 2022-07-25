using System.Collections.Generic;
using UnityEngine.Events;

public static class ConsoleEventCenter
{
    public static bool ShowConsole
    {
        get
        {
            return _showConsole;
        }
        set
        {
            _showConsole = value;
            ShowConsoleChanged.Invoke(value);
        }
    }
    private static bool _showConsole;

    public static UnityEvent<bool> ShowConsoleChanged { get; private set; }

    public static void Reload()
    {
        commandList = new List<BaseDebugCommand>();

        ShowConsoleChanged = new UnityEvent<bool>();

        Help = new DebugCommand("help", "���������� ������ ��������� ������", "help");
        Teleport = new DebugCommand<int, int, int>("tp", "����������������� � �����", "tp X Y Z");

        commandList.Add(Help);
        commandList.Add(Teleport);
    }

    #region Commands

    public static List<BaseDebugCommand> commandList;

    public static DebugCommand Help;
    public static DebugCommand<int, int, int> Teleport;

    #endregion
}
