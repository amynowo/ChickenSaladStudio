using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Worm : MonoBehaviour
{
    double timeInstantiated;
    public float assignedTime; // When the worm supposed to be eaten
    
    // Start is called before the first frame update
    void Start()
    {
        timeInstantiated = MusicManager.GetAudioSourceTime();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        double timeSinceInstantiated = MusicManager.GetAudioSourceTime() - timeInstantiated;
        float t = (float)(timeSinceInstantiated / (MusicManager.Instance.wormTime * 2));

        GetComponent<SpriteRenderer>().enabled = true;
        if (t > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(Vector3.up * MusicManager.Instance.wormSpawnY, Vector3.up * MusicManager.Instance.wormDespawnY, t); 
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
