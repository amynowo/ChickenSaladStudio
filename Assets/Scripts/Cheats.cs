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
    
    [SerializeField] private GameObject motherlodeCheatLock;
    [SerializeField] private GameObject motherlodeCheatOnButton;
    [SerializeField] private GameObject motherlodeCheatOffButton;
    
    [SerializeField] private GameObject petaCheatLock;
    [SerializeField] private GameObject petaCheatOnButton;
    [SerializeField] private GameObject petaCheatOffButton;
    
    [SerializeField] private GameObject resetCheatLock;
    [SerializeField] private GameObject resetCheatOnButton;
    [SerializeField] private GameObject resetCheatOffButton;
    
    private bool godModeCheatOn;
    private bool shortcutCheatOn;
    private bool motherlodeCheatOn;
    private bool petaCheatOn;
    private bool resetCheatOn;
    
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
        
        motherlodeCheatLock.SetActive(PlayerPrefs.GetInt("MotherlodeCheatLocked") == 1);
        motherlodeCheatOn = PlayerPrefs.GetInt("MotherlodeCheat") == 1;
        motherlodeCheatOnButton.SetActive(motherlodeCheatOn);
        motherlodeCheatOffButton.SetActive(PlayerPrefs.GetInt("MotherlodeCheatLocked") == 0 && !motherlodeCheatOn);
        
        petaCheatLock.SetActive(PlayerPrefs.GetInt("PETACheatLocked") == 1);
        petaCheatOn = PlayerPrefs.GetInt("PETACheat") == 1;
        petaCheatOnButton.SetActive(petaCheatOn);
        petaCheatOffButton.SetActive(PlayerPrefs.GetInt("PETACheatLocked") == 0 && !petaCheatOn);
        
        resetCheatLock.SetActive(PlayerPrefs.GetInt("ResetCheatLocked") == 1);
        resetCheatOn = PlayerPrefs.GetInt("ResetCheat") == 1;
        resetCheatOnButton.SetActive(resetCheatOn);
        resetCheatOffButton.SetActive(PlayerPrefs.GetInt("ResetCheatLocked") == 0 && !resetCheatOn);
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
        else if (cheatCode == "motherlode")
        {
            if (PlayerPrefs.GetInt("MotherlodeCheatLocked") == 0)
            {
                StartCoroutine(nameof(ExistingCheatPopup));
            }
            else
            {
                PlayerPrefs.SetInt("MotherlodeCheatLocked", 0);
                motherlodeCheatLock.SetActive(false);
                motherlodeCheatOffButton.SetActive(true);
            }
        }
        else if (cheatCode == "PETA")
        {
            if (PlayerPrefs.GetInt("PETACheatLocked") == 0)
            {
                StartCoroutine(nameof(ExistingCheatPopup));
            }
            else
            {
                PlayerPrefs.SetInt("PETACheatLocked", 0);
                petaCheatLock.SetActive(false);
                petaCheatOffButton.SetActive(true);
            }
        }
        else if (cheatCode == "oopsie")
        {
            if (PlayerPrefs.GetInt("ResetCheatLocked") == 0)
            {
                StartCoroutine(nameof(ExistingCheatPopup));
            }
            else
            {
                PlayerPrefs.SetInt("ResetCheatLocked", 0);
                resetCheatLock.SetActive(false);
                resetCheatOffButton.SetActive(true);
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
        }
        else if (cheat == "shortcut")
        {
            shortcutCheatOnButton.SetActive(!shortcutCheatOn);
            shortcutCheatOffButton.SetActive(shortcutCheatOn);
            shortcutCheatOn = !shortcutCheatOn;
            PlayerPrefs.SetInt("ShortcutCheat", shortcutCheatOn ? 1 : 0);
        }
        else if (cheat == "motherlode")
        {
            motherlodeCheatOnButton.SetActive(!motherlodeCheatOn);
            motherlodeCheatOffButton.SetActive(motherlodeCheatOn);
            motherlodeCheatOn = !motherlodeCheatOn;
            PlayerPrefs.SetInt("MotherlodeCheat", motherlodeCheatOn ? 1 : 0);
        }
        else if (cheat == "peta")
        {
            petaCheatOnButton.SetActive(!petaCheatOn);
            petaCheatOffButton.SetActive(petaCheatOn);
            petaCheatOn = !petaCheatOn;
            PlayerPrefs.SetInt("PETACheat", petaCheatOn ? 1 : 0);
        }
        else if (cheat == "reset")
        {
            resetCheatOnButton.SetActive(!resetCheatOn);
            resetCheatOffButton.SetActive(resetCheatOn);
            resetCheatOn = !resetCheatOn;
            Debug.Log(resetCheatOn);
            PlayerPrefs.SetInt("ResetCheat", resetCheatOn ? 1 : 0);

            if (resetCheatOn)
            {
                PlayerPrefs.DeleteAll();
                GlobalVariables.reset = true;
                SceneManager.LoadScene("StartScene");
            }
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
