using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    Resolution[] resolutions;
    int resolutionLength = 0;
    List<string> options = new List<string>();

    private void Awake()
    {
        StartCoroutine(UpdateInformationFromDatabase());
    }

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionLength = resolutions.Length;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        for (int i = resolutions.Length - 1; i >= 0; i--)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = resolutionLength - 1 - i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        //Save Resolution
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        saveManager.resolutionSettingsIndex = currentResolutionIndex;
        saveManager.Save();
    }

    public void SetResolution (int resolutionIndex)
    {
        int Index = resolutionLength - 1 - resolutionIndex;
        Resolution resolution = resolutions[Index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);

        //Save Quality
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        saveManager.qualitySettingsIndex = qualityIndex;
        saveManager.Save();
    }

    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private IEnumerator UpdateInformationFromDatabase()
    {
        yield return new WaitForSeconds(0.1f);
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        SetResolution(saveManager.resolutionSettingsIndex);
        SetQuality(saveManager.qualitySettingsIndex);

        //Update Dropdowns
        resolutionDropdown.value = saveManager.resolutionSettingsIndex;
        resolutionDropdown.RefreshShownValue();
        qualityDropdown.value = saveManager.qualitySettingsIndex;
        qualityDropdown.RefreshShownValue();
    }
}
