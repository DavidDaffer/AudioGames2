using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioPool 
{
    private AudioSource[] audioSources;
    private int maxSourcesAllowed;

    public AudioPool(int maxSourcesAllowed)
    {
        audioSources = new AudioSource[maxSourcesAllowed];
        this.maxSourcesAllowed = maxSourcesAllowed;

    }

    public AudioSource Play(AudioSource audioSource)
    {
        for(int i = 0; i < maxSourcesAllowed; i++)
        {
            if(audioSources[i].enabled == false)
            {
                audioSources[i].enabled = true;
                return audioSource;
            }
        }
        return null;
    }
}
