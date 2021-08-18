using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeSecStartPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Start Recording Three Sec rule
        }
    }
}
