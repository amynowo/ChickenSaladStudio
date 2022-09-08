using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StrikeManager : MonoBehaviour
{
    public static StrikeManager Instance;
    public Sprite[] strikeSprites;
    static int strikeCount = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public static void AddStrike()
    {
        Instance.GetComponentsInChildren<SpriteRenderer>()[strikeCount].sprite = Instance.strikeSprites[1];
        strikeCount++;
        if (strikeCount == 3)
        {
            Time.timeScale = 0;
            MusicManager.Instance.musicAudioSource.Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
