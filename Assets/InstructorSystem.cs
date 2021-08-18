using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstructorSystem : MonoBehaviour
{
    [SerializeField] private Image instructorImage;
    [SerializeField] private Sprite normalAvatarSprite;
    [SerializeField] private Sprite zombieAvatarSprite;

    [SerializeField] private GameObject instructorPopUp;
    [SerializeField] private TextMeshProUGUI popUpTxt;

    [SerializeField] private float resetTime = 4f;
    private Coroutine resetCoroutine;

    public void InstructorPopUp(string text)
    {
        UpdateInstructorAvatar();
        instructorPopUp.gameObject.SetActive(true);
        popUpTxt.text = text;

        //Coroutine Resetter
        if(resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetTime);
        popUpTxt.text = "";
        instructorPopUp.gameObject.SetActive(false);
    }

    private void UpdateInstructorAvatar()
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (!saveManager.zombiesActivated)
        {
            instructorImage.sprite = normalAvatarSprite;
        }
        else
        {
            instructorImage.sprite = zombieAvatarSprite;
        }
    }
}
