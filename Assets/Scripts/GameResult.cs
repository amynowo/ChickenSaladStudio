using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    public static GameResult Instance;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] AudioSource gameResultFailSFX;
    [SerializeField] AudioSource gameResultPassSFX;
    public Animator[] birdAnimators;
    
    [SerializeField] GameObject gameResultMenu;
    [SerializeField] GameObject[] gameResultMenuOverlays;
    [SerializeField] SpriteRenderer resultImage;
    public Sprite[] resultSprites;
    [SerializeField] TextMeshProUGUI statistics;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject birds;
    
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
        LevelManager.Instance.musicAudioSource.Stop();
        StartCoroutine(PlaySfx(pass));
    }
    
    IEnumerator PlaySfx(bool pass)
    {
        foreach (var bird in birds.GetComponentsInChildren<BoxCollider2D>())
            bird.enabled = false;

        if (pass)
        {
            foreach (var birdAnimator in birdAnimators)
                birdAnimator.SetTrigger("Pass");

            gameResultPassSFX.Play();
        }
        else
        {
            foreach (var birdAnimator in birdAnimators)
            {
                birdAnimator.SetBool("Fail", true);
                birdAnimator.SetTrigger("FruitMissed");
            }

            gameResultFailSFX.Play();
        }

        yield return new WaitForSeconds(3);
        OpenGameResult(pass);
    }
    
    public void OpenGameResult(bool pass)
    {
        audioMixer.SetFloat("Theme", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        if (pass)
        {
            gameResultMenuOverlays[0].SetActive(false);
            resultImage.sprite = Instance.resultSprites[1];
        }
        else
        {
            gameResultMenuOverlays[1].SetActive(false);
            resultImage.sprite = Instance.resultSprites[0];
        }
        
        gameResultMenu.SetActive(true);
        
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
        statistics.text = $"hitrate: {(int)Math.Round((double)(100 * wormsHit) / totalWorms)}%";
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
