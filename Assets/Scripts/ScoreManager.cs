using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TextMeshPro scoreText;
    public static GameObject[] strikeObjects;

    public static int totalFruits;
    public static int fruitsHit;
    public static int okHits;
    public static int goodHits;
    public static int perfectHits;
    public static int score;
    public static int currentCombo;
    public static int highestCombo;
    public static int bonusScore;
    public static int levelPoints; // points per level
    public static int allPoints; // total points

    public static int amountFruitPerLevel; // total amount of fruit per level
    
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

        ResetStats();
    }

    public static void ResetStats()
    {
        totalFruits = 0;
        fruitsHit = 0;
        okHits = 0;
        goodHits = 0;
        perfectHits = 0;
        score = 0;
        currentCombo = 0;
        highestCombo = 0;
        bonusScore = 0;
        levelPoints = 0;
}
    
    public static void Hit(string accuracy)
    {
        fruitsHit++;
        totalFruits++;
        currentCombo++;

        if (accuracy == "Ok")
        {
            okHits++;
            score += 1;
        }
        else if (accuracy == "Good")
        {
            goodHits++;
            score += 2;
        }
        else
        {
            perfectHits++;
            score += 4;
        }

        //Instance.hitSFX.Play();
    }
    public static void Miss()
    {
        // resets combo
        totalFruits++;
        if (currentCombo > highestCombo)
            highestCombo = currentCombo;
        currentCombo = 0;
        //Instance.missSFX.Play();
    }

    void CheckGameOver()
    {
        if (!gameFinished && laneCheck.All(x => x) && totalFruits > 0)
        {
            gameFinished = true;
            if (currentCombo > highestCombo)
                highestCombo = currentCombo; // sets highest combo
            
            Invoke(nameof(FinishGame), 1.5f);
        }
    }
    
    void FinishGame()
    {
        if (totalFruits > 0)
            GameResult.Instance.GetComponent<GameResult>().EndLevel(true);
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();
        CheckGameOver();
    }
}
