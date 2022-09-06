using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelaySeconds;
    public double errorMargin; // in seconds
    public int inputDelayMilliseconds;

    public string fileLocation;
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
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
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
    void Update()
    {
        
    }
}
