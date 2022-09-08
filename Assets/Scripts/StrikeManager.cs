using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StrikeManager : MonoBehaviour
{
    public static StrikeManager Instance;
    public Sprite[] strikeSprites;
    static int strikeCount;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        strikeCount = 0;
    }

    public void AddStrike()
    {
        if (strikeCount <= 3)
            Instance.GetComponentsInChildren<SpriteRenderer>()[strikeCount].sprite = Instance.strikeSprites[1];

        strikeCount++;
        if (strikeCount == 3)
        {
            GameResult.Instance.GetComponent<GameResult>().EndLevel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
