using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private AudioClip clipToPlay;
    [SerializeField] private AudioStarterMode mode;
    [Space(10)]

    [Header("Для линий")]
    [SerializeField, Range(1, 8)] private int targetLine;
    [SerializeField] private bool withMix;
    [SerializeField] private bool once;

    public void StartClip()
    {
        switch (mode)
        {
            case AudioStarterMode.ShotNow:
                AudioPack.MusicSystem.SetShotNow(clipToPlay);
                break;
            case AudioStarterMode.ShotMetronome:
                AudioPack.MusicSystem.SetShotOnStartLoop(clipToPlay);
                break;
            case AudioStarterMode.AddInLine:
                AudioPack.MusicSystem.AddAudioLineItem(new AudioLineItem() { clip = clipToPlay,
                                                            lineNumber = targetLine, mix = withMix });
                break;
            case AudioStarterMode.StopLine:
                AudioPack.MusicSystem.StopLine(targetLine);
                break;
            default:
                AudioPack.MusicSystem.SetShotNow(clipToPlay);
                break;
        }

        if(once)
            Destroy(gameObject);
    }
}

public enum AudioStarterMode
{
    ShotNow,
    ShotMetronome,
    AddInLine,
    StopLine
}
