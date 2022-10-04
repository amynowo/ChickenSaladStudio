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
    public GameObject scrollbar;
    private float scrollPosition = 0;
    private float[] position;
    
    // Start is called before the first frame update
    void Start()
    {
        SetLevelAvailability();
    }

    void SetLevelAvailability()
    {
        foreach (var level in GlobalVariables.levels)
        {
            if (PlayerPrefs.GetInt("ShortcutCheat") == 1)
            {
                var levelObject = GetComponent<Transform>().GetComponentsInChildren<Transform>().First(x => x.gameObject.name == $"Level {level.Key}").gameObject;
                levelObject.GetComponentInChildren<Transform>().Find("Button").gameObject.SetActive(true);
                levelObject.GetComponentInChildren<Transform>().Find("Lock").gameObject.SetActive(false);
            }
            else
            {
                var levelObject = GetComponent<Transform>().GetComponentsInChildren<Transform>().First(x => x.gameObject.name == $"Level {level.Key}").gameObject;
                levelObject.GetComponentInChildren<Transform>().Find("Button").gameObject.SetActive(level.Value);
                levelObject.GetComponentInChildren<Transform>().Find("Lock").gameObject.SetActive(!level.Value);
            }
        }
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
        position = new float[transform.childCount];
        float distance = 1f / (position.Length - 1f);
        for (int i = 0; i < position.Length; i++)
        {
            position[i] = distance * i;
        }

        if (Input.touchCount > 0)
        {
            scrollPosition = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (scrollPosition < position[i] + (distance / 2) && scrollPosition > position[i] - (distance / 2))
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, position[i], 0.1f);
            }
        }
        
        for (int i = 0; i < position.Length; i++)
        {
            if (scrollPosition < position[i] + (distance / 2) && scrollPosition > position[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                for (int a = 0; a < position.Length; a++)
                {
                    if (a != i)
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                }
            }
        }
    }
}
