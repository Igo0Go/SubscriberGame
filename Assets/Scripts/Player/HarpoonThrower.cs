using System.Collections;
using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(LineRenderer))]
public class HarpoonThrower : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private Transform harpoon;
    [SerializeField]
    private Transform harpoonPoint;
    [SerializeField, Min(1)]
    private float harponMoveSpeed;
    [SerializeField, Min(100)]
    private float harponLength = 100;
    [SerializeField, Min(1)]
    private float electroRadius = 1;
    [SerializeField]
    private LayerMask ignoreMask;
    [SerializeField]
    private PlayerLocomotion playerLocomotion;
    [SerializeField]
    private UnityEvent UseHarpoon;

    [SerializeField]
    private Gradient defaultHarpoonColor;
    [SerializeField]
    private Gradient electroHarpoonColor;

    private bool UseShock
    { 
        get
        {
            return _useShock;
        }
        set
        {
            _useShock = value;
            lineRenderer.colorGradient = _useShock? electroHarpoonColor : defaultHarpoonColor;
        }
    }
    private bool _useShock = false;

    private bool drawLine;
    private Vector3 oldPos;
    private LineRenderer lineRenderer;

    private Action shootInput;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        UseShock = false;
        shootInput = LaunchHarpoon;
    }

    void Update()
    {
        shootInput?.Invoke();
    }

    private void LateUpdate()
    {
        DrawLine();
    }

    private void DrawLine()
    {
        if(drawLine)
        {
            lineRenderer.SetPosition(0, harpoonPoint.position);
            lineRenderer.SetPosition(1, harpoon.position);
        }
    }

    private void LaunchHarpoon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 direction = playerCamera.transform.forward * harponLength;

            if (Physics.Raycast(playerCamera.transform.position, direction.normalized, out RaycastHit hit, direction.magnitude,
                ~ignoreMask))
            {
                direction = (hit.point - harpoon.position).normalized;
            }
            else
            {
                direction = (playerCamera.transform.position + direction - harpoon.position).normalized;
            }

            harpoon.parent = null;
            harpoon.forward = direction;
            oldPos = harpoon.position;
            shootInput = MoveHarpoon;
            drawLine = true;
            UseHarpoon?.Invoke();
        }
    }

    private void MoveHarpoon()
    {
        harpoon.position += harpoon.forward * harponMoveSpeed * Time.deltaTime;
        if(Physics.Linecast(oldPos, harpoon.position, out RaycastHit hit, ~ignoreMask))
        {
            if(hit.collider.CompareTag("InteractiveObject"))
            {
                hit.transform.parent = harpoon;
                shootInput = UseHarpoonAsGrabTool;
                return;
            }
            harpoon.parent = hit.transform;
            shootInput = UseHarpoonAsPoint;
            return;
        }
        oldPos = harpoon.position;

        if((harpoonPoint.position - harpoon.position).magnitude > harponLength)
        {
            shootInput = ReturnGarpoon;
        }

        if(Input.GetMouseButton(1))
        {
            ReturnGarpoon();
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            ReturnGarpoon();
        }

        UseElectroImput();
    }

    private void UseHarpoonAsPoint()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerLocomotion.SetLocomotionOpportunity(false);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            playerLocomotion.SetLocomotionOpportunity(true);
        }

        if (Input.GetMouseButton(1))
        {
            UseHarpoon?.Invoke();
            Vector3 direction = harpoon.position - playerLocomotion.transform.position;

            playerLocomotion.SmoothMoveByDirection(direction.normalized * harponMoveSpeed * Time.deltaTime);

            if(direction.magnitude <= 1.5f)
            {
                playerLocomotion.SetLocomotionOpportunityAndCharacterController(true);

                harpoon.position = harpoonPoint.position;
                harpoon.forward = harpoonPoint.forward;
                harpoon.parent = harpoonPoint;
                harpoon.localScale = Vector3.one;
                lineRenderer.SetPosition(0, harpoonPoint.position);
                lineRenderer.SetPosition(1, harpoonPoint.position);
                drawLine = false;
                shootInput = LaunchHarpoon;
            }
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            ReturnGarpoon();
        }

        UseElectroImput();
    }

    private void UseHarpoonAsGrabTool()
    {
        if (Input.GetMouseButton(1))
        {
            ReturnGarpoon();
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            ReturnGarpoon();
        }

        UseElectroImput();
    }

    private void UseElectroImput()
    {
        if (Input.GetMouseButtonDown(0) && !UseShock)
        {
            StartCoroutine(ElectroCoroutine(2));
        }
    }

    private void ReturnGarpoon()
    {
        UseHarpoon?.Invoke();
        Vector3 Direction = harpoonPoint.position - harpoon.position;

        harpoon.position += Direction.normalized * harponMoveSpeed * 2 * Time.deltaTime;
        harpoon.forward = -Direction.normalized;

        if (Direction.magnitude <= 1.5f)
        {
            harpoon.position = harpoonPoint.position;
            harpoon.forward = harpoonPoint.forward;
            harpoon.parent = harpoonPoint;
            lineRenderer.SetPosition(0, harpoonPoint.position);
            lineRenderer.SetPosition(1, harpoonPoint.position);
            drawLine = false;
            shootInput = LaunchHarpoon;
        }
    }

    private IEnumerator ElectroCoroutine(float delayTime)
    {
        UseShock = true;
        var items = Physics.OverlapSphere(harpoon.position, electroRadius, ~ignoreMask);
        foreach (var item in items)
        {
            if (item.TryGetComponent(out AliveController controller))
            {
                UseHarpoon?.Invoke();
                controller.GetDamage(0);
            }
        }
        yield return new WaitForSeconds(delayTime);
        UseShock = false;
    }
}
