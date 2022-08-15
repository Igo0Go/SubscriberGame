using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class AudioVolumeChanger : MonoBehaviour
{
    private AudioSource source;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);

        source = GetComponent<AudioSource>();
        AudioPack.AudioSystem.AddNewAudiosource(source);
        source.volume = Settings.SoundsVolume;
        Settings.SoundsVolumeChanged.AddListener(OnChangeVolume);
    }

    private void OnChangeVolume(float value)
    {
        source.volume = value;
    }
}
