using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Melanchall.DryWetMidi.Interaction;
using Unity.VisualScripting;

public class Lane : MonoBehaviour
{
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;

    [Range(1, 4)]
    public int laneNumber;

    public Animator birdAnimator;
    public Animator accuracyAnimator;
    public GameObject fruitPrefab;
    List<Fruit> fruits = new List<Fruit>();

    public GameObject noteAccuracyDisplay;
    public List<double> timeStamps = new List<double>();
    
    int spawnIndex = 0;
    public int inputIndex = 0;
    
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
                double noteHit = Math.Abs(audioTime - timeStamp);
                if (noteHit <= marginOfError)
                {
                    // Accuracy OK
                    if (noteHit > noteOkAccRange1 && noteHit <= noteOkAccRange2) // hit > 0.10, hit <= 0.20
                    {
                        accuracyAnimator.SetTrigger("Ok");
                        Hit("Ok");
                    }

                    // Accuracy GOOD
                    if (noteHit > noteGoodAccRange1 && noteHit <= noteGoodAccRange2) // hit > 0.05, hit <= 0.10
                    {
                        accuracyAnimator.SetTrigger("Good");
                        Hit("Good");
                    }
                    
                    // Accuracy PERFECT
                    if (noteHit <= notePerfectAccRange) // hit <= 0.05
                    {
                        accuracyAnimator.SetTrigger("Perfect");
                        Hit("Perfect");
                    }

                    Destroy(Instantiate(noteAccuracyDisplay), 0.2f);

                    // Tap on worm
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
                Miss();
                accuracyAnimator.SetTrigger("Miss");
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

    private void Hit(string accuracy)
    {
        ScoreManager.Hit(accuracy);
    }
        
    private void Miss()
    {
        ScoreManager.Miss();
        LifeManager.Instance.RemoveLife();
    }
}
