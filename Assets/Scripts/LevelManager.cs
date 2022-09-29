using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Audio;

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
