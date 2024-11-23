using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveSystemSpace.SaveClasses.EquipmentData.WeaponData;

public class WeaponUpgradesMenu : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool pannelsLoaded;
    public bool pannelsActivated;
    [SerializeField] bool atTop;

    [Header("Strings")]
    readonly string header = "Weapon Upgrades";

    [Header("Transforms")]
    public Transform contents;
    public Transform pugioContents;
    public Transform ulfberhtContents;

    [Header("TMP_Texts")]
    public TMP_Text headerText;

    [Header("Arrays")]
    public UpgradeItemsSO[] itemsSO;
    public UpgradeTemplate[] pannelsPugio;
    public UpgradeTemplate[] pannelsUlfberht;
    public GameObject[] weapons;

    [Header("Lists")]
    readonly List<UpgradeTemplate[]> weaponPannels = new();

    #endregion

    #region StartUpdate Methods

    void Awake()
    {
        PopulatePannelsLists();
    }

    void Update()
    {
        if (!pannelsActivated)
        {
            for (int i = 0; i < weapons.Length; i++)
                for (int j = 0; j < PlayerComponents.Instance.playerEquipment.boughtWeapons.Count; j++)
                    if (PlayerComponents.Instance.playerEquipment.boughtWeapons[j].title != null && PlayerComponents.Instance.playerEquipment.boughtWeapons[j].title.Contains(weapons[i].name))
                        weapons[i].SetActive(true);
            
            List<GameObject[]> weaponObjects = new()
            {
                FindObject(pannelsPugio),
                FindObject(pannelsUlfberht),
            };

            for (int i = 0; i < weaponObjects.Count; i++)
                for (int j = 0; j < itemsSO.Length; j++)
                    weaponObjects[i][j].SetActive(true);

            pugioContents.position = new Vector3(1000f, pugioContents.position.y);
            ulfberhtContents.position = new Vector3(1000f, ulfberhtContents.position.y);

            CheckUpgradesPurchaseable();

            pannelsActivated = true;
        }
        
        if (!pannelsLoaded) LoadUpgradePannels();
        CheckUpgradesPurchaseable();

        if (!atTop)
        {
            contents.position = new Vector3(contents.position.x, contents.position.y - 5000f);
            atTop = true;
        }
    }

    #endregion

    #region General Methods

    void LoadUpgradePannels()
    {
        var playerUpgrades = PlayerComponents.Instance.playerUpgrades;

        // Pugio
        for (int i = 0; i < weaponPannels.Count; i++)
            for (int j = 0; j < itemsSO.Length; j++)
            {
                weaponPannels[i][j].titleText.text = itemsSO[j].title;
                weaponPannels[i][j].descriptionText.text = itemsSO[j].description;
                weaponPannels[i][j].priceText.text = "Price: " + itemsSO[j].price.ToString();
                weaponPannels[i][j].counter.fillAmount = playerUpgrades.upgradesPugio.Where(x => x.title == itemsSO[j].title).Count() * 0.067f;

                if (weaponPannels[i][j].titleText.text == specialAttackUpgradeString) weaponPannels[i][j].border.SetActive(false);
                if (playerUpgrades.upgradesPugio.Where(x => x.title == itemsSO[j].title).Count() == itemsSO[j].max) weaponPannels[i][j].lights.SetActive(true);
            }

        pannelsLoaded = true;
    }

    void CheckUpgradesPurchaseable()
    {
        var playerUpgrades = PlayerComponents.Instance.playerUpgrades;
        var upgradeMenu = UpgradeMenu.Instance;

        List<Button[]> weaponButtons = new()
        {
            FindButton(pannelsPugio),
            FindButton(pannelsUlfberht),
        };

        for (int i = 0; i < weaponButtons.Count; i++)
            for (int j = 0; j < itemsSO.Length; j++)
                if (itemsSO[j].title == specialCooldownUpgradeString)
                {
                    for (int k = 0; k < playerUpgrades.upgradesPugio.Count; k++)
                        if (upgradeMenu.souls >= itemsSO[k].price && playerUpgrades.upgradesPugio[k].title.Contains("Special Attack") && playerUpgrades.upgradesPugio.Where(x => x.title == itemsSO[k].title).Count() < itemsSO[k].max)
                        {
                            weaponButtons[i][k].interactable = true;
                            break;
                        }
                }
                else if (itemsSO[j].title == specialAttackUpgradeString)
                    if (playerUpgrades.upgradesPugio.Count != 0)
                    {
                        for (int k = 0; k < playerUpgrades.upgradesPugio.Count; k++)
                            if (upgradeMenu.souls >= itemsSO[k].price && !playerUpgrades.upgradesPugio[k].title.Contains(itemsSO[k].title))
                                weaponButtons[i][k].interactable = true;
                            else
                            {
                                weaponButtons[i][k].interactable = false;
                                break;
                            }
                    }
                    else if (upgradeMenu.souls >= itemsSO[j].price) weaponButtons[i][j].interactable = true;
                else if (upgradeMenu.souls >= itemsSO[j].price && playerUpgrades.upgradesPugio.Where(x => x.title == itemsSO[j].title).Count() < itemsSO[j].max)
                    weaponButtons[i][j].interactable = true;
                else weaponButtons[i][j].interactable = false;
    }

    public void PurchaseUpgrade(int btnNo)
    {
        if (UpgradeMenu.Instance.souls < itemsSO[btnNo].price) return;

        var playerData = SaveSystem.loadedPlayerData;

        playerData.currentSouls -= itemsSO[btnNo].price;
        PlayerComponents.Instance.playerLevel.souls -= itemsSO[btnNo].price;

        SaveSystem.Instance.Save(playerData, SaveSystem.playerDataPath);

        PlayerComponents.Instance.playerUpgrades.AddUpgrade(itemsSO[btnNo], itemsSO[btnNo].title);
        pannelsLoaded = false;
    }

    public void ChangeHeader()
    {
        headerText.text = header;
        contents.position = new Vector3(contents.position.x, contents.position.y - 5000f);
    }

    void PopulatePannelsLists()
    {
        weaponPannels.AddRange(new List<UpgradeTemplate[]>
        {
            pannelsPugio,
            pannelsUlfberht,
        });
    }

    GameObject[] FindObject(UpgradeTemplate[] pannels)
    {
        GameObject[] objects = new GameObject[pannels.Length];
        for (int i = 0; i < pannels.Length; i++)
            objects[i] = pannels[i].gameObject;
        
        return objects;
    }

    Button[] FindButton(UpgradeTemplate[] pannels)
    {
        Button[] buttons = new Button[pannels.Length];
        for (int i = 0; i < pannels.Length; i++)
            buttons[i] = pannels[i].transform.Find("BuyButton").GetComponent<Button>();

        return buttons;
    }

    #endregion
}
