using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popUpText;
    private float HoldTime = 3f;
    [SerializeField] private float fadeTime = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float offset = 2f;
    private Vector2 startingPos;

    private bool fading = false;
    private bool moving = false;

    private Coroutine FadeOutCoroutine;

    private void Start()
    {
        startingPos = popUpText.GetComponent<RectTransform>().anchoredPosition;
        offset += popUpText.GetComponent<RectTransform>().anchoredPosition.y;
    }

    private void Update()
    {
        if (fading)
        {
            if (popUpText.color.a > 0)
            {
                Color col;
                col = popUpText.color;
                col.a -= fadeTime * Time.deltaTime;
                popUpText.color = col;
            }
            else
            {
                fading = false;
                popUpText.GetComponent<RectTransform>().anchoredPosition = startingPos;
            }
        }

        if (moving)
        {
            float pos = popUpText.GetComponent<RectTransform>().anchoredPosition.y;
            if(pos <= offset)
            {
                pos += moveSpeed * Time.deltaTime;
                RectTransform rec = popUpText.GetComponent<RectTransform>();
                rec.anchoredPosition = new Vector2(rec.anchoredPosition.x, pos);
            }
            else
            {
                moving = false;             
            }
        }
    }

    public void PopUp(string PopUpValue, Color newCol)
    {
        newCol.a = 1f;
        popUpText.color = newCol;

        popUpText.gameObject.SetActive(true);
        popUpText.text = PopUpValue;

        if (FadeOutCoroutine != null)
        {
            StopCoroutine(FadeOutCoroutine);
            popUpText.GetComponent<RectTransform>().anchoredPosition = startingPos;
        }
        FadeOutCoroutine = StartCoroutine(FadeOut());
        MoveUp();
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(HoldTime);
        fading = true;
    }

    private void MoveUp()
    {
        moving = true;
    }
}

