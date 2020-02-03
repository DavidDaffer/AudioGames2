using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopController : MonoBehaviour
{
    public GameObject respawnPoint;
    private GameManager gm;

    public void Start()
    {
        gm = GameManager.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            float x = respawnPoint.transform.position.x - this.transform.position.x;
            float y = respawnPoint.transform.position.y - this.transform.position.y;
            float z = respawnPoint.transform.position.z - this.transform.position.z;
            other.gameObject.transform.Translate(new Vector3(x,y,z), Space.World);

            gm.goToNextLevel();
        }
    }

}
