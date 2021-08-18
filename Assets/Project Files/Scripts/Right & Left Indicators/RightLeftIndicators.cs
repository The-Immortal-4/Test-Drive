using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightLeftIndicators : MonoBehaviour
{
    [SerializeField] private Image rightArrow;
    [SerializeField] private Image leftArrow;

    [SerializeField] private Material rightIndicatorMat;
    [SerializeField] private Material leftIndicatorMat;

    [SerializeField] private float indicatorSpeed = 0.5f;
    [SerializeField] private float indicatorButtonLifetime = 4f;

    public bool rightOn = false;
    public bool leftOn = false;
    private bool indicatorLifetimeReset = false;

    [SerializeField] private AudioClip TickSound;
    [SerializeField] private AudioClip TokSound;
    [SerializeField] private AudioSource audioSource;

    //Reference to coroutines
    private Coroutine ResetIndicatorAfterLifetimeCr;
    private Coroutine LeftFlasherCr;
    private Coroutine RightFlasherCr;

    private void Start()
    {
        rightIndicatorMat.DisableKeyword("_EMISSION");
        leftIndicatorMat.DisableKeyword("_EMISSION");
    }

    // Update is called once per frame
    private void Update()
    {
        //Right Indicator Button
        if (Input.GetButtonDown("Right Indicator"))
        {
            if (!rightOn && !leftOn)
            {
                if (!indicatorLifetimeReset)
                {
                    indicatorLifetimeReset = true;
                    rightOn = true;                   
                    StopCoroutineItem(RightFlasherCr);
                    StopCoroutineItem(ResetIndicatorAfterLifetimeCr);
                    RightFlasherCr = StartCoroutine(flasherR(rightArrow));
                    ResetIndicatorAfterLifetimeCr = StartCoroutine(ResetIndicatorsAfterLifetime());             
                }
            }
            else if(!rightOn && leftOn)
            {
                 rightOn = true;
                 leftOn = false;
                 StopCoroutineItem(RightFlasherCr);
                 StopCoroutineItem(ResetIndicatorAfterLifetimeCr);
                 RightFlasherCr = StartCoroutine(flasherR(rightArrow));
                 ResetIndicatorAfterLifetimeCr = StartCoroutine(ResetIndicatorsAfterLifetime());
            }
            else
            {
                rightOn = false;
                StopCoroutineItem(RightFlasherCr);
                StopCoroutineItem(ResetIndicatorAfterLifetimeCr);
                ResetIndicatorAfterLifetimeCr = null;
                RightFlasherCr = null;
                indicatorLifetimeReset = false;
                ChangeAlpha(rightArrow, 0.25f);
                ChangeEmmision(rightIndicatorMat, false);
            }
        }

        //Left Indicator Button
        if (Input.GetButtonDown("Left Indicator"))
        {
            if (!rightOn && !leftOn)
            {
                if (!indicatorLifetimeReset)
                {
                    leftOn = true;
                    StopCoroutineItem(LeftFlasherCr);
                    StopCoroutineItem(ResetIndicatorAfterLifetimeCr);
                    LeftFlasherCr = StartCoroutine(flasherL(leftArrow));
                    ResetIndicatorAfterLifetimeCr = StartCoroutine(ResetIndicatorsAfterLifetime());
                }
            }
            else if (rightOn && !leftOn)
            {
                 rightOn = false;
                 leftOn = true;
                 StopCoroutineItem(LeftFlasherCr);
                 StopCoroutineItem(ResetIndicatorAfterLifetimeCr);
                 LeftFlasherCr = StartCoroutine(flasherL(leftArrow));
                 ResetIndicatorAfterLifetimeCr = StartCoroutine(ResetIndicatorsAfterLifetime());
            }
            else
            {
                leftOn = false;
                StopCoroutineItem(LeftFlasherCr);
                StopCoroutineItem(ResetIndicatorAfterLifetimeCr);
                ResetIndicatorAfterLifetimeCr = null;
                LeftFlasherCr = null;
                indicatorLifetimeReset = false;
                ChangeAlpha(leftArrow, 0.25f);
                ChangeEmmision(leftIndicatorMat, false);
            }
        }
    }

    //To Apply StopCoroutine to multiple items
    private void StopCoroutineItem(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    //Change the alpha color
    private void ChangeAlpha(Image arrow, float a)
    {
        Color col = arrow.color;
        col.a = a;
        arrow.color = col;
    }

    //Change the Mat Emmision
    private void ChangeEmmision(Material mat, bool value)
    {
        if (value)
        {
            mat.EnableKeyword("_EMISSION");
        }
        else
        {
            mat.DisableKeyword("_EMISSION");
        }
    }

    //allows flashing animation for Right Arrow
    private IEnumerator flasherR(Image arrow)
    {
        while (rightOn)
        {
            yield return
            new WaitForSeconds(indicatorSpeed);
            //Check Again if rightOn is still true since you wait for some time
            if (rightOn)
            {
                ChangeAlpha(arrow, 1);
                ChangeEmmision(rightIndicatorMat, true);
                audioSource.PlayOneShot(TickSound);
            }

            yield return
                new WaitForSeconds(indicatorSpeed);
            if (rightOn)
            {
                ChangeAlpha(arrow, 0.25f);
                ChangeEmmision(rightIndicatorMat, false);
                audioSource.PlayOneShot(TokSound);
            }
        }
        ChangeAlpha(arrow, 0.25f);
        ChangeEmmision(rightIndicatorMat, false);
    }

    //allows flashing animation for Left Arrow
    private IEnumerator flasherL(Image arrow)
    {
        while (leftOn)
        {
            yield return
            new WaitForSeconds(indicatorSpeed);
            //Check Again if leftOn is still true since you wait for some time
            if (leftOn)
            {
                ChangeAlpha(arrow, 1);
                ChangeEmmision(leftIndicatorMat, true);
                audioSource.PlayOneShot(TickSound);
            }
            yield return
                new WaitForSeconds(indicatorSpeed);
            if (leftOn)
            {
                ChangeAlpha(arrow, 0.25f);
                ChangeEmmision(leftIndicatorMat, false);
                audioSource.PlayOneShot(TokSound);
            }
        }
        ChangeAlpha(arrow, 0.25f);
        ChangeEmmision(leftIndicatorMat, false);
    }

    //Disable Indicator after certain time
    private IEnumerator ResetIndicatorsAfterLifetime()
    {
        yield return new WaitForSeconds(indicatorButtonLifetime);
        ResetIndicatorAfterLifetimeCr = null;
        indicatorLifetimeReset = false;
        leftOn = false;
        rightOn = false;
    }
}
