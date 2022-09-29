using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.75f);
            audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
            audioMixer.SetFloat("Theme", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        }
        else
        {
            audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
            audioMixer.SetFloat("Theme", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        }

        if (!PlayerPrefs.HasKey("VolumeMute"))
        {
            PlayerPrefs.SetInt("VolumeMute", 0);
        }
        else
        {
            bool volumeMute = PlayerPrefs.GetInt("VolumeMute") == 1;
            audioMixer.SetFloat("Master", volumeMute ? -80.0f : 0.0f);
        }
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }
    
    public void OpenSettings()
    {
        SceneManager.LoadScene("SettingsScene");
    }
    
    public IEnumerator LoadMidiFiles()
    {
        // Check if the device is Android.
        if (Application.streamingAssetsPath.Contains("://") || Application.streamingAssetsPath.Contains(":///"))
        {
            var persistentMidiDataPath = Path.Combine(Application.persistentDataPath, "MIDI");
            
            // Check if the MIDI directory exists.
            if (!Directory.Exists(persistentMidiDataPath))
                Directory.CreateDirectory(persistentMidiDataPath);
            
            // If the directory exists, check if it conaints all the MIDI files according to the levels.
            foreach (var level in GlobalVariables.levels)
            {
                var midiFileName = $"lvl_{level.Key}.mid";
                var midiPersistentDataFilePath = Path.Combine(persistentMidiDataPath, midiFileName);
                
                // Check if the MIDI file is present in persistent data - if not, fetch it from StreamingAssets. 
                if (!File.Exists(midiPersistentDataFilePath))
                {
                    var midiStreamingAssetsPath = Path.Combine(Application.streamingAssetsPath, midiFileName);
                    byte[] midiByteData;
                    
                    using (UnityWebRequest request = UnityWebRequest.Get(midiStreamingAssetsPath))
                    {
                        yield return request.SendWebRequest();
                        midiByteData = request.downloadHandler.data;
                    }

                    File.WriteAllBytes(persistentMidiDataPath, midiByteData);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}