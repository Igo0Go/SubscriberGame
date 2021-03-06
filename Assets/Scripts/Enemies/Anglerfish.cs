using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
public class Anglerfish : Enemy
{
    [Header("????????")]
    [SerializeField]
    [Min(0)]
    private float moveSpeed = 1;

    [Space(10)]
    [Header("?????????")]
    [SerializeField]
    [Min(0)]
    private float targetThresholdDistance = 7;
    [SerializeField]
    [Min(0)]
    private float stopAttackDistance = 4;
    [SerializeField]
    [Min(0)]
    private float hearThresholdDistance = 3;
    [SerializeField]
    [Min(0)]
    private float pursuitThresholdDistance = 10;

    [Space(10)]
    [Header("?????")]
    [SerializeField]
    [Min(0)]
    private float sleepTime = 30f;
    [SerializeField]
    [Min(0)]
    private float shockTime = 10f;

    [Space(10)]
    [Header("??????????? ? ?????????")]
    [SerializeField]
    [Min(0)]
    private float probabilityOfSleep = 0.1f;
    [SerializeField]
    [Min(0)]
    private float attackSpeedMultiplicator = 1;
    [SerializeField]
    [Range(0.1f, 2)]
    private float rotateSpeedMultiplicator = 1;

    [Space(10)]
    [Header("?????? ?????????")]
    [SerializeField]
    private LayerMask ignoreMask;
    [SerializeField]
    private List<Transform> wayPoints;
    [SerializeField]
    private EnemyStateIndicator indicator;

    [Space(10)]
    [Header("??????? ?????????")]
    [SerializeField]
    private UnityEvent onAttack;

    [Space(10)]
    [Header("???????")]
    [SerializeField]
    private bool drawTargetThresholdDistance;
    [SerializeField]
    private bool drawStopAttackDistance;
    [SerializeField]
    private bool drawHearThresholdDistance;
    [SerializeField]
    private bool drawPursuitThresholdDistance;

    private int currentWayPointIndex;
    private int isHunt = 0;
    private bool shock = false;
    private float rotateT = 0;
    private Transform myTransform;
    private Transform target;

    private Animator anim;

    public override void GetDamage(int damage)
    {
        if(damage == 0)
        {
            StopAllCoroutines();
            ToSleep(shockTime);
            StartCoroutine(ShockCoroutine(shockTime));
        }
        else
        {
            base.GetDamage(damage);
        }
    }

    protected override void Dead()
    {
        curentBehaviour = Sleep;
        anim.SetTrigger("Dead");
        base.Dead();
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Explosion"))
        {
            int distance = (int)Vector3.Distance(other.transform.position, myTransform.position);

            if(distance == 0)
            {
                GetDamage(15);
            }

            GetDamage(15 / distance);
        }
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
        if (target!= null)
        {
            Vector3 direction = GetWayDirectionToTarget();

            if (direction == Vector3.zero)
                return;

            if(isHunt > 2 && direction.magnitude > pursuitThresholdDistance)
            {
                isHunt--;
                GetNewBehaviour();
                return;
            }

            if (direction.magnitude > targetThresholdDistance)
            {
                rotateT = Mathf.Clamp01(rotateT + Time.deltaTime * rotateSpeedMultiplicator);
                myTransform.forward = Vector3.Lerp(myTransform.forward, direction.normalized, rotateT);
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
                    return;
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
        rotateT = 0;
        if(isHunt > 0)
            isHunt--;

        if(isHunt > 0)
        {
            if (UnityEngine.Random.Range(0f, 1f) <= probabilityOfSleep)
            {
                ToSleep(sleepTime);
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

    private void OnHearSound(SoundOrigin soundOrigin)
    {
        if (isHunt < 3 && target != soundOrigin && !shock)
        {
            if(Vector3.Distance(myTransform.position, soundOrigin.transform.position) < hearThresholdDistance 
                + soundOrigin.soundDistance)
            {
                anim.SetBool("Sleep", false);
                target = soundOrigin.transform;
                isHunt = 3;
                StartCoroutine(indicator.ChangeIndicatorState(IndicatorState.Question));
                curentBehaviour = ToTargetSwim;
            }
        }
    }

    private void ToSleep(float sleepTime)
    {
        target = null;
        StartCoroutine(StartMethodWithDelayCoroutine(sleepTime, GetNewBehaviour));
        StartCoroutine(indicator.ChangeIndicatorState(IndicatorState.Sleep));
        anim.SetBool("Attack", false);
        anim.SetBool("Sleep", true);
        curentBehaviour = Sleep;
    }

    private Vector3 GetWayDirectionToTarget()
    {
        Vector3 direction = target.position - transform.position;

        //Collider[] obstacles = Physics.OverlapSphere(myTransform.position, obstacleThresholdDistance, ~ignoreMask);
        //Vector3 bufer;
        //for (int i = 0; i < obstacles.Length; i++)
        //{
        //    if(obstacles[i].transform != myTransform)
        //    {
        //        //if(obstacles[i].TryGetComponent(out PlayerLocomotion player))
        //        //{
        //        //    OnHearSound(player.transform);
        //        //}

        //        bufer = myTransform.position - obstacles[i].transform.position;

        //        direction += bufer.normalized * (1 / bufer.magnitude) * obstacleSensivity * Time.deltaTime;
        //    }
        //}

        return direction;
    }

    private IEnumerator AttackCoroutine()
    {
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;
            myTransform.position -= myTransform.forward * moveSpeed * Time.deltaTime;
            yield return null;
        }

        t = 0;

        yield return new WaitForSeconds(1f);



        while (true)
        {
            myTransform.position += myTransform.forward * moveSpeed * attackSpeedMultiplicator * Time.deltaTime;
            if (Physics.SphereCast(myTransform.position, 3, myTransform.forward, out RaycastHit hit,
                stopAttackDistance, ~ignoreMask))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    if (hit.collider.TryGetComponent(out AliveController alive))
                    {
                        alive.GetDamage(damage);
                    }
                }
                break;
            }
            yield return null;
        }
        anim.SetBool("Attack", false);
        onAttack?.Invoke();
        while (t < 1)
        {
            t += Time.deltaTime;
            myTransform.position -= myTransform.forward * moveSpeed * Time.deltaTime;
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

    private IEnumerator ShockCoroutine(float shockTime)
    {
        shock = true;
        yield return new WaitForSeconds(shockTime);
        shock = false;
        GetNewBehaviour();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(drawHearThresholdDistance)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, hearThresholdDistance);
        }
        if (drawStopAttackDistance)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stopAttackDistance);
        }
        if (drawPursuitThresholdDistance)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, pursuitThresholdDistance);
        }
        if (drawTargetThresholdDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, targetThresholdDistance);
        }
    }
#endif
}
