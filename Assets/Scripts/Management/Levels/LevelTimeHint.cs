using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimeHint : Level
{
    public GameObject phone;
    public AudioSource playerTalk;
    public AudioClip answerPhone1;
    public AudioClip phoneTalk;
    public AudioClip answerPhone2;
    public AudioClip phoneRinging;

    private GameManager gm;
    private AudioManager am;

    private bool acceptedCall;

    public override void LevelCleanUp()
    {
        phone.layer = LayerMask.NameToLayer("Default");
        StopCoroutine("phoneConversation");
        phone.GetComponent<AudioSource>().Stop(); // stop rining
    }

    public override void LevelInit()
    {
        gm = GameManager.Instance;
        am = AudioManager.Instance;

        phone.layer = LayerMask.NameToLayer("Interactable");
        AudioSource phoneAS = phone.GetComponent<AudioSource>();
        phoneAS.clip = phoneRinging;
        phoneAS.PlayDelayed(5);
        phoneAS.loop = true;
        phone.GetComponent<Interactable>().oneTimeInteract = true;
        phone.GetComponent<Interactable>().info = "LevelTimeHint";
        gm.levelFinished(); // you can ignore the phone if you want..
    }

    public override void LevelUpdate()
    {
        
    }

    public void PickupPhone(string info)
    {
        AudioSource phoneAS = phone.GetComponent<AudioSource>();
        phoneAS.loop = false;
        phoneAS.Stop(); // stop rining
        StartCoroutine("phoneConversation");
    }

    IEnumerator phoneConversation()
    {
        AudioSource phoneAS = phone.GetComponent<AudioSource>();
        playerTalk.clip = answerPhone1;
        playerTalk.Play();
        while(playerTalk.isPlaying)
        {
            yield return null;
        }
        phoneAS.clip = phoneTalk;
        phoneAS.Play();
        while(phoneAS.isPlaying)
        {
            yield return null;
        }
        playerTalk.clip = answerPhone2;
        playerTalk.Play();
        while (playerTalk.isPlaying)
        {
            yield return null;
        }
        yield return null;
    }
}
