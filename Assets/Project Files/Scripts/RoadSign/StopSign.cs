using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StopSign : MonoBehaviour
{
    [SerializeField] private int m_Score = 50;
    [SerializeField] private int m_DecreaseScore = 30;

    bool isTiming = false;
    float currentTime;

    private bool isLeftIndicated;
    private bool isRightIndicated;

    private bool isRecordingTurning = false;
    public bool ruleCompleted = false;

    private Coroutine TurningCoroutine;
    private RoadRulePopUpUI roadRulePopUpUI;
    private ScoringSystem scoringSystem;

    private void Start()
    {
        roadRulePopUpUI = FindObjectOfType<RoadRulePopUpUI>();
        scoringSystem = FindObjectOfType<ScoringSystem>();
    }

    private void Update()
    {
        if (isTiming)
        {
            if (currentTime < 3.0f)
            {
                currentTime += Time.deltaTime;
                if (roadRulePopUpUI != null)
                {
                    roadRulePopUpUI.RoadRulePopUp(Mathf.Round(currentTime).ToString(), Color.green);
                }
            }
            else 
            {
                isTiming = false;
                ruleCompleted = true;
                if (roadRulePopUpUI != null)
                {
                    roadRulePopUpUI.RoadRulePopUpInstance("Go!", Color.green);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartTimer();
            UpdateIndicatorInfo(other.gameObject);
            isRecordingTurning = true;
            if (TurningCoroutine != null)
            {
                StopCoroutine(TurningCoroutine);
            }
            TurningCoroutine = StartCoroutine(ResetTurningRecord());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (roadRulePopUpUI != null)
            {
                roadRulePopUpUI.RoadRulePopUpInstanceStop();
            }
            isTiming = false;
            if (currentTime < 3.0f)
            {
                if (roadRulePopUpUI != null && scoringSystem != null)
                {
                    scoringSystem.DecreaseScore(m_DecreaseScore);
                    roadRulePopUpUI.RoadRulePopUp("Failed to follow rule!", Color.red);
                }
            }
            currentTime = 0;
        }
    }

    public void LeftTurningComplete(GameObject player)
    {
         if (ruleCompleted)
         {
            if (isLeftIndicated && isRecordingTurning)
            {
                if (roadRulePopUpUI != null && scoringSystem != null)
                {
                    scoringSystem.AddScore(m_Score);
                    roadRulePopUpUI.RoadRulePopUp("Stop Sign Followed!", Color.green);
                }
            }
            else if (!isLeftIndicated && isRecordingTurning)
            {
                if (roadRulePopUpUI != null && scoringSystem != null)
                {
                    scoringSystem.DecreaseScore(m_DecreaseScore);
                    roadRulePopUpUI.RoadRulePopUp("Failed to Indicate!", Color.red);
                }
            }
         }
    }

    public void RightTurningComplete(GameObject player)
    {
         if (ruleCompleted)
         {
               if (isRightIndicated && isRecordingTurning)
               {
                    if (roadRulePopUpUI != null && scoringSystem != null)
                    {
                         scoringSystem.AddScore(m_Score);
                         roadRulePopUpUI.RoadRulePopUp("Stop Sign Followed!", Color.green);
                    }
               }
               else if (!isRightIndicated && isRecordingTurning)
               {
                    if (roadRulePopUpUI != null && scoringSystem != null)
                    {
                         scoringSystem.DecreaseScore(m_DecreaseScore);
                         roadRulePopUpUI.RoadRulePopUp("Failed to Indicate!", Color.red);
                    }
               }
         }
    }

    IEnumerator ResetTurningRecord()
    {
        yield return new WaitForSeconds(10f);
        isRecordingTurning = false;
    }

    private void UpdateIndicatorInfo(GameObject indicator)
    {
        RightLeftIndicators indi = indicator.GetComponent<RightLeftIndicators>();
        isLeftIndicated = indi.leftOn;
        isRightIndicated = indi.rightOn;
    }

    private void StartTimer()
    {
        isTiming = true;
    }
}
