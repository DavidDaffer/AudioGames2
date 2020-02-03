using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
    public bool startWithAnyKeyDown = false;
    public bool isMenu;
    public string goToScreen;

    public void Start()
    {
        if (isMenu)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void Update()
    {
        if(Input.anyKeyDown && startWithAnyKeyDown)
        {
            LoadLevelByName(goToScreen);
        }
    }


    public void LoadLevelByIndex(int index)
    {

        SceneManager.LoadScene(index);
        //Application.LoadLevel(index);
    }

    public void LoadLevelByName(string name)
    {
        SceneManager.LoadScene(name);
    }


    public void exitApplication()
    {
        Application.Quit();
    }

}
