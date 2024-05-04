using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentMenu : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public float souls;

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

    [Header("TMP_Pros")]
    public TMP_Text soulsText;

    [Header("Arrays")]
    public LoadoutItemsSO[] equipmentItemsSOWeapons;
    public LoadoutItemsSO[] equipmentItemsSOCompanion;
    public LoadoutItemsSO[] equipmentItemsSOUpperArmor;
    public LoadoutItemsSO[] equipmentItemsSOLowerArmor;
    public LoadoutTemplate[] equipmentPannelsWeapons;
    public LoadoutTemplate[] equipmentPannelsCompanion;
    public LoadoutTemplate[] equipmentPannelsUpperArmor;
    public LoadoutTemplate[] equipmentPannelsLowerArmor;
    public GameObject[] equipmentPannelsSOWeapons;
    public GameObject[] equipmentPannelsSOCompanion;
    public GameObject[] equipmentPannelsSOUpperArmor;
    public GameObject[] equipmentPannelsSOLowerArmor;
    public Button[] equipmentButtonsWeapons;
    public Button[] equipmentButtonsCompanion;
    public Button[] equipmentButtonsUpperArmor;
    public Button[] equipmentButtonsLowerArmor;

    [Header("Components")]
    Alexander alexander;
    UIManager uiManager;
    NPCSpawner npcSpawner;
    PlayerEquipment playerEquipment;
    LoadoutMenu loadoutMenu;

    #endregion

    #region StartUpdate Methods

    // Update is called once per frame
    void Update()
    {
        uiManager = GameObject.FindWithTag("Managers").GetComponent<UIManager>();
        playerEquipment = GameObject.FindWithTag("Player").GetComponent<PlayerEquipment>();
        loadoutMenu = GameObject.FindWithTag("Canvas").transform.Find("Menus/npcConversations/Barbara").GetComponent<LoadoutMenu>();

        if (uiManager.npcsActive)
        {
            npcSpawner = GameObject.FindWithTag("NPC").GetComponent<NPCSpawner>();
        }
        
        if (!pannelsActivated)
        {
            for (int i = 0; i < equipmentItemsSOWeapons.Length; i++)
            {
                if (!playerEquipment.boughtWeapons.Contains(equipmentItemsSOWeapons[i]))
                {
                    equipmentPannelsSOWeapons[i].SetActive(true);
                    equipmentPannelsWeapons[i].priceObj.SetActive(true);
                }
                else
                {
                    equipmentPannelsSOWeapons[i].SetActive(false);
                }
            }
            for (int i = 0; i < equipmentItemsSOCompanion.Length; i++)
            {
                if (!playerEquipment.boughtCompanions.Contains(equipmentItemsSOCompanion[i]))
                {
                    equipmentPannelsSOCompanion[i].SetActive(true);
                    equipmentPannelsCompanion[i].priceObj.SetActive(true);
                }
                else
                {
                    equipmentPannelsSOCompanion[i].SetActive(false);
                }
            }
            for (int i = 0; i < equipmentItemsSOUpperArmor.Length; i++)
            {
                if (!playerEquipment.boughtUpperArmors.Contains(equipmentItemsSOUpperArmor[i]))
                {
                    equipmentPannelsSOUpperArmor[i].SetActive(true);
                    equipmentPannelsUpperArmor[i].priceObj.SetActive(true);
                }
                else
                {
                    equipmentPannelsSOUpperArmor[i].SetActive(false);
                }
            }
            for (int i = 0; i < equipmentItemsSOLowerArmor.Length; i++)
            {
                if (!playerEquipment.boughtLowerArmors.Contains(equipmentItemsSOLowerArmor[i]))
                {
                    equipmentPannelsSOLowerArmor[i].SetActive(true);
                    equipmentPannelsLowerArmor[i].priceObj.SetActive(true);
                }
                else
                {
                    equipmentPannelsSOLowerArmor[i].SetActive(false);
                }
            }

            weaponContents.position = new Vector3(weaponContents.position.x, -10000f, 0);
            companionContents.position = new Vector3(companionContents.position.x, -10000f, 0f);
            upperArmorContents.position = new Vector3(upperArmorContents.position.x, -10000f, 0f);
            lowerArmorContents.position = new Vector3(lowerArmorContents.position.x, -10000f, 0f);

            CheckEquipmentPurchaseable();

            pannelsActivated = true;
        }

        if (uiManager.npcsActive && npcSpawner.alexSpawned)
        {
            alexander = GameObject.FindWithTag("NPC").GetComponentInChildren<Alexander>();
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

        souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;
        CheckEquipmentPurchaseable();
    }

    #endregion

    #region General Methods

    void LoadLoadoutPannels()
    {
        for (int i = 0; i < equipmentItemsSOWeapons.Length; i++)
        {
            equipmentPannelsWeapons[i].titleText.text = equipmentItemsSOWeapons[i].title;
            equipmentPannelsWeapons[i].descriptionText.text = equipmentItemsSOWeapons[i].description;
            equipmentPannelsWeapons[i].priceText.text = "Price: " + equipmentItemsSOWeapons[i].price.ToString();
            equipmentPannelsWeapons[i].buttonText.text = "Purchase";
        }
        for (int i = 0; i < equipmentItemsSOCompanion.Length; i++)
        {
            equipmentPannelsCompanion[i].titleText.text = equipmentItemsSOCompanion[i].title;
            equipmentPannelsCompanion[i].descriptionText.text = equipmentItemsSOCompanion[i].description;
            equipmentPannelsCompanion[i].priceText.text = "Price: " + equipmentItemsSOCompanion[i].price.ToString();
            equipmentPannelsCompanion[i].buttonText.text = "Purchase";
        }
        for (int i = 0; i < equipmentItemsSOUpperArmor.Length; i++)
        {
            equipmentPannelsUpperArmor[i].titleText.text = equipmentItemsSOUpperArmor[i].title;
            equipmentPannelsUpperArmor[i].descriptionText.text = equipmentItemsSOUpperArmor[i].description;
            equipmentPannelsUpperArmor[i].priceText.text = "Price: " + equipmentItemsSOUpperArmor[i].price.ToString();
            equipmentPannelsUpperArmor[i].buttonText.text = "Purchase";
        }
        for (int i = 0; i < equipmentItemsSOLowerArmor.Length; i++)
        {
            equipmentPannelsLowerArmor[i].titleText.text = equipmentItemsSOLowerArmor[i].title;
            equipmentPannelsLowerArmor[i].descriptionText.text = equipmentItemsSOLowerArmor[i].description;
            equipmentPannelsLowerArmor[i].priceText.text = "Price: " + equipmentItemsSOLowerArmor[i].price.ToString();
            equipmentPannelsLowerArmor[i].buttonText.text = "Purchase";
        }

        pannelsLoaded = true;
    }

    void CheckEquipmentPurchaseable()
    {
        for (int i = 0; i < equipmentItemsSOWeapons.Length; i++)
        {
            equipmentButtonsWeapons[i].interactable = souls >= equipmentItemsSOWeapons[i].price;
        }
        for (int i = 0; i < equipmentItemsSOCompanion.Length; i++)
        {
            equipmentButtonsCompanion[i].interactable = souls >= equipmentItemsSOCompanion[i].price;
        }
        for (int i = 0; i < equipmentItemsSOUpperArmor.Length; i++)
        {
            equipmentButtonsUpperArmor[i].interactable = souls >= equipmentItemsSOUpperArmor[i].price;
        }
        for (int i = 0; i < equipmentItemsSOLowerArmor.Length; i++)
        {
            equipmentButtonsLowerArmor[i].interactable = souls >= equipmentItemsSOLowerArmor[i].price;
        }
    }

    public void PurchaseEquipmentWeapon(int btnNo)
    {
        if (souls >= equipmentItemsSOWeapons[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= equipmentItemsSOWeapons[btnNo].price;
            souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;

            pannelsActivated = false;
            loadoutMenu.pannelsActivated = false;

            //Unlock purchased item.
            playerEquipment.PurchaseWeapon(equipmentItemsSOWeapons[btnNo]);
        }
    }

    public void PurchaseEquipmentCompanion(int btnNo)
    {
        if (souls >= equipmentItemsSOCompanion[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= equipmentItemsSOCompanion[btnNo].price;
            souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;

            pannelsActivated = false;
            loadoutMenu.pannelsActivated = false;

            //Unlock purchased item.
            playerEquipment.PurchaseCompanion(equipmentItemsSOCompanion[btnNo]);
        }
    }

    public void PurchaseEquipmentUpperArmor(int btnNo)
    {
        if (souls >= equipmentItemsSOUpperArmor[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= equipmentItemsSOUpperArmor[btnNo].price;
            souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;

            pannelsActivated = false;
            loadoutMenu.pannelsActivated = false;

            //Unlock purchased item.
            playerEquipment.PurchaseUpperArmor(equipmentItemsSOUpperArmor[btnNo]);
        }
    }

    public void PurchaseEquipmentLowerArmor(int btnNo)
    {
        if (souls >= equipmentItemsSOLowerArmor[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= equipmentItemsSOLowerArmor[btnNo].price;
            souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;

            pannelsActivated = false;
            loadoutMenu.pannelsActivated = false;

            //Unlock purchased item.
            playerEquipment.PurchaseLowerArmor(equipmentItemsSOLowerArmor[btnNo]);
        }
    }
    
    public void OpenStore()
    {
        menuOpen = true;
    }

    public void CloseStore()
    {
        menuOpen = false;
        uiManager.alexanderTalking = false;
        alexander.talking = false;
    }

    #endregion
}
