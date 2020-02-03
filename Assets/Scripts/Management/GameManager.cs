
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class GameManager : Singleton_GO<GameManager>
{
    public enum InteractableType
    {
        Door,
        Armchair,
        Mirror,
        Phone
    };

    private Level[] levels;

    private AudioManager audioManager;

    //Level Management Variables
    private int currentLevelNumber;
    private Level currentLevel;
    private float timeStampStartLevel;

    public GameObject progessionDoor;
    private Interactable pDoorInteractable;

    public GameObject startDoor;
    private Interactable startDoorInteractable;

    public GameObject player;
    public GameObject camera;
    public GameObject enemy;

    private CameraPlayer3D cameraScript;
    private bool moushinderu;

    private float jumpScareDelay;

    public TextMeshPro hintText;

    private void Awake()
    {
        audioManager = AudioManager.Instance;

        // references to door which is closed until level is cleared. 
        pDoorInteractable = progessionDoor.GetComponent<Interactable>();
        if(pDoorInteractable == null)
        {
            pDoorInteractable = progessionDoor.GetComponentInChildren<Interactable>();
        }

        startDoorInteractable = startDoor.GetComponent<Interactable>();
        if (startDoorInteractable == null)
        {
            startDoorInteractable = startDoor.GetComponentInChildren<Interactable>();
        }

        cameraScript = camera.GetComponent<CameraPlayer3D>();

        levels = this.GetComponents<Level>();
        jumpScareDelay = 1.0f;
    }

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        hintText.text = "";
        hintText.enabled = false;

        moushinderu = false;

        //Init Level to level 1
        goToNextLevel();
    }

    public void Update()
    {
        clockIsTicking();

        currentLevel.LevelUpdate();

        audioManager.checkAudioPause();
    }

    public void interactedWith(InteractableType inter, string info)
    {
        switch(inter)
        {
            case InteractableType.Mirror:
                FindObjectOfType<LevelBath1>().gotMirror(info);
                break;
            case InteractableType.Armchair:
                FindObjectOfType<LevelArmChair>().tidyUp(info);
                break;
            case InteractableType.Phone:
                if(info == "LevelTimeHint")
                {
                    FindObjectOfType<LevelTimeHint>().PickupPhone(info);
                }
                else
                {
                    GetComponent<LevelPhone>().PickupPhone(info);
                }
                
                break;
            default:
                break;
                
        }
    }

    public void KillPlayer()
    {
        StartCoroutine(Die());
    }

    private void clockIsTicking()
    {
        if(currentLevel.playerCanDie)
        {
            audioManager.setClockChange(true);
            if (!moushinderu)
            {
                //currentTimeInLevel = Time.time - timeStampStartLevel
                float timeLeft = getTimeLeft();

                if (timeLeft <= 0)
                {
                    moushinderu = true;
                    StartCoroutine(Die());
                }
            }
        }
        else
        {
            audioManager.setClockChange(false);
        }
    }


    IEnumerator Die()
    {

        yield return new WaitForSeconds(jumpScareDelay);

        MonsterDeath();

        yield return new WaitForSeconds(2f);

        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene("DeathScreen");

    }


    private void MonsterDeath()
    {

        float x = player.transform.position.x;
        float y = player.transform.position.y;
        float z = player.transform.position.z;
        GameObject iEnemy = Instantiate(enemy, new Vector3(x, y, z), Quaternion.identity);

        iEnemy.transform.Translate(-player.transform.forward * 1f);
        iEnemy.transform.Translate(0f, player.transform.position.y - iEnemy.transform.position.y, 0f);
        iEnemy.transform.Rotate(0f, player.transform.rotation.eulerAngles.y, 0f);
        iEnemy.transform.Rotate(0f, 10f, 0f);

        cameraScript.setControllable(false);
        player.GetComponent<Player>().kill();

        Vector3 posEnemy = iEnemy.transform.position;
        Vector3 posCamera = camera.transform.position;
        Vector3 translation = new Vector3(posEnemy.x - posCamera.x, 0f, posEnemy.z - posCamera.z);

        Monster MonsterScript = iEnemy.GetComponent<Monster>();
        camera.transform.LookAt(MonsterScript.lookAtPos);
        camera.GetComponent<Camera>().nearClipPlane = 0.01f;

        MonsterScript.attackAnimation();

    }

    public int getCurrentLevelNumber()
    {
        return this.currentLevelNumber;
    }

    public Level getCurrentLevel()
    {
        return this.currentLevel;
    }

    public float getJumpScareDelay()
    {
        return this.jumpScareDelay;
    }

    //Only to be triggerd in the dark room
    public void goToNextLevel()
    {
        //clean up old level
        if (currentLevelNumber > 0)
        {
            currentLevel.LevelCleanUp();
            unlockDoorStart();
        }

        //next level
        this.currentLevelNumber++;
        timeStampStartLevel = Time.time;

        bool levelFound = false;
        //next level load
        foreach (Level l in levels)
        {
            if (l.levelNumber == currentLevelNumber)
            {
                currentLevel = l;
                levelFound = true;
            }
        }
        if(!levelFound)
        {
            currentLevel = new Nothing();
        }

        if(currentLevel.lockDock)
        {
            lockDoorP();
        }

        //next level init
        currentLevel.LevelInit();
    }

    public void lockDoorP()
    {
        pDoorInteractable.setInteractable(false);
    }

    //Call this function from your puzzle logic script.
    public void levelFinished()
    {
        pDoorInteractable.setInteractable(true);
    }

    public void unlockDoorStart()
    {
        startDoorInteractable.setInteractable(true);
    }

    public bool getMoushinderu()
    {
        return moushinderu;
    }
    public float getTimeLeft()
    {
        return currentLevel.timeForFinishingLevel - (Time.time - timeStampStartLevel);
    }
    public float getTimeForFinishingLevel()
    {
        return this.currentLevel.timeForFinishingLevel;
    }
}

