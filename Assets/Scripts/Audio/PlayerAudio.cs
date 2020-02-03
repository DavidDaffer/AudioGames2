using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudio : MonoBehaviour
{
    public AudioClip[] footSteps;
    private AudioSource audioSource;
    float timeStampFootStep;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        timeStampFootStep = 0f;
    }

    public void playFootstep()
    {
        //Event triggered by animation!
        Player player = gameObject.GetComponent<Player>();
        if (player.getPlayerState() == Player.State.Normal) // don't play sounds if the player isn't in NORMAL state
        {
            if ((Time.fixedTime - timeStampFootStep) > 0.5f)
            {
                int rand = Random.Range(0, footSteps.Length);
                audioSource.clip = footSteps[rand];
                audioSource.Play();
                timeStampFootStep = Time.fixedTime;
            }
        }
    }
}
