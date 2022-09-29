using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
public class Sound : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject volumeOnButton;
    [SerializeField] private GameObject volumeOffButton;
    [SerializeField] private Slider musicVolumeSlider;
    
    private bool volumeMute;
    
    // Start is called before the first frame update
    void Start()
    {
        SetMusicVolume();
    }
    
    void SetMusicVolume()
    {
        volumeMute = PlayerPrefs.GetInt("VolumeMute") == 1;
        
        volumeOffButton.SetActive(volumeMute);
        volumeOnButton.SetActive(!volumeMute);

        audioMixer.SetFloat("Master", volumeMute ? -80.0f : 0.0f);
        musicVolumeSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume"));
    }

    public void Mute()
    { 
        volumeOnButton.SetActive(volumeMute);
        volumeOffButton.SetActive(!volumeMute);
        volumeMute = !volumeMute;
        PlayerPrefs.SetInt("VolumeMute", volumeMute ? 1 : 0);
        audioMixer.SetFloat("Master", volumeMute ? -80.0f : 0.0f);
    }
    
    public void UpdateValueOnChange(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        audioMixer.SetFloat("Theme", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
    }

    public void Back()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
