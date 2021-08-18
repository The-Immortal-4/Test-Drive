using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aa : MonoBehaviour
{
    public uint  numma = 1;


    private void Update()
    {

        Debug.Log(numma);

        if (Input.GetButtonDown("Escape"))
        {
            numma -= 10;
        }

    }

    private void Start()
    {
        
    }



}
