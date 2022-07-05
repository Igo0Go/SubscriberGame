using System.Collections.Generic;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private List<AudioClip> clips;

    public void StopMusicAndPlayClipByIndex(int index)
    {
        musicAudioSource.Stop();
        musicAudioSource.PlayOneShot(clips[index]);
    }

    public void PlayMusicAgain()
    {
        musicAudioSource.Play();
    }
}
