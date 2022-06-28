using System.Collections;
using UnityEngine;

/// <summary>
/// При поступлении сигнала компонент будет с интервалом в copyDelay секунд делать копию
/// originalObject постепенно выстраивая мостик по вектору forward. Будет создано copyCount копий.
/// ОДНОРАЗОВЫЙ МОДУЛЬ
/// </summary>
[HelpURL("https://docs.google.com/document/d/1xR0RN_82iTmtgA6GBYHx7zRur1ggU5LGJN4J4IqOLA0/edit?usp=sharing")]
public class MultiplierModule : LogicModule
{
    [Tooltip("Объект, копии которого будут создаваться")]
    [SerializeField]
    private GameObject originalObject;

    [Tooltip("Количество создаваемых копий")]
    [SerializeField]
    private int copyCount;

    [Tooltip("Задержка между создаваемыми копиями")]
    [SerializeField]
    [Min(0)]
    private float copyDelay;

    private IEnumerator CreateDuplicateCoroutine()
    {
        for (int i = 0; i < copyCount; i++)
        {
            var obj = Instantiate(
                originalObject,
                transform.position + transform.forward * originalObject.transform.localScale.z * (i + 1),
                transform.rotation);
            obj.transform.parent = transform;
            yield return new WaitForSeconds(copyDelay);
        }
    }

    /// <summary>
    /// Начать создание копий
    /// </summary>
    [ContextMenu("Начать создание копий")]
    public override void ActivateModule()
    {
        StartCoroutine(CreateDuplicateCoroutine());
    }

    public override void ReturnToDefaultState()
    {

    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            Vector3 oldPos = transform.position;
            Vector3 pos;
            Gizmos.color = Color.yellow;
            for (int i = 0; i < copyCount; i++)
            {
                pos = transform.position + transform.forward * originalObject.transform.localScale.z * (i + 1);
                Gizmos.DrawLine(oldPos, pos);
                Gizmos.DrawSphere(pos, 0.1f);
                oldPos = pos;
            }
        }
    }
}
