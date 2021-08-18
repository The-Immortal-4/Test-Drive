using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrow : MonoBehaviour
{
    [SerializeField] private Transform NavigationGroup;
    bool modelDisabled = false;
    private NavigationPoint currentNavPoint;

    public float distanceToNavPointBuffer = 15;
    private bool levelCompleted = false;

    // Update is called once per frame
    private void Update()
    {
        if (NavigationGroup != null)
        {
            if (currentNavPoint == null)
            {
                SelectNavigationPoint();
            }
            else if (currentNavPoint.isReached)
            {
                SelectNavigationPoint();
            }
            else
            {
                EnableModel();
                Vector3 targetPos = currentNavPoint.transform.position;
                targetPos.y = transform.position.y;
                if (Vector3.Distance(transform.position, targetPos) > distanceToNavPointBuffer)
                {
                    transform.LookAt(targetPos);
                }
                
            }
        }
        else
        {
            DisableModel();
        }
    }

    private void SelectNavigationPoint()
    {
        for(int i = 0; i < NavigationGroup.childCount; i++)
        {
            NavigationPoint navPoint = NavigationGroup.transform.GetChild(i).GetComponent<NavigationPoint>();
            if (!navPoint.isReached)
            {
                currentNavPoint = navPoint;
                return;
            }
            if (i == NavigationGroup.childCount - 1)
            {
                DisableModel();
            }
        }
    }

    private void EnableModel()
    {
        if(modelDisabled == true)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void DisableModel()
    {
        if (modelDisabled == false)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            //StartCoroutine(Message());
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        if (!levelCompleted)
        {
            FindObjectOfType<LevelManager>().LevelCompleted();
        }
    }
}
