using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeMusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    private GameObject[] otherGameObjects;
    private bool notFirstObject = false;

    private void Awake()
    {
        otherGameObjects = GameObject.FindGameObjectsWithTag("ThemeMusic");
 
        foreach (GameObject otherGameObject in otherGameObjects)
        {
            if (otherGameObject.scene.buildIndex == -1)
            {
                notFirstObject = true;
            }
        }
 
        if (notFirstObject == true)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
    }
}
