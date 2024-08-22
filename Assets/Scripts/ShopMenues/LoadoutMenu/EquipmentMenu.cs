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
    public bool menuCanClose;
    public bool menuCanOpen;
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

    [Header("TMP_Pros")]
    public TMP_Text soulsText;

    [Header("Scrollbars")]
    public Scrollbar weaponScroll;
    public Scrollbar companionScroll;
    public Scrollbar armorScroll;
    public Scrollbar backScroll;

    [Header("Arrays")]
    public LoadoutItemsSO[] equipmentItemsSOWeapons;
    public LoadoutItemsSO[] equipmentItemsSOCompanion;
    public LoadoutItemsSO[] equipmentItemsSOArmor;
    public LoadoutItemsSO[] equipmentItemsSOBack;
    public LoadoutTemplate[] equipmentPannelsWeapons;
    public LoadoutTemplate[] equipmentPannelsCompanion;
    public LoadoutTemplate[] equipmentPannelsArmor;
    public LoadoutTemplate[] equipmentPannelsBack;
    public GameObject[] equipmentPannelsSOWeapons;
    public GameObject[] equipmentPannelsSOCompanion;
    public GameObject[] equipmentPannelsSOArmor;
    public GameObject[] equipmentPannelsSOBack;
    public Button[] equipmentButtonsWeapons;
    public Button[] equipmentButtonsCompanion;
    public Button[] equipmentButtonsArmor;
    public Button[] equipmentButtonsBack;
    public Button[] equipmentButtonsDisable1;
    public Button[] equipmentButtonsDisable2;

    [Header("Components")]
    Alexander alexander;
    UIManager uiManager;
    NPCSpawner npcSpawner;
    PlayerEquipment playerEquipment;
    LoadoutMenu loadoutMenu;
    UpgradeMenu upgradeMenu;
    SFXAudioManager sfxManager;
    Interactor interactor;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        menuCanOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        var managers = GameObject.FindWithTag("Managers");
        var player = GameObject.FindWithTag("Player");

        uiManager = managers.GetComponent<UIManager>();
        playerEquipment = player.GetComponent<PlayerEquipment>();
        loadoutMenu = GameObject.FindWithTag("Canvas").transform.Find("Menus/npcConversations/Barbara").GetComponent<LoadoutMenu>();
        upgradeMenu = GameObject.FindWithTag("Canvas").transform.Find("Menus/npcConversations/Jens").GetComponent<UpgradeMenu>();
        sfxManager = managers.GetComponent<SFXAudioManager>();
        interactor = player.GetComponent<Interactor>();

        if (uiManager.npcsActive)
        {
            npcSpawner = GameObject.FindWithTag("NPC").GetComponent<NPCSpawner>();

            if (npcSpawner.alexSpawned)
            {
                alexander = npcSpawner.alexander;
            }
        }
        
        if (!pannelsActivated && playerEquipment.equipmentLoaded)
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
            for (int i = 0; i < equipmentItemsSOBack.Length; i++)
            {
                if (!playerEquipment.boughtBacks.Contains(equipmentItemsSOBack[i]))
                {
                    equipmentPannelsSOBack[i].SetActive(true);
                    equipmentPannelsBack[i].priceObj.SetActive(true);
                }
                else
                {
                    equipmentPannelsSOBack[i].SetActive(false);
                }
            }

            weaponContents.position = new Vector3(weaponContents.position.x, -10000f, 0);
            companionContents.position = new Vector3(companionContents.position.x, -10000f, 0f);
            armorContents.position = new Vector3(armorContents.position.x, -10000f, 0f);
            backContents.position = new Vector3(backContents.position.x, -10000f, 0f);

            CheckEquipmentPurchaseable();

            pannelsActivated = true;
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

            equipmentButtonsDisable1[i].gameObject.SetActive(false);
            equipmentButtonsDisable2[i].gameObject.SetActive(false);
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
        for (int i = 0; i < equipmentItemsSOBack.Length; i++)
        {
            equipmentPannelsBack[i].titleText.text = equipmentItemsSOBack[i].title;
            equipmentPannelsBack[i].descriptionText.text = equipmentItemsSOBack[i].description;
            equipmentPannelsBack[i].priceText.text = "Price: " + equipmentItemsSOBack[i].price.ToString();
            equipmentPannelsBack[i].buttonText.text = "Purchase";
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
        for (int i = 0; i < equipmentItemsSOBack.Length; i++)
        {
            equipmentButtonsBack[i].interactable = souls >= equipmentItemsSOBack[i].price;
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
            upgradeMenu.ReloadPannels();

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
            upgradeMenu.ReloadPannels();

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
            upgradeMenu.ReloadPannels();

            //Unlock purchased item.
            playerEquipment.PurchaseArmor(equipmentItemsSOArmor[btnNo]);
        }
    }

    public void PurchaseEquipmentBack(int btnNo)
    {
        if (souls >= equipmentItemsSOBack[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= equipmentItemsSOBack[btnNo].price;
            souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;

            pannelsActivated = false;
            loadoutMenu.pannelsActivated = false;
            upgradeMenu.ReloadPannels();

            //Unlock purchased item.
            playerEquipment.PurchaseBack(equipmentItemsSOBack[btnNo]);
        }
    }
    
    public void OpenStore()
    {
        if (!interactor.interacting)
        {
            menuCanOpen = false;
            menuOpen = true;
            menuCanClose = true;
        }
    }

    public void CloseStore()
    {
        menuOpen = false;
        menuCanClose = false;
        menuCanOpen = true;
        uiManager.alexanderTalking = false;
        alexander.talking = false;

        sfxManager.PlayAlexanderVO(false);
    }

    #endregion
}
