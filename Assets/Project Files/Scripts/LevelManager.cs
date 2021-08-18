using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int m_LevelNo = 1;
    [SerializeField] private GameState gameManager;

    [SerializeField] private int OneStarThreshold;
    [SerializeField] private int TwoStarThreshold;
    [SerializeField] private int ThreeStarThreshold;

    [SerializeField] private Image Star1;
    [SerializeField] private Image Star2;
    [SerializeField] private Image Star3;

    public void LevelCompleted()
    {
        StartCoroutine(LevelCompletedDelay());
    }

    IEnumerator LevelCompletedDelay()
    {
        float delayTime = 3f;
        UpdateStars();
        //Adding Delay to Level Completed menu pop up
        yield return new WaitForSeconds(delayTime);
        gameManager.EnableGameWonMenu();
        Time.timeScale = 0f;
        //Pause game
    }

    private void UpdateStars()
    {
        int currentScore = FindObjectOfType<ScoringSystem>().currentScore;

        float fadedAlphaValue = 1f;
        if (currentScore >= ThreeStarThreshold)
        {
            ChangeImageAlpha(Star1, 1f, true);
            ChangeImageAlpha(Star2, 1f, true);
            ChangeImageAlpha(Star3, 1f, true);
            SaveLevelStars(3);
            //3 Stars gained
        }
        else if (currentScore >= TwoStarThreshold)
        {
            ChangeImageAlpha(Star1, 1f, true);
            ChangeImageAlpha(Star2, 1f, true);
            ChangeImageAlpha(Star3, fadedAlphaValue, false);
            SaveLevelStars(2);
            //2 Stars gained
        }
        else if (currentScore >= OneStarThreshold)
        {
            ChangeImageAlpha(Star1, 1f, true);
            ChangeImageAlpha(Star2, fadedAlphaValue, false);
            ChangeImageAlpha(Star3, fadedAlphaValue, false);
            SaveLevelStars(1);
            //1 Stars gained
        }
        else
        {
            ChangeImageAlpha(Star1, fadedAlphaValue, false);
            ChangeImageAlpha(Star2, fadedAlphaValue, false);
            ChangeImageAlpha(Star3, fadedAlphaValue, false);
            SaveLevelStars(0);
            //0 Stars gained
        }
    }

    private void SaveLevelStars(int stars)
    {
        switch (m_LevelNo)
        {
            case 1:
                FindObjectOfType<SaveManager>().level1Star = stars;
                FindObjectOfType<SaveManager>().Save();
                break;
            case 2:
                FindObjectOfType<SaveManager>().level2Star = stars;
                FindObjectOfType<SaveManager>().Save();
                break;
            case 3:
                FindObjectOfType<SaveManager>().level3Star = stars;
                FindObjectOfType<SaveManager>().Save();
                break;
        }
    }

    private void ChangeImageAlpha(Image img, float alpha, bool state)
    {
        if (state)
        {
            Color col;
            ColorUtility.TryParseHtmlString("#E5B115", out col);
            col.a = alpha;
            img.color = col;
            //Achieved Stars
        }
        else
        {
            Color col;
            ColorUtility.TryParseHtmlString("#144C9C", out col);
            col.a = alpha;
            img.color = col;
            //Unachieved Stars
        }
    }

}
