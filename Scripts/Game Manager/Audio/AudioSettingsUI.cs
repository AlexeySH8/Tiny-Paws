using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    private const string MUSIC_VOLUME = "musicVolume";
    private const string SFX_VOLUME = "sfxVolume";

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _soundSlider;

    void Start()
    {
        LoadVolume();
    }

    public void SetMusicVolume()
    {
        float volume = _musicSlider.value;
        _mixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = _soundSlider.value;
        _mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

    private void LoadVolume()
    {
        _musicSlider.value = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.5f);
        _soundSlider.value = PlayerPrefs.GetFloat(SFX_VOLUME,0.5f);
        SetMusicVolume();
        SetSFXVolume();
    }
}
