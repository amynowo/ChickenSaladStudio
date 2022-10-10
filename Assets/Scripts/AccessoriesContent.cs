using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class AccessoriesContent : MonoBehaviour
{
    [SerializeField] public GameObject accessoriesSelectionObject;
    [SerializeField] public GameObject[] accessoriesContentObjects;

    [SerializeField] public TextMeshProUGUI unlockOverlayText;
    [SerializeField] public TextMeshProUGUI playerPointsText;

    public int birdIndex;

    public GameObject buttonLeft;
    public GameObject buttonRight;

    private int currentAccessory;
    private int currentAccessoryPrice;
        
    // Start is called before the first frame update
    void Start()
    {
        currentAccessory = 0;
        SetCosmeticsAvailability();
        SetPlayerPoints();
    }

    void SetCosmeticsAvailability()
    {
        foreach (var accessory in accessoriesContentObjects)
        {
            if (PlayerPrefs.GetInt("ShortcutCheat") == 1)
            {
                accessory.GetComponentInChildren<Transform>().Find("Unlocked").gameObject.SetActive(true);
                accessory.GetComponentInChildren<Transform>().Find("Locked").gameObject.SetActive(false);
            }
            else
            {
                if (birdIndex == 0)
                {
                    string collectionName = accessory.name.Split("Collection").Last();
                    
                    int collectionUnlockedCount = 0;
                    int collectionEnabledCount = 0;
                    for (int i = 1; i < 5; i++)
                    {
                        collectionUnlockedCount += PlayerPrefs.GetInt($"Bird{i + collectionName}Unlocked");
                        collectionEnabledCount += PlayerPrefs.GetString($"Bird{i}Skin") == $"Bird{i + collectionName}" ? 1 : 0;
                    }
                    
                    bool collectionUnlocked = collectionUnlockedCount == 4;
                    bool collectionEnabled = collectionEnabledCount == 4;

                    accessory.GetComponentInChildren<Transform>().Find("Locked").gameObject.SetActive(!collectionUnlocked);
                    accessory.GetComponentInChildren<Transform>().Find("Unlocked").gameObject.SetActive(collectionUnlocked);
                    accessory.GetComponentInChildren<Transform>().Find("Unlocked").GetComponentInChildren<Toggle>().GetComponentInChildren<Transform>().Find("Disabled").gameObject.SetActive(!collectionEnabled);
                    accessory.GetComponentInChildren<Transform>().Find("Unlocked").GetComponentInChildren<Toggle>().GetComponentInChildren<Transform>().Find("Enabled").gameObject.SetActive(collectionEnabled);
                }
                else
                {
                    bool accessoryUnlocked = PlayerPrefs.GetInt($"{accessory.name}Unlocked") == 1;
                    bool accessoryEnabled = PlayerPrefs.GetString($"Bird{birdIndex}Skin") == accessory.name;

                    accessory.GetComponentInChildren<Transform>().Find("Locked").gameObject.SetActive(!accessoryUnlocked);
                    accessory.GetComponentInChildren<Transform>().Find("Unlocked").gameObject.SetActive(accessoryUnlocked);
                    accessory.GetComponentInChildren<Transform>().Find("Unlocked").GetComponentInChildren<Toggle>().GetComponentInChildren<Transform>().Find("Disabled").gameObject.SetActive(!accessoryEnabled);
                    accessory.GetComponentInChildren<Transform>().Find("Unlocked").GetComponentInChildren<Toggle>().GetComponentInChildren<Transform>().Find("Enabled").gameObject.SetActive(accessoryEnabled);
                }
            }
        }
    }

    public void Left()
    {
        accessoriesContentObjects[currentAccessory].SetActive(false);
        currentAccessory--;
        accessoriesContentObjects[currentAccessory].SetActive(true);
    }
    
    public void Right()
    {
        accessoriesContentObjects[currentAccessory].SetActive(false);
        currentAccessory++;
        accessoriesContentObjects[currentAccessory].SetActive(true);
    }

    public void ChangeUnlockOverlayText()
    {
        currentAccessoryPrice = accessoriesContentObjects[currentAccessory].name.StartsWith("Collection") ? 1500 : 400;
        unlockOverlayText.text = $"unlock for {currentAccessoryPrice}";
    }

    public void Unlock()
    {
        string accessoryName = accessoriesContentObjects[currentAccessory].name;
        int currentPlayerPoints = PlayerPrefs.GetInt("PlayerPoints");
        Debug.Log(birdIndex);
        if (birdIndex == 0)
        {
            if (currentPlayerPoints >= currentAccessoryPrice)
            {
                for (int i = 1; i < 5; i++)
                    PlayerPrefs.SetInt($"Bird{i + accessoryName.Split("Collection").Last()}Unlocked", 1);

                currentPlayerPoints -= currentAccessoryPrice;
                PlayerPrefs.SetInt("PlayerPoints", currentPlayerPoints);
                SetPlayerPoints();
            }
        }
        else
        {
            if (currentPlayerPoints >= currentAccessoryPrice)
            {
                PlayerPrefs.SetInt($"{accessoryName}Unlocked", 1);
                currentPlayerPoints -= currentAccessoryPrice;
                PlayerPrefs.SetInt("PlayerPoints", currentPlayerPoints);
                  
                SetPlayerPoints();
            }
        }
        
        SetCosmeticsAvailability();
    }

    private void SetPlayerPoints()
    {
        playerPointsText.text = $"points\n{PlayerPrefs.GetInt("PlayerPoints")}";
    }

    public void Enable()
    {
        bool disabled = accessoriesContentObjects[currentAccessory].GetComponentInChildren<Transform>().Find("Unlocked").GetComponentInChildren<Toggle>().GetComponentInChildren<Transform>().Find("Disabled").gameObject.activeSelf;

        if (disabled)
        {
            string accessoryName = accessoriesContentObjects[currentAccessory].name;
            if (birdIndex == 0)
            {
                string collectionName = accessoryName.Split("Collection").Last();
                for (int i = 1; i < 5; i++)
                    PlayerPrefs.SetString($"Bird{i}Skin", $"Bird{i + collectionName}");
            }
            else
            {
                PlayerPrefs.SetString($"Bird{birdIndex}Skin", accessoryName);
            }
            
            SetCosmeticsAvailability();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAccessory == 0)
        {
            buttonLeft.SetActive(false);
            buttonRight.SetActive(true);
        }
        else if (currentAccessory == accessoriesContentObjects.Length - 1)
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
