using UnityEngine;

public class SwimmingPlayerAliveController : AliveController
{
    [SerializeField]
    private PlayerLocomotion playerLocomotion;

    [SerializeField]
    private Transform savePoint;

    public override void GetDamage(int damage)
    {
        playerLocomotion.SetLocomotionOpportunity(false);
        playerLocomotion.FastTeleportToPoint(savePoint);
        playerLocomotion.SetLocomotionType(LocomotionType.Default);
        OnDead?.Invoke();
    }
}
