using System.Collections.Generic;
using UnityEngine;
using System;
using Melanchall.DryWetMidi.Interaction;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;

    [Range(1, 4)]
    public int laneNumber;
    
    public GameObject wormPrefab;
    List<Worm> worms = new List<Worm>();
    public List<double> timeStamps = new List<double>();
    
    int spawnIndex = 0;
    int inputIndex = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    
    public void SetTimeStamps(Note[] array)
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
            
            if (Touchbox.currentLane == laneNumber)
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    // Tap on worm
                    Hit();
                    Destroy(worms[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    // Tap on no worm
                    Miss();
                }
                Touchbox.currentLane = 0;
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                // Missed worm
                Miss();
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
        StrikeManager.AddStrike();
    }
}
