using UnityEngine;

public class EffectsAudioSystem : MonoBehaviour, IGameSystem
{
    [SerializeField]
    private AudioSource shotSource;
    [SerializeField]
    private AudioSource loopSource;

    private void Awake()
    {
        AudioPack.EffectsAudioSystem = this;
    }

    public void SetUp()
    {

    }

    public void UseItem(AudioEffectItem item)
    {
        switch (item.effectType)
        {
            case AudioEffectType.playOneShot:
                shotSource.PlayOneShot(item.clip);
                break;
            case AudioEffectType.loop:
                Clear();
                loopSource.clip = item.clip;
                loopSource.Play();
                break;
            case AudioEffectType.clear:
                Clear();
                break;
            default:
                Clear();
                break;
        }
    }

    private void Clear()
    {
        if(loopSource.isPlaying)
            loopSource.Stop();
    }
}

[System.Serializable]
public class AudioEffectItem
{
    public AudioClip clip;
    public AudioEffectType effectType;
}

public enum AudioEffectType
{
    playOneShot,
    loop,
    clear
}