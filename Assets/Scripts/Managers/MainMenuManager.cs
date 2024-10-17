using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static MainMenuManager Instance;

    [Header("Ints")]
    public int screenMode;
    int currentSouls;
    int totalSouls;
    int totalLevels;
    int demonsKilled;
    int devilsKilled;

    [Header("Floats")]
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    [Header("Bools")]
    bool started;

    [Header("Strings")]
    string highestLayer;

    [Header("Arrays")]
    Resolution[] resolutions;

    [Header("TMP_Texts")]
    public TMP_Text masterVolumeText;
    public TMP_Text musicVolumeText;
    public TMP_Text sfxVolumeText;
    public TMP_Text highesteLayerText;
    public TMP_Text currentSoulsText;
    public TMP_Text totalSoulsText;
    public TMP_Text totalLevelsText;
    public TMP_Text demonsKilledText;
    public TMP_Text devilsKilledText;

    [Header("Buttons")]
    public Button newGameButton;
    public Button continueGameButton;

    [Header("Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Dropdowns")]
    public TMP_Dropdown fullscreenModeDropdown;
    public TMP_Dropdown resolutionDropdown;

    [Header("Components")]
    public AudioMixer audioMixer;

    #endregion

    #region StartUpdate Methods

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        var settingsData = SaveSystem.loadedSettingsData;
        var playerData = SaveSystem.loadedPlayerData;

        if (playerData.newGame)
        {
            continueGameButton.interactable = false;
        }

        masterVolume = settingsData.masterVolume;
        musicVolume = settingsData.musicVolume;
        sfxVolume = settingsData.sfxVolume;
        screenMode = settingsData.screenMode;

        currentSouls = playerData.currentSouls;
        totalSouls = playerData.totalSouls;
        totalLevels = playerData.totalLevels;
        demonsKilled = playerData.demonsKilled;
        devilsKilled = playerData.devilsKilled;

        CalculateLayerReached(SaveSystem.loadedLayerData.highestLayerReached);

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

        UpdateStatText();

        started = true;
    }

    #endregion

    #region General Methods

    public void NewGameButton()
    {
        DisableButtons();
        SaveSystem.Instance.StartNewGame();
        SceneManager.LoadScene("Hub");
    }

    public void ContinueButton()
    {
        DisableButtons();
        SceneManager.LoadSceneAsync("Hub");
    }

    void DisableButtons()
    {
        newGameButton.interactable = false;
        continueGameButton.interactable = false;
    }

    void CalculateLayerReached(int layer)
    {
        switch (layer)
        {
            case 0:
                highestLayer = "Hub...";
                break;
            
            case 1:
                highestLayer = "9th layer";
                break;
            
            case 2:
                highestLayer = "8th layer";
                break;
            
            case 3:
                highestLayer = "7th layer";
                break;
            
            case 4:
                highestLayer = "6th layer";
                break;
            
            case 5:
                highestLayer = "5th layer";
                break;
            
            case 6:
                highestLayer = "4th layer";
                break;
            
            case 7:
                highestLayer = "3rd layer";
                break;
            
            case 8:
                highestLayer = "2nd layer";
                break;
            
            case 9:
                highestLayer = "1st layer";
                break;
            
            case 10:
                highestLayer = "Void";
                break;
            
            case 11:
                highestLayer = "Gates";
                break;
        }
    }

    #endregion

    #region Slider Methods

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);

        float volumePercent = (volume + 30) * 2;
        masterVolumeText.text = $"{volumePercent}%";
        
        masterVolume = volume;

        var settingsData = SaveSystem.loadedSettingsData;
        settingsData.masterVolume = volume;

        SaveSystem.Instance.Save(settingsData, SaveSystem.settingsDataPath);

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

        var settingsData = SaveSystem.loadedSettingsData;
        settingsData.musicVolume = volume;

        SaveSystem.Instance.Save(settingsData, SaveSystem.settingsDataPath);

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

        var settingsData = SaveSystem.loadedSettingsData;
        settingsData.sfxVolume = volume;

        SaveSystem.Instance.Save(settingsData, SaveSystem.settingsDataPath);

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

        var settingsData = SaveSystem.loadedSettingsData;
        settingsData.screenMode = mode;

        SaveSystem.Instance.Save(settingsData, SaveSystem.settingsDataPath);

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

    #region UpdateText Methods

    void UpdateStatText()
    {
        highesteLayerText.text = $"Your highest reached layer is the {highestLayer}";
        
        if (currentSouls != 1)
        {
            currentSoulsText.text = $"You currently have {currentSouls} souls in your collection";
        }
        else
        {
            currentSoulsText.text = $"You currently have {currentSouls} soul in your collection";
        }
        
        if (totalSouls != 1)
        {
            totalSoulsText.text = $"You have collected {totalSouls} souls";
        }
        else
        {
            totalSoulsText.text = $"You have collected {totalSouls} soul";
        }

        if (totalLevels != 1)
        {
            totalLevelsText.text = $"You have gained {totalLevels} levels";
        }
        else
        {
            totalLevelsText.text = $"You have gained {totalLevels} level";
        }

        if (demonsKilled != 1)
        {
            demonsKilledText.text = $"You have killed {demonsKilled} demons";
        }
        else
        {
            demonsKilledText.text = $"You have killed {demonsKilled} demon";
        }

        if (devilsKilled != 1)
        {
            devilsKilledText.text = $"You have killed {devilsKilled} devils";
        }
        else
        {
            devilsKilledText.text = $"You have killed {devilsKilled} devil";
        }
    }

    #endregion
}
