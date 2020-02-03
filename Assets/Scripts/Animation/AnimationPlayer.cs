using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour {


    private Animator anim;

    private float inputH;
    private float inputV;
    private bool run;
    private bool jump;


    private Player player;

    private bool cJump;

    public void comandJump()
    {
        cJump = true;
    }


    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Animate();
    }

    public void Animate()
    {
        /*
        if (cJump)
        {
            jump = true;
            cJump = false;
        }
        else
        {
            jump = false;
        }
        

        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }
        */

        inputH = Input.GetAxis("Horizontal");
        inputV = Input.GetAxis("Vertical");

        anim.SetFloat("inputH", inputH);
        anim.SetFloat("inputV", inputV);
        anim.SetBool("run", run);
        //anim.SetBool("jump", jump);
    }
}
