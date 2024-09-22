using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutMenu : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static LoadoutMenu Instance;

    [Header("Ints")]
    int primaryIndex = -1;
    int secondaryIndex = -1;

    [Header("Bools")]
    public bool menuOpen;
    public bool menuCanClose;
    public bool menuCanOpen;
    bool pannelsLoaded;
    bool playerStoped;
    public bool pannelsActivated;
    bool isBackpackActive;
    bool isSeedBagActive;

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
    public Button[] selectLoadoutButtonsPrimaryWeapons;
    public Button[] selectLoadoutButtonsSecondaryWeapons;
    public Button[] selectLoadoutButtonsCompanion;
    public Button[] selectLoadoutButtonsPrimaryCompanion;
    public Button[] selectLoadoutButtonsSecondaryCompanion;
    public Button[] selectLoadoutButtonsArmor;
    public Button[] selectLoadoutButtonsBack;

    [Header("LoadoutItemsSO")]
    LoadoutItemsSO selectedPrimaryWeapon;
    LoadoutItemsSO selectedSecondaryWeapon;
    LoadoutItemsSO selectedPrimaryCompanion;
    LoadoutItemsSO selectedSecondaryCompanion;
    LoadoutItemsSO selectedArmor;
    LoadoutItemsSO selectedBack;

    [Header("Components")]
    Barbara barbara;
    PlayerLoadout playerLoadout;
    UIManager uiManager;
    NPCSpawner npcSpawner;
    PlayerEquipment playerEquipment;
    SFXAudioManager sfxManager;

    #endregion

    #region StartUpdate Methods

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        menuCanOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        var player = PlayerComponents.Instance.player;

        playerLoadout = player.GetComponent<PlayerLoadout>();
        playerEquipment = player.GetComponent<PlayerEquipment>();
        uiManager = managers.GetComponent<UIManager>();
        sfxManager = managers.GetComponent<SFXAudioManager>();

        if (UIManager.Instance.npcsActive)
        {
            npcSpawner = GameObject.FindWithTag("NPC").GetComponent<NPCSpawner>();
        }
        
        if (!pannelsActivated && PlayerComponents.Instance.playerEquipment.equipmentLoaded)
        {
            for (int i = 0; i < loadoutItemsSOWeapons.Length; i++)
            {
                if (PlayerComponents.Instance.playerEquipment.boughtWeapons.Contains(loadoutItemsSOWeapons[i]))
                {
                    loadoutPannelsSOWeapons[i].SetActive(true);
                }
            }
            for (int i = 0; i < loadoutItemsSOCompanion.Length; i++)
            {
                if (PlayerComponents.Instance.playerEquipment.boughtCompanions.Contains(loadoutItemsSOCompanion[i]))
                {
                    loadoutPannelsSOCompanion[i].SetActive(true);
                }
            }
            for (int i = 0; i < loadoutItemsSOArmor.Length; i++)
            {
                if (PlayerComponents.Instance.playerEquipment.boughtArmors.Contains(loadoutItemsSOArmor[i]))
                {
                    loadoutPannelsSOArmor[i].SetActive(true);
                }
            }
            for (int i = 0; i < loadoutItemsSOBack.Length; i++)
            {
                if (PlayerComponents.Instance.playerEquipment.boughtBacks.Contains(loadoutItemsSOBack[i]))
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
        LoadWeapons();
        LoadCompanions();
        LoadArmors();
        LoadBacks();

        if (!selectedPrimaryWeapon)
        {
            selectedPrimaryWeapon = loadoutItemsSOWeapons[0];
            selectLoadoutButtonsWeapons[0].interactable = false;
        }
        if (!selectedPrimaryCompanion)
        {
            selectedPrimaryCompanion = loadoutItemsSOCompanion[0];
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

    void LoadWeapons()
    {
        if (!selectedBack)
        {
            if (PlayerComponents.Instance.playerLoadout.backpackActive)
            {
                isBackpackActive = true;
            }
            else
            {
                isBackpackActive = false;
            }
        }
        else
        {
            if (selectedBack.title == "Backpack")
            {
                isBackpackActive = true;
            }
            else
            {
                isBackpackActive = false;
            }
        }
        if (isBackpackActive)
        {
            var playerLoadout = PlayerComponents.Instance.playerLoadout;

            for (int i = 0; i < loadoutItemsSOWeapons.Length; i++)
            {
                loadoutPannelsWeapons[i].titleText.text = loadoutItemsSOWeapons[i].title;
                loadoutPannelsWeapons[i].descriptionText.text = loadoutItemsSOWeapons[i].description;
                
                selectLoadoutButtonsWeapons[i].gameObject.SetActive(false);
                selectLoadoutButtonsPrimaryWeapons[i].gameObject.SetActive(true);
                selectLoadoutButtonsSecondaryWeapons[i].gameObject.SetActive(true);

                if (playerLoadout.selectedPrimaryWeapon && (loadoutItemsSOWeapons[i].title == playerLoadout.selectedPrimaryWeapon.title || 
                    loadoutItemsSOWeapons[i].title == playerLoadout.selectedSecondaryWeapon.title))
                {
                    if (loadoutItemsSOWeapons[i].title == playerLoadout.selectedPrimaryWeapon.title)
                    {
                        selectedPrimaryWeapon = loadoutItemsSOWeapons[i];
                    }
                    primaryIndex = i;
                    selectLoadoutButtonsPrimaryWeapons[i].interactable = false;
                }
                else
                {
                    selectLoadoutButtonsPrimaryWeapons[i].interactable = true;
                }

                if (playerLoadout.selectedSecondaryWeapon && (loadoutItemsSOWeapons[i].title == playerLoadout.selectedSecondaryWeapon.title ||
                    loadoutItemsSOWeapons[i].title == playerLoadout.selectedPrimaryWeapon.title))
                {
                    if (loadoutItemsSOWeapons[i].title == playerLoadout.selectedSecondaryWeapon.title)
                    {
                        selectedSecondaryWeapon = loadoutItemsSOWeapons[i];
                    }
                    secondaryIndex = i;
                    selectLoadoutButtonsSecondaryWeapons[i].interactable = false;
                }
                else
                {
                    selectLoadoutButtonsSecondaryWeapons[i].interactable = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < loadoutItemsSOWeapons.Length; i++)
            {
                loadoutPannelsWeapons[i].titleText.text = loadoutItemsSOWeapons[i].title;
                loadoutPannelsWeapons[i].descriptionText.text = loadoutItemsSOWeapons[i].description;
                
                selectLoadoutButtonsWeapons[i].gameObject.SetActive(true);
                selectLoadoutButtonsPrimaryWeapons[i].gameObject.SetActive(false);
                selectLoadoutButtonsSecondaryWeapons[i].gameObject.SetActive(false);

                if (PlayerComponents.Instance.playerLoadout.selectedPrimaryWeapon && loadoutItemsSOWeapons[i].title == PlayerComponents.Instance.playerLoadout.selectedPrimaryWeapon.title)
                {
                    selectedPrimaryWeapon = loadoutItemsSOWeapons[i];
                    selectLoadoutButtonsWeapons[i].interactable = false;
                }
                else
                {
                    selectLoadoutButtonsWeapons[i].interactable = true;
                }
            }
        }
    }

    void LoadCompanions()
    {
        if (!selectedBack)
        {
            if (PlayerComponents.Instance.playerLoadout.seedBagActive)
            {
                isSeedBagActive = true;
            }
            else
            {
                isSeedBagActive = false;
            }
        }
        else
        {
            if (selectedBack.title == "Seed Bag")
            {
                isSeedBagActive = true;
            }
            else
            {
                isSeedBagActive = false;
            }
        }
        if (isSeedBagActive)
        {
            var playerLoadout = PlayerComponents.Instance.playerLoadout;

            for (int i = 0; i < loadoutItemsSOCompanion.Length; i++)
            {
                loadoutPannelsCompanion[i].titleText.text = loadoutItemsSOCompanion[i].title;
                loadoutPannelsCompanion[i].descriptionText.text = loadoutItemsSOCompanion[i].description;
                
                selectLoadoutButtonsCompanion[i].gameObject.SetActive(false);
                selectLoadoutButtonsPrimaryCompanion[i].gameObject.SetActive(true);
                selectLoadoutButtonsSecondaryCompanion[i].gameObject.SetActive(true);

                if (playerLoadout.selectedPrimaryCompanion && (loadoutItemsSOCompanion[i].title == playerLoadout.selectedPrimaryCompanion.title || 
                    loadoutItemsSOCompanion[i].title == playerLoadout.selectedSecondaryCompanion.title))
                {
                    if (loadoutItemsSOCompanion[i].title == playerLoadout.selectedPrimaryCompanion.title)
                    {
                        selectedPrimaryCompanion = loadoutItemsSOCompanion[i];
                    }
                    primaryIndex = i;
                    selectLoadoutButtonsPrimaryCompanion[i].interactable = false;
                }
                else
                {
                    selectLoadoutButtonsPrimaryCompanion[i].interactable = true;
                }

                if (playerLoadout.selectedSecondaryCompanion && (loadoutItemsSOCompanion[i].title == playerLoadout.selectedSecondaryCompanion.title ||
                    loadoutItemsSOCompanion[i].title == playerLoadout.selectedPrimaryCompanion.title))
                {
                    if (loadoutItemsSOCompanion[i].title == playerLoadout.selectedSecondaryCompanion.title)
                    {
                        selectedSecondaryCompanion = loadoutItemsSOCompanion[i];
                    }
                    secondaryIndex = i;
                    selectLoadoutButtonsSecondaryCompanion[i].interactable = false;
                }
                else
                {
                    selectLoadoutButtonsSecondaryCompanion[i].interactable = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < loadoutItemsSOCompanion.Length; i++)
            {
                loadoutPannelsCompanion[i].titleText.text = loadoutItemsSOCompanion[i].title;
                loadoutPannelsCompanion[i].descriptionText.text = loadoutItemsSOCompanion[i].description;
                
                selectLoadoutButtonsCompanion[i].gameObject.SetActive(true);
                selectLoadoutButtonsPrimaryCompanion[i].gameObject.SetActive(false);
                selectLoadoutButtonsSecondaryCompanion[i].gameObject.SetActive(false);

                if (PlayerComponents.Instance.playerLoadout.selectedPrimaryCompanion && loadoutItemsSOCompanion[i].title == PlayerComponents.Instance.playerLoadout.selectedPrimaryCompanion.title)
                {
                    selectedPrimaryCompanion = loadoutItemsSOCompanion[i];
                    selectLoadoutButtonsCompanion[i].interactable = false;
                }
                else
                {
                    selectLoadoutButtonsCompanion[i].interactable = true;
                }
            }
        }
    }

    void LoadArmors()
    {
        for (int i = 0; i < loadoutItemsSOArmor.Length; i++)
        {
            loadoutPannelsArmor[i].titleText.text = loadoutItemsSOArmor[i].title;
            loadoutPannelsArmor[i].descriptionText.text = loadoutItemsSOArmor[i].description;

            if (PlayerComponents.Instance.playerLoadout.selectedArmor && loadoutItemsSOArmor[i].title == PlayerComponents.Instance.playerLoadout.selectedArmor.title)
            {
                selectedArmor = loadoutItemsSOArmor[i];
                selectLoadoutButtonsArmor[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsArmor[i].interactable = true;
            }
        }
    }

    void LoadBacks()
    {
        for (int i = 0; i < loadoutItemsSOBack.Length; i++)
        {
            loadoutPannelsBack[i].titleText.text = loadoutItemsSOBack[i].title;
            loadoutPannelsBack[i].descriptionText.text = loadoutItemsSOBack[i].description;

            if (PlayerComponents.Instance.playerLoadout.selectedBack && loadoutItemsSOBack[i].title == PlayerComponents.Instance.playerLoadout.selectedBack.title)
            {
                selectedBack = loadoutItemsSOBack[i];
                selectLoadoutButtonsBack[i].interactable = false;
            }
            else
            {
                selectLoadoutButtonsBack[i].interactable = true;
            }
        }
    }

    public void SelectedLoadoutWeapon(int btnNo)
    {
        selectedPrimaryWeapon = loadoutItemsSOWeapons[btnNo];
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
    public void SelectedLoadoutPrimaryWeapon(int btnNo)
    {
        selectedPrimaryWeapon = loadoutItemsSOWeapons[btnNo];
        primaryIndex = btnNo;
        for (int i = 0; i < selectLoadoutButtonsPrimaryWeapons.Length; i++)
        {
            if (selectLoadoutButtonsPrimaryWeapons[i] == selectLoadoutButtonsPrimaryWeapons[btnNo])
            {
                selectLoadoutButtonsPrimaryWeapons[i].interactable = false;
                selectLoadoutButtonsSecondaryWeapons[i].interactable = false;
            }
            else if (selectLoadoutButtonsPrimaryWeapons[i] != selectLoadoutButtonsPrimaryWeapons[secondaryIndex] && 
                        selectLoadoutButtonsSecondaryWeapons[i] != selectLoadoutButtonsSecondaryWeapons[secondaryIndex])
            {
                selectLoadoutButtonsPrimaryWeapons[i].interactable = true;
                selectLoadoutButtonsSecondaryWeapons[i].interactable = true;
            }
        }
    }
    public void SelectedLoadoutSecondaryWeapon(int btnNo)
    {
        selectedSecondaryWeapon = loadoutItemsSOWeapons[btnNo];
        secondaryIndex = btnNo;
        for (int i = 0; i < selectLoadoutButtonsSecondaryWeapons.Length; i++)
        {
            if (selectLoadoutButtonsSecondaryWeapons[i] == selectLoadoutButtonsSecondaryWeapons[btnNo])
            {
                selectLoadoutButtonsPrimaryWeapons[i].interactable = false;
                selectLoadoutButtonsSecondaryWeapons[i].interactable = false;
            }
            else if (selectLoadoutButtonsPrimaryWeapons[i] != selectLoadoutButtonsPrimaryWeapons[primaryIndex] && 
                        selectLoadoutButtonsSecondaryWeapons[i] != selectLoadoutButtonsSecondaryWeapons[primaryIndex])
            {
                selectLoadoutButtonsPrimaryWeapons[i].interactable = true;
                selectLoadoutButtonsSecondaryWeapons[i].interactable = true;
            }
        }
    }
    public void SelectedLoadoutCompanion(int btnNo)
    {
        selectedPrimaryCompanion = loadoutItemsSOCompanion[btnNo];
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
    public void SelectedLoadoutPrimaryCompanion(int btnNo)
    {
        selectedPrimaryCompanion = loadoutItemsSOCompanion[btnNo];
        primaryIndex = btnNo;
        for (int i = 0; i < selectLoadoutButtonsPrimaryCompanion.Length; i++)
        {
            if (selectLoadoutButtonsPrimaryCompanion[i] == selectLoadoutButtonsPrimaryCompanion[btnNo])
            {
                selectLoadoutButtonsPrimaryCompanion[i].interactable = false;
                selectLoadoutButtonsSecondaryCompanion[i].interactable = false;
            }
            else if (selectLoadoutButtonsPrimaryCompanion[i] != selectLoadoutButtonsPrimaryCompanion[secondaryIndex] && 
                        selectLoadoutButtonsSecondaryCompanion[i] != selectLoadoutButtonsSecondaryCompanion[secondaryIndex])
            {
                selectLoadoutButtonsPrimaryCompanion[i].interactable = true;
                selectLoadoutButtonsSecondaryCompanion[i].interactable = true;
            }
        }
    }
    public void SelectedLoadoutSecondaryCompanion(int btnNo)
    {
        selectedSecondaryCompanion = loadoutItemsSOCompanion[btnNo];
        secondaryIndex = btnNo;
        for (int i = 0; i < selectLoadoutButtonsSecondaryCompanion.Length; i++)
        {
            if (selectLoadoutButtonsSecondaryCompanion[i] == selectLoadoutButtonsSecondaryCompanion[btnNo])
            {
                selectLoadoutButtonsPrimaryCompanion[i].interactable = false;
                selectLoadoutButtonsSecondaryCompanion[i].interactable = false;
            }
            else if (selectLoadoutButtonsPrimaryCompanion[i] != selectLoadoutButtonsPrimaryCompanion[primaryIndex] && 
                        selectLoadoutButtonsSecondaryCompanion[i] != selectLoadoutButtonsSecondaryCompanion[primaryIndex])
            {
                selectLoadoutButtonsPrimaryCompanion[i].interactable = true;
                selectLoadoutButtonsSecondaryCompanion[i].interactable = true;
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

        LoadWeapons();
        LoadCompanions();
    }

    public void ConfirmSelected()
    {
        PlayerComponents.Instance.playerLoadout.SetLoadout(selectedPrimaryWeapon, selectedSecondaryWeapon, selectedPrimaryCompanion, selectedSecondaryCompanion, selectedArmor, selectedBack);

        ExitStore();
    }

    public void ExitStore()
    {
        selectedPrimaryWeapon = null;
        selectedSecondaryWeapon = null;
        selectedPrimaryCompanion = null;
        selectedSecondaryCompanion = null;
        selectedArmor = null;
        selectedBack = null;
        LoadLoadoutPannels();
        CloseStore();
    }

    IEnumerator SetMenuCanClose()
    {
        yield return new WaitForSeconds(0.1f);
        menuCanClose = true;
    }

    IEnumerator SetMenuCanOpen()
    {
        yield return new WaitForSeconds(0.1f);
        menuCanOpen = true;
    }

    public void OpenStore()
    {
        menuCanOpen = false;
        menuOpen = true;
        StartCoroutine(SetMenuCanClose());
    }

    public void CloseStore()
    {
        if (barbara)
        {
            menuOpen = false;
            menuCanClose = false;
            uiManager.barbaraTalking = false;
            barbara.talking = false;
            StartCoroutine(SetMenuCanOpen());

            sfxManager.PlayBarbaraVO(false);
        }
    }

    #endregion
}
