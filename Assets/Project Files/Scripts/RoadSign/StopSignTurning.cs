using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSignTurning : MonoBehaviour
{
    [SerializeField] private bool isRightTurning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isRightTurning)
            {
                //Left Turning
                GetComponentInParent<StopSign>().LeftTurningComplete(other.gameObject);
            }
            else
            {
                //Right Turning
                GetComponentInParent<StopSign>().RightTurningComplete(other.gameObject);
            }
        }
    }
}
