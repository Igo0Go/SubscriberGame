using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioSystemData", menuName = "IgoGo/AudioSystemDatabase")]
public class AudioSystemDatabase : ScriptableObject
{
    public List<AudioMode> audioModes;
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
