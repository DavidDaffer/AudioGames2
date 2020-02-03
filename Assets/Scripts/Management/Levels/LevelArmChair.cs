using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelArmChair : Level
{
    public GameObject armchair;
    private SphereCollider sCollider;
    private Animator anim;
    private GameManager gm;
    private AudioManager am;

    private Collider playerCollider;

    public int armChairLevel = 2;

    private bool finished;

    // Start is called before the first frame update
    void Awake()
    {
        anim = armchair.GetComponent<Animator>();
        gm = GameManager.Instance;
        am = AudioManager.Instance;

        playerCollider = FindObjectOfType<Player>().gameObject.GetComponent<Collider>();
    }

    private void Start()
    {
        finished = false;
        sCollider = armchair.GetComponent<SphereCollider>();
        sCollider.enabled = false;
    }

    public override void LevelInit()
    {
        armchair.GetComponent<SphereCollider>().enabled = true;
    }

    public override void LevelCleanUp()
    {
        armchair.GetComponent<SphereCollider>().enabled = false;
    }

    public override void LevelUpdate()
    {
        if (sCollider.bounds.Intersects(playerCollider.bounds))
        {
            armchair.gameObject.layer = LayerMask.NameToLayer("Interactable");
            sCollider.enabled = false;
            anim.SetTrigger("crash");
            am.playDefaultJumpScare();
        }

        if(finished)
        {
            gm.levelFinished();
        }
    }

    public void tidyUp(string info)
    {
        finished = true;
    }

    //called from animation
    public void playArmchairSound()
    {
        am.playDefaultJumpScare();
    }
}
