﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour{

    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void Start() {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> settings = new List<string>();
        int currentResolutionIndex = 0;

        for(int i = 0; i< resolutions.Length; i++) {
            string setting = resolutions[i].width + " x " + resolutions[i].height;
            settings.Add(setting);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(settings);
    }

    public void SetVolume (float volume) {
        audioMixer.SetFloat("volume", volume);
        Debug.Log(volume);
    }

    public void SetQuality (int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }
    
}
