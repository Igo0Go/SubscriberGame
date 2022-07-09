using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class HeadTrigerReactor : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnEnterInTheWater;
    [SerializeField]
    private UnityEvent OnOutOfTheWater;
    [SerializeField]
    private GameObject waterSplashParticles;
    [SerializeField]
    private PlayerLocomotion playerLocomotion;

    private int airValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Air"))
        {
            if(airValue == 0)
            {
                Instantiate(waterSplashParticles, transform.position, Quaternion.identity);
                playerLocomotion.SetLocomotionType(LocomotionType.Default);
                OnOutOfTheWater?.Invoke();
            }
            airValue++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Air"))
        {
            airValue--;
            if(airValue <= 0)
            {
                airValue = 0;
                OnEnterInTheWater?.Invoke();
                playerLocomotion.SetLocomotionType(LocomotionType.Water);
                Instantiate(waterSplashParticles, transform.position, Quaternion.identity);
            }
        }
    }
}
