using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // to get back to main menu scene
using System;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicVolumeSlider;
    
    public void Pause()
    {
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        MusicManager.Instance.musicAudioSource.Pause();
        SetMusicVolumeSlider();
    }

    void SetMusicVolumeSlider()
    {
        musicVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume"));
    }
    
    public void ChangeMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
        MusicManager.Instance.musicAudioSource.UnPause();
    }

    public void Home()
    {
        GameResult.Instance.ResetStats();
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
}
