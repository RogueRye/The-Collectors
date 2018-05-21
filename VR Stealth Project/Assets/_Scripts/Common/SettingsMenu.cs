using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/*
 * When you apply this script to your game perform the following:
 * 
 * Project > PlayerSettings > Display Resolution Diaglog (disabled)
 * 
 * */


public class SettingsMenu : MonoBehaviour {

    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    Dropdown resolutionDropdown;
    Resolution[] resolutions;
 
    private void Start()
    {
        SetupResolutionDropDown();
    }

    //Attatch to slider OnValueChange event
    //The slider needs to go from -80 to 0
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume); // need to create audio mixer parameter named "volume"
    }

    //Set to a dropdown containing LOW - MEDIUM - HIGH in that order
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); 
    }

    //Set to a Toggle UI object
    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void SetupResolutionDropDown()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if(resolutions[i].height == Screen.currentResolution.height && 
                resolutions[i].width == Screen.currentResolution.width)
            {
                currentResolutionIndex = i; 
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

}
