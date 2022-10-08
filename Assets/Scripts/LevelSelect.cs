using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;

public class LevelSelect : MonoBehaviour
{
    public GameObject[] levels;
    private int currentLevel;
    
    public GameObject buttonLeft;
    public GameObject buttonRight;
    
    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0;
        buttonLeft.SetActive(false);
        SetLevelAvailability();
    }

    void SetLevelAvailability()
    {
        foreach (var level in GlobalVariables.levels)
        {
            var levelObject = levels[level.Key - 1];
            if (PlayerPrefs.GetInt("ShortcutCheat") == 1)
            {
                levelObject.GetComponentInChildren<Transform>().Find("Unlocked").gameObject.SetActive(true);
                levelObject.GetComponentInChildren<Transform>().Find("Locked").gameObject.SetActive(false);
            }
            else
            {
                levelObject.GetComponentInChildren<Transform>().Find("Unlocked").gameObject.SetActive(level.Value);
                levelObject.GetComponentInChildren<Transform>().Find("Locked").gameObject.SetActive(!level.Value);
            }
        }

        GetComponent<Transform>().GetComponentsInChildren<Transform>().First(x => x.gameObject.name == $"Level 1").gameObject.SetActive(true);
    }

    public void Left()
    {
        levels[currentLevel].SetActive(false);
        currentLevel--;
        levels[currentLevel].SetActive(true);
    }
    
    public void Right()
    {
        levels[currentLevel].SetActive(false);
        currentLevel++;
        levels[currentLevel].SetActive(true);
    }

    public void StartLevel(int level)
    {
        GlobalVariables.currentLevel = level;
        SceneManager.LoadScene("GameScene");
    }
    
    public void Home()
    {
        SceneManager.LoadScene("StartScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLevel == 0)
        {
            buttonLeft.SetActive(false);
            buttonRight.SetActive(true);
        }
        else if (currentLevel == levels.Length - 1)
        {
            buttonLeft.SetActive(true);
            buttonRight.SetActive(false);
        }
        else
        {
            buttonLeft.SetActive(true);
            buttonRight.SetActive(true);
        }
    }
}
