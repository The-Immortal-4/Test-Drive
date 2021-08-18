using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDialogTrigger : MonoBehaviour
{
    [SerializeField] private string audioName;

    private void PlayAudio()
    {
        AudioDialogSystem audioDialogSys = FindObjectOfType<AudioDialogSystem>();
        audioDialogSys.PlayAudio(audioName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(audioName != null)
            {
                PlayAudio();
            }
        }
    }
}
