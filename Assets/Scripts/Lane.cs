using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Melanchall.DryWetMidi.Interaction;
using UnityEditor;
using UnityEngine.Android;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;
    public KeyCode input;
    public Touch touch;
    public GameObject wormPrefab;
    List<Worm> worms = new List<Worm>();
    public List<double> timeStamps = new List<double>();
    
    int spawnIndex = 0;
    int inputIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, MusicManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
    
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (MusicManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - MusicManager.Instance.wormTime)
            {
                // Inistantiating a worm and adding it to the list of worms
                var worm = Instantiate(wormPrefab, transform);
                worms.Add(worm.GetComponent<Worm>());
                
                // Setting the worms assigned time so the worm know when to spawn
                worm.GetComponent<Worm>().assignedTime = (float)timeStamps[spawnIndex];
                
                // Moving on to the next worm
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = MusicManager.Instance.errorMargin;
            double audioTime = MusicManager.GetAudioSourceTime() - (MusicManager.Instance.inputDelayMilliseconds / 1000.0);

            Debug.Log(Input.touchCount);
            
            if (Input.GetKeyDown(input))
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    print($"Hit on {inputIndex} note");
                    Destroy(worms[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                print($"Missed {inputIndex} note");
                inputIndex++;
            }
        }
    }
    
    
    private void Hit()
    {
        ScoreManager.Hit();
    }
        
    private void Miss()
    {
        ScoreManager.Miss();
    }
}
