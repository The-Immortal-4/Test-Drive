using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GiveWaySign : MonoBehaviour
{
    [SerializeField] private int m_Score = 50;
    [SerializeField] private int m_DecreaseScore = 30;

    private bool inRange = false;
    private bool reset = false;
    public bool ruleCompleted = false;

    private bool isLeftIndicated;
    private bool isRightIndicated;

    private bool isRecordingTurning = false;

    CarChecker carChecker;
    Coroutine TurningCoroutine;

    private RoadRulePopUpUI roadRulePopUpUI;
    private ScoringSystem scoringSystem;

    private void Start()
    {
        roadRulePopUpUI = FindObjectOfType<RoadRulePopUpUI>();
        scoringSystem = FindObjectOfType<ScoringSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange)
        {
            if (carChecker != null)
            {
                if (carChecker.isClear)
                {
                    if (!reset)
                    {
                        if (roadRulePopUpUI != null)
                        {
                            roadRulePopUpUI.RoadRulePopUpInstance("Road Is Clear", Color.green);
                        }
                        ruleCompleted = true;
                        reset = true;
                    }               
                }
                else
                {
                    if (reset)
                    {
                        if (roadRulePopUpUI != null)
                        {
                            roadRulePopUpUI.RoadRulePopUpInstance("Incoming Traffic", Color.red);
                        }
                        ruleCompleted = false;
                        reset = false;
                    } 
                }
            }
        }
    }

    public void LeftTurningComplete(GameObject player)
    {
        if (isRecordingTurning)
        {
            if (ruleCompleted)
            {
                if (isLeftIndicated && isRecordingTurning)
                {
                    if (roadRulePopUpUI != null && scoringSystem != null)
                    {
                        scoringSystem.AddScore(m_Score);
                        roadRulePopUpUI.RoadRulePopUp("Giveaway Sign Followed.", Color.green);
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
            else
            {
                roadRulePopUpUI.RoadRulePopUp("Failed to follow Giveaway Sign rule.", Color.red);
            }
        }
    }

    public void RightTurningComplete(GameObject player)
    {
        if (isRecordingTurning)
        {
            if (ruleCompleted)
            {
                if (isRightIndicated && isRecordingTurning)
                {
                    if (roadRulePopUpUI != null && scoringSystem != null)
                    {
                        scoringSystem.AddScore(m_Score);
                        roadRulePopUpUI.RoadRulePopUp("Giveaway Sign Followed.", Color.green);
                    }
                }
                else if (!isRightIndicated && isRecordingTurning)
                {
                    if (roadRulePopUpUI != null && scoringSystem !=null)
                    {
                        scoringSystem.DecreaseScore(m_DecreaseScore);
                        roadRulePopUpUI.RoadRulePopUp("Failed to Indicate!", Color.red);
                    }
                }
            }
            else
            {
                if (roadRulePopUpUI != null)
                {
                    roadRulePopUpUI.RoadRulePopUp("Failed to follow Giveaway Sign rule.", Color.red);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = true;
            isRecordingTurning = true;
            UpdateIndicatorInfo(other.gameObject);
            if (TurningCoroutine != null)
            {
                StopCoroutine(TurningCoroutine);
            }
            TurningCoroutine = StartCoroutine(ResetTurningRecord());
            carChecker = GetComponentInChildren<CarChecker>();
            if (carChecker.isClear)
            {
                if (roadRulePopUpUI != null)
                {
                    roadRulePopUpUI.RoadRulePopUpInstance("Road Is Clear", Color.green);
                }
            }
            else
            {
                if (roadRulePopUpUI != null)
                {
                    roadRulePopUpUI.RoadRulePopUpInstance("Incoming Traffic", Color.red);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inRange = false;
            if (roadRulePopUpUI != null)
            {
                roadRulePopUpUI.RoadRulePopUpInstanceStop();
            }
        }
    }
}
