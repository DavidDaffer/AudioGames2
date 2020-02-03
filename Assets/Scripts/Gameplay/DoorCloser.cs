using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCloser : MonoBehaviour
{
    public GameObject door;
    private Interactable doorInteract;

    private void Awake()
    {
        doorInteract = door.GetComponent<Interactable>();
        if(doorInteract == null)
        {
            doorInteract = door.GetComponentInChildren<Interactable>();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            doorInteract.playAnimation("DoorOpen");
        }
    }
}

