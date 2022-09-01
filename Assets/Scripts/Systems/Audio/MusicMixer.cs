using UnityEngine;

public class MusicMixer : MonoBehaviour
{
    [SerializeField] private AudioClip newMusic;
    [SerializeField] private bool useMetronome = false;
    [SerializeField] private bool hardChange = false;
    [SerializeField] private bool once;

    public void MixMusic()
    {
        if (hardChange)
        {
            if(useMetronome)
            {
                AudioPack.MusicSystem.SetMainClipAsMetronome(newMusic);
            }
            else
            {
                AudioPack.MusicSystem.SetMainClip(newMusic);
            }

        }
        else
        {
            AudioPack.MusicSystem.MixMainClip(newMusic, useMetronome);
        }

        if (once)
            Destroy(gameObject);
    }
}
