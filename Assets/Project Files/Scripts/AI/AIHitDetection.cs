using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHitDetection : MonoBehaviour
{
    [SerializeField] private int m_DecreaseScore = 40;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("AIPedestrian"))
        {
            //other.gameObject.GetComponent<CharacterNavigationController>().Dead();
            //Apply Hit Damage to AI
            
        }
    }
}
