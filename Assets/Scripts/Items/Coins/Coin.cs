using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip itemClip;
    [SerializeField] private int price = 10;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider nonTrigger;

    private Vector3 Direction => target == null ? Vector3.zero : target.position - myTransform.position + Vector3.up;

    private Transform myTransform;
    private Transform target;

    private void Start()
    {
        myTransform = transform;
        myTransform.parent = null;
        if (rb != null)
        {
            rb.AddForce(new Vector3(Random.Range(0, 1f), 5, Random.Range(0, 1f)), ForceMode.Impulse);
        }
        else
        {
            LevelProggress.maxCoinsOnLevel += price;
            UIPack.LevelProgressPanel.UpdateCoins();
        }
        StartCoroutine(RotateCoroutine());
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        if(rb != null)
        {
            Destroy(rb);
            Destroy(nonTrigger);
        }
        StopAllCoroutines();
        StartCoroutine(MoveToTargetCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            myTransform.Rotate(Vector3.up, speed * 5 * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator MoveToTargetCoroutine()
    {
        while (Direction.magnitude > 0.1f)
        {
            myTransform.position += speed * Time.deltaTime * Direction.normalized;
            yield return null;
        }

        StreamerPack.CoinsCounter.AddCoins(price, itemClip);
        Destroy(gameObject);
    }
}
