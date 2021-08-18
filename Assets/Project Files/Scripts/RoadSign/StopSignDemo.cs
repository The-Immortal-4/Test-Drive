using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class StopSignDemo : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    bool isTiming = false;
    float currentTime;

    private void Start()
    {
        timerText = GameObject.FindGameObjectWithTag("RoadRuleTimeText")
            .GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (isTiming)
        {
            currentTime += Time.deltaTime;
            UpdateText(Mathf.Round(currentTime).ToString());
        }

        if(currentTime >= 3.0f)
        {
            isTiming = false;
            currentTime = 0;
            UpdateText("0");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartTimer();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isTiming = false;
            currentTime = 0;
            UpdateText("0");
        }
    }


    private void StartTimer()
    {
        isTiming = true;
    }

    private void UpdateText(string value)
    {
        timerText.text = value;
    }
}
