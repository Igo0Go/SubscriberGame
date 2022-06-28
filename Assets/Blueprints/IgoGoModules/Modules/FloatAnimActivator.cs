using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// Этот модуль позволяет менять значение у аниматоров, которые имеют параметр типа float.
/// Метод Use() будет увеличивать значение параметра paremterName на step
/// </summary>
[HelpURL("https://docs.google.com/document/d/1Sw-B7NHomlTNUnNXfALGFmGL5xmMiyvQL1KU1phIGUA/edit?usp=sharing")]
public class FloatAnimActivator : LogicModule
{
    [Header("Настройки AnimActivator")]
    [Tooltip("Аниматор должен содержать параметр Value (float)"), SerializeField]
    private List<Animator> animObjects;

    [SerializeField, Range(0, 1), Tooltip("Шаг изменения параметра анимации")]
    private float step = 0.1f;

    [SerializeField, Tooltip("Название параметра, с которым нужно работать")]
    private string parameterName = "Value";

    private float target;
    private float currentValue;

    private void Start()
    {
        ReturnToDefaultState();
    }

    /// <summary>
    /// Увеличить целевое значение параметра аниматора на step.
    /// </summary>
    [ContextMenu("Добавить значение модуля")]
    public override void ActivateModule()
    {
        target += step;
        used = true;
        StartCoroutine(ChangeValueCoroutine());
    }

    /// <summary>
    /// Вернуть параметр аниматора к целевому значению 0.
    /// </summary>
    [ContextMenu("Вернуть в изначальное состояние")]
    public override void ReturnToDefaultState()
    {
        used = false;
        target = 0;
        StartCoroutine(ChangeValueCoroutine());
    }

    private void SetActiveForAll(float value)
    {
        for (int i = 0; i < animObjects.Count; i++)
        {
            if (animObjects[i] != null)
            {
                animObjects[i].SetFloat(parameterName, value, Time.deltaTime, Time.deltaTime);
            }
            else
            {
                Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :"
                    + gameObject.name);
            }
        }
    }

    private IEnumerator ChangeValueCoroutine()
    {
        float t = 0;
        float startValue = currentValue;
        while(t < 1)
        {
            t += Time.deltaTime;
            currentValue = Mathf.Lerp(startValue, target, t);
            SetActiveForAll(currentValue);
            yield return null;
        }
        currentValue = target;
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, 0.3f);
            for (int i = 0; i < animObjects.Count; i++)
            {
                if (animObjects[i] != null)
                {
                    Gizmos.DrawLine(transform.position, animObjects[i].transform.position);
                }
                else
                {
                    Debug.LogError("Элемент " + i + " равен null. Вероятно, была утеряна ссылка. Источник :"
                        + gameObject.name);
                }
            }
        }
    }
}
