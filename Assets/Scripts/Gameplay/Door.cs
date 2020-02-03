using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Door : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip doorOpeningSound;
    public AudioClip doorClosingSound;

    public bool keepClosed = false;
    private Interactable interactable;

    public void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        interactable = GetComponent<Interactable>();
    }

    public void playDoorOpeningSound()
    {
        audioSource.clip = doorOpeningSound;
        audioSource.Play();
    }

    public void playDoorClosingSound()
    {
        audioSource.clip = doorClosingSound;
        audioSource.Play();

        if (keepClosed)
        {
            interactable.setInteractable(false);
        }
    }
}
