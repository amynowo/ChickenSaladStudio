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
    [SerializeField] Button advancedStatButton;
    [SerializeField] GameObject advancedStatsOverlay;
    [SerializeField] TextMeshProUGUI statistics;
    [SerializeField] GameObject birds;
    
    [SerializeField] GameObject[] coverGameObjects;
    
    private static readonly int Pass = Animator.StringToHash("Pass");
    private static readonly int Fail = Animator.StringToHash("Fail");
    private static readonly int FruitMissed = Animator.StringToHash("FruitMissed");

    void Start()
    {
        Instance = this;
        gameResultMenu.SetActive(false);
        advancedStatsOverlay.SetActive(false);
    }

    public void EndLevel(bool pass)
    {
        foreach (var obj in coverGameObjects)
            obj.SetActive(false);
        
        LevelManager.Instance.musicAudioSource.Stop();
        StartCoroutine(PlaySfx(pass));
        AdvancedStats();
    }

    public void AdvancedStats()
    {
        advancedStatsOverlay.SetActive(true);
        resultImage.gameObject.SetActive(false);
    }

    public void AdvancedStatsBack()
    {
        resultImage.gameObject.SetActive(true);
        advancedStatsOverlay.SetActive(false);
    }

    IEnumerator PlaySfx(bool pass)
    {
        foreach (var bird in birds.GetComponentsInChildren<BoxCollider2D>())
            bird.enabled = false;

        if (pass)
        {
            foreach (var bird in GameObject.FindGameObjectsWithTag($"{PlayerPrefs.GetString("BirdSkin")}Skin"))
                bird.GetComponent<Animator>().SetTrigger(Pass);

            gameResultPassSFX.Play();
        }
        else
        {
            foreach (var bird in GameObject.FindGameObjectsWithTag($"{PlayerPrefs.GetString("BirdSkin")}Skin"))
            {
                bird.GetComponent<Animator>().SetBool(Fail, true);
                bird.GetComponent<Animator>().SetTrigger(FruitMissed);
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
            CheckLevelUnlock();
        }
        else
        {
            gameResultMenuOverlays[1].SetActive(false);
            resultImage.sprite = Instance.resultSprites[0];
        }
        
        gameResultMenu.SetActive(true);
        
        DisplayStats();
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
    
    void DisplayStats()
    {
        Debug.Log($"Ok: {ScoreManager.okHits} | Good: {ScoreManager.goodHits} | Perfect: {ScoreManager.perfectHits} | Score: {ScoreManager.score} | Highest combo: {ScoreManager.highestCombo} | Points: {ScoreManager.score + ScoreManager.highestCombo}");
        statistics.text = $"accuracy: {Math.Round((ScoreManager.score / ((double)ScoreManager.totalFruits * 4)) * 100)}%";
        
        if (ScoreManager.score > PlayerPrefs.GetInt($"Level{GlobalVariables.currentLevel}HighScore"))
            PlayerPrefs.SetInt($"Level{GlobalVariables.currentLevel}HighScore", 0);
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
