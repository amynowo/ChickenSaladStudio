using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class GlobalVariables : MonoBehaviour
{
    private bool notFirstObject = false;

    public static Dictionary<int, bool> levels = new() { { 1, true }, { 2, false }, { 3, false }, { 4, false }};
    public static int currentLevel = 1;
    
    private IEnumerator Start()
    {
        UpdateLevelStates();
        GetHighscores();
        GetBirdSkins();
        GetCheats();
        StartCoroutine(nameof(LoadMidiFiles));
        yield break;
    }

    void UpdateLevelStates()
    {
        if (!PlayerPrefs.HasKey("Level2Unlocked"))
        {
            PlayerPrefs.SetInt("Level2Unlocked", 0);
            PlayerPrefs.SetInt("Level3Unlocked", 0);
            PlayerPrefs.SetInt("Level4Unlocked", 0);
        }
        else
        {
            levels[2] = PlayerPrefs.GetInt("Level2Unlocked") == 1;
            levels[3] = PlayerPrefs.GetInt("Level3Unlocked") == 1;
            levels[4] = PlayerPrefs.GetInt("Level4Unlocked") == 1;
        }
    }

    void GetHighscores()
    {
        if (!PlayerPrefs.HasKey("Level1Highscore"))
        {
            PlayerPrefs.SetInt("Level1Highscore", 0);
            PlayerPrefs.SetInt("Level2Highscore", 0);
            PlayerPrefs.SetInt("Level3Highscore", 0);
            PlayerPrefs.SetInt("Level4Highscore", 0);
        }
    }

    void GetBirdSkins()
    {
        PlayerPrefs.DeleteKey("Bird1Skin");
        if (!PlayerPrefs.HasKey("Bird1Skin"))
        {
            PlayerPrefs.SetString("Bird1Skin", "Default");
            PlayerPrefs.SetString("Bird2Skin", "Halloween");
            PlayerPrefs.SetString("Bird3Skin", "Christmas");
            PlayerPrefs.SetString("Bird4Skin", "Sunglasses");
            
            // All birds
            PlayerPrefs.SetInt("Default", 0);
            PlayerPrefs.SetInt("Halloween", 1);
            PlayerPrefs.SetInt("Winter", 2);
            PlayerPrefs.SetInt("Christmas", 3);
            
            // Bird 1 (Chico)
            PlayerPrefs.SetInt("Hat", 4);
            PlayerPrefs.SetInt("Space", 5);
            
            // Bird 2 (Guava)
            PlayerPrefs.SetInt("Flower", 4);
            PlayerPrefs.SetInt("HeartGlasses", 5);
            
            // Bird 3 (Loki)
            PlayerPrefs.SetInt("Gangsta", 4);
            PlayerPrefs.SetInt("Halster", 5);
            
            // Bird 4 (Bolo)
            PlayerPrefs.SetInt("Choker", 4);
            PlayerPrefs.SetInt("Sunglasses", 5);
        }
    }

    void GetCheats()
    {
        if (!PlayerPrefs.HasKey("GodModeCheat"))
        {
            PlayerPrefs.SetInt("GodModeCheatLocked", 1);
            PlayerPrefs.SetInt("GodModeCheat", 0);
            
            PlayerPrefs.SetInt("ShortcutCheatLocked", 1);
            PlayerPrefs.SetInt("ShortcutCheat", 0);
        }
    }
    
    void Awake()
    {
        foreach (GameObject otherGameObject in GameObject.FindGameObjectsWithTag("GlobalVariables"))
        {
            if (otherGameObject.scene.buildIndex == -1)
                notFirstObject = true;
        }
        
        if (notFirstObject == true)
            Destroy(gameObject);
        DontDestroyOnLoad(transform.gameObject);
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
            foreach (var level in levels)
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

                    File.WriteAllBytes(midiPersistentDataFilePath, midiByteData);
                }
            }
        }
    }
}
