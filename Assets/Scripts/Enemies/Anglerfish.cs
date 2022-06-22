using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
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
    private bool shock = false;
    private Transform myTransform;
    private Transform target;
    private Action curentBehaviour;
    private Animator anim;

    public override void GetDamage(int damage)
    {
        if(damage == 0)
        {
            StartCoroutine(ShockCoroutine(5));
        }
        else
        {
            base.GetDamage(damage);

        }
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        myTransform = transform;
        curentBehaviour = Sleep;
        GetNewBehaviour();
        anim.SetBool("Sleep", false);
        anim.SetBool("Attack", false);
    }

    private void Start()
    {
        FindObjectOfType<SoundOrigin>().SoundLaunched.AddListener(OnHearSound);
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
        if(isHunt <= 0)
        {
            StartCoroutine(indicator.ChangeIndicatorState(IndicatorState.DefaultState));
        }
        else
        {
            StartCoroutine(indicator.ChangeIndicatorState(IndicatorState.Question));
        }

        curentBehaviour = ToTargetSwim;
        anim.SetBool("Sleep", false);
    }

    private void ToTargetSwim()
    {
        if(target!= null)
        {
            Vector3 direction = GetWayDirectionToTarget();

            if (direction == Vector3.zero)
                return;

            if(isHunt > 2 && direction.magnitude > targetThresholdDistance * 5)
            {
                isHunt--;
                GetNewBehaviour();
                return;
            }

            if (direction.magnitude > targetThresholdDistance)
            {
                myTransform.forward = direction.normalized;
                myTransform.position += direction.normalized * moveSpeed * Time.deltaTime;
            }
            else
            {
                if (isHunt > 2)
                {
                    StartCoroutine(indicator.ChangeIndicatorState(IndicatorState.Agressive));
                    StartCoroutine(AttackCoroutine());
                    anim.SetBool("Attack", true);
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
        if(isHunt > 0)
            isHunt--;

        if(isHunt > 0)
        {
            if (UnityEngine.Random.Range(0, 10) == 0)
            {
                ToSleep(30);
            }
            else
            {
                FindNewWayPoint();
            }
        }
        else
        {
            FindNewWayPoint();
        }
    }

    private void OnHearSound(Transform soundOrigin)
    {
        if (target != soundOrigin && !shock)
        {
            if(Vector3.Distance(myTransform.position, soundOrigin.position) < targetThresholdDistance * 3)
            {
                target = soundOrigin;
                isHunt = 3;
                StartCoroutine(indicator.ChangeIndicatorState(IndicatorState.Question));
                curentBehaviour = ToTargetSwim;
            }
        }
    }

    private void ToSleep(float sleepTime)
    {
        StartCoroutine(StartMethodWithDelayCoroutine(sleepTime, GetNewBehaviour));
        StartCoroutine(indicator.ChangeIndicatorState(IndicatorState.Sleep));
        anim.SetBool("Sleep", true);
        curentBehaviour = Sleep;
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
                //if(obstacles[i].TryGetComponent(out PlayerLocomotion player))
                //{
                //    OnHearSound(player.transform);
                //}

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
            myTransform.position -= myTransform.forward * moveSpeed * 2 * Time.deltaTime;
            yield return null;
        }

        t = 0;

        yield return new WaitForSeconds(0.3f);

        while(true)
        {
            myTransform.position += myTransform.forward * moveSpeed * 3 * Time.deltaTime;
            if (Physics.SphereCast(myTransform.position, 1f, myTransform.forward, out RaycastHit hit,
                targetThresholdDistance / 2, ~ignoreMask))
            {
                anim.SetBool("Attack", false);
                break;
            }
            yield return null;
        }

        while (t < 1)
        {
            t += Time.deltaTime;
            myTransform.position -= myTransform.forward * moveSpeed * 2 * Time.deltaTime;
            yield return null;
        }

        StartCoroutine(indicator.ChangeIndicatorState(IndicatorState.Question));
        GetNewBehaviour();
    }

    private IEnumerator StartMethodWithDelayCoroutine(float delay, Action method)
    {
        yield return new WaitForSeconds(delay);
        method();
    }

    private IEnumerator ShockCoroutine(int shockTime)
    {
        shock = true;
        ToSleep(shockTime);
        yield return new WaitForSeconds(shockTime);
        shock = false;
    }
}
