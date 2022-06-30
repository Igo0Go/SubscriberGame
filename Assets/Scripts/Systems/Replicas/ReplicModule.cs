using UnityEngine;

public class ReplicModule : MonoBehaviour
{
    [SerializeField]
    private ReplicSystem system;
    [SerializeField]
    private ReplicaPack pack;

    public void UsePack()
    {
        system.AddNewReplicaPack(pack);
        Destroy(gameObject, Time.deltaTime);
    }
}
