using UnityEngine;
using UnityEngine.Events;

public static class GameTools
{
    public static bool OpportunityToView { get; set; }

    public static bool DrawSubs
    {
        get
        {
            return _drawSubs;
        }
        set
        {
            _drawSubs = value;
            DrawSubsChanged?.Invoke(value);
        }
    }
    private static bool _drawSubs = true;

    private static UnityEvent<bool> DrawSubsChanged;

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
        else if (OpportunityToView)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public static void SetUpReplicsystem(UnityAction<bool> action)
    {
        DrawSubsChanged = new UnityEvent<bool>();
        DrawSubsChanged.AddListener(action);
    }
}

public static class Vector3Extention
{
    public static Vector3 Multiplicate(this Vector3 left, Vector3 right)
    {
        return new Vector3(left.x * right.x, left.y * right.y, left.z * right.z);
    }
}
