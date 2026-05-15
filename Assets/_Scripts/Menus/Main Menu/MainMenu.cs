using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider sfxSlider;

    public TMP_Dropdown graphicsDropdown;
    public int graphicQuality;

    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;

    public Slider brightnessSlider;
    public float brightnessSliderValue;
    public Image brightnessPanel;

    void Start()
    {
        //sounds
        LoadVolume();
        MusicManager.Instance.PlayMusic("MainMenu");

        //graphics
        graphicQuality = PlayerPrefs.GetInt("qualityNumber", 3);
        graphicsDropdown.value = graphicQuality;
        SetQuality();

        //resolution
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //brightness
        brightnessSlider.value = PlayerPrefs.GetFloat("brightness", 0.5f);
        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, brightnessSlider.value);
    }

    public void Play()
    {
        LevelManager.Instance.LoadScene("Game", "CrossFade");
        MusicManager.Instance.PlayMusic("Boss");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void UpdateMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audioMixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        audioMixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void SetQuality()
    {
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
        PlayerPrefs.SetInt("qualityNumber", graphicsDropdown.value);
        graphicQuality = graphicsDropdown.value;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetBrightness(float brightnessValue)
    {
        brightnessSliderValue = brightnessValue;
        PlayerPrefs.SetFloat("brightness", brightnessSliderValue);
        brightnessPanel.color = new Color(brightnessPanel.color.r, brightnessPanel.color.g, brightnessPanel.color.b, brightnessSlider.value);
    }
}
