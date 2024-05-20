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
    public Transform armorContents;
    public Transform backContents;

    [Header("Arrays")]
    public LoadoutItemsSO[] loadoutItemsSOWeapons;
    public LoadoutItemsSO[] loadoutItemsSOCompanion;
    public LoadoutItemsSO[] loadoutItemsSOArmor;
    public LoadoutItemsSO[] loadoutItemsSOBack;
    public LoadoutTemplate[] loadoutPannelsWeapons;
    public LoadoutTemplate[] loadoutPannelsCompanion;
    public LoadoutTemplate[] loadoutPannelsArmor;
    public LoadoutTemplate[] loadoutPannelsBack;
    public GameObject[] loadoutPannelsSOWeapons;
    public GameObject[] loadoutPannelsSOCompanion;
    public GameObject[] loadoutPannelsSOArmor;
    public GameObject[] loadoutPannelsSOBack;
    public Button[] selectLoadoutButtonsWeapons;
    public Button[] selectLoadoutButtonsCompanion;
    public Button[] selectLoadoutButtonsArmor;
    public Button[] selectLoadoutButtonsBack;

    [Header("LoadoutItemsSO")]
    LoadoutItemsSO selectedWeapon;
    LoadoutItemsSO selectedCompanion;
    LoadoutItemsSO selectedArmor;
    LoadoutItemsSO selectedBack;

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
            for (int i = 0; i < loadoutItemsSOArmor.Length; i++)
            {
                if (playerEquipment.boughtArmors.Contains(loadoutItemsSOArmor[i]))
                {
                    loadoutPannelsSOArmor[i].SetActive(true);
                }
            }
            for (int i = 0; i < loadoutItemsSOBack.Length; i++)
            {
                if (playerEquipment.boughtBacks.Contains(loadoutItemsSOBack[i]))
                {
                    loadoutPannelsSOBack[i].SetActive(true);
                }
            }

            weaponContents.position = new Vector3(weaponContents.position.x, -10000f);
            companionContents.position = new Vector3(companionContents.position.x, -10000f);
            armorContents.position = new Vector3(armorContents.position.x, -10000f);
            backContents.position = new Vector3(backContents.position.x, -10000f);

            pannelsActivated = true;
        }

        if (uiManager.npcsActive && npcSpawner.barbSpawned)
        {
            barbara = GameObject.FindWithTag("NPC").GetComponentInChildren<Barbara>();
        }
        
        if (!pannelsLoaded && playerLoadout.start)
        {
            LoadLoadoutPannels();
        }

        if (menuOpen)
        {
            contents.SetActive(true);
            Time.timeScale = 0f;
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().startBool = false;
            playerStoped = true;
            Cursor.visible = true;
        }
        else
        {
            contents.SetActive(false);
            if (playerStoped)
            {
                Time.timeScale = 1f;
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().startBool = true;
                playerStoped = false;
                Cursor.visible = false;
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
        for (int i = 0; i < loadoutItemsSOArmor.Length; i++)
        {
            loadoutPannelsArmor[i].titleText.text = loadoutItemsSOArmor[i].title;
            loadoutPannelsArmor[i].descriptionText.text = loadoutItemsSOArmor[i].description;

            if (playerLoadout.selectedArmor && loadoutItemsSOArmor[i].title == playerLoadout.selectedArmor.title)
            {
                selectedArmor = loadoutItemsSOArmor[i];
                selectLoadoutButtonsArmor[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsArmor[i].interactable = true;
            }
        }
        for (int i = 0; i < loadoutItemsSOBack.Length; i++)
        {
            loadoutPannelsBack[i].titleText.text = loadoutItemsSOBack[i].title;
            loadoutPannelsBack[i].descriptionText.text = loadoutItemsSOBack[i].description;

            if (playerLoadout.selectedBack && loadoutItemsSOBack[i].title == playerLoadout.selectedBack.title)
            {
                selectedBack = loadoutItemsSOBack[i];
                selectLoadoutButtonsBack[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsBack[i].interactable = true;
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
        if (!selectedArmor)
        {
            selectedArmor = loadoutItemsSOArmor[0];
            selectLoadoutButtonsArmor[0].interactable = false;
        }
        if (!selectedBack)
        {
            selectedBack = loadoutItemsSOBack[0];
            selectLoadoutButtonsBack[0].interactable = false;
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
    public void SelectedLoadoutArmor(int btnNo)
    {
        selectedArmor = loadoutItemsSOArmor[btnNo];
        for (int i = 0; i < selectLoadoutButtonsArmor.Length; i++)
        {
            if (selectLoadoutButtonsArmor[i] == selectLoadoutButtonsArmor[btnNo])
            {
                selectLoadoutButtonsArmor[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsArmor[i].interactable = true;
            }
        }
    }
    public void SelectedLoadoutBack(int btnNo)
    {
        selectedBack = loadoutItemsSOBack[btnNo];
        for (int i = 0; i < selectLoadoutButtonsBack.Length; i++)
        {
            if (selectLoadoutButtonsBack[i] == selectLoadoutButtonsBack[btnNo])
            {
                selectLoadoutButtonsBack[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsBack[i].interactable = true;
            }
        }
    }

    public void ConfirmSelected()
    {
        playerLoadout.SetLoadout(selectedWeapon, selectedCompanion, selectedArmor, selectedBack);

        ExitStore();
    }

    public void ExitStore()
    {
        selectedWeapon = null;
        selectedCompanion = null;
        selectedArmor = null;
        selectedBack = null;
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
