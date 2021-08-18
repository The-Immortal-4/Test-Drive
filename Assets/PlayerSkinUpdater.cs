using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinUpdater : MonoBehaviour
{
    [SerializeField] private GameObject[] DefaultWheelMesh;

    [SerializeField] private GameObject StandardCar;
    [SerializeField] private GameObject SuperCar;
    [SerializeField] private GameObject HoverCar;

    private void Start()
    {
        UpdateCarSkin();
    }

    private void UpdateCarSkin()
    {
        SaveManager saveMan = FindObjectOfType<SaveManager>();
        if (saveMan.hoverCarActivated)
        {
            //Activate Hover Car
            HoverCar.SetActive(true);
            SuperCar.SetActive(false);
            StandardCar.SetActive(false);

            DisableWheelMesh();
            //Disable Default wheel
        }
        else if (saveMan.superCarActivated)
        {
            //Activate Super Car
            HoverCar.SetActive(false);
            SuperCar.SetActive(true);
            StandardCar.SetActive(false);

            EnableWheelMesh();
            //Enable Default wheel
        }
        else
        {
            //Activate Standard Car
            HoverCar.SetActive(false);
            SuperCar.SetActive(false);
            StandardCar.SetActive(true);

            EnableWheelMesh();
            //Enable Default wheel
        }
    }

    private void DisableWheelMesh()
    {
        foreach(GameObject gm in DefaultWheelMesh)
        {
            gm.SetActive(false);
        }
    }

    private void EnableWheelMesh()
    {
        foreach (GameObject gm in DefaultWheelMesh)
        {
            gm.SetActive(true);
        }
    }
}
