using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;
using System.Linq;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelaySeconds;
    public double errorMargin; // in seconds
    public int inputDelayMilliseconds;

    public string fileName;
    public float wormTime;
    public float wormSpawnY;
    public float wormTapY;
    public float wormDespawnY
    {
        get
        {
            return wormTapY - (wormSpawnY - wormTapY);
        }
    }

    public static MidiFile midiFile;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ReadFromFile();
    }


    private void ReadFromFile()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            midiFile = MidiFile.Read(Path.Combine(Application.persistentDataPath, fileName));
        }
        else
        {
            midiFile = MidiFile.Read(Path.Combine(Application.dataPath, fileName));
        }
        
        /*string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        Debug.Log(filePath);
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityWebRequest www = UnityWebRequest.Get(fileName);
            www.SendWebRequest();
            while (!www.isDone)
            {
            }
            Debug.Log(www.result);
            Debug.Log(www.downloadHandler.data.Length);
            File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "MIDI", fileName), www.downloadHandler.data);
            midiFile = MidiFile.Read(Path.Combine(Application.persistentDataPath, "MIDI", fileName));
            
        }
        else
        {
            midiFile = MidiFile.Read(filePath);
        }*/
        
        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes)
            lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelaySeconds);
    }

    public void StartSong()
    {
        audioSource.Play();
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
