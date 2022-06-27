using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private GameObject decal;

    private int damage;
    private float speed;
    private float lifeTime;
    private LayerMask ignoreMask;

    private Transform myTransform;
    private Vector3 oldPosition;

    public void LaunchBullet(float bulletSpeed, float bulletLifeTime, LayerMask layerMask, int damage, Vector3 direction)
    {
        this.damage = damage;
        speed = bulletSpeed;
        lifeTime = bulletLifeTime;
        ignoreMask = layerMask;

        myTransform = transform;
        oldPosition = myTransform.position;

        myTransform.forward = direction;

        Destroy(gameObject, lifeTime);

        StartCoroutine(MoveBulletCoroutine());
    }

    private IEnumerator MoveBulletCoroutine()
    {
        while (true)
        {
            Move();
            yield return null;
        }
    }

    protected virtual void Move()
    {
        myTransform.position += myTransform.forward * speed * Time.deltaTime;
        CheckHit();
        oldPosition = myTransform.position;
    }

    private void CheckHit()
    {
        if(Physics.Linecast(oldPosition, myTransform.position, out RaycastHit hit, ~ignoreMask))
        {
            Hit(hit);
            Destroy(gameObject);
        }
    }

    protected virtual void Hit(RaycastHit hit)
    {
        Transform decalTransform = Instantiate(decal, hit.point, Quaternion.identity).transform;
        decalTransform.forward = hit.normal;

        if(hit.collider.TryGetComponent(out AliveController alive))
        {
            alive.GetDamage(damage);
        }
    }
}
