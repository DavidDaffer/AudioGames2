using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Monster : MonoBehaviour
{
    private AudioSource audioSource;
    public Transform lookAtPos;
    private Animator anim;

    private bool attack;

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if(attack == true)
        anim.SetTrigger("attack");
    }

    public void attackAnimation()
    {
        audioSource.Play();
        attack = true;
    }
}
