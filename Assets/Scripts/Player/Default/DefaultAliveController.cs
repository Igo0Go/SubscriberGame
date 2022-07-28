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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagHolder.Darkness))
        {
            GameCenter.Bot.LightOff();
        }
    }
}
