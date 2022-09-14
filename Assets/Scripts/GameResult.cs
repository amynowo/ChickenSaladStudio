using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] GameObject birds;
    
    static bool gamePassed;
    public int wormsHit;
    public int totalWorms;
    public int highestCombo;
    public int lives = 3;

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

        foreach (var bird in birds.GetComponentsInChildren<BoxCollider2D>())
            bird.enabled = false;

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
        Instance.gameResultMenu.GetComponentInChildren<TextMeshPro>().text = $"Hitrate: {(int)Math.Round((double)(100 * wormsHit) / totalWorms)}%";
    }

    public void ResetStats()
    {
        wormsHit = 0;
        totalWorms = 0;
        highestCombo = 0;
        lives = 3;
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
