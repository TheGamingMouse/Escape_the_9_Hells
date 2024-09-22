using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static SettingsManager Instance;

    [Header("Ints")]
    public int screenMode;

    [Header("Floats")]
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    [Header("Bools")]
    bool started;

    [Header("Arrays")]
    Resolution[] resolutions;

    [Header("TMP_Texts")]
    TMP_Text masterVolumeText;
    TMP_Text musicVolumeText;
    TMP_Text sfxVolumeText;

    [Header("Sliders")]
    Slider masterVolumeSlider;
    Slider musicVolumeSlider;
    Slider sfxVolumeSlider;

    [Header("Dropdowns")]
    TMP_Dropdown fullscreenModeDropdown;
    TMP_Dropdown resolutionDropdown;

    [Header("Components")]
    public AudioMixer audioMixer;

    #endregion

    #region StartUpdate Methods

    void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        FindElements();

        var settingsData = SaveSystem.loadedSettingsData;

        masterVolume = settingsData.masterVolume;
        musicVolume = settingsData.musicVolume;
        sfxVolume = settingsData.sfxVolume;
        screenMode = settingsData.screenMode;

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new();

        int currentResolution = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolution = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolution;
        resolutionDropdown.RefreshShownValue();

        SetResolution(currentResolution);
        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
        SetFullscreenMode(screenMode);
        
        started = true;
    }

    #endregion

    #region General Methods

    void FindElements()
    {
        var settingsMenu = GameObject.FindWithTag("Canvas").transform.Find("Menus/PauseMenu/SettingsMenu/Scroll Rect/View/Contents");

        var texts = settingsMenu.Find("Texts");
        masterVolumeText = texts.Find("MasterVolume/MasterText").GetComponent<TextMeshProUGUI>();
        musicVolumeText = texts.Find("MusicVolume/MusicText").GetComponent<TextMeshProUGUI>();
        sfxVolumeText = texts.Find("SFXVolume/SFXText").GetComponent<TextMeshProUGUI>();

        var sliders = settingsMenu.Find("Sliders");
        masterVolumeSlider = sliders.Find("MasterVolume").GetComponent<Slider>();
        musicVolumeSlider = sliders.Find("MusicVolume").GetComponent<Slider>();
        sfxVolumeSlider = sliders.Find("SFXVolume").GetComponent<Slider>();

        var dropdowns = settingsMenu.Find("Dropdowns");
        fullscreenModeDropdown = dropdowns.Find("FullscreenMode").GetComponent<TMP_Dropdown>();
        resolutionDropdown = dropdowns.Find("Resolution").GetComponent<TMP_Dropdown>();
    }

    #endregion

    #region Slider Methods

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);

        float volumePercent = (volume + 30) * 2;
        masterVolumeText.text = $"{volumePercent}%";
        
        masterVolume = volume;

        if (!started)
        {
            masterVolumeSlider.value = volume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);

        float volumePercent = (volume + 30) * 2;
        musicVolumeText.text = $"{volumePercent}%";
        
        musicVolume = volume;

        if (!started)
        {
            musicVolumeSlider.value = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);

        float volumePercent = (volume + 30) * 2;
        sfxVolumeText.text = $"{volumePercent}%";
        
        sfxVolume = volume;

        if (!started)
        {
            sfxVolumeSlider.value = volume;
        }
    }

    #endregion

    #region Dropdown Methods

    public void SetFullscreenMode(int mode)
    {
        switch (mode)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                screenMode = 0;
                break;
            
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                screenMode = 1;
                break;
            
            case 2:
                if (SystemInfo.operatingSystem.Contains("Windows"))
                {
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                }
                else if (SystemInfo.operatingSystem.Contains("Mac"))
                {
                    Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                }
                screenMode = 2;
                break;
        }

        if (!started)
        {
            fullscreenModeDropdown.value = screenMode;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    #endregion
}
