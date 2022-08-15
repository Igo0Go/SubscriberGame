using UnityEngine;

public class MusicStarter : MonoBehaviour
{
    [SerializeField] private AudioClip clipToPlay;
    [SerializeField] private AudioStarterMode mode;
    [SerializeField, Range(1, 8)] private int targetLine;

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
                AudioPack.MusicSystem.AddAudioLineItem(new AudioLineItem() { clip = clipToPlay, lineNumber = targetLine });
                break;
            case AudioStarterMode.StopLine:
                AudioPack.MusicSystem.StopLine(targetLine);
                break;
            default:
                AudioPack.MusicSystem.SetShotNow(clipToPlay);
                break;
        }
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
