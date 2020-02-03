using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Interactable : MonoBehaviour
{
    public bool hasAnimation = false;
    public GameManager.InteractableType interactableType;
    public string info;
    public bool oneTimeInteract = false;
    public bool interactableAtBegin = true;
    private bool isInteractable;
    private GameManager gameManager;
    private Animator anim;

    private bool animPlay;

    public AudioClip isNotInteractableSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameManager.Instance;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        isInteractable = interactableAtBegin;
        animPlay = false;
    }
    public void interact()
    {
        if(isInteractable)
        {
            if(hasAnimation)
            {
                playAnimation("All");
            }

            if(oneTimeInteract)
            {
                this.gameObject.layer = LayerMask.NameToLayer("Default");
            }

            gameManager.interactedWith(this.interactableType, info);
        }
        else
        {
            if(audioSource !=null)
            {
                audioSource.clip = isNotInteractableSound;
                audioSource.Play();
            }
        }
    }

    //Important for door closing
    //Use this method if you want to trigger animation within an other script
    public void playAnimation(string nameOfCurrentState)
    {
        if(anim.IsInTransition(0) == false)
        {
            if (nameOfCurrentState == "All")
            {
                anim.SetBool("play", animPlay = !animPlay);
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName(nameOfCurrentState))
            {
                anim.SetBool("play", animPlay = !animPlay);
            }
        }
    }

    public bool getIsInteractable()
    {
        return isInteractable;
    }

    public void setInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
    }

}
