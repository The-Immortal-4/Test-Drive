using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioDialogSystem : MonoBehaviour
{
    [SerializeField] private Dialog []m_Dialogs;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(string name)
    {
        SaveManager saveManager = FindObjectOfType<SaveManager>();
        if (saveManager.zombiesActivated)
        {
            for(int i = 0; i < m_Dialogs.Length; i++)
            {
                if(m_Dialogs[i].m_Name == name)
                {
                    audioSource.clip = m_Dialogs[i].m_ZombieAudio;
                    audioSource.Play();
                    //Play Zombie Audio here
                }
            }
        }
        else
        {
            for (int i = 0; i < m_Dialogs.Length; i++)
            {
                if (m_Dialogs[i].m_Name == name)
                {
                    audioSource.clip = m_Dialogs[i].m_NormalAudio;
                    audioSource.Play();
                    //Play Normal Audio here
                }
            }
        }
    }

    //Struct Defination
    [Serializable]
    private struct Dialog
    {
        public string m_Name;
        public AudioClip m_NormalAudio;
        public AudioClip m_ZombieAudio;
    }
}
