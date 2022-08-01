using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EasterEggDatabase", menuName = "IgoGo/EasterEggDatabase")]
public class EasterEggDatabase : ScriptableObject
{
    public List<EasterEggItem> EasterEggItems;

    public void RefreshIds()
    {
        for (int i = 0; i < EasterEggItems.Count; i++)
        {
            EasterEggItems[i].Id = i;
        }
    }
}

[System.Serializable]
public class EasterEggItem
{
    public int Id { get; set; }

    public string name;
    [TextArea()]
    public string description;
    public string url;
    public Sprite icon;
}
