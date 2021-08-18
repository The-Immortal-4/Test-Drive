using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoadRulePopUpUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roadRulePopUpText;
    [SerializeField] private float textResetTime = 5f;
    bool reset = false;

    Coroutine ResetCoroutine;

    public void RoadRulePopUp(string text,Color col)
    {
        reset = true;
        roadRulePopUpText.gameObject.SetActive(true);
        roadRulePopUpText.text = text;
        roadRulePopUpText.color = col;

        if (ResetCoroutine != null)
        {
            StopCoroutine(ResetCoroutine);
        }
        ResetCoroutine = StartCoroutine(ResetText());
        //Saving a Coroutine data
    }

    public void RoadRulePopUpInstance(string text, Color col)
    {
        if (ResetCoroutine != null)
        {
            StopCoroutine(ResetCoroutine);
        }
        roadRulePopUpText.gameObject.SetActive(true);
        roadRulePopUpText.text = text;
        roadRulePopUpText.color = col;
    }

    public void RoadRulePopUpInstanceStop()
    {
        if (ResetCoroutine != null)
        {
            StopCoroutine(ResetCoroutine);
        }
        roadRulePopUpText.text = "";
        roadRulePopUpText.gameObject.SetActive(false);
    }

    private IEnumerator ResetText()
    {
        yield return new WaitForSeconds(textResetTime);
        roadRulePopUpText.text = "";
        roadRulePopUpText.gameObject.SetActive(false);
    }
}
