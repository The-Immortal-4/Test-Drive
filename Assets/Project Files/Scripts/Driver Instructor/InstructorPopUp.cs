using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructorPopUp : MonoBehaviour
{
    public GameObject instructorPopUp;
    public Text UIText;
    public TextMeshProUGUI PopUpText;

    public bool hideTips = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Tab Key"))
        {
            hideTips = !hideTips;

            if (hideTips)
            {
                UIText.text = "Show Tips";
                instructorPopUp.SetActive(false);
            }
            else
            {
                UIText.text =  "Hide Tips";
                instructorPopUp.SetActive(true);
            }
        }
    }

    public void UpdateDialogText(string text)
    {
        PopUpText.text = text;
    }
}
