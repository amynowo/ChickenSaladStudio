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
            PlayerPrefs.SetString("Bird1Skin", "Bird1Default");
            PlayerPrefs.SetString("Bird2Skin", "Bird2Default");
            PlayerPrefs.SetString("Bird3Skin", "Bird3Default");
            PlayerPrefs.SetString("Bird4Skin", "Bird4Default");
            
            // Bird 1 (Chico)
            PlayerPrefs.SetInt("Bird1DefaultUnlocked", 1);
            PlayerPrefs.SetInt("Bird1HalloweenUnlocked", 0);
            PlayerPrefs.SetInt("Bird1WinterUnlocked", 0);
            PlayerPrefs.SetInt("Bird1ChristmasUnlocked", 0);
            PlayerPrefs.SetInt("Bird1HatUnlocked", 0);
            PlayerPrefs.SetInt("Bird1SpaceUnlocked", 0);
            
            PlayerPrefs.SetInt("Bird1Default", 0);
            PlayerPrefs.SetInt("Bird1Halloween", 1);
            PlayerPrefs.SetInt("Bird1Winter", 2);
            PlayerPrefs.SetInt("Bird1Christmas", 3);
            PlayerPrefs.SetInt("Bird1Hat", 4);
            PlayerPrefs.SetInt("Bird1Space", 5);
            
            // Bird 2 (Guava)
            PlayerPrefs.SetInt("Bird2DefaultUnlocked", 1);
            PlayerPrefs.SetInt("Bird2HalloweenUnlocked", 0);
            PlayerPrefs.SetInt("Bird2WinterUnlocked", 0);
            PlayerPrefs.SetInt("Bird2ChristmasUnlocked", 0);
            PlayerPrefs.SetInt("Bird2FlowerUnlocked", 0);
            PlayerPrefs.SetInt("Bird2HeartGlassesUnlocked", 0);
            
            PlayerPrefs.SetInt("Bird2Default", 0);
            PlayerPrefs.SetInt("Bird2Halloween", 1);
            PlayerPrefs.SetInt("Bird2Winter", 2);
            PlayerPrefs.SetInt("Bird2Christmas", 3);
            PlayerPrefs.SetInt("Bird2Flower", 4);
            PlayerPrefs.SetInt("Bird2HeartGlasses", 5);
            
            // Bird 3 (Loki)
            PlayerPrefs.SetInt("Bird3DefaultUnlocked", 1);
            PlayerPrefs.SetInt("Bird3HalloweenUnlocked", 0);
            PlayerPrefs.SetInt("Bird3WinterUnlocked", 0);
            PlayerPrefs.SetInt("Bird3ChristmasUnlocked", 0);
            PlayerPrefs.SetInt("Bird3GangstaUnlocked", 0);
            PlayerPrefs.SetInt("Bird3HalsterUnlocked", 0);
            
            PlayerPrefs.SetInt("Bird3Default", 0);
            PlayerPrefs.SetInt("Bird3Halloween", 1);
            PlayerPrefs.SetInt("Bird3Winter", 2);
            PlayerPrefs.SetInt("Bird3Christmas", 3);
            PlayerPrefs.SetInt("Bird3Gangsta", 4);
            PlayerPrefs.SetInt("Bird3Halster", 5);
            
            // Bird 4 (Bolo)
            PlayerPrefs.SetInt("Bird4DefaultUnlocked", 1);
            PlayerPrefs.SetInt("Bird4HalloweenUnlocked", 0);
            PlayerPrefs.SetInt("Bird4WinterUnlocked", 0);
            PlayerPrefs.SetInt("Bird4ChristmasUnlocked", 0);
            PlayerPrefs.SetInt("Bird4ChokerUnlocked", 0);
            PlayerPrefs.SetInt("Bird4SunglassesUnlocked", 0);
            
            PlayerPrefs.SetInt("Bird4Default", 0);
            PlayerPrefs.SetInt("Bird4Halloween", 1);
            PlayerPrefs.SetInt("Bird4Winter", 2);
            PlayerPrefs.SetInt("Bird4Christmas", 3);
            PlayerPrefs.SetInt("Bird4Choker", 4);
            PlayerPrefs.SetInt("Bird4Sunglasses", 5);
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
