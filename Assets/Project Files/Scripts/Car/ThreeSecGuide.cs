using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeSecGuide : MonoBehaviour
{
    [SerializeField] private LineRenderer rightLineRenderer;
    [SerializeField] private float lineOneThresshold = 1f;
    [SerializeField] private float lineTwoThresshold = 2f;
    [SerializeField] private float lineThreeThresshold = 3f;

    [SerializeField] private GameObject LineOne;
    [SerializeField] private GameObject LineTwo;
    [SerializeField] private GameObject LineThree;

    private void Update()
    {
        float lineZValue = rightLineRenderer.GetPosition(1).z;
        if (lineZValue > lineOneThresshold)
        {
            //Enable
            LineOne.SetActive(true);
        }
        else
        {
            //Disable
            LineOne.SetActive(false);
        }
        if (lineZValue > lineTwoThresshold)
        {
            //Enable
            LineTwo.SetActive(true);
        }
        else
        {
            //Disable
            LineTwo.SetActive(false);
        }
        if (lineZValue > lineThreeThresshold)
        {
            //Enable
            LineThree.SetActive(true);
        }
        else
        {
            //Disable
            LineThree.SetActive(false);
        }
    }
}
