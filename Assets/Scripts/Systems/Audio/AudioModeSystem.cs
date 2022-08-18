using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioModeSystem : MonoBehaviour, IGameSystem
{
    [SerializeField]
    private AudioSystemDatabase audioSystemDatabase;

    [SerializeField]
    private AudioModeType startType = AudioModeType.Standard;

    private List<AudioSource> sources;

    private AudioMixerGroup mixerGroupBufer;

    private void Awake()
    {
        AudioPack.AudioSystem = this;
    }

    public void SetUp()
    {
        sources = new List<AudioSource>(FindObjectsOfType<AudioSource>());
        SetModeForAllSources(FindAudioModeOfType(startType));
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

    public void AddNewAudiosource(AudioSource source)
    {
        sources.Add(source);
        source.outputAudioMixerGroup = mixerGroupBufer;
    }

    private void SetModeForAllSources(AudioMode mode)
    {
        mixerGroupBufer = mode.mixer;
        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i] == null)
            {
                sources.RemoveAt(i);
                i--;
            }
            else
            {
                sources[i].outputAudioMixerGroup = mixerGroupBufer;
            }
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
