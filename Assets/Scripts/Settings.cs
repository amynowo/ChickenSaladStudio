using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Sound()
    {
        SceneManager.LoadScene("SoundScene");
    }

    public void InsertCheat()
    {
        SceneManager.LoadScene("CheatScene");
    }
    
    public void Back()
    {
        SceneManager.LoadScene("StartScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
