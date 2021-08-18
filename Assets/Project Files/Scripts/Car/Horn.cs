using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horn : MonoBehaviour
{
    [SerializeField] private bool dynamicSound = true;

    [SerializeField] private AudioSource hornAudioSource;
    [SerializeField] private AudioClip oneShortHornAudioClip;
    [SerializeField] private AudioClip dynamicAudioClip;

    void Update()
    {
        if (Input.GetButtonDown("Horn"))
        {
            ApplyHorn();
            //Applied Horn
        }
        if (Input.GetButtonUp("Horn"))
        {
            if (dynamicSound)
            {
                if (hornAudioSource.isPlaying)
                {
                    hornAudioSource.Stop();
                }
            }
        }
    }

    private void ApplyHorn()
    {
        if (dynamicSound)
        {
            hornAudioSource.loop = true;
            hornAudioSource.clip = dynamicAudioClip;
            hornAudioSource.Play();
        }
        else
        {
            hornAudioSource.loop = false;
            //Checking if its playing
            if (!hornAudioSource.isPlaying)
            {
                hornAudioSource.PlayOneShot(oneShortHornAudioClip);
            }
        }
    }
}
