using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Скрипт перемещения
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[HelpURL("https://docs.google.com/document/d/1llgWK3zJK7km7DMyi_GHh63LZUJngJppIuZIfccWwtc/edit?usp=sharing")]
public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField, Range(1, 10), Tooltip("Скорость перемещения")] private float speed = 5f;
    [SerializeField, Range(1, 50), Tooltip("Сила прыжка")] private float jumpForce = 15.0f;
    [SerializeField, Range(-40, -1)]
    [Tooltip("Ограничение скорости падения. Это требуется, чтобы персонаж," +
        "падающий с большой высоты не проникал сквозь текстуры.")]
    private float terminalVelocity = -10.0f;
    [SerializeField, Range(0.1f, 5), Tooltip("Сила притяжения. g=1 - земная гравитация")] private float gravity = 1f;
    [SerializeField] private Transform jumpCheck;
    [SerializeField] private LayerMask ignoreMask;

    [SerializeField]
    private UnityEvent useSound;

    private LocomotionType locomotionType;
    private Rigidbody rb;
    private Vector3 moveVector;
    private float vertSpeed;
    private bool fall;
    private float fallTimer;
    private readonly float minFall = -1.5f;

    /// <summary>
    /// Этот коэффициент используется, чтобы добиться ощущения "правильной" гравитации при gravity = 1.
    /// </summary>
    private const float gravMultiplayer = 9.8f * 5f;

    private Transform myTransform;
    private Collider transformFixator;

    private void Awake()
    {
        GameCenter.PlayerLocomotion = this;
    }

    private void Start()
    {
        myTransform = transform;
        GameCenter.OpportunityToMove = true;
        vertSpeed = minFall;
        rb = GetComponent<Rigidbody>();
        fall = true;
        locomotionType = LocomotionType.Default;
        ConsoleEventCenter.Teleport.Execute.AddListener(FastTeleportToPoint);
    }
    private void Update()
    {
        if (GameCenter.OpportunityToMove)
        {
            switch (locomotionType)
            {
                case LocomotionType.Default:
                    Jump();
                    PlayerMove();
                    break;
                case LocomotionType.Water:
                    PlayerSwim();
                    FallInTheWater();
                    break;
                default:
                    Jump();
                    PlayerMove();
                    break;
            }
        }
    }

    #region Блокировка и телепортация

    /// <summary>
    /// Плавно переместить игрока в точку (предварительно нужно заблокировать)
    /// </summary>
    /// <param name="point">Точка, куда нужно переместить и по которой нужно повернуть персонажа</param>
    public void SmoothMoveToPoint(Transform point)
    {
        StartCoroutine(SmoothMoveToPointCoroutine(point));
    }

    /// <summary>
    /// Двигать игрока в каком-то направлении
    /// </summary>
    /// <param name="direction">направление движение</param>
    public void SmoothMoveByDirection(Vector3 direction)
    {
        rb.MovePosition(direction);
    }

    /// <summary>
    /// Быстрая телепортация (без необходимости блокировать)
    /// </summary>
    /// <param name="point"></param>
    public void FastTeleportToPoint(Transform point)
    {
        myTransform.SetPositionAndRotation(point.position, point.rotation);
        rb.velocity = Vector3.zero;
    }
    public void FastTeleportToPoint(int x, int y, int z)
    {
        myTransform.position = new Vector3(x, y, z);
        rb.velocity = Vector3.zero;
    }

    public void SetLocomotionType(LocomotionType locType)
    {
        locomotionType = locType;
        switch (locomotionType)
        {
            case LocomotionType.Default:
                vertSpeed = jumpForce;
                break;
            case LocomotionType.Water:
                vertSpeed = terminalVelocity / 10;
                break;
            case LocomotionType.Empty:
                break;
            default:
                vertSpeed = jumpForce;
                break;
        }
    }

    #endregion

    private void Jump()
    {
        if (IsGrounded())
        {
            fallTimer = 0;
            fall = true;
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpForce;
            }
            else
            {
                vertSpeed = 0;
            }
        }
        else
        {
            if (fall)
            {
                vertSpeed -= gravity * gravMultiplayer * Time.deltaTime;
                if (vertSpeed < terminalVelocity)
                {
                    vertSpeed = terminalVelocity;
                }
            }
            else
            {
                fallTimer -= Time.deltaTime;
                if (fallTimer <= 0)
                {
                    fallTimer = 0;
                    fall = true;
                }
                vertSpeed = 0;
            }
        }
    }
    private void PlayerMove()
    {
        float deltaX = Input.GetAxisRaw("Horizontal");
        float deltaZ = Input.GetAxisRaw("Vertical");

        moveVector = myTransform.forward * deltaZ + myTransform.right * deltaX;
        moveVector.y = 0;
        moveVector = moveVector.normalized * speed;
        moveVector.y = vertSpeed;
        moveVector *= Time.deltaTime;
        myTransform.position += moveVector;
    }
    private void PlayerSwim()
    {
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");
        float deltaY = Input.GetAxis("UpDown");

        if(Vector3.Magnitude(new Vector3(deltaX, deltaY, deltaZ)) > 0.3f)
        {
            useSound?.Invoke();
        }


        if (deltaY > 0)
            vertSpeed = 0;

        moveVector = myTransform.forward * deltaZ + myTransform.right * deltaX;
        moveVector.y = 0;
        moveVector = moveVector.normalized * speed;
        moveVector.y = deltaY * speed;
        moveVector *= Time.deltaTime;
        myTransform.position += moveVector + Time.deltaTime * vertSpeed * Vector3.up;
    }
    private void FallInTheWater()
    {
        if (moveVector.magnitude <= 0.1f)
        {
            vertSpeed = SmoothChangeToTarget(vertSpeed, -1);
        }
        else
        {
            vertSpeed = SmoothChangeToTarget(vertSpeed, 0);
        }
    }

    private bool IsGrounded()
    {
        Collider[] bufer = Physics.OverlapBox(jumpCheck.position, jumpCheck.localScale, Quaternion.identity, ~ignoreMask);

        if (bufer != null && bufer.Length > 0)
            return true;
        else
            return false;
    }

    private float SmoothChangeToTarget(float value, float target)
    {
        if(Mathf.Abs(target - value) > Time.deltaTime * 3f)
        {
            if (value < target)
            {
                value += Time.deltaTime;
            }
            else
            {
                value -= Time.deltaTime;
            }
        }
        else
        {
            value = target;
        }

        return value;
    }
    private IEnumerator SmoothMoveToPointCoroutine(Transform point)
    {
        float t = 0;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (t < 1)
        {
            t += Time.deltaTime * 2;
            myTransform.SetPositionAndRotation(Vector3.Lerp(startPos, point.position, t), Quaternion.Lerp(startRot, point.rotation, t));
            yield return null;
        }
        myTransform.SetPositionAndRotation(point.position, point.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(TagHolder.TransfromFixator))
        {
            transformFixator = other;
            myTransform.parent = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == transformFixator)
        {
            myTransform.parent = null;
        }
    }
}

public enum LocomotionType
{
    Default,
    Water,
    Empty
}
