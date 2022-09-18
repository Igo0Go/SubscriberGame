using UnityEngine;

public class MusicMixer : LogicModule
{
    [SerializeField] private AudioClip newMusic;
    [SerializeField] private bool useMetronome = false;
    [SerializeField] private bool hardChange = false;
    [SerializeField] private bool once;

    public override void ActivateModule()
    {
        MixMusic();
    }

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
                StopAllCoroutines();
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
