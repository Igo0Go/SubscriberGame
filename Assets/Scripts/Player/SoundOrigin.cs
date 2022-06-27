using UnityEngine;
using UnityEngine.Events;

public class SoundOrigin : MonoBehaviour
{
    [Min(1)]
    public float soundDistance = 5;

    [HideInInspector]
    public UnityEvent<SoundOrigin> SoundLaunched;

    private void Awake()
    {
        SoundLaunched = new UnityEvent<SoundOrigin>();
    }

    public void StartSound()
    {
        SoundLaunched?.Invoke(this);
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, soundDistance);
    }
#endif
}