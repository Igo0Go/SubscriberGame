using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DonateDatabase", menuName = "IgoGo/DonateDatabase")]
public class DonateDatabase : ScriptableObject
{
    public List<DonateTargetItem> donateTargets;
    public List<DonateCastomizeItem> castomizeItems;
}

[System.Serializable]
public class DonateTargetItem
{
    public int targetSubscriberSumm;
    public DonateItem donate;
}

[System.Serializable]
public class DonateCastomizeItem
{
    public int sum;
    public string headerText;
    public AudioClip sound;
}
