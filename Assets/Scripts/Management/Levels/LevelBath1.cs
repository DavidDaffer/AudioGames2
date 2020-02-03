using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBath1 : Level
{
    public GameObject MirrorPickUp;
    public GameObject Mirror;

    private bool hasMirror;
    private bool finished;

    private GameManager gm;
    private AudioManager am;

    private bool monsterSpawned;


    private void Awake()
    {
        gm = GameManager.Instance;
        am = AudioManager.Instance;

        MirrorPickUp.SetActive(false);

        monsterSpawned = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //level = 4;
        hasMirror = false;
        finished = false;
    }

    public void gotMirror(string info)
    {
        if(info.Equals("MirrorPickUp"))
        {
            hasMirror = true;
            Mirror.GetComponent<Interactable>().setInteractable(true);
            MirrorPickUp.SetActive(false);
        }
        else if (info.Equals("Mirror"))
        {
            if(hasMirror)
            {
                finished = true;
            }
        }
    }

    public override void LevelInit()
    {
        MirrorPickUp.SetActive(true);
        Mirror.layer = LayerMask.NameToLayer("Interactable");
        Mirror.GetComponent<MeshRenderer>().enabled = false;
        Mirror.GetComponent<Interactable>().setInteractable(false);

        gm.hintText.enabled = true;
        gm.hintText.text = "Have you looked in the mirror lately?";
    }

    public override void LevelCleanUp()
    {
        Mirror.GetComponent<MeshRenderer>().enabled = true;
        Mirror.layer = LayerMask.NameToLayer("Default");
        MirrorPickUp.SetActive(false);

        gm.hintText.enabled = false;
        gm.hintText.text = "";
    }

    public override void LevelUpdate()
    {
        if (finished)
        {
            if(!monsterSpawned)
            {
                monsterSpawned = true;
                StartCoroutine(MonsterSpawnShort());
            }
            gm.levelFinished();
        }
    }

    IEnumerator MonsterSpawnShort()
    {
        Mirror.GetComponent<MeshRenderer>().enabled = true;

        am.playDefaultJumpScare();

        float x = gm.player.transform.position.x;
        float y = gm.player.transform.position.y;
        float z = gm.player.transform.position.z;
        GameObject iEnemy = Instantiate(gm.enemy, new Vector3(x, y, z), Quaternion.identity);

        iEnemy.transform.Translate(-gm.player.transform.forward * 1f);
        iEnemy.transform.Translate(0f, gm.player.transform.position.y - iEnemy.transform.position.y, 0f);
        iEnemy.transform.Rotate(0f, gm.player.transform.rotation.eulerAngles.y, 0f);

        iEnemy.transform.Rotate(0f, 10f, 0f);

        yield return new WaitForSeconds(1f);

        Destroy(iEnemy);
    }

}
