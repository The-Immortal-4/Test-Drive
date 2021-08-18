using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeSecEndPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Three Sec End line
            if (other.gameObject.GetComponent<VehicleDetection>().faulted)
            {
                //Rule not followed
                FindObjectOfType<TutorialLevelManager>().ResetPlayer();
            }
            else
            {
                //Rule followed
                FindObjectOfType<TutorialLevelManager>().TutorialLevelCompleted();
            }
        }
    }
}
