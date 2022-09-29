using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // to get back to main menu scene
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject pauseOptions;

    [SerializeField] private GameObject soundSettings;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject volumeOnButton;
    [SerializeField] private GameObject volumeOffButton;
    [SerializeField] private Slider musicVolumeSlider;
    
    [SerializeField] GameObject birds;
    [SerializeField] GameObject branch;
    [SerializeField] TextMeshProUGUI countdownText;
    
    private bool volumeMute;
    
    public void Pause()
    {
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        soundSettings.SetActive(false);
        Time.timeScale = 0;
        LevelManager.Instance.musicAudioSource.Pause();
        
        audioMixer.SetFloat("Theme", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        SetMusicVolume();
        
        foreach (var bird in birds.GetComponentsInChildren<SpriteRenderer>())
            bird.sortingOrder = 1;
        branch.GetComponent<SpriteRenderer>().sortingOrder = 0;
    }

    void SetMusicVolume()
    {
        volumeMute = PlayerPrefs.GetInt("VolumeMute") == 1;
        
        volumeOffButton.SetActive(volumeMute);
        volumeOnButton.SetActive(!volumeMute);

        audioMixer.SetFloat("Master", volumeMute ? -80.0f : 0.0f);
        musicVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume"));
    }
    
    public void Sound()
    {
        pauseOptions.SetActive(false);
        soundSettings.SetActive(true);
    }
    
    public void Mute()
    {
        volumeOnButton.SetActive(volumeMute);
        volumeOffButton.SetActive(!volumeMute);
        volumeMute = !volumeMute;
        PlayerPrefs.SetInt("VolumeMute", volumeMute ? 1 : 0);
        audioMixer.SetFloat("Master", volumeMute ? -80.0f : 0.0f);
    }
    
    public void ChangeMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        audioMixer.SetFloat("Theme", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
    }
    
    public void Back()
    {
        soundSettings.SetActive(false);
        pauseOptions.SetActive(true);
    }

    public void Continue()
    {
        foreach (var bird in birds.GetComponentsInChildren<SpriteRenderer>())
            bird.sortingOrder = 5;
        branch.GetComponent<SpriteRenderer>().sortingOrder = 4;
        
        audioMixer.SetFloat("Theme", -80);
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
        countdownText.gameObject.SetActive(true);
        
        StartCoroutine(nameof(StartCountdown));
    }
    
    IEnumerator StartCountdown()
    {
        int countdownTime = 3;
        while (countdownTime > 0)
        {
            countdownText.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }
        countdownText.text = "GO";
        yield return new WaitForSeconds(1f);
        
        ReturnToGame();
    }

    void ReturnToGame()
    {
        countdownText.gameObject.SetActive(false);
        LevelManager.Instance.musicAudioSource.UnPause();
    }

    public void Home()
    {
        GameResult.Instance.ResetStats();
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
}
