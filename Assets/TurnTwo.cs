using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTwo : MonoBehaviour
{
    [SerializeField] private TurnOne turnone;

    public bool isLeftTurning;

    [SerializeField] private StopSign stopSign;
    [SerializeField] private GiveWaySign giveWaySign;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (isLeftTurning)
            {
                LeftTurningComplete(other.gameObject);
            }
            else
            {
                RightTurningComplete(other.gameObject);
            }
        }
    }

    public void LeftTurningComplete(GameObject player)
    {
         TutorialLevelManager tutorialLevelManager = FindObjectOfType<TutorialLevelManager>();
         if(tutorialLevelManager.currentGame == TutorialLevelManager.MiniGame.indicatorMini)
         {
              if (turnone.isLeftIndicated && turnone.isRecordingTurning)
              {
                   FindObjectOfType<TutorialLevelManager>().TutorialLevelCompleted();
              }
              else if (!turnone.isLeftIndicated && turnone.isRecordingTurning)
              {
                   FindObjectOfType<TutorialLevelManager>().ResetPlayer();
              }
         }
        else if (tutorialLevelManager.currentGame == TutorialLevelManager.MiniGame.stopSignMini)
        {
            if (turnone.isLeftIndicated && turnone.isRecordingTurning)
            {
                if (stopSign != null) 
                { 
                    if (stopSign.ruleCompleted)
                    {
                        //Stop Sign completed
                        FindObjectOfType<TutorialLevelManager>().TutorialLevelCompleted();
                    }
                }
            }
            else if (!turnone.isLeftIndicated && turnone.isRecordingTurning)
            {
                FindObjectOfType<TutorialLevelManager>().ResetPlayer();
            }
        }
        else if (tutorialLevelManager.currentGame == TutorialLevelManager.MiniGame.giveWayMini)
        {
            if (turnone.isLeftIndicated && turnone.isRecordingTurning)
            {
                if (giveWaySign != null)
                {
                    if (giveWaySign.ruleCompleted)
                    {
                        FindObjectOfType<TutorialLevelManager>().TutorialLevelCompleted();
                    }
                }
            }
            else if (!turnone.isLeftIndicated && turnone.isRecordingTurning)
            {
                FindObjectOfType<TutorialLevelManager>().ResetPlayer();
            }
        }
        else if (tutorialLevelManager.currentGame == TutorialLevelManager.MiniGame.threeSecRuleMini)
        {
            if (turnone.isLeftIndicated && turnone.isRecordingTurning)
            {
                FindObjectOfType<TutorialLevelManager>().TutorialLevelCompleted();
            }
            else if (!turnone.isLeftIndicated && turnone.isRecordingTurning)
            {
                FindObjectOfType<TutorialLevelManager>().ResetPlayer();
            }
        }


    }

    public void RightTurningComplete(GameObject player)
    {
            if (turnone.isRightIndicated && turnone.isRecordingTurning)
            {
            FindObjectOfType<TutorialLevelManager>().TutorialLevelCompleted();
            }
            else if (!turnone.isRightIndicated && turnone.isRecordingTurning)
            {
                FindObjectOfType<TutorialLevelManager>().ResetPlayer();
            }
    } 
}
