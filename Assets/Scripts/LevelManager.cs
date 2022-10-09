using System;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public Image backgroundImageObject;
    public Sprite[] backgroundImages;
    
    [SerializeField] private AudioMixer audioMixer;
    public AudioSource musicAudioSource;
    public AudioClip[] audioClips;
    
    public Lane[] lanes;
    
    public float songDelaySeconds;
    public double errorMargin; // in seconds
    public int inputDelayMilliseconds;
    
    public float fruitTime;
    public float fruitSpawnY;
    public float fruitTapY;
    public float fruitDespawnY
    {
        get { return fruitTapY - (fruitSpawnY - fruitTapY); }
    }

    public MidiFile midiFile;

    public GameObject hitBar;
    
    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        audioMixer.SetFloat("Theme", -80);
        musicAudioSource.clip = audioClips[GlobalVariables.currentLevel - 1];

        backgroundImageObject.sprite = backgroundImages[GlobalVariables.currentLevel - 1];

        var screenWidth = Screen.currentResolution.width;
        var screenHeight = Screen.currentResolution.height;
        
        if (screenWidth > screenHeight)
        {
            fruitTapY = (float)-3.25;
        }
        else
        {
            decimal ratio = Decimal.Parse(screenWidth.ToString()) / Decimal.Parse(screenHeight.ToString());
            decimal calc = Decimal.Subtract(Decimal.Multiply(Decimal.Parse("3,6111095061735536"), ratio), Decimal.Parse("4,6666657037041315"));
            fruitTapY = (float)calc;
        }

        Instance = this;
        hitBar.transform.SetPositionAndRotation(new Vector3(0.0f, fruitTapY), new Quaternion());
        
        if (GlobalVariables.currentLevel != 5)
            GetDataFromMidi();
    }
    
    public void GetDataFromMidi()
    {
        string fileName = $"lvl_{GlobalVariables.currentLevel}.mid";
        var filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        var persistentFilePath = Path.Combine(Application.persistentDataPath, "MIDI", fileName);
        
        if (filePath.Contains("://") || filePath.Contains(":///"))
            midiFile = MidiFile.Read(persistentFilePath);
        else
            midiFile = MidiFile.Read(filePath);
        
        var notes = midiFile.GetNotes();
        var array = new Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes)
            lane.SetTimeStamps(array);

        Invoke(nameof(PlaySong), songDelaySeconds);
    }

    public void PlaySong()
    {
        musicAudioSource.Play();
    }

    public double GetAudioSourceTime()
    {
        return (double)musicAudioSource.timeSamples / musicAudioSource.clip.frequency;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
