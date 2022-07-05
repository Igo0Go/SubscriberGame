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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            OnEnterInTheWater?.Invoke();
            Instantiate(waterSplashParticles, transform.position, Quaternion.identity);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Instantiate(waterSplashParticles, transform.position, Quaternion.identity);
            OnOutOfTheWater?.Invoke();
        }
    }
}
