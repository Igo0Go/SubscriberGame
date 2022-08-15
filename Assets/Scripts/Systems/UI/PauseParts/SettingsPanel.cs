using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Slider voiceSlider;
    [SerializeField] private Slider soundsSlider;
    [SerializeField] private Slider musicSlider;

    [SerializeField, Space(10)] private Toggle drawSubsToggle;

    private void Awake()
    {
        voiceSlider.value = Settings.VoiceVolume;
        voiceSlider.onValueChanged.AddListener(OnChangeVoice);

        soundsSlider.value = Settings.SoundsVolume;
        soundsSlider.onValueChanged.AddListener(OnChangeSounds);

        musicSlider.value = Settings.MusicVolume;
        musicSlider.onValueChanged.AddListener(OnChangeMusic);

        drawSubsToggle.isOn = Settings.UseSubs;
        drawSubsToggle.onValueChanged = new Toggle.ToggleEvent();
        drawSubsToggle.onValueChanged.AddListener(OnDrawSubsChanged);
    }

    private void OnDrawSubsChanged(bool value)
    {
        PlayerPack.ReplicSystem.OnDrawSubsChanged(value);
    }

    private void OnChangeVoice(float value)
    {
        Settings.VoiceVolume = value;
    }
    private void OnChangeSounds(float value)
    {
        Settings.SoundsVolume = value;
    }
    private void OnChangeMusic(float value)
    {
        Settings.MusicVolume = value;
    }
}
