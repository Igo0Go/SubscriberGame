using UnityEngine;

public class DefaultAliveController : AliveController
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagHolder.DeadZone))
        {
            UIPack.DeadPanel.SetReasonByType(other.GetComponent<DeadReasonPoint>().deadReason);
            PlayerPack.PlayerLocomotion.FastTeleportToPoint(PlayerPack.SavePoint);
        }
        else if (other.CompareTag(TagHolder.Darkness))
        {
            PlayerPack.Bot.LightOn();
        }
        else if (other.CompareTag(TagHolder.SavePoint))
        {
            PlayerPack.SavePoint = other.transform;
            Destroy(other);
        }
        else if (other.CompareTag(TagHolder.Enemy))
        {
            if (other.TryGetComponent(out PatrolEnemy patrol))
            {
                patrol.Warning();
            }
        }
        else if (other.TryGetComponent(out Coin coin))
        {
            coin.SetTarget(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagHolder.Darkness))
        {
            PlayerPack.Bot.LightOff();
        }
    }
}
