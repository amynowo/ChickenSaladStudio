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
    [SerializeField] SpriteRenderer resultImage;
    public Sprite[] resultSprites;
    [SerializeField] GameObject pauseButton;
    
    static bool gamePassed;
    public int wormsHit;
    public int totalWorms;
    public int highestCombo;
    public int strikes;

    void Start()
    {
        Instance = this;
        gameResultMenu.SetActive(false);
    }
    
    public void EndLevel(bool pass)
    {
        pauseButton.SetActive(false);
        gameResultMenu.SetActive(true);
        Time.timeScale = 0;
        MusicManager.Instance.musicAudioSource.Pause();

        gamePassed = pass;
        SaveHighscore();
    }

    void SaveHighscore()
    {
        bool newHighscore = false;
        int currentHighscore = PlayerPrefs.GetInt("highscore", 0);
        if (currentHighscore < wormsHit)
        {
            PlayerPrefs.SetInt("highscore", wormsHit);
            newHighscore = true;
        }

        DisplayStats(newHighscore);
    }

    void DisplayStats(bool newHighscore)
    {
        highestCombo = (highestCombo == 0 ? highestCombo = ScoreManager.comboScore : highestCombo);
        resultImage.sprite = Instance.resultSprites[(!gamePassed ? 0 : 1)];
        Instance.gameResultMenu.GetComponentInChildren<TextMeshPro>().text = $"{(newHighscore ? "New high score!\n" : $"Current high score: {PlayerPrefs.GetInt("highscore")}\n")}Worms hit: {wormsHit}/{totalWorms}\nHighest combo: {highestCombo}\nStrikes: {strikes}";
    }

    public void ResetStats()
    {
        wormsHit = 0;
        totalWorms = 0;
        highestCombo = 0;
        strikes = 0;
        ScoreManager.wormsHit = 0;
        ScoreManager.comboScore = 0;
    }
    
    public void Restart()
    {
        ResetStats();
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        ResetStats();
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
}
