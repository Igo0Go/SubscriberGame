using UnityEngine;

public class DefaultAliveController : AliveController
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagHolder.DeadZone))
        {
            GameCenter.PlayerLocomotion.FastTeleportToPoint(GameCenter.SavePoint);
        }
        else if(other.CompareTag(TagHolder.Darkness))
        {
            GameCenter.Bot.LightOn();
        }
        else if (other.CompareTag(TagHolder.SavePoint))
        {
            GameCenter.SavePoint = other.transform;
            Destroy(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagHolder.Darkness))
        {
            GameCenter.Bot.LightOff();
        }
    }
}
