
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;

public class AudioManager : Singleton_GO<AudioManager>
{
    public AudioMixerSnapshot scStandard;
    public AudioMixerSnapshot scPaused;
    public AudioMixerSnapshot scSilence;

    private GameManager gm;

    public GameObject clock;
    private bool clockChange;
    private AudioSource audioClock;
    private AudioReverbFilter clockFilter;
    public float minClockVolume = 0.25f;
    public float maxClockVolume = 0.75f;
    public float minClockFilter = 0.25f;
    public float maxClockFilter = 0.5f;

    public AudioClip acBackgroundMusic;
    public AudioClip acScaryMusic;
    public AudioClip acScaryEffect;
    private AudioSource[] cameraAudio;
    public float minScaryMusicVolume1 = 0f;
    public float maxScaryMusicVolume1 = 1f;
    public float minScaryMusicVolume2 = 0f;
    public float maxScaryMusicVolume2 = 1f;

    public AudioClip defaultJumpScare;

    private void Awake()
    {
        gm = GameManager.Instance;

        audioClock = clock.GetComponent<AudioSource>();
        clockFilter = clock.GetComponent<AudioReverbFilter>();
        cameraAudio = FindObjectOfType<Camera>().GetComponents<AudioSource>();
    }

    private void Start()
    {
        cameraAudio[0].clip = acBackgroundMusic;
        cameraAudio[1].clip = acScaryMusic;
        cameraAudio[2].clip = acScaryEffect;
        cameraAudio[3].clip = defaultJumpScare;

        cameraAudio[0].volume = 1;
        cameraAudio[1].volume = 0f;
        cameraAudio[2].volume = 0f;
        cameraAudio[3].volume = 1f;

        cameraAudio[0].Play();
        cameraAudio[1].Play();
        cameraAudio[2].Play();

        cameraAudio[0].loop = true;
        cameraAudio[1].loop = true;
        cameraAudio[2].loop = true;
        cameraAudio[3].loop = false;

        clockChange = false;
    }

    public void Update()
    {
        //Write here the code which has only to do with audio changes. 
        checkAudioPause();
        checkMoushinderu();
    }
    
    public void checkAudioPause()
    {
        if (ManageInGameMenu.isPaused)
        {
            scPaused.TransitionTo(0.01f);
        }
        else
        {
            scStandard.TransitionTo(0.01f);
        }
    }

    public void checkMoushinderu()
    {
        if(clockChange)
        {
            if (!gm.getMoushinderu())
            {
                float timeLeft = gm.getTimeLeft();
                float changeRate = (1 - timeLeft / gm.getTimeForFinishingLevel());
                audioClock.volume = minClockVolume + (maxClockVolume - minClockVolume) * changeRate;
                clockFilter.room = minClockFilter + (maxClockFilter - minClockFilter) * changeRate;
                cameraAudio[0].volume = 1f - cameraAudio[1].volume;
                cameraAudio[1].volume = minScaryMusicVolume1 + (maxScaryMusicVolume1 - minScaryMusicVolume1) * changeRate;
                cameraAudio[2].volume = minScaryMusicVolume2 + (maxScaryMusicVolume2 - minScaryMusicVolume2) * changeRate;
            }
            else
            {
                scSilence.TransitionTo(0.01f);
            }
        }
        else
        {
            cameraAudio[0].volume = 1f;
            cameraAudio[1].volume = 0f;
            cameraAudio[2].volume = 0f;
        }
    }

    public void setClockChange(bool clockChange)
    {
        this.clockChange = clockChange;
    }

    public void playDefaultJumpScare()
    {
        cameraAudio[3].Play();
    }
}

