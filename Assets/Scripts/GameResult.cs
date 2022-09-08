using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResult : MonoBehaviour
{
    public static GameResult Instance;
    [SerializeField] GameObject gameResultMenu;
    public Sprite[] resultSprites;
    
    public static int wormsHit;
    public static int totalWorms;
    public static int highestCombo;
    public static int strikes;

    void Start()
    {
        Instance = this;
    }
    
    public void EndLevel(string result)
    {
        gameResultMenu.SetActive(true);
        Time.timeScale = 0;
        MusicManager.Instance.musicAudioSource.Pause();

        highestCombo = (highestCombo == 0 ? highestCombo = ScoreManager.comboScore : highestCombo);
        if (result == "pass")
            Instance.gameResultMenu.GetComponentInChildren<SpriteRenderer>().sprite = Instance.resultSprites[0];
        else
            Instance.gameResultMenu.GetComponentInChildren<SpriteRenderer>().sprite = Instance.resultSprites[1];

        Instance.gameResultMenu.GetComponentInChildren<TextMeshPro>().text = $"Worms hit: {wormsHit}/{totalWorms}\nHighest combo: {highestCombo}\nStrikes: {strikes}";
    }

    public void Replay()
    {
        ResetStats();
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    void ResetStats()
    {
        wormsHit = 0;
        totalWorms = 0;
        highestCombo = 0;
        strikes = 0;
        ScoreManager.wormsHit = 0;
        ScoreManager.comboScore = 0;
    }

    public void Home(int sceneId)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneId);
    }
}
