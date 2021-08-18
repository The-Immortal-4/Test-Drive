using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxUpdater : MonoBehaviour
{
    //[SerializeField] private Material mat;

    private void Start()
    {
        UpdateSkyBox();
    }

    public void UpdateSkyBox()
    {
        StartCoroutine(UpdateSkyBoxRoutine());
    }

    IEnumerator UpdateSkyBoxRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        SaveManager saveManger = FindObjectOfType<SaveManager>();
        if (saveManger.zombiesActivated)
        {
            SetColor("ZenithColor", "#FF8700");
            SetColor("HorizonColor", "#4C74B4");
            SetColor("NadirColor", "#003D9F");
            //ZOmbie Colors
        }
        else
        {
            SetColor("ZenithColor", "#0093FF");
            SetColor("HorizonColor", "#D1D1D1");
            SetColor("NadirColor", "#6C6C6C");
            //Normal Colors
        }
    }

    private void SetColor(string colorName, string colorCode)
    {
        Color color;
        ColorUtility.TryParseHtmlString(colorCode, out color);
        //mat.SetColor(colorName, color);
        RenderSettings.skybox.SetColor(colorName, color);
    }
}
