using UnityEngine;


public class AudioModeSystem : MonoBehaviour
{
    [SerializeField]
    private AudioSystemDatabase audioSystemDatabase;

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
        foreach (var item in audioSystemDatabase.audioModes)
        {
            if(item.modeType == type)
            {
                return item;
            }
        }
        return null;
    }
}
