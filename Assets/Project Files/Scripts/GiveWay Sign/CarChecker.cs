using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarChecker : MonoBehaviour
{
    public bool isClear = true;
    private int numberOfCars = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AICar")
        {
            numberOfCars += 1;

            if (numberOfCars < 1)
            {
                isClear = true;

            }
            else
            {
                isClear = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "AICar")
        {
            numberOfCars -= 1;

            if (numberOfCars < 1)
            {
                isClear = true;

            }
            else
            {
                isClear = false;
            }
        }
    }
}
