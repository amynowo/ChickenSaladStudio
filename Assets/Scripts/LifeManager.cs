using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;
    public GameObject[] lifeObjects;
    public static int lifeCount;
    
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        lifeCount = 3;
    }

    public void RemoveLife()
    {
        if (lifeCount > 0)
            Instance.lifeObjects[lifeCount-1].SetActive(false);

        lifeCount--;
        if (lifeCount == 0)
        {
            GameResult.Instance.GetComponent<GameResult>().EndLevel(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
