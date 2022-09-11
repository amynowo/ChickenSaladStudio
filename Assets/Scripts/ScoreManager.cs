using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    public static GameObject[] strikeObjects;
    public static int wormsHit;
    public static int comboScore;
    
    public bool[] laneCheck;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        laneCheck = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            laneCheck[i] = false;
        }
    }
    
    
    public static void Hit()
    {
        wormsHit++;
        GameResult.Instance.wormsHit++;
        comboScore++;
        //Instance.hitSFX.Play();
    }
    public static void Miss()
    {
        if (comboScore > GameResult.Instance.highestCombo)
            GameResult.Instance.highestCombo = comboScore;
        comboScore = 0;
        //Instance.missSFX.Play();
    }

    void CheckGameOver()
    {
        if (laneCheck.All(x => x))
        {
            Invoke(nameof(FinishGame), 2);
        }
    }
    
    void FinishGame()
    {
        if (GameResult.Instance.totalWorms > 0)
            GameResult.Instance.GetComponent<GameResult>().EndLevel(true);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = wormsHit.ToString();
        CheckGameOver();
    }
}
