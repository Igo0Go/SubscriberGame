using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Submarine : MonoBehaviour
{
    [SerializeField]
    private Vector3 engineForce = Vector3.one;

    [SerializeField]
    private Vector3 sensivity = Vector3.one;

    [SerializeField]
    [Range(0.01f,1)]
    private float stabilizatorForce = 0.5f;

    private Rigidbody rb;
    private Vector3 moveVector;
    private Vector3 rotVector;
    private Transform myTransform;

    private float t;
    private const float multiplicator = 100;

    void Start()
    {
        myTransform = transform;
        rb = GetComponent<Rigidbody>();
        GameTools.opportunityToView = true;
        GameTools.SetCursorVisible(false);
    }

    void Update()
    {
        Move();
        ReadRotateInput();
    }
    private void LateUpdate()
    {
        Rotate();
    }

    private void ReadRotateInput()
    {
        Vector3 bufer;

        bufer.x = -Input.GetAxis("Mouse Y") * sensivity.x;
        bufer.y = Input.GetAxis("Mouse X") * sensivity.y;
        bufer.z = -Input.GetAxis("ZRot") * sensivity.z;

        if (bufer == Vector3.zero)
        {
            if (t < 0)
            {
                t = 0;
            }
            t += Time.deltaTime * stabilizatorForce;
            rotVector = Vector3.Lerp(rotVector, bufer, t);
        }
        else
        {
            t = -1;
            rotVector = Vector3.Lerp(rotVector, bufer, 1 - stabilizatorForce);
        }
    }

    private void Move()
    {
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");
        float deltaY = Input.GetAxis("UpDown");

        moveVector = new Vector3(deltaX, deltaY, deltaZ);

        moveVector.Normalize();
        moveVector = moveVector.Multiplicate(engineForce) * Time.deltaTime;

        rb.AddRelativeForce(moveVector, ForceMode.Impulse);
    }

    private void Rotate()
    {
        rb.MoveRotation(myTransform.rotation * Quaternion.Euler(rotVector));
    }
}
