using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutMenu : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool menuOpen;
    bool pannelsLoaded;
    bool playerStoped;
    public bool pannelsActivated;

    [Header("GameObjects")]
    public GameObject contents;

    [Header("Transforms")]
    public Transform weaponContents;
    public Transform companionContents;
    public Transform upperArmorContents;
    public Transform lowerArmorContents;

    [Header("Arrays")]
    public LoadoutItemsSO[] loadoutItemsSOWeapons;
    public LoadoutItemsSO[] loadoutItemsSOCompanion;
    public LoadoutItemsSO[] loadoutItemsSOUpperArmor;
    public LoadoutItemsSO[] loadoutItemsSOLowerArmor;
    public LoadoutTemplate[] loadoutPannelsWeapons;
    public LoadoutTemplate[] loadoutPannelsCompanion;
    public LoadoutTemplate[] loadoutPannelsUpperArmor;
    public LoadoutTemplate[] loadoutPannelsLowerArmor;
    public GameObject[] loadoutPannelsSOWeapons;
    public GameObject[] loadoutPannelsSOCompanion;
    public GameObject[] loadoutPannelsSOUpperArmor;
    public GameObject[] loadoutPannelsSOLowerArmor;
    public Button[] selectLoadoutButtonsWeapons;
    public Button[] selectLoadoutButtonsCompanion;
    public Button[] selectLoadoutButtonsUpperArmor;
    public Button[] selectLoadoutButtonsLowerArmor;

    [Header("LoadoutItemsSO")]
    LoadoutItemsSO selectedWeapon;
    LoadoutItemsSO selectedCompanion;
    LoadoutItemsSO selectedUpperArmor;
    LoadoutItemsSO selectedLowerArmor;

    [Header("Components")]
    Barbara barbara;
    PlayerLoadout playerLoadout;
    UIManager uiManager;
    NPCSpawner npcSpawner;
    PlayerEquipment playerEquipment;

    #endregion

    #region StartUpdate Methods

    // Update is called once per frame
    void Update()
    {
        playerLoadout = GameObject.FindWithTag("Player").GetComponent<PlayerLoadout>();
        uiManager = GameObject.FindWithTag("Managers").GetComponent<UIManager>();
        playerEquipment = GameObject.FindWithTag("Player").GetComponent<PlayerEquipment>();

        if (uiManager.npcsActive)
        {
            npcSpawner = GameObject.FindWithTag("NPC").GetComponent<NPCSpawner>();
        }
        
        if (!pannelsActivated && playerEquipment.equipmentLoaded)
        {
            for (int i = 0; i < loadoutItemsSOWeapons.Length; i++)
            {
                if (playerEquipment.boughtWeapons.Contains(loadoutItemsSOWeapons[i]))
                {
                    loadoutPannelsSOWeapons[i].SetActive(true);
                }
            }
            for (int i = 0; i < loadoutItemsSOCompanion.Length; i++)
            {
                if (playerEquipment.boughtCompanions.Contains(loadoutItemsSOCompanion[i]))
                {
                    loadoutPannelsSOCompanion[i].SetActive(true);
                }
            }
            for (int i = 0; i < loadoutItemsSOUpperArmor.Length; i++)
            {
                if (playerEquipment.boughtUpperArmors.Contains(loadoutItemsSOUpperArmor[i]))
                {
                    loadoutPannelsSOUpperArmor[i].SetActive(true);
                }
            }
            for (int i = 0; i < loadoutItemsSOLowerArmor.Length; i++)
            {
                if (playerEquipment.boughtLowerArmors.Contains(loadoutItemsSOLowerArmor[i]))
                {
                    loadoutPannelsSOLowerArmor[i].SetActive(true);
                }
            }

            weaponContents.position = new Vector3(weaponContents.position.x, -10000f, 0);
            companionContents.position = new Vector3(companionContents.position.x, -10000f, 0f);
            upperArmorContents.position = new Vector3(upperArmorContents.position.x, -10000f, 0f);
            lowerArmorContents.position = new Vector3(lowerArmorContents.position.x, -10000f, 0f);

            pannelsActivated = true;
        }

        if (uiManager.npcsActive && npcSpawner.barbSpawned)
        {
            barbara = GameObject.FindWithTag("NPC").GetComponentInChildren<Barbara>();
        }
        
        if (!pannelsLoaded)
        {
            LoadLoadoutPannels();
        }

        if (menuOpen)
        {
            contents.SetActive(true);
            Time.timeScale = 0f;
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().startBool = false;
            playerStoped = true;
        }
        else
        {
            contents.SetActive(false);
            if (playerStoped)
            {
                Time.timeScale = 1f;
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().startBool = true;
                playerStoped = false;
            }
        }
    }

    #endregion

    #region General Methods

    void LoadLoadoutPannels()
    {
        for (int i = 0; i < loadoutItemsSOWeapons.Length; i++)
        {
            loadoutPannelsWeapons[i].titleText.text = loadoutItemsSOWeapons[i].title;
            loadoutPannelsWeapons[i].descriptionText.text = loadoutItemsSOWeapons[i].description;

            if (playerLoadout.selectedWeapon && loadoutItemsSOWeapons[i].title == playerLoadout.selectedWeapon.title)
            {
                selectedWeapon = loadoutItemsSOWeapons[i];
                selectLoadoutButtonsWeapons[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsWeapons[i].interactable = true;
            }
        }
        for (int i = 0; i < loadoutItemsSOCompanion.Length; i++)
        {
            loadoutPannelsCompanion[i].titleText.text = loadoutItemsSOCompanion[i].title;
            loadoutPannelsCompanion[i].descriptionText.text = loadoutItemsSOCompanion[i].description;

            if (playerLoadout.selectedCompanion && loadoutItemsSOCompanion[i].title == playerLoadout.selectedCompanion.title)
            {
                selectedCompanion = loadoutItemsSOCompanion[i];
                selectLoadoutButtonsCompanion[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsCompanion[i].interactable = true;
            }
        }
        for (int i = 0; i < loadoutItemsSOUpperArmor.Length; i++)
        {
            loadoutPannelsUpperArmor[i].titleText.text = loadoutItemsSOUpperArmor[i].title;
            loadoutPannelsUpperArmor[i].descriptionText.text = loadoutItemsSOUpperArmor[i].description;

            if (playerLoadout.selectedUpperArmor && loadoutItemsSOUpperArmor[i].title == playerLoadout.selectedUpperArmor.title)
            {
                selectedUpperArmor = loadoutItemsSOUpperArmor[i];
                selectLoadoutButtonsUpperArmor[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsUpperArmor[i].interactable = true;
            }
        }
        for (int i = 0; i < loadoutItemsSOLowerArmor.Length; i++)
        {
            loadoutPannelsLowerArmor[i].titleText.text = loadoutItemsSOLowerArmor[i].title;
            loadoutPannelsLowerArmor[i].descriptionText.text = loadoutItemsSOLowerArmor[i].description;

            if (playerLoadout.selectedLowerArmor && loadoutItemsSOLowerArmor[i].title == playerLoadout.selectedLowerArmor.title)
            {
                selectedLowerArmor = loadoutItemsSOLowerArmor[i];
                selectLoadoutButtonsLowerArmor[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsLowerArmor[i].interactable = true;
            }
        }

        if (!selectedWeapon)
        {
            selectedWeapon = loadoutItemsSOWeapons[0];
            selectLoadoutButtonsWeapons[0].interactable = false;
        }
        if (!selectedCompanion)
        {
            selectedCompanion = loadoutItemsSOCompanion[0];
            selectLoadoutButtonsCompanion[0].interactable = false;
        }
        if (!selectedUpperArmor)
        {
            selectedUpperArmor = loadoutItemsSOUpperArmor[0];
            selectLoadoutButtonsUpperArmor[0].interactable = false;
        }
        if (!selectedLowerArmor)
        {
            selectedLowerArmor = loadoutItemsSOLowerArmor[0];
            selectLoadoutButtonsLowerArmor[0].interactable = false;
        }

        pannelsLoaded = true;
    }

    public void SelectedLoadoutWeapon(int btnNo)
    {
        selectedWeapon = loadoutItemsSOWeapons[btnNo];
        for (int i = 0; i < selectLoadoutButtonsWeapons.Length; i++)
        {
            if (selectLoadoutButtonsWeapons[i] == selectLoadoutButtonsWeapons[btnNo])
            {
                selectLoadoutButtonsWeapons[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsWeapons[i].interactable = true;
            }
        }
    }
    public void SelectedLoadoutCompanion(int btnNo)
    {
        selectedCompanion = loadoutItemsSOCompanion[btnNo];
        for (int i = 0; i < selectLoadoutButtonsCompanion.Length; i++)
        {
            if (selectLoadoutButtonsCompanion[i] == selectLoadoutButtonsCompanion[btnNo])
            {
                selectLoadoutButtonsCompanion[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsCompanion[i].interactable = true;
            }
        }
    }
    public void SelectedLoadoutUpperArmor(int btnNo)
    {
        selectedUpperArmor = loadoutItemsSOUpperArmor[btnNo];
        for (int i = 0; i < selectLoadoutButtonsUpperArmor.Length; i++)
        {
            if (selectLoadoutButtonsUpperArmor[i] == selectLoadoutButtonsUpperArmor[btnNo])
            {
                selectLoadoutButtonsUpperArmor[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsUpperArmor[i].interactable = true;
            }
        }
    }
    public void SelectedLoadoutLowerArmor(int btnNo)
    {
        selectedLowerArmor = loadoutItemsSOLowerArmor[btnNo];
        for (int i = 0; i < selectLoadoutButtonsLowerArmor.Length; i++)
        {
            if (selectLoadoutButtonsLowerArmor[i] == selectLoadoutButtonsLowerArmor[btnNo])
            {
                selectLoadoutButtonsLowerArmor[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsLowerArmor[i].interactable = true;
            }
        }
    }

    public void ConfirmSelected()
    {
        playerLoadout.SetLoadout(selectedWeapon, selectedCompanion, selectedUpperArmor, selectedLowerArmor);

        selectedWeapon = null;
        selectedCompanion = null;
        selectedUpperArmor = null;
        selectedLowerArmor = null;
        LoadLoadoutPannels();
        CloseStore();
    }

    public void ExitStore()
    {
        selectedWeapon = null;
        selectedCompanion = null;
        selectedUpperArmor = null;
        selectedLowerArmor = null;
        LoadLoadoutPannels();
        CloseStore();
    }

    public void OpenStore()
    {
        menuOpen = true;
    }

    public void CloseStore()
    {
        menuOpen = false;
        uiManager.barbaraTalking = false;
        barbara.talking = false;
    }

    #endregion
}
