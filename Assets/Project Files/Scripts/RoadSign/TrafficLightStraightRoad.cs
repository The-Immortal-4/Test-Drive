using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightStraightRoad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
             GetComponentInParent<TrafficLight>().StraightRoadComplete(other.gameObject);
        }
    }
}
