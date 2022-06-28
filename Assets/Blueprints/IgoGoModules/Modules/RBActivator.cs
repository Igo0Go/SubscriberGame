using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Активирует UseGravity у указанных Rigidbody
/// </summary>
[HelpURL("https://docs.google.com/document/d/1AvLXucyEeiVbQzsc6hsnHIO1Z_iVZgJ92suJRtlhpbs/edit?usp=sharing")]
public class RBActivator : LogicModule
{
    [Tooltip("Объекты, которые должны бдуту упасть по команде.")]
    [SerializeField]
    private List<Rigidbody> rigidbodies;

    /// <summary>
    /// Включить rigidbody у всех объектов
    /// </summary>
    [ContextMenu("Включить rigidbody у всех объектов")]
    public override void ActivateModule()
    {
        foreach (var item in rigidbodies)
        {
            if (item != null)
            {
                item.isKinematic = false;
                item.useGravity = true;
                used = true;
            }
        }
    }
    /// <summary>
    /// Выключить rigidbody у всех объектов
    /// </summary>
    [ContextMenu("Выключить rigidbody у всех объектов")]
    public override void ReturnToDefaultState()
    {
        foreach (var item in rigidbodies)
        {
            item.useGravity = false;
            item.isKinematic = true;
        }
        used = false;
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < rigidbodies.Count; i++)
            {
                if (rigidbodies[i] != null)
                {
                    Gizmos.DrawLine(transform.position, rigidbodies[i].transform.position);
                }
                else
                {
                    Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :" + gameObject.name);
                }
            }
        }
    }
}
