using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement; 

public class DisplaySettings : MonoBehaviour
{

    public TMP_Dropdown resolutionDropdown; 
    public Toggle fullscreenToggle; 


    private Resolution[] supportedResolutions = new Resolution[]
    {
        new Resolution { width = 1920, height = 1080, refreshRateRatio = new RefreshRate { numerator = 60, denominator = 1 } }, 
        new Resolution { width = 1366, height = 768, refreshRateRatio = new RefreshRate { numerator = 60, denominator = 1 } },
        new Resolution { width = 1280, height = 720, refreshRateRatio = new RefreshRate { numerator = 60, denominator = 1 } }
    };

    void Start()
    {
        int currentMonitorResIndex = 0;
        Resolution currentMonitorRes = Screen.currentResolution;
        for (int i = 0; i < supportedResolutions.Length; i++)
        {
            if (supportedResolutions[i].width == currentMonitorRes.width &&
                supportedResolutions[i].height == currentMonitorRes.height)
            {
                currentMonitorResIndex = i;
                break; 
            }
        }

        int savedResIndex = PlayerPrefs.GetInt("ResolutionIndex", currentMonitorResIndex);
        if (savedResIndex >= supportedResolutions.Length) 
            savedResIndex = 0;

        bool savedFullscreen = PlayerPrefs.GetInt("FullscreenMode", Screen.fullScreen ? 1 : 0) == 1;

        resolutionDropdown.value = savedResIndex;
        fullscreenToggle.isOn = savedFullscreen;

        ApplySettings(savedResIndex, savedFullscreen);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreenMode);
    }

    public void SetResolution(int index)
    {
        PlayerPrefs.SetInt("ResolutionIndex", index);
        ApplySettings(index, fullscreenToggle.isOn);
    }

    public void SetFullscreenMode(bool isFullscreen) 
    {
        PlayerPrefs.SetInt("FullscreenMode", isFullscreen ? 1 : 0);
        ApplySettings(resolutionDropdown.value, isFullscreen);
    }

    void ApplySettings(int resIndex, bool isFullscreen)
    {
        Resolution res = supportedResolutions[resIndex];     
        Screen.SetResolution(res.width, res.height, isFullscreen ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed);

      
        PlayerPrefs.Save();
    }
}