using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Submarine : MonoBehaviour
{
    [SerializeField]
    [Min(0.1f)]
    private float engineForce = 1;

    [SerializeField, Range(0, 2)]
    private float sensivity = 0.5f;

    private Rigidbody rb;
    private Vector3 moveVector;
    private Vector3 rotVector;
    private Transform myTransform;

    private float rotationX;
    private float rotationY;
    private float rotationZ;
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
        ReadInput();
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void LateUpdate()
    {
        Rotate();
    }

    private void ReadInput()
    {
        float deltaX = Input.GetAxis("Horizontal");
        float deltaZ = Input.GetAxis("Vertical");
        float deltaY = Input.GetAxis("UpDown");

        moveVector = new Vector3(deltaX, deltaY, deltaZ);

        rotationX = Input.GetAxis("Mouse Y") * sensivity;
        rotationY = Input.GetAxis("Mouse X") * sensivity; 
        rotationZ = Input.GetAxis("ZRot") * sensivity;

        Vector3 bufer = new Vector3(-rotationX, rotationY, -rotationZ);

        if(bufer == Vector3.zero)
        {
            if(t > 1)
            {
                t = 0;
            }

            rotVector = Vector3.Lerp(rotVector, Vector3.zero, t);

            t += Time.deltaTime;
        }
        else
        {
            t = 2;
            rotVector = bufer * multiplicator * Time.deltaTime;
        }
    }

    private void Move()
    {
        moveVector = myTransform.right * moveVector.x +
            myTransform.up * moveVector.y +
            myTransform.forward * moveVector.z;
        moveVector *= engineForce;

        rb.AddForce(moveVector);
    }

    private void Rotate()
    {
        myTransform.rotation *= Quaternion.Euler(rotVector);
    }
}
