using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [SerializeField, Min(1)]
    private float speed = 1;
    [SerializeField]
    private GameObject visorParts;

    [SerializeField]
    private List<Transform> wayPoints;

    private Animator anim;
    private Transform myTransform;
    private bool forwardWay;
    private int currentWayTargetIndex;

    private void Awake()
    {
        myTransform = transform;
        visorParts.SetActive(false);
        anim = GetComponent<Animator>();
    }

    public void ActivateBot()
    {
        visorParts.SetActive(true);
        forwardWay = true;
        currentWayTargetIndex = 0;
        StartCoroutine(MoveCoroutine());
    }

    public void Warning()
    {
        StopAllCoroutines();
        anim.SetTrigger("Warning");
    }
    public void Check()
    {
        StopAllCoroutines();
        anim.SetTrigger("Check");
    }

    public void OnStopCheckAnimation()
    {
        StartCoroutine(MoveCoroutine());
    }
    public void OnStopWarningAnimation()
    {
        StartCoroutine(MoveCoroutine());
    }

    private void CheckWayPoint()
    {
        if (forwardWay)
        {
            currentWayTargetIndex++;
            if (currentWayTargetIndex >= wayPoints.Count)
            {
                forwardWay = false;
                Check();
                return;
            }
        }
        else
        {
            currentWayTargetIndex--;
            if (currentWayTargetIndex < 0)
            {
                forwardWay = true;
                Check();
                return;
            }
        }

        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        if(currentWayTargetIndex >= 0 && currentWayTargetIndex < wayPoints.Count)
        {
            while (true)
            {
                Vector3 dir = wayPoints[currentWayTargetIndex].position - myTransform.position;
                myTransform.forward = dir;
                float step = speed * Time.deltaTime;
                myTransform.position += step * dir.normalized;

                if (dir.magnitude < 2 * step)
                {
                    break;
                }
                yield return null;
            }
        }

        CheckWayPoint();
    }
}
