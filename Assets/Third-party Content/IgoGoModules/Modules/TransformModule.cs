using UnityEngine;
using System.Collections;

public enum LoopType
{
    Once,
    PingPong,
    Repeat
}

/// <summary>
/// ������ ������ ������������ ��� �������� ������� ����� ����� �������
/// </summary>
[HelpURL("https://docs.google.com/document/d/1YA0fSq6q6Nbarg9PwMFs5GzNYFtaNGaD8IXdj8drbr0/edit?usp=sharing")]
public class TransformModule : LogicModule
{
    [SerializeField, Tooltip("�������� ������� ����� ������������ ���������")]
    private Vector3 target = Vector3.forward;

    [SerializeField]
    [Tooltip("��� �����������:" +
        "Once - ���� ��������=���� �����������. " +
        "loop - ����� ������ ������� �� ������, �� ��������������� �� ������. " +
        "Ping-Pong - �������� ����-�������.")]
    private LoopType loopType;

    [SerializeField, Tooltip("������� ����� ������� ����������� (�)"), Min(0)]
    private float duration = 1;

    [SerializeField, Tooltip("�������� ����� ������������� (�)"), Min(0)]
    private float delay = 0;

    [SerializeField, Tooltip("������ ��������� ��������")]
    private AnimationCurve accelCurve;

    private Vector3 localStart;
    private Vector3 localTarget;
    private bool activate = false;
    private float time = 0f;
    private float position = 0f;
    private float direction = -1f;

    private void Awake()
    {
        direction = loopType == LoopType.Once ? -1 : 1;
        activate = false;
        time = position = 0;
        localStart = transform.localPosition;
        localTarget = localStart + transform.right * target.x + transform.up * target.y + transform.forward * target.z;
    }

    /// <summary>
    /// Once - ������ ��������
    /// Loop,PingPong - ������/���������� ��������
    /// </summary>
    [ContextMenu("������������ ������")]
    public override void ActivateModule()
    {
        switch (loopType)
        {
            case LoopType.Once:
                direction *= -1;
                StopAllCoroutines();
                StartCoroutine(MoveOnce());
                break;
            case LoopType.PingPong:
                if(activate)
                {
                    ReturnToDefaultState();
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(MovePingPong());
                }
                break;
            case LoopType.Repeat:
                if (activate)
                {
                    StopAllCoroutines();
                }
                else
                {
                    StartCoroutine(MoveRepeat());
                }
                break;
            default:
                break;
        }


        if (loopType == LoopType.Once)
        {
            activate = true;
        }
        else
        {
            activate = !activate;
        }
    }

    /// <summary>
    /// ������� � ����������� �������
    /// </summary>
    [ContextMenu("������� � �������� ���������")]
    public override void ReturnToDefaultState()
    {
        StopAllCoroutines();
        StartCoroutine(ReturnToDefaultStateCoroutine());
    }

    private void PerformTransform(float position)
    {
        var curvePosition = accelCurve.Evaluate(position);
        var pos = Vector3.Lerp(localStart, localTarget, curvePosition);
        transform.localPosition = pos;
    }

    private IEnumerator MoveOnce()
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            time += direction * Time.deltaTime / duration;
            position = Mathf.Clamp01(time);
            PerformTransform(time);
            if (position <= 0)
            {
                time = position = 0;
                break;
            }
            if (position >= 1)
            {
                time = position = 1;
                break;
            }
            yield return null;
        }
        PerformTransform(time);
    }

    private IEnumerator MovePingPong()
    {
        yield return new WaitForSeconds(delay);
        activate = true;
        while (true)
        {
            time += direction * Time.deltaTime / duration;
            position = Mathf.PingPong(time, 1f);
            PerformTransform(position);
            yield return null;
        }
    }

    private IEnumerator MoveRepeat()
    {
        yield return new WaitForSeconds(delay);
        activate = true;
        while (true)
        {
            time += direction * Time.deltaTime / duration;
            position = Mathf.Repeat(time, 1f);
            PerformTransform(position);
            yield return null;
        }
    }

    private IEnumerator ReturnToDefaultStateCoroutine()
    {
        yield return new WaitForSeconds(delay);
        activate = false;
        direction = -1;

        while (time > 1)
        {
            time--;
        }

        while (time >= 0)
        {
            time += direction * Time.deltaTime / duration;
            position = Mathf.Clamp01(time);
            PerformTransform(position);
            yield return null;
        }
        position = time = 0;
        direction = 1;
    }

    private void OnDrawGizmos()
    {
        if(debug)
        {
            GizmosExtention.DrawWireCubeByTransformWithOffset(transform, target);
        }
    }
}

public static class GizmosExtention
{
    public static void DrawWireCubeByTransformWithOffset(Transform origin, Vector3 target)
    {
        Gizmos.color = Color.cyan;

        Vector3 targetPos = origin.position + origin.forward * target.z + origin.right * target.x + origin.up * target.y;
        Vector3 bufer1 = targetPos + origin.forward * origin.localScale.z / 2 +
            origin.right * origin.localScale.x / 2 + origin.up * origin.localScale.y / 2;
        Vector3 bufer2 = targetPos - origin.forward * origin.localScale.z / 2 +
            origin.right * origin.localScale.x / 2 + origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        bufer2 = targetPos + origin.forward * origin.localScale.z / 2 -
            origin.right * origin.localScale.x / 2 + origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        bufer2 = targetPos + origin.forward * origin.localScale.z / 2 +
            origin.right * origin.localScale.x / 2 - origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        bufer1 = bufer2;

        bufer2 = targetPos + origin.forward * origin.localScale.z / 2 -
            origin.right * origin.localScale.x / 2 - origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        bufer2 = targetPos - origin.forward * origin.localScale.z / 2 +
            origin.right * origin.localScale.x / 2 - origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        bufer1 = bufer2;

        bufer2 = targetPos - origin.forward * origin.localScale.z / 2 -
            origin.right * origin.localScale.x / 2 - origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        bufer2 = targetPos - origin.forward * origin.localScale.z / 2 +
            origin.right * origin.localScale.x / 2 + origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        bufer1 = bufer2;

        bufer2 = targetPos - origin.forward * origin.localScale.z / 2 -
            origin.right * origin.localScale.x / 2 + origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        bufer1 = bufer2;

        bufer2 = targetPos + origin.forward * origin.localScale.z / 2 -
            origin.right * origin.localScale.x / 2 + origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        bufer2 = targetPos - origin.forward * origin.localScale.z / 2 -
            origin.right * origin.localScale.x / 2 - origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        bufer1 = targetPos + origin.forward * origin.localScale.z / 2 -
            origin.right * origin.localScale.x / 2 - origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        bufer2 = targetPos + origin.forward * origin.localScale.z / 2 -
            origin.right * origin.localScale.x / 2 + origin.up * origin.localScale.y / 2;

        Debug.DrawLine(bufer2, bufer1, Color.cyan);

        Gizmos.DrawLine(origin.position, targetPos);
    }
}
