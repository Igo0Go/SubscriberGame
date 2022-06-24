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
            GameTools.levelSpawner.SpawnMeInPosition(transform.position, waterSplashParticles);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            GameTools.levelSpawner.SpawnMeInPosition(transform.position, waterSplashParticles);
            OnOutOfTheWater?.Invoke();
        }
    }
}
