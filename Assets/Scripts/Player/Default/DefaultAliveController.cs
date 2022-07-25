using UnityEngine;

public class DefaultAliveController : AliveController
{
    const string deadZoneTag = "DeadZone";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(deadZoneTag))
        {
            GameCenter.PlayerLocomotion.FastTeleportToPoint(GameCenter.SavePoint);
        }
    }
}
