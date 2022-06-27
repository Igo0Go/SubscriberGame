using UnityEngine;
using UnityEngine.Events;

public class Minigun : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransfrom;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform rotor;
    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private float rotorRotateSpeed = 1;
    [SerializeField, Min(0.1f)]
    private float shootDelay = 5;

    [Min(0.1f), SerializeField]
    private float bulletSpeed = 1;
    [Min(0.1f), SerializeField]
    private float bulletLifeTime = 1;
    [SerializeField]
    private LayerMask ignoreMask;
    [SerializeField, Min(1)]
    private int damage = 1;

    [SerializeField]
    private UnityEvent OnShot;

    private float currentRotateSpeed;
    private float currentDelay;

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if(currentRotateSpeed < rotorRotateSpeed)
            {
                currentRotateSpeed += Time.deltaTime * 2;
            }

            currentDelay += currentRotateSpeed;
            if (currentDelay >= shootDelay)
            {
                currentDelay = 0;
                Vector3 direction = cameraTransfrom.position + cameraTransfrom.forward * 100;
                if (Physics.Raycast(cameraTransfrom.position, cameraTransfrom.forward, out RaycastHit hit, 100, ~ignoreMask))
                    direction = hit.point - shootPoint.position;
                else
                    direction = direction - shootPoint.position;

                muzzleFlash.Play();
                Instantiate(bullet, shootPoint.position, shootPoint.rotation).GetComponent<Bullet>().
                    LaunchBullet(bulletSpeed, bulletLifeTime, ignoreMask, damage, direction);
                OnShot.Invoke();
            }
        }
        else
        {
            if (currentRotateSpeed > 0)
            {
                currentRotateSpeed -= Time.deltaTime;
            }
        }

        rotor.Rotate(rotor.forward, currentRotateSpeed, Space.World);
    }
}
