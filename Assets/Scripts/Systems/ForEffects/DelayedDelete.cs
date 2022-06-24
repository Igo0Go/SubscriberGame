using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedDelete : MonoBehaviour
{
    [Min(0)]
    [SerializeField]
    private float lifeTime = 1;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
