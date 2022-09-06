using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    static int comboScore;
    static int strikeCount;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        comboScore = 0;
    }
    
    
    public static void Hit()
    {
        comboScore += 1;
        //Instance.hitSFX.Play();
    }
    public static void Miss()
    {
        strikeCount++;
        //Instance.missSFX.Play();    
    }
    

    // Update is called once per frame
    void Update()
    {
        scoreText.text = comboScore.ToString();
    }
}
