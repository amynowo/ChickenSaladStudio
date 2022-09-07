using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // to get back to main menu scene
using System;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu; // SerializeField attribute makes var private but will show up in editor


    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        MusicManager.Instance.audioSource.Pause();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        MusicManager.Instance.audioSource.UnPause();


    }

    public void Home(int sceneId)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneId);
    }
}
