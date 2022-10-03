using System.Collections.Generic;
using UnityEngine;
using System;
using Melanchall.DryWetMidi.Interaction;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;

    [Range(1, 4)]
    public int laneNumber;

    public Animator birdAnimator;
    public GameObject fruitPrefab;
    List<Fruit> fruits = new List<Fruit>();
    public List<double> timeStamps = new List<double>();
    
    int spawnIndex = 0;
    public int inputIndex = 0;
    
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
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, LevelManager.Instance.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
    
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (LevelManager.Instance.GetAudioSourceTime() >= timeStamps[spawnIndex] - LevelManager.Instance.fruitTime)
            {
                // Inistantiating a fruit and adding it to the list of fruits.
                var fruit = Instantiate(fruitPrefab, transform);
                fruits.Add(fruit.GetComponent<Fruit>());

                // Setting the fruits assigned time so the fruit know when to spawn.
                fruit.GetComponent<Fruit>().assignedTime = (float)timeStamps[spawnIndex];
                
                // Moving on to the next fruit.
                spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = LevelManager.Instance.errorMargin;
            double audioTime = LevelManager.Instance.GetAudioSourceTime() - (LevelManager.Instance.inputDelayMilliseconds / 1000.0);

            if (Touchbox.currentLane == laneNumber && Touchbox.currentIndex == inputIndex)
            {
                if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    // Tap on fruit
                    Hit();
                    birdAnimator.SetTrigger("FruitCaught");
                    Destroy(fruits[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    // Tap on no fruit
                    //Miss();
                }
                
                Touchbox.currentLane = 0;
                Touchbox.currentIndex = 0;
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                // Missed fruit
                Miss();
                birdAnimator.SetTrigger("FruitMissed");
                inputIndex++;
            }
        }
        else
        {
            if(inputIndex > 0)
                ScoreManager.Instance.laneCheck[laneNumber - 1] = true;
        }
    }

    private void Hit()
    {
        GameResult.Instance.totalFruits++;
        ScoreManager.Hit();
    }
        
    private void Miss()
    {
        GameResult.Instance.totalFruits++;
        ScoreManager.Miss();
        LifeManager.Instance.RemoveLife();
    }
}
