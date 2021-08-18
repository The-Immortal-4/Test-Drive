using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StarsUpdater : MonoBehaviour
{
    [SerializeField] private int LevelNo = 1;
    [SerializeField] private TextMeshProUGUI starsCountText;

    private void Start()
    {
        switch (LevelNo)
        {
            case 1:
                starsCountText.text = FindObjectOfType<SaveManager>().level1Star.ToString() + "/3";
                break;
            case 2:
                starsCountText.text = FindObjectOfType<SaveManager>().level2Star.ToString() + "/3";
                break;
            case 3:
                starsCountText.text = FindObjectOfType<SaveManager>().level3Star.ToString() + "/3";
                break;
        }
    }
}
