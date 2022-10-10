using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    public static GameResult Instance;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] AudioSource gameResultFailSFX;
    [SerializeField] AudioSource gameResultPassSFX;
    public GameObject touchBoxes;
    
    [SerializeField] GameObject gameResultMenu;
    [SerializeField] GameObject[] gameResultMenuOverlays;
    [SerializeField] SpriteRenderer resultImage;
    public Sprite[] resultSprites;
    [SerializeField] TextMeshProUGUI statistics;
    [SerializeField] Touchbox[] birds;
    
    [SerializeField] GameObject[] coverGameObjects;

    // stats, score, points
    [SerializeField] GameObject statsOverlay;
    [SerializeField] Button statsOverlayExit;
    [SerializeField] TextMeshProUGUI statisticsAcc; // points and accuracy
    [SerializeField] TextMeshProUGUI statisticsCombo;
    [SerializeField] TextMeshProUGUI statisticsBonus;
    [SerializeField] TextMeshProUGUI statisticsPoints;
    [SerializeField] TextMeshProUGUI statisticsOk;
    [SerializeField] TextMeshProUGUI statisticsGood;
    [SerializeField] TextMeshProUGUI statisticsPerfect;


    private static readonly int Pass = Animator.StringToHash("Pass");
    private static readonly int Fail = Animator.StringToHash("Fail");
    private static readonly int FruitMissed = Animator.StringToHash("FruitMissed");

    public bool levelPass;

    void Start()
    {
        Instance = this;
        gameResultMenu.SetActive(false);
    }

    public void EndLevel(bool pass)
    {
        levelPass = pass;
        foreach (var obj in coverGameObjects)
            obj.SetActive(false);

        LevelManager.Instance.musicAudioSource.Stop();
        StartCoroutine(PlaySfx());
    }

    IEnumerator PlaySfx()
    {
        foreach (var bird in birds)
            bird.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        if (levelPass)
        {
            foreach (var bird in birds)
                bird.animator.SetTrigger(Pass);

            gameResultPassSFX.Play();
        }
        else
        {
            foreach (var bird in birds)
            {
                bird.animator.SetBool(Fail, true);
                bird.animator.SetTrigger(FruitMissed);
            }

            gameResultFailSFX.Play();
        }

        yield return new WaitForSeconds(3);
        OpenGameResult();
    }
    
    public void OpenGameResult()
    {
        audioMixer.SetFloat("Theme", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume")) * 20);
        if (levelPass)
        {
            gameResultMenuOverlays[0].SetActive(false);
            resultImage.sprite = Instance.resultSprites[1];
            CheckLevelUnlock();
        }
        else
        {
            gameResultMenuOverlays[1].SetActive(false);
            resultImage.sprite = Instance.resultSprites[0];
        }

        DisplayStats();

        gameResultMenu.SetActive(true);
        
    }
    
    void CheckLevelUnlock()
    {
        if (GlobalVariables.levels.ContainsKey(GlobalVariables.currentLevel + 1))
        {
            if (!GlobalVariables.levels[GlobalVariables.currentLevel + 1])
            {
                PlayerPrefs.SetInt($"Level{GlobalVariables.currentLevel + 1}Unlocked", 1);
                GlobalVariables.levels[GlobalVariables.currentLevel + 1] = true;
            }
        }
    }
    
    public double CalculateAccuracy()
    {
        return Math.Round((ScoreManager.score / ((double)ScoreManager.totalFruits * 4)) * 100);
    }
    public int CalculateBonusAcc()
    {
        if (CalculateAccuracy() == 100)
        {
            return 150;
        }
        else
        {
            return 0;
        }
    }

    public int CalculateBonusCombo()
    {
        if (ScoreManager.highestCombo == Lane.AmountNotes())
        {
            return 100;
        }
        else
        {
            return 0;
        }
    }

    public int CalculatePoints()
    {
        ScoreManager.levelPoints = ScoreManager.score + ScoreManager.highestCombo + (int)CalculateBonusAcc() + (int)CalculateBonusCombo();
        ScoreManager.allPoints += ScoreManager.levelPoints;
        return ScoreManager.levelPoints;
    }

    void DisplayStats() 
    {
        resultImage.gameObject.SetActive(false);

        // score with accuracy
        statisticsAcc.text = $"accuracy {CalculateAccuracy()} / 100";
        
        if (ScoreManager.score > PlayerPrefs.GetInt($"Level{GlobalVariables.currentLevel}HighScore"))
            PlayerPrefs.SetInt($"Level{GlobalVariables.currentLevel}HighScore", 0);

        // highest combo
        statisticsCombo.text = $"highest combo {ScoreManager.highestCombo}";
        
        if (levelPass)
        {
            statisticsPoints.text = levelPass ? $"points {CalculatePoints()}" : "";
            statisticsBonus.text = $"bonus {CalculateBonusAcc() + CalculateBonusCombo()}";
            
            PlayerPrefs.SetInt("PlayerPoints", PlayerPrefs.GetInt("PlayerPoints", 0) + CalculatePoints());
        }
        else
        {
            statisticsPoints.text = "";
            statisticsBonus.text = "";
        }
        
        // stats per acc
        statisticsOk.text = $"ok hits {ScoreManager.okHits}";
        statisticsGood.text = $"good hits {ScoreManager.goodHits}";
        statisticsPerfect.text = $"perfect hits {ScoreManager.perfectHits}";
    }

    public void ExitStats()
    {
        resultImage.gameObject.SetActive(true);
        statsOverlay.SetActive(false);
    }

    public void Restart()
    {
        ScoreManager.ResetStats();
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        ScoreManager.ResetStats();
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
}
