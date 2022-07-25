using System.Collections.Generic;

public static class ConsoleEventCenter
{
    public static void Reload()
    {
        commandList = new List<BaseDebugCommand>();

        Help = new DebugCommand("help", "показывает список доступных команд", "help");
        Teleport = new DebugCommand<int, int, int>("tp", "телепортироваться к точке", "tp X Y Z");

        commandList.Add(Help);
        commandList.Add(Teleport);
    }

    #region Commands

    public static List<BaseDebugCommand> commandList;

    public static DebugCommand Help;
    public static DebugCommand<int, int, int> Teleport;

    #endregion
}
