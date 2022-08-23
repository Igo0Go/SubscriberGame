using UnityEngine;
using UnityEngine.Playables;

public class timelineActivator : MonoBehaviour
{
    [SerializeField] private PlayableDirector playable;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            playable.Play();
            Destroy(gameObject);
        }
    }
}
