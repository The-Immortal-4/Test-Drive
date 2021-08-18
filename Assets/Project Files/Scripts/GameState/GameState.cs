using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameState : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip buttonClickSFX;

    public GameObject pausePanel;
    public GameObject levelCompletedPanel;
    public GameObject controlspanel;

    public GameObject[] deactivatePanels;

    public bool controlOn =false;
    private bool checkControl;
    private bool checkInstructor;

    [SerializeField] private TextMeshProUGUI levelCompScoreTxt;

    public enum States {Running, IsPause, GameWon, GameOver};
    public States currentState;


    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateState(States.Running);
        Time.timeScale = 1f;
    }


    public void UpdateState(States state)
    {
        currentState = state;
    }

    public void Update()
    {

        if (Input.GetButtonDown("Escape"))
        {
            //Enable Pause menu only while game is running
            if (currentState == States.Running)
            {
                PauseMenuOn();
                return;
            }
            //Disable Pause menu only while game is Paused
            if (currentState == States.IsPause)
            {
                PauseMenuOff();
                return;
            }       
        }

        if (Input.GetButtonDown("Control"))
        {
            //Enable Control buton only while game is running
            if (currentState == States.Running)
            {
                ToggleControl();
            }
        }

    }

    //toggles controls on ui
    private void ToggleControl()
    {
        controlOn = !controlOn;
        controlspanel.SetActive(controlOn);
    }

    private void PauseMenuOn()
    {
        foreach (GameObject a in deactivatePanels)
        {
            if(a.gameObject.name == "Controls")
            {
                if (a.activeSelf)
                {
                    a.SetActive(false);
                    checkControl = true;
                }
                else
                {
                    
                }
            }
            else if (a.gameObject.name == "Driver Instructor")
            {
                if (a.activeSelf)
                {
                    a.SetActive(false);
                    checkInstructor = true;
                }
                else
                {

                }
            }
            else
            {
                if (a.activeSelf)
                {
                    a.SetActive(false);
                }
            }
            
        }

        currentState = States.IsPause;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void PauseMenuOff()
    {
        foreach (GameObject a in deactivatePanels)
        {
            if (a.gameObject.name == "Controls")
            {
                if (checkControl)
                {
                    a.SetActive(true);
                    checkControl = false;
                }
                else
                {
                    
                }
            }
            else if (a.gameObject.name == "Driver Instructor")
            {
                if (checkInstructor)
                {
                    a.SetActive(true);
                    checkInstructor = false;
                }
                else
                {

                }
            }
            else
            {
                if (!a.activeSelf)
                {
                    a.SetActive(true);
                }
            }
        }

        currentState = States.Running;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void EnableGameWonMenu()
    {
        UpdateState(States.GameWon);
        levelCompletedPanel.SetActive(true);

        //Update Level Completed Score
        int cScore = FindObjectOfType<ScoringSystem>().currentScore;
        levelCompScoreTxt.text = cScore.ToString();
    }

    //Resume button in options panel
    public void ResumeButton()
    {
        audioSource.PlayOneShot(buttonClickSFX);
        PauseMenuOff();
    }

    public void ContinueBtnClick()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextScene >= SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);
            //If there is no next scene load main menu
        }
        else
        {
            SceneManager.LoadScene(nextScene);
            //Load next Scene
        }
    }

    public void MenuBtnClick()
    {
        SceneManager.LoadScene(0);
    }

    //Initiates Scene
    public void SceneSelector(string scene)
    {
        audioSource.PlayOneShot(buttonClickSFX); ;
        SceneManager.LoadScene(scene);
    }
}

