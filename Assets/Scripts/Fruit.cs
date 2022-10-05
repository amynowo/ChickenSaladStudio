using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fruit : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime; // When the fruit supposed to be eaten
    
    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = LevelManager.Instance.GetAudioSourceTime();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        double timeSinceInstantiated = LevelManager.Instance.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (LevelManager.Instance.fruitTime * 2));

        //GetComponent<SpriteRenderer>().enabled = true;
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.up * LevelManager.Instance.fruitSpawnY, Vector3.up * LevelManager.Instance.fruitDespawnY, t); 
            //GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
