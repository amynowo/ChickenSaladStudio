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
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
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
    

    // Update is called once per frame
    void Update()
    {
        scoreText.text = wormsHit.ToString();
    }
}
