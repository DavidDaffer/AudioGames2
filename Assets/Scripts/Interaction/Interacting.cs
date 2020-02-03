using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interacting : MonoBehaviour
{
    public Image interactionSymbol;
    public float interactingDistanz = 1f;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        bool hits = Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hit, interactingDistanz, 1 << LayerMask.NameToLayer("Interactable"));
        if(hit.collider)
        {
            Debug.Log(hit.collider.gameObject.name);
            interactionSymbol.enabled = true;
            if(Input.GetMouseButtonDown(0))
            {
                hit.collider.gameObject.GetComponent<Interactable>().interact();
            }
        }
        else
        {
            interactionSymbol.enabled = false;
        }
    }
}
