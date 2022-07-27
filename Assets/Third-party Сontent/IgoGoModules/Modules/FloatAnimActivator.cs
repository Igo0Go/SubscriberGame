using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// ���� ������ ��������� ������ �������� � ����������, ������� ����� �������� ���� float.
/// ����� Use() ����� ����������� �������� ��������� paremterName �� step
/// </summary>
[HelpURL("https://docs.google.com/document/d/1Sw-B7NHomlTNUnNXfALGFmGL5xmMiyvQL1KU1phIGUA/edit?usp=sharing")]
public class FloatAnimActivator : LogicModule
{
    [Header("��������� AnimActivator")]
    [Tooltip("�������� ������ ��������� �������� Value (float)"), SerializeField]
    private List<Animator> animObjects;

    [SerializeField, Range(0, 1), Tooltip("��� ��������� ��������� ��������")]
    private float step = 0.1f;

    [SerializeField, Tooltip("�������� ���������, � ������� ����� ��������")]
    private string parameterName = "Value";

    private float target;
    private float currentValue;

    private void Start()
    {
        ReturnToDefaultState();
    }

    /// <summary>
    /// ��������� ������� �������� ��������� ��������� �� step.
    /// </summary>
    [ContextMenu("�������� �������� ������")]
    public override void ActivateModule()
    {
        target += step;
        used = true;
        StartCoroutine(ChangeValueCoroutine());
    }

    /// <summary>
    /// ������� �������� ��������� � �������� �������� 0.
    /// </summary>
    [ContextMenu("������� � ����������� ���������")]
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
                Debug.LogError("������� " + i + " ����� null. ��������, ���� ������� ������. �������� :"
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
                    Debug.LogError("������� " + i + " ����� null. ��������, ���� ������� ������. �������� :"
                        + gameObject.name);
                }
            }
        }
    }
}
