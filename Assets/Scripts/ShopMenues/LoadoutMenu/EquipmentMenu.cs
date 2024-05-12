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
    public Transform armorContents;
    public Transform TBDContents;

    [Header("TMP_Pros")]
    public TMP_Text soulsText;

    [Header("Arrays")]
    public LoadoutItemsSO[] equipmentItemsSOWeapons;
    public LoadoutItemsSO[] equipmentItemsSOCompanion;
    public LoadoutItemsSO[] equipmentItemsSOArmor;
    public LoadoutItemsSO[] equipmentItemsSOTBD;
    public LoadoutTemplate[] equipmentPannelsWeapons;
    public LoadoutTemplate[] equipmentPannelsCompanion;
    public LoadoutTemplate[] equipmentPannelsArmor;
    public LoadoutTemplate[] equipmentPannelsTBD;
    public GameObject[] equipmentPannelsSOWeapons;
    public GameObject[] equipmentPannelsSOCompanion;
    public GameObject[] equipmentPannelsSOArmor;
    public GameObject[] equipmentPannelsSOTBD;
    public Button[] equipmentButtonsWeapons;
    public Button[] equipmentButtonsCompanion;
    public Button[] equipmentButtonsArmor;
    public Button[] equipmentButtonsTBD;

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
            for (int i = 0; i < equipmentItemsSOArmor.Length; i++)
            {
                if (!playerEquipment.boughtArmors.Contains(equipmentItemsSOArmor[i]))
                {
                    equipmentPannelsSOArmor[i].SetActive(true);
                    equipmentPannelsArmor[i].priceObj.SetActive(true);
                }
                else
                {
                    equipmentPannelsSOArmor[i].SetActive(false);
                }
            }
            for (int i = 0; i < equipmentItemsSOTBD.Length; i++)
            {
                if (!playerEquipment.boughtTBDs.Contains(equipmentItemsSOTBD[i]))
                {
                    equipmentPannelsSOTBD[i].SetActive(true);
                    equipmentPannelsTBD[i].priceObj.SetActive(true);
                }
                else
                {
                    equipmentPannelsSOTBD[i].SetActive(false);
                }
            }

            weaponContents.position = new Vector3(weaponContents.position.x, -10000f, 0);
            companionContents.position = new Vector3(companionContents.position.x, -10000f, 0f);
            armorContents.position = new Vector3(armorContents.position.x, -10000f, 0f);
            TBDContents.position = new Vector3(TBDContents.position.x, -10000f, 0f);

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
        for (int i = 0; i < equipmentItemsSOArmor.Length; i++)
        {
            equipmentPannelsArmor[i].titleText.text = equipmentItemsSOArmor[i].title;
            equipmentPannelsArmor[i].descriptionText.text = equipmentItemsSOArmor[i].description;
            equipmentPannelsArmor[i].priceText.text = "Price: " + equipmentItemsSOArmor[i].price.ToString();
            equipmentPannelsArmor[i].buttonText.text = "Purchase";
        }
        for (int i = 0; i < equipmentItemsSOTBD.Length; i++)
        {
            equipmentPannelsTBD[i].titleText.text = equipmentItemsSOTBD[i].title;
            equipmentPannelsTBD[i].descriptionText.text = equipmentItemsSOTBD[i].description;
            equipmentPannelsTBD[i].priceText.text = "Price: " + equipmentItemsSOTBD[i].price.ToString();
            equipmentPannelsTBD[i].buttonText.text = "Purchase";
        }

        pannelsLoaded = true;
    }

    void CheckEquipmentPurchaseable()
    {
        soulsText.text = $"{souls}";
        
        for (int i = 0; i < equipmentItemsSOWeapons.Length; i++)
        {
            equipmentButtonsWeapons[i].interactable = souls >= equipmentItemsSOWeapons[i].price;
        }
        for (int i = 0; i < equipmentItemsSOCompanion.Length; i++)
        {
            equipmentButtonsCompanion[i].interactable = souls >= equipmentItemsSOCompanion[i].price;
        }
        for (int i = 0; i < equipmentItemsSOArmor.Length; i++)
        {
            equipmentButtonsArmor[i].interactable = souls >= equipmentItemsSOArmor[i].price;
        }
        for (int i = 0; i < equipmentItemsSOTBD.Length; i++)
        {
            equipmentButtonsTBD[i].interactable = souls >= equipmentItemsSOTBD[i].price;
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

    public void PurchaseEquipmentArmor(int btnNo)
    {
        if (souls >= equipmentItemsSOArmor[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= equipmentItemsSOArmor[btnNo].price;
            souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;

            pannelsActivated = false;
            loadoutMenu.pannelsActivated = false;

            //Unlock purchased item.
            playerEquipment.PurchaseArmor(equipmentItemsSOArmor[btnNo]);
        }
    }

    public void PurchaseEquipmentTBD(int btnNo)
    {
        if (souls >= equipmentItemsSOTBD[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= equipmentItemsSOTBD[btnNo].price;
            souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;

            pannelsActivated = false;
            loadoutMenu.pannelsActivated = false;

            //Unlock purchased item.
            playerEquipment.PurchaseTBD(equipmentItemsSOTBD[btnNo]);
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
