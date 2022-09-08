using System.Collections;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using Unity.VisualScripting;
using UnityEngine.Networking;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    public AudioSource musicAudioSource;
    public Lane[] lanes;
    public float songDelaySeconds;
    public double errorMargin; // in seconds
    public int inputDelayMilliseconds;

    public string fileName;
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

    public /*static*/ MidiFile midiFile;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ReadFromFile();
    }

    public void ReadFromFile()
    {
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
                UnityWebRequest www = UnityWebRequest.Get(filePath);
                www.SetRequestHeader("Cache-Control", "max-age=0, no-cache, no-store");
                www.SetRequestHeader("Pragma", "no-cache");
                while (!www.SendWebRequest().isDone) { ;}
                midiByteData = www.downloadHandler.data;

                Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "MIDI"));
                File.WriteAllBytes(persistentFilePath, midiByteData);
                
                www.Dispose();
                
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

    public /*static*/ double GetAudioSourceTime()
    {
        return (double)musicAudioSource.timeSamples / musicAudioSource.clip.frequency;
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
