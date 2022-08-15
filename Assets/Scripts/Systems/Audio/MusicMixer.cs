using UnityEngine;

public class MusicMixer : MonoBehaviour
{
    [SerializeField] private AudioClip newMusic;
    [SerializeField] private bool useMetronome = false;
    [SerializeField] private bool hardChange = false;

    public void MixMusic()
    {
        if (hardChange)
        {
            AudioPack.MusicSystem.SetMainClip(newMusic);
        }
        else
        {
            AudioPack.MusicSystem.MixMainClip(newMusic, useMetronome);
        }
    }
}
