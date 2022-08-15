using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Используется для физических объектов, которые упали в запрещённую зону. По умолчанию, такая зана помечается
/// тэгом Finish. Если объект под воздействием гравитации упал туда, его вернёт на стартовую позицию
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[HelpURL("https://docs.google.com/document/d/1AvLXucyEeiVbQzsc6hsnHIO1Z_iVZgJ92suJRtlhpbs/edit?usp=sharing")]
public class ReturnTransformModule : MonoBehaviour
{
    [SerializeField]
    private List<string> targetTags;

    private Rigidbody rb;
    private Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (var tag in targetTags)
        {
            if (other.CompareTag(tag))
            {
                rb.velocity = Vector3.zero;
                transform.position = startPos;
                return;
            }
        }

    }
}
