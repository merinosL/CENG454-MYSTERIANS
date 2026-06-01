using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public GameObject settingsMenuPanel;
    public Slider soundSlider;
    public Slider musicSlider;
    public AudioMixer audioMixer;

    void Start()
    {
        if (settingsMenuPanel != null)
            settingsMenuPanel.SetActive(false);

        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0f);

        musicSlider.value = musicVol;
        soundSlider.value = sfxVol;

        audioMixer.SetFloat("MusicVolume", musicVol);
        audioMixer.SetFloat("SFXVolume", sfxVol);

        soundSlider.onValueChanged.AddListener(SetSFX);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void OpenSettings()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        if (settingsMenuPanel != null) settingsMenuPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFX(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}