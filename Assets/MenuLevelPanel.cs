using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLevelPanel : MonoBehaviour
{
    public void ActivateGameObject(GameObject gm)
    {
        gm.SetActive(true);
    }

    public void DeactivateGameObject(GameObject gm)
    {
        gm.SetActive(false);
    }
}
