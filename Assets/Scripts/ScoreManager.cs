using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public AudioSource hitSFX;
    public AudioSource missSFX;
    public TMPro.TextMeshPro scoreText;
    public static GameObject[] strikeObjects;
    static int comboScore;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        comboScore = 0;
    }
    
    
    public static void Hit()
    {
        comboScore += 1;
        //Instance.hitSFX.Play();
    }
    public static void Miss()
    {
        Debug.Log("MISS");
        //Instance.missSFX.Play();
    }
    

    // Update is called once per frame
    void Update()
    {
        scoreText.text = comboScore.ToString();
    }
}
