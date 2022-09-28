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

    public static Dictionary<int, bool> levels = new() { { 1, true }, { 2, true }, { 3, true }, { 4, true } };
    public static int currentLevel = 1;

    private IEnumerator Start()
    {
        Debug.Log("Start");
        StartCoroutine(nameof(LoadMidiFiles));
        yield break;
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
