using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPhone : Level
{
    public GameObject phone;
    public GameObject player;
    public GameObject cameraPlayer;
    public AudioSource playerAS;
    public AudioClip phoneTalk;
    public AudioClip phoneRinging;
    public AudioClip heartRace;

    private GameManager gm;
    private AudioManager am;
    private bool enemySpawned;
    private GameObject enemyInstance;
    private float spawnTime = 0.0f;

    public override void LevelCleanUp()
    {
        phone.layer = LayerMask.NameToLayer("Default");
        StopCoroutine("phoneConversation");
        enemySpawned = false;
        AudioSource phoneAS = phone.GetComponent<AudioSource>();
        phoneAS.Stop();
    }

    public override void LevelInit()
    {
        gm = GameManager.Instance;
        am = AudioManager.Instance;

        phone.layer = LayerMask.NameToLayer("Interactable");
        AudioSource phoneAS = phone.GetComponent<AudioSource>();
        phoneAS.clip = phoneRinging;
        phoneAS.PlayDelayed(5);
        phoneAS.loop = true;
        phone.GetComponent<Interactable>().oneTimeInteract = true;
        phone.GetComponent<Interactable>().info = "LevelPhone";
    }

    public override void LevelUpdate()
    {
        if(enemySpawned)
        {
            spawnTime += Time.deltaTime;
            if (spawnTime > 10.0f) // 2 seconds
            {
                // return to player movement
                player.GetComponent<Player>().setLocked(false);
                playerAS.loop = false;
                playerAS.Stop();
                Destroy(enemyInstance);
                enemySpawned = false;
            }
            float rotationA = cameraPlayer.transform.localEulerAngles.y % 360;
            if(rotationA > 315 || rotationA < 40)
            {
                player.GetComponent<Player>().setLocked(false);
                if (player.GetComponent<Player>().getPlayerState() == Player.State.Normal)
                {
                    Destroy(enemyInstance, gm.getJumpScareDelay());
                    player.GetComponent<Player>().kill();
                    gm.KillPlayer();
                }
            }
        }
    }

    public void PickupPhone(string info)
    {
        AudioSource phoneAS = phone.GetComponent<AudioSource>();
        phoneAS.loop = false;
        gm.levelFinished();
        phoneAS.Stop(); // stop rining
        StartCoroutine("phoneConversation");
    }

    IEnumerator phoneConversation()
    {
        // take player control
        cameraPlayer.GetComponent<CameraPlayer3D>().setControllable(false);
        player.GetComponent<Player>().setLocked(true);
        AudioSource phoneAS = phone.GetComponent<AudioSource>();
        phoneAS.clip = phoneTalk;
        phoneAS.Play();
        while (phoneAS.isPlaying)
        {
            yield return null;
        }
        //spawn enemy behind
        Vector3 dir = player.transform.forward;
        Vector3 position = player.transform.position - dir * 3;
        // add view direction
        enemyInstance = Instantiate(gm.enemy, position, Quaternion.Euler(0, 0, 0));
        enemyInstance.transform.LookAt(player.transform);
        enemySpawned = true;
        //return control to player camera
        cameraPlayer.GetComponent<CameraPlayer3D>().setControllable(true);
        
        playerAS.clip = heartRace;
        playerAS.loop = true;
        playerAS.Play();
        yield return null;
    }
}
