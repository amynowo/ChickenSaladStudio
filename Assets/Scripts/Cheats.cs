using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cheats : MonoBehaviour
{
    private string cheatCode;
    [SerializeField] private GameObject invalidCheatCodePopup;
    [SerializeField] private GameObject existingCheatCodePopup;
    
    [SerializeField] private GameObject godModeCheatLock;
    [SerializeField] private GameObject godModeCheatOnButton;
    [SerializeField] private GameObject godModeCheatOffButton;
    
    [SerializeField] private GameObject shortcutCheatLock;
    [SerializeField] private GameObject shortcutCheatOnButton;
    [SerializeField] private GameObject shortcutCheatOffButton;
    
    private bool godModeCheatOn;
    private bool shortcutCheatOn;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadCheats();
    }

    void LoadCheats()
    {
        godModeCheatLock.SetActive(PlayerPrefs.GetInt("GodModeCheatLocked") == 1);
        godModeCheatOn = PlayerPrefs.GetInt("GodModeCheat") == 1;
        godModeCheatOnButton.SetActive(godModeCheatOn);
        godModeCheatOffButton.SetActive(PlayerPrefs.GetInt("GodModeCheatLocked") == 0 && !godModeCheatOn);

        shortcutCheatLock.SetActive(PlayerPrefs.GetInt("ShortcutCheatLocked") == 1);
        shortcutCheatOn = PlayerPrefs.GetInt("ShortcutCheat") == 1;
        shortcutCheatOnButton.SetActive(shortcutCheatOn);
        shortcutCheatOffButton.SetActive(PlayerPrefs.GetInt("ShortcutCheatLocked") == 0 && !shortcutCheatOn);
    }

    public void GetCheatCodeInput(string cheatCodeInput)
    {
        cheatCode = cheatCodeInput;
    }

    public void SubmitCheat()
    {
        if (cheatCode == "godmode")
        {
            if (PlayerPrefs.GetInt("GodModeCheatLocked") == 0)
            {
                StartCoroutine(nameof(ExistingCheatPopup));
            }
            else
            {
                PlayerPrefs.SetInt("GodModeCheatLocked", 0);
                godModeCheatLock.SetActive(false);
                godModeCheatOffButton.SetActive(true);
            }
        }
        else if (cheatCode == "shortcut")
        {
            if (PlayerPrefs.GetInt("ShortcutCheatLocked") == 0)
            {
                StartCoroutine(nameof(ExistingCheatPopup));
            }
            else
            {
                PlayerPrefs.SetInt("ShortcutCheatLocked", 0);
                shortcutCheatLock.SetActive(false);
                shortcutCheatOffButton.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(nameof(InvalidCheatPopup));
        }
    }

    IEnumerator InvalidCheatPopup()
    {
        invalidCheatCodePopup.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        invalidCheatCodePopup.SetActive(false);
    }
    
    IEnumerator ExistingCheatPopup()
    {
        existingCheatCodePopup.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        existingCheatCodePopup.SetActive(false);
    }

    public void ToggleCheat(string cheat)
    {
        if (cheat == "godmode")
        {
            godModeCheatOnButton.SetActive(!godModeCheatOn);
            godModeCheatOffButton.SetActive(godModeCheatOn);
            godModeCheatOn = !godModeCheatOn;
            PlayerPrefs.SetInt("GodModeCheat", godModeCheatOn ? 1 : 0);

            foreach (var level in GlobalVariables.levels)
            {
                 
            }
        }
        else if (cheat == "shortcut")
        {
            shortcutCheatOnButton.SetActive(!shortcutCheatOn);
            shortcutCheatOffButton.SetActive(shortcutCheatOn);
            shortcutCheatOn = !shortcutCheatOn;
            PlayerPrefs.SetInt("ShortcutCheat", shortcutCheatOn ? 1 : 0);
        }
    }
    
    public void Back()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
