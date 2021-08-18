using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructorPopUpPoint : MonoBehaviour
{
    [SerializeField] [TextArea] private string dialogText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<InstructorSystem>().InstructorPopUp(dialogText);
        }
    }
}
