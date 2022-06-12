using System.Collections;
using System.Collections.Generic;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            OnEnterInTheWater?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            OnOutOfTheWater?.Invoke();
        }
    }
}
