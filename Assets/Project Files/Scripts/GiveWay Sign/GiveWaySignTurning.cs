using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveWaySignTurning : MonoBehaviour
{
    [SerializeField] private bool isRightTurning = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isRightTurning)
            {
                //Left Turning
                GetComponentInParent<GiveWaySign>().LeftTurningComplete(other.gameObject);
            }
            else
            {
                //Right Turning
                GetComponentInParent<GiveWaySign>().RightTurningComplete(other.gameObject);
            }
        }
    }
}
