using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ManageInGameMenu : MonoBehaviour
{
    public GameObject panel;

    public static bool isPaused;
	void Start ()
    {
        isPaused = false;
        panel.SetActive(false);
    }

    public void goToMainMenu()
    {
        panel.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        panel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        panel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
    }

    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
	}


}
