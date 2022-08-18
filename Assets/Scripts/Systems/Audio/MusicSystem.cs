using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MusicSystem : MonoBehaviour, IGameSystem
{
    [SerializeField] private AudioSource mainLoopAudioSource;
    [SerializeField] private AudioSource oneShotSource;
    [SerializeField] private List<AudioSource> musicLines;

    private readonly List<AudioClip> shotQueue = new List<AudioClip>();
    private readonly List<AudioLineItem> loopQueue = new List<AudioLineItem>();

    private const float changeVolumeMultiplicator = 0.5f;

    private void Awake()
    {
        AudioPack.MusicSystem = this;
    }

    public void SetUp()
    {
        SetVolumeForAll(Settings.MusicVolume);
        mainLoopAudioSource.loop = true;
        foreach (var item in musicLines)
        {
            item.loop = true;
        }
        Settings.MusicVolumeChanged.AddListener(SetVolumeForAll);
    }

    private IEnumerator MetronomeCoroutine()
    {
        AudioSource bufer;
        while (true)
        {
            if(!GameCenter.GlobalPause)
            {
                foreach (var clip in shotQueue)
                {
                    oneShotSource.PlayOneShot(clip);
                }
                shotQueue.Clear();

                bool restart = false;
                foreach (var item in loopQueue)
                {
                    restart = true;
                    mainLoopAudioSource.Stop();
                    bufer = musicLines[item.lineNumber - 1];
                    bufer.clip = item.clip;
                    if(item.mix)
                    {
                        bufer.volume = 0;
                        StartCoroutine(SlowGrowVolumeCoroutine(bufer));
                    }
                    else
                    {
                        bufer.volume = Settings.MusicVolume;
                    }
                    bufer.Play();
                }

                loopQueue.Clear();

                if(restart)
                    mainLoopAudioSource.Play();
            }
            yield return new WaitForSeconds(mainLoopAudioSource.clip.length);
        }
    }

    private IEnumerator MixMainClipCoroutine(AudioClip clip, bool useMetronome)
    {
        while (mainLoopAudioSource.volume > 0)
        {
            mainLoopAudioSource.volume -= Time.deltaTime * changeVolumeMultiplicator;
            SetVolumeForAll(mainLoopAudioSource.volume);
            yield return null;
        }
        if(useMetronome)
        {
            SetMainClipAsMetronome(clip);
        }
        else
        {
            SetMainClip(clip);
        }
        while (mainLoopAudioSource.volume < Settings.MusicVolume)
        {
            mainLoopAudioSource.volume += Time.deltaTime * changeVolumeMultiplicator;
            SetVolumeForAll(mainLoopAudioSource.volume);
            yield return null;
        }
    }

    private IEnumerator SlowGrowVolumeCoroutine(AudioSource source)
    {
        while (source.volume < Settings.MusicVolume)
        {
            source.volume += Time.deltaTime * changeVolumeMultiplicator;
            yield return null;
        }
    }

    public void StopAll()
    {
        StopAllCoroutines();
        for (int i = 1; i <= musicLines.Count; i++)
        {
            StopLine(i);
        }
        mainLoopAudioSource.Stop();

    }
    public void SetMainClipAsMetronome(AudioClip clip)
    {
        mainLoopAudioSource.clip = clip;
        mainLoopAudioSource.Play();
        StartCoroutine(MetronomeCoroutine());
    }
    public void SetMainClip(AudioClip clip)
    {
        StopAll();
        mainLoopAudioSource.clip = clip;
        mainLoopAudioSource.Play();
    }
    public void MixMainClip(AudioClip clip, bool useMetronome)
    {
        StopAllCoroutines();
        StartCoroutine(MixMainClipCoroutine(clip, useMetronome));
    }

    public void SetShotOnStartLoop(AudioClip clip)
    {
        shotQueue.Add(clip);
    }
    public void SetShotNow(AudioClip clip)
    {
        oneShotSource.PlayOneShot(clip);
    }

    public void AddAudioLineItem(AudioLineItem item)
    {
        loopQueue.Add(item);
    }
    public void StopLine(int number)
    {
        musicLines[number - 1].Stop();
    }
    public void SetPauseForAll(bool value)
    {
        if(value)
        {
            mainLoopAudioSource.Pause();
            foreach (var item in musicLines)
            {
                item.Pause();
            }
        }
        else
        {
            mainLoopAudioSource.UnPause();
            foreach (var item in musicLines)
            {
                item.UnPause();
            }
        }
    }

    private void SetVolumeForAll(float volume)
    {
        mainLoopAudioSource.volume = volume;
        oneShotSource.volume = volume;
        foreach (var item in musicLines)
        {
            item.volume = volume;
        }
    }
}

[System.Serializable]
public class AudioLineItem
{
    [Range(1,8)]
    public int lineNumber = 1;
    public bool mix = false;
    public AudioClip clip;
}
