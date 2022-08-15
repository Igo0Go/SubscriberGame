using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    [SerializeField]
    private List<AudioMode> audioModes;

    [SerializeField]
    private AudioModeType startType = AudioModeType.Standard;

    private AudioSource[] sources;

    private void Start()
    {
        sources = FindObjectsOfType<AudioSource>();
        SetModeForAllSources(FindAudioModeOfType(startType));
        AudioPack.AudioSystem = this;
    }

    public void SetWaterAudioMode()
    {
        AudioMode mode = FindAudioModeOfType(AudioModeType.Water);
        SetModeForAllSources(mode);
    }
    public void SetStandardAudioMode()
    {
        AudioMode mode = FindAudioModeOfType(AudioModeType.Standard);
        SetModeForAllSources(mode);
    }

    private void SetModeForAllSources(AudioMode mode)
    {
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].outputAudioMixerGroup = mode.mixer;
        }
    }
    private AudioMode FindAudioModeOfType(AudioModeType type)
    {
        foreach (var item in audioModes)
        {
            if(item.modeType == type)
            {
                return item;
            }
        }
        return null;
    }
}

[System.Serializable]
public class AudioMode
{
    public AudioModeType modeType;
    public AudioMixerGroup mixer;
}

public enum AudioModeType
{
    Standard,
    Water
}
