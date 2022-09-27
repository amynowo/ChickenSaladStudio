using System;
using System.Collections;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public SpriteRenderer backgroundImageObject;
    public Sprite[] backgroundImages;
    [SerializeField] private AudioMixer audioMixer;
    public AudioSource musicAudioSource;
    public AudioClip[] audioClips;
    public Lane[] lanes;
    public float songDelaySeconds;
    public double errorMargin; // in seconds
    public int inputDelayMilliseconds;

    //public string fileName;
    public float wormTime;
    public float wormSpawnY;
    public float wormTapY;
    public float wormDespawnY
    {
        get { return wormTapY - (wormSpawnY - wormTapY); }
    }

    public MidiFile midiFile;
    
    // Start is called before the first frame update
    void Start()
    {
        audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        audioMixer.SetFloat("Theme", -80);
        musicAudioSource.clip = audioClips[GlobalVariables.currentLevel - 1];

        backgroundImageObject.sprite = backgroundImages[GlobalVariables.currentLevel - 1];
        
        Instance = this;
        StartCoroutine(nameof(ReadFromFile));
    }

    IEnumerator ReadFromFile()
    {
        string fileName = $"lvl_{GlobalVariables.currentLevel}.mid";
        var filePath = Path.Combine(Application.streamingAssetsPath, fileName);
        var persistentFilePath = Path.Combine(Application.persistentDataPath, "MIDI", fileName);
        byte[] midiByteData;

        if (filePath.Contains("://") || filePath.Contains(":///"))
        {
            if (Directory.Exists(Path.Combine(Application.persistentDataPath, "MIDI")))
            {
                midiFile = MidiFile.Read(persistentFilePath);
            }
            else
            {
                using (UnityWebRequest request = UnityWebRequest.Get(filePath))
                {
                    yield return request.SendWebRequest();
                    midiByteData = request.downloadHandler.data;
                }

                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "MIDI"));
                File.WriteAllBytes(persistentFilePath, midiByteData);
                
                midiFile = MidiFile.Read(persistentFilePath);
            }
        }
        else
        {
            midiFile = MidiFile.Read(filePath);
        }

        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
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
