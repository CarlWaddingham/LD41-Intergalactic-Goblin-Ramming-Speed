using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

    public bool active;
    public GameObject pauseMenu;
    public GameObject settingsMenu;

    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
	}

    void PauseGame()
    {
        active = !active;

        pauseMenu.SetActive(active);

        if (active)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Resume()
    {
        PauseGame();
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Settings()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }
}
