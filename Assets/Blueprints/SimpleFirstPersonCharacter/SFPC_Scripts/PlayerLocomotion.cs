using System.Collections;
using UnityEngine;

/// <summary>
/// Скрипт перемещения
/// </summary>
[RequireComponent(typeof(CharacterController))]
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

    private CharacterController charController;
    private Vector3 moveVector;
    private float vertSpeed;
    private bool fall;
    private float fallTimer;
    private bool opportunityToMove;
    private float minFall = -1.5f;

    /// <summary>
    /// Этот коэффициент используется, чтобы добиться ощущения "правильной" гравитации при gravity = 1.
    /// </summary>
    private const float gravMultiplayer = 9.8f * 5f;

    private Transform myTransform;
    private Collider transformFixator;

    private void Start()
    {
        myTransform = transform;
        opportunityToMove = true;
        vertSpeed = minFall;
        charController = GetComponent<CharacterController>();
        fall = true;
    }
    void Update()
    {
        if(opportunityToMove)
        {
            Jump();
            PlayerMove();
        }
    }

    #region Блокировка и телепортация

    /// <summary>
    /// Запретить игроку перемещаться
    /// </summary>
    public void SetLocomotionOpportunity(bool value)
    {
        opportunityToMove = charController.enabled = value;
    }

    /// <summary>
    /// Плавно переместить игрока в точку (предварительно нужно заблокировать)
    /// </summary>
    /// <param name="point">Точка, куда нужно переместить и по которой нужно повернуть персонажа</param>
    public void SmoothMoveToPoint(Transform point)
    {
        StartCoroutine(SmoothMoveToPointCoroutine(point));
    }


    /// <summary>
    /// Задаёт значение enabled для characterController (требуется для телепортации)
    /// </summary>
    /// <param name="value">целевое состояние</param>
    public void SetBlockValueToPlayer(bool value)
    {
        charController.enabled = !value;
    }

    /// <summary>
    /// Телепортировать игрока в точку (предварительно нужно заблокировать)
    /// </summary>
    /// <param name="point">Целевое положение</param>
    public void TeleportToPoint(Transform point)
    {
        myTransform.position = point.position;
        myTransform.rotation = point.rotation;
    }

    /// <summary>
    /// Быстрая телепортация (без необходимости блокировтаь)
    /// </summary>
    /// <param name="point"></param>
    public void FastTeleportToPoint(Transform point)
    {
        charController.enabled = false;
        myTransform.position = point.position;
        myTransform.rotation = point.rotation;
        charController.enabled = true;
    }

    #endregion

    private void Jump()
    {
        if (charController.isGrounded)
        {
            fallTimer = 0;
            fall = true;
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpForce;
            }
            else
            {
                vertSpeed = minFall;
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
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");

        moveVector = myTransform.forward * deltaZ + myTransform.right * deltaX;
        moveVector.y = 0;
        moveVector = moveVector.normalized * speed;
        moveVector.y = vertSpeed;
        moveVector *= Time.deltaTime;
        charController.Move(moveVector);
    }

    private IEnumerator SmoothMoveToPointCoroutine(Transform point)
    {
        float t = 0;
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        while (t < 1)
        {
            t += Time.deltaTime * 2;
            transform.position = Vector3.Lerp(startPos, point.position, t);
            transform.rotation = Quaternion.Lerp(startRot, point.rotation, t);
            yield return null;
        }
        transform.position = point.position;
        transform.rotation = point.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("TransformFixator"))
        {
            transformFixator = other;
            myTransform.parent = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other == transformFixator)
        {
            myTransform.parent = null;
        }
    }
}
