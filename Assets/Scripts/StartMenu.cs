using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    
    void Awake()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.75f);
            audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
