using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransfrom;
    [SerializeField]
    private GameObject rocket;
    [SerializeField, Min(0.1f)]
    private float delay = 1;
    [SerializeField, Min(0)]
    private int currentRocketCount = 6;


    [SerializeField]
    private Transform shootPoint;
    [SerializeField]
    private ParticleSystem muzzleFlash;

    [Min(0.1f), SerializeField]
    private float bulletSpeed = 1;
    [Min(0.1f), SerializeField]
    private float bulletLifeTime = 1;
    [SerializeField]
    private LayerMask ignoreMask;
    [SerializeField, Min(1)]
    private int damage = 1;

    [SerializeField]
    private Text rocketCountText;

    [SerializeField]
    private UnityEvent OnShot;

    private Bullet currentBullet;
    private float currentDelay;

    void Start()
    {
        rocketCountText.text = currentRocketCount.ToString();
        currentDelay = 0;
        Reload();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1) && currentDelay == delay)
        {
            currentDelay = 0;
            currentRocketCount--;
            rocketCountText.text = currentRocketCount.ToString();
            Vector3 direction = cameraTransfrom.position + cameraTransfrom.forward * 100;
            if (Physics.Raycast(cameraTransfrom.position, cameraTransfrom.forward, out RaycastHit hit, 100, ~ignoreMask))
                direction = hit.point - shootPoint.position;
            else
                direction = direction - shootPoint.position;

            muzzleFlash.Play();
            currentBullet.transform.parent = null;
            currentBullet.LaunchBullet(bulletSpeed, bulletLifeTime, ignoreMask, damage, direction);
            OnShot.Invoke();

            Reload();
        }
    }

    private void Reload()
    {
        if (currentRocketCount > 0)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private IEnumerator ReloadCoroutine()
    {
        currentBullet = Instantiate(rocket, shootPoint.position - shootPoint.forward, 
            shootPoint.rotation, shootPoint).GetComponent<Bullet>();

        while(currentDelay < delay)
        {
            currentDelay += Time.deltaTime;

            currentBullet.transform.position = Vector3.Lerp(shootPoint.position - shootPoint.forward, shootPoint.position,
                currentDelay / delay);

            yield return null;
        }
        currentDelay = delay;
    }
}
