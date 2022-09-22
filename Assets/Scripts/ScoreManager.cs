using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    //public TMPro.TextMeshPro scoreText;
    public static GameObject[] strikeObjects;
    public static int wormsHit;
    public static int comboScore;
    
    public bool[] laneCheck;
    private bool gameFinished = false;
    
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
        if (!gameFinished && laneCheck.All(x => x) && GameResult.Instance.totalWorms > 0)
        {
            gameFinished = true;
            Invoke(nameof(FinishGame), 1.5f);
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
        GetComponent<TextMeshPro>().text = wormsHit.ToString();
        CheckGameOver();
    }
}
