using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable
{
    public bool locked;

    /*override public void interact()
    {
        if (isInteractable)
        {
            if(locked)
            {
                //playAnimation("Rattle");
                //play sound WontBudge
            }
            else
            {
                if (hasAnimation)
                {
                    playAnimation("All");
                }
            }
            
            gameManager.interactedWith(GameManager.InteractableType.Door);
        }
    }*/
}
