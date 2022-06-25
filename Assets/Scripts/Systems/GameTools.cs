using UnityEngine;

public static class GameTools
{
    public static Spawner levelSpawner;

    public static bool opportunityToView;

    /// <summary>
    /// ������ ��������� �������
    /// </summary>
    /// <param name="value">����� �� ������������ ��������</param>
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

public static class Vector3Extention
{
    public static Vector3 Multiplicate(this Vector3 left, Vector3 right)
    {
        return new Vector3(left.x * right.x, left.y * right.y, left.z * right.z);
    }
}
