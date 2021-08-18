using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    
    public AudioMixerGroup audioMixer;
    public Slider audioSlider;

    private float tempAudioValue;
    private bool isMuted = false;


    //set slider value
    public void SetSlider(float value)
    {
        UpdateMixer(value);
    }

    //updates mixer
    private void UpdateMixer(float value)
    {
        if (!isMuted)
        {
            
            audioMixer.audioMixer.SetFloat("Volume", value);
            audioMixer.audioMixer.GetFloat("Volume", out float tempAudioValue);
            isMuted = false;
        }
        
    }

    public void MuteAudio(bool value)
    {

        if (value)
        {
            UpdateMixer(-80);
            isMuted = true;
        }
        else
        {
            isMuted = false; ;
            UpdateMixer(tempAudioValue);
        }
        
    }


}
