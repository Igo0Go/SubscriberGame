using System.Collections.Generic;

public static class ConsoleEventCenter
{
    public static void Reload()
    {
        commandList = new List<BaseDebugCommand>();

        Help = new DebugCommand("help", "���������� ������ ��������� ������", "help");
        Teleport = new DebugCommand<int, int, int>("tp", "����������������� � �����", "tp X Y Z");
        ClearSlot = new DebugCommand<int>("clearSaveSlot", "������� ��� ����� ����������", "clearSaveSlot [Slot Number]");
        ClearSlot.Execute.AddListener(SaveLoadSystem.ClearSlots);

        commandList.Add(Help);
        commandList.Add(Teleport);
        commandList.Add(ClearSlot);
    }

    #region Commands

    public static List<BaseDebugCommand> commandList;

    public static DebugCommand Help;
    public static DebugCommand<int> ClearSlot;
    public static DebugCommand<int, int, int> Teleport;

    #endregion
}
