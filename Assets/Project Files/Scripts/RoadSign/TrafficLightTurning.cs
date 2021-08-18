using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightTurning : MonoBehaviour
{
    [SerializeField] private bool isRightTurning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isRightTurning)
            {
                //Left Turning
                GetComponentInParent<TrafficLight>().LeftTurningComplete(other.gameObject);
            }
            else
            {
                //Right Turning
                GetComponentInParent<TrafficLight>().RightTurningComplete(other.gameObject);
            }
        }
    }
}
