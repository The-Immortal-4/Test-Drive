using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTaken : MonoBehaviour
{
    public GameObject minusScore;

    private void OnTriggerEnter(Collider other)
    {
        //int score = -100;
        FindObjectOfType<ScoringSystem>().DecreaseScore(100);
        //int score = -100;
        
    }
}
