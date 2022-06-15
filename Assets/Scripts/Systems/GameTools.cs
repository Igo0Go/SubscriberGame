using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTools
{
    public static bool opportunityToView;

    /// <summary>
    /// Задать видимость курсора
    /// </summary>
    /// <param name="value">Можно ли пользоваться курсором</param>
    public static void SetCursorVisible(bool value)
    {
        if (value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (opportunityToView)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
