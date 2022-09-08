using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResult : MonoBehaviour
{
    public static GameResult Instance;
    [SerializeField] GameObject gameResultMenu;

    void Start()
    {
        Instance = this;
    }
    
    public void EndLevel()
    {
        gameResultMenu.SetActive(true);
        Time.timeScale = 0;
        MusicManager.Instance.musicAudioSource.Pause();
    }

    public void Replay()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        Time.timeScale = 1;
    }

    public void Home(int sceneId)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneId);
    }
}
