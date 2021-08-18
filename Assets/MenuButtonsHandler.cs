using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MenuButtonsHandler : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSFX;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject starShopPanel;
    [SerializeField] private GameObject controlsPage;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //Exits game
    public void Exit()
    {
        audioSource.PlayOneShot(buttonClickSFX);
        Application.Quit();
    }

    //Initiates Scene
    public void SceneSelector(string scene)
    {
        audioSource.PlayOneShot(buttonClickSFX); ;
        SceneManager.LoadScene(scene);
    }

    //Play Button
    public void PlayBtnClick()
    {
        audioSource.PlayOneShot(buttonClickSFX);
        mainMenuPanel.SetActive(false);
        levelsPanel.SetActive(true);
    }

    //Star Shop Button
    public void StarShopBtnClick()
    {
        audioSource.PlayOneShot(buttonClickSFX);
        mainMenuPanel.SetActive(false);
        starShopPanel.SetActive(true);
    }

    //Star Shop  back Button
    public void StarShopBackBtnClick()
    {
        audioSource.PlayOneShot(buttonClickSFX);
        mainMenuPanel.SetActive(true);
        starShopPanel.SetActive(false);
    }

    //Settings Button
    public void SettingsBtnClick()
    {
        audioSource.PlayOneShot(buttonClickSFX);
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    //Settings Back Button
    public void SettingsBackBtnClick()
    {
        audioSource.PlayOneShot(buttonClickSFX);
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    //Level selector Back Button
    public void LevelsPanelBackBtnClick()
    {
        audioSource.PlayOneShot(buttonClickSFX);
        mainMenuPanel.SetActive(true);
        levelsPanel.SetActive(false);
    }

    // KeyBinding Settings 
    public void ControlsPageBtnClick()
    {
        audioSource.PlayOneShot(buttonClickSFX);
        controlsPage.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void ControlsPageBackBtnClick()
    {
        audioSource.PlayOneShot(buttonClickSFX);
        controlsPage.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void PlayButtonClickSound()
    {
        audioSource.PlayOneShot(buttonClickSFX);
    }
}
