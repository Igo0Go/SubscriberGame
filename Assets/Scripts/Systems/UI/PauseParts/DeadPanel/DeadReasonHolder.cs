using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DeadReasonHolder", menuName = "IgoGo/DeadReasonHolder")]
public class DeadReasonHolder : ScriptableObject
{
    public List<DeadReasonItem> deadReasonItems;

    public DeadReasonItem FindReasonItemByType(DeadReason deadReason)
    {
        for (int i = 0; i < deadReasonItems.Count; i++)
        {
            if(deadReasonItems[i].type == deadReason)
            {
                return deadReasonItems[i];
            }
        }
        return null;
    }
}

public enum DeadReason
{
    Fall,
    Fire,
    Trap,
    Patrol
}

[System.Serializable]
public class DeadReasonItem
{
    public DeadReason type;
    public Sprite icon;
    [TextArea]
    public List<string> reasonTexts;
}
