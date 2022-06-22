using UnityEngine;
using UnityEngine.Events;

public class SoundOrigin : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<Transform> SoundLaunched;

    private Transform myTransform;

    private void Awake()
    {
        myTransform = transform;
        SoundLaunched = new UnityEvent<Transform>();
    }

    public void StartSound()
    {
        SoundLaunched?.Invoke(myTransform);
    }
}