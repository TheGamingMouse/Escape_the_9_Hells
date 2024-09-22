using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    #region Variables

    [Header("Instance")]
    public static UpgradeMenu Instance;

    [Header("Floats")]
    public float souls;

    [Header("Bools")]
    public bool menuOpen;
    public bool menuCanClose;
    public bool menuCanOpen;
    bool playerStoped;
    public bool headerUpdated;

    [Header("Strings")]
    readonly string header = "Select Eqipment to Upgrade";

    [Header("GameObjects")]
    public GameObject contentsObj;
    public GameObject upgradeSelection;
    public GameObject weaponUpgrades;
    public GameObject companionUpgrades;
    public GameObject armorUpgrades;
    public GameObject backUpgrades;

    [Header("Transforms")]
    public Transform contentsWeapons;
    public Transform contentsCompanions;
    public Transform contentsArmors;
    public Transform contentsBacks;

    [Header("TMP_Pros")]
    public TMP_Text soulsText;
    public TMP_Text headerText;

    [Header("Components")]
    Jens jens;
    public WeaponUpgradesMenu weaponsMenu;
    public CompanionUpgradesMenu companionMenu;
    public ArmorUpgradesMenu armorMenu;
    public BackUpgradesMenu backMenu;
    Interactor interactor;

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
        interactor = PlayerComponents.Instance.player.GetComponent<Interactor>();

        if (UIManager.Instance.npcsActive)
        {
            if (NPCSpawner.Instance.jensSpawned)
            {
                jens = NPCSpawner.Instance.jens;
            }
        }

        if (!headerUpdated)
        {
            headerText.text = header;
            
            headerUpdated = true;
        }

        if (menuOpen)
        {
            contentsObj.SetActive(true);
            Time.timeScale = 0f;
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().startBool = false;
            playerStoped = true;
            Cursor.visible = true;
        }
        else
        {
            contentsObj.SetActive(false);
            if (playerStoped)
            {
                Time.timeScale = 1f;
                GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().startBool = true;
                playerStoped = false;
                Cursor.visible = false;
            }
        }

        souls = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls;
        soulsText.text = souls.ToString();
    }

    #endregion

    #region General Methods

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
        upgradeSelection.SetActive(true);
        weaponUpgrades.SetActive(false);
        companionUpgrades.SetActive(false);
        armorUpgrades.SetActive(false);
        backUpgrades.SetActive(false);

        menuOpen = false;
        menuCanClose = false;
        menuCanOpen = true;
        UIManager.Instance.jensTalking = false;
        jens.talking = false;
    }

    public void ReloadPannels()
    {
        weaponsMenu.pannelsActivated = false;
        companionMenu.pannelsActivated = false;
        armorMenu.pannelsActivated = false;
        backMenu.pannelsActivated = false;
    }

    public void ChangeHeader()
    {
        headerText.text = header;
    }

    public void ToTheTop()
    {
        contentsWeapons.position = new Vector3(contentsWeapons.position.x, contentsWeapons.position.y - 5000f);
        contentsCompanions.position = new Vector3(contentsCompanions.position.x, contentsCompanions.position.y - 5000f);
        contentsArmors	.position = new Vector3(contentsArmors.position.x, contentsArmors.position.y - 5000f);
        contentsBacks.position = new Vector3(contentsBacks.position.x, contentsBacks.position.y - 5000f);
    }

    #endregion
}
