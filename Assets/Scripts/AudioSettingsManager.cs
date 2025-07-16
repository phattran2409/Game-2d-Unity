using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;

    void Start()
    {
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);

        float volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        masterVolumeSlider.value = volume;
        SetMasterVolume(volume);
    }

    public void OnMasterVolumeChanged(float volume)
    {
        SetMasterVolume(volume);

        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    private void SetMasterVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MusicVolume", dB);
        audioMixer.SetFloat("SFXVolume", dB);
    }
}
