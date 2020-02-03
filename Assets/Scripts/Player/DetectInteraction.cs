using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionCheck : MonoBehaviour
{
    private static bool check;
    private static GameObject interactionObject;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        bool hits = Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, 3.0f, 1 << LayerMask.NameToLayer("Interactable"));
        if(hit.collider)
        {
            check = true;
            interactionObject = hit.collider.gameObject;
        }
        else
        {
            check = false;
        }
    }

    public static bool CanInteract()
    {
        return check;
    }

    public static GameObject GetObjectOfInteraction()
    {
        return interactionObject;
    }
}
