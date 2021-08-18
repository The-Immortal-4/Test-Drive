using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadestarianHitDetection : MonoBehaviour
{
    [SerializeField] private int m_DecreaseScore = 40;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("AIPedestrian"))
        {
            other.gameObject.GetComponent<CharacterNavigationController>().Die();
            ScoreDecreasePopUp();
        }
    }

    private void ScoreDecreasePopUp()
    {
         FindObjectOfType<ScoringSystem>().DecreaseScore(m_DecreaseScore);
         FindObjectOfType<RoadRulePopUpUI>().RoadRulePopUp("You Hit a Pedestrian!", Color.red);
         //Decrease Score
    }
}
