using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "IgoGo/ItemDatabase")]
public class ItemsDatabase : ScriptableObject
{
    public List<CoinsSpawnerItem> coinsSpawnerItems;
}

[System.Serializable]
public class CoinsSpawnerItem
{
    public int price;
    public GameObject prefab;
}