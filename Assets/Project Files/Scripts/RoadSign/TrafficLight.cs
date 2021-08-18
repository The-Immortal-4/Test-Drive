using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (BoxCollider))]
public class TrafficLight : MonoBehaviour
{
    [SerializeField] private int m_Score = 50;
    [SerializeField] private int m_DecreaseScore = 30;

    [SerializeField] private GameObject RedLightObject;
    [SerializeField] private GameObject YellowLightObject;
    [SerializeField] private GameObject GreenLightObject;

    private bool isRedLight = false;
    private bool isYellowLight = false;
    private bool isGreenLight = false;

    private bool isLeftIndicated;
    private bool isRightIndicated;

    private bool isRecordingTurning = false;
    private bool ruleCompleted = false;

    private RoadRulePopUpUI roadRulePopUpUI;
    private ScoringSystem scoringSystem;

    private void Start()
    {
        roadRulePopUpUI = FindObjectOfType<RoadRulePopUpUI>();
        scoringSystem = FindObjectOfType<ScoringSystem>();
    }

    private void CheckAllLights()
    {
        isRedLight = RedLightObject.activeSelf;
        isYellowLight = YellowLightObject.activeSelf;
        isGreenLight = GreenLightObject.activeSelf;
    }

    IEnumerator ResetTurningRecord()
    {
        yield return new WaitForSeconds(10f);
        isRecordingTurning = false;
    }

    IEnumerator ResetRuleCompleted()
    {
        yield return new WaitForSeconds(7f);
        ruleCompleted = false;
    }

    private void UpdateIndicatorInfo(GameObject indicator)
    {
        RightLeftIndicators indi = indicator.GetComponent<RightLeftIndicators>();
        isLeftIndicated = indi.leftOn;
        isRightIndicated = indi.rightOn;
    }

    public void LeftTurningComplete(GameObject player)
    {
        if (ruleCompleted)
        {
            if (isLeftIndicated && isRecordingTurning)
            {
                if (scoringSystem != null && roadRulePopUpUI != null)
                {
                    scoringSystem.AddScore(m_Score);
                    roadRulePopUpUI.RoadRulePopUp("Traffic light rule Followed.", Color.green);
                }
            }
            else if (!isLeftIndicated && isRecordingTurning)
            {
                if (scoringSystem != null && roadRulePopUpUI != null)
                {
                    scoringSystem.DecreaseScore(m_DecreaseScore);
                    roadRulePopUpUI.RoadRulePopUp("Traffic light rule not Followed!", Color.red);
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
                if (scoringSystem != null && roadRulePopUpUI != null)
                {
                    scoringSystem.AddScore(m_Score);
                    roadRulePopUpUI.RoadRulePopUp("Traffic light rule Followed.", Color.green);
                }
            }
            else if (!isRightIndicated && isRecordingTurning)
            {
                if (scoringSystem != null && roadRulePopUpUI != null)
                {
                    scoringSystem.DecreaseScore(m_DecreaseScore);
                    roadRulePopUpUI.RoadRulePopUp("Traffic light rule not Followed!", Color.red);
                }
            }
        }
    }

    public void StraightRoadComplete(GameObject player)
    {
        if (ruleCompleted && isRecordingTurning)
        {
            if (scoringSystem != null && roadRulePopUpUI != null)
            {
                scoringSystem.AddScore(m_Score);
                roadRulePopUpUI.RoadRulePopUp("Traffic light rule Followed.", Color.green);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CheckAllLights();
            UpdateIndicatorInfo(other.gameObject);
            isRecordingTurning = true;         
            StartCoroutine(ResetTurningRecord());
            StartCoroutine(ResetRuleCompleted());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isGreenLight)
            {
                ruleCompleted = true;
            }
            else if (isYellowLight)
            {
                ruleCompleted = true;
            }
            else
            {
                ruleCompleted = false;
                if (scoringSystem != null && roadRulePopUpUI != null)
                {
                    scoringSystem.DecreaseScore(m_DecreaseScore);
                    roadRulePopUpUI.RoadRulePopUp("Failed to follow traffic light rule!", Color.red);
                }
            }
        }
    }
}
