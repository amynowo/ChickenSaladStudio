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

    // Accuracy prefabs
    public GameObject missDisplay;
    public GameObject okAccDisplay;
    public GameObject goodAccDisplay;
    public GameObject perfecAcctDisplay;
    GameObject noteAccuracyDisplay;

    List<Worm> worms = new List<Worm>();
    public List<double> timeStamps = new List<double>();
    
    int spawnIndex = 0;
    int inputIndex = 0;

    // range within error margin to get 'ok' accuracy
    public double noteOkAccRange1 = 0.10;
    public double noteOkAccRange2 = 0.20;

    // range within error margin to get 'good' accuracy
    public double noteGoodAccRange1 = 0.05; 
    public double noteGoodAccRange2 = 0.10;

    // range within error margin to get 'perfect' accuracy
    public double notePerfectAccRange = 0.05; 

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
    
    // Update is called once per frame, fixed update is called at set time
    void FixedUpdate()
    {
        if (spawnIndex < timeStamps.Count)
        {
            if (LevelManager.Instance.GetAudioSourceTime() >= timeStamps[spawnIndex] - LevelManager.Instance.wormTime)
            {
                // Instantiating a worm and adding it to the list of worms
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
            double marginOfError = LevelManager.Instance.errorMargin;
            double audioTime = LevelManager.Instance.GetAudioSourceTime() - (LevelManager.Instance.inputDelayMilliseconds / 1000.0);
            
            if (Touchbox.currentLane == laneNumber)
            {
                double noteHit = Math.Abs(audioTime - timeStamp);
                if (noteHit <= marginOfError)
                {
                    // Accuracy OK
                    if (noteHit > noteOkAccRange1 && noteHit <= noteOkAccRange2) // hit > 0.10, hit <= 0.20
                    {
                        Debug.Log("OK!: " + noteHit);
                        noteAccuracyDisplay = okAccDisplay;
                    }

                    // Accuracy GOOD
                    if (noteHit > noteGoodAccRange1 && noteHit <= noteGoodAccRange2) // hit > 0.05, hit <= 0.10
                    {
                        Debug.Log("Good!: " + noteHit);
                        noteAccuracyDisplay = goodAccDisplay;
                    }

                    // Accuracy PERFECT
                    if (noteHit <= notePerfectAccRange) // hit <= 0.05
                    {
                        Debug.Log("Perfect!: " + noteHit);
                        noteAccuracyDisplay = perfecAcctDisplay;
                    }

                    Destroy(Instantiate(noteAccuracyDisplay), 0.2f);

                    // Tap on worm
                    Hit();
                    Destroy(worms[inputIndex].gameObject);
                    inputIndex++;
                }
                else
                {
                    // Tap on no worm
                    //Miss();
                }
                Touchbox.currentLane = 0;
            }
            if (timeStamp + marginOfError <= audioTime)
            {
                // Missed worm
                Debug.Log("Miss");
                Destroy(Instantiate(missDisplay), 0.2f);
                Miss();
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
        GameResult.Instance.totalWorms++;
        ScoreManager.Hit();
    }
        
    private void Miss()
    {
        GameResult.Instance.totalWorms++;
        ScoreManager.Miss();
        LifeManager.Instance.RemoveLife();
    }
}
