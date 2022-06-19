using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Anglerfish : AliveController
{
    [SerializeField]
    [Min(0)]
    private float moveSpeed = 1;
    [SerializeField]
    [Min(0)]
    private float targetThresholdDistance;
    [SerializeField]
    [Min(0)]
    private float obstacleThresholdDistance;
    [SerializeField]
    private LayerMask ignoreMask;
    [SerializeField]
    private List<Transform> wayPoints;
    [SerializeField]
    private EnemyStateIndicator indicator;
    [SerializeField]
    [Min(0)]
    private float obstacleSensivity = 0.5f;

    private int currentWayPointIndex;
    private int isHunt = 0;
    private Transform myTransform;
    private Transform target;
    private Action curentBehaviour;
    private Rigidbody rb;

    private void Awake()
    {
        myTransform = transform;
        curentBehaviour = Sleep;
        GetNewBehaviour();
    }

    private void Update()
    {
        curentBehaviour();
    }

    private void Sleep()
    {

    }

    private void FindNewWayPoint()
    {
        Transform bufer = wayPoints[currentWayPointIndex];
        wayPoints.RemoveAt(currentWayPointIndex);
        currentWayPointIndex = UnityEngine.Random.Range(0, wayPoints.Count - 1);
        wayPoints.Add(bufer);
        target = wayPoints[currentWayPointIndex];
        StartCoroutine(indicator.ChangeIndicatorState(IndicatorState.DefaultState));
        curentBehaviour = ToTargetSwim;
    }

    private void ToTargetSwim()
    {
        if(target!= null)
        {
            Vector3 direction = GetWayDirectionToTarget();

            if (direction.magnitude > targetThresholdDistance)
            {
                myTransform.forward = direction.normalized;
                myTransform.position += direction.normalized * moveSpeed * Time.deltaTime;
            }
            else
            {
                if (isHunt > 0)
                {
                    StartCoroutine(AttackCoroutine());
                    curentBehaviour = Sleep;
                }
                else
                {
                    GetNewBehaviour();
                }
            }
        }
    }

    private void GetNewBehaviour()
    {
        FindNewWayPoint();

        //isHunt--;
        //if (UnityEngine.Random.Range(0, 10) == 0)
        //{
        //    StartCoroutine(StartMethodWithDelayCoroutine(30, GetNewBehaviour));
        //    StartCoroutine(indicator.ChangeIndicatorState(IndicatorState.Sleep));
        //    curentBehaviour = Sleep;
        //}
        //else
        //{
        //    FindNewWayPoint();
        //}
    }

    private Vector3 GetWayDirectionToTarget()
    {
        Vector3 direction = target.position - transform.position;

        Collider[] obstacles = Physics.OverlapSphere(myTransform.position, obstacleThresholdDistance * 2, ~ignoreMask);
        Vector3 bufer;
        for (int i = 0; i < obstacles.Length; i++)
        {
            if(obstacles[i].transform != myTransform)
            {
                bufer = myTransform.position - obstacles[i].transform.position;

                direction += bufer.normalized * (1 / bufer.magnitude) * obstacleSensivity * Time.deltaTime;
            }
        }

        return direction;
    }

    private IEnumerator AttackCoroutine()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            myTransform.position -= myTransform.forward * moveSpeed / 2 * Time.deltaTime;
            yield return null;
        }

        t = 0;

        while(true)
        {
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
            if (Physics.SphereCast(myTransform.position, 1f, myTransform.forward, out RaycastHit hit,
                targetThresholdDistance, ~ignoreMask))
            {
                break;
            }
        }

        while (t < 1)
        {
            t += Time.deltaTime;
            myTransform.position -= myTransform.forward * moveSpeed / 2 * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator StartMethodWithDelayCoroutine(float delay, Action method)
    {
        yield return new WaitForSeconds(delay);
        method();
    }
}
