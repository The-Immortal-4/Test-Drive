using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoringSystem : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int currentScore;
    public AudioSource audioSource;
    //public uint score;

    private PopUpUI popUpUI;

    private void Start()
    {
        popUpUI = FindObjectOfType<PopUpUI>();
    }

    public void AddScore(int Score)
    {

        currentScore += Score;
        UpdateScoreText();
        if (popUpUI != null)
        {
            popUpUI.PopUp("+ " + Score, Color.green);
        }
    }

    public void DecreaseScore(int Score)
    {
        currentScore -= Score;
        if (currentScore < 0)
        {
            currentScore = 0;
            //Reset it to zero if less then zero
        }
        if (popUpUI != null)
        {
            popUpUI.PopUp("- " + Score, Color.red);
        }
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = currentScore.ToString();
    }

    public void PlayCoinSound(AudioClip Sound)
    {
        audioSource.PlayOneShot(Sound);
    }
    
}
