using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static SaveSystemSpace.SaveClasses;

public class ArmorUpgradesMenu : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool pannelsLoaded;
    public bool pannelsActivated;
    bool atTop;

    [Header("Strings")]
    readonly string header = "Armor Upgrades";

    [Header("Transforms")]
    public Transform contents;
    public Transform leatherContents;
    public Transform hideContents;
    public Transform ringMailContents;
    public Transform plateContents;

    [Header("TMP_Texts")]
    public TMP_Text headerText;

    [Header("Arrays")]
    public UpgradeItemsSO[] itemsSO;
    public UpgradeTemplate[] pannelsLeather;
    public UpgradeTemplate[] pannelsHide;
    public UpgradeTemplate[] pannelsRingMail;
    public UpgradeTemplate[] pannelsPlate;
    public GameObject[] armors;

    [Header("Lists")]
    readonly List<UpgradeTemplate[]> armorPannels = new();

    #endregion

    #region StartUpdate Methods

    void Awake()
    {
        PopulateArmorPannels();
    }

    void Update()
    {
        if (!pannelsActivated)
        {
            for (int i = 0; i < armors.Length; i++)
                for (int j = 0; j < PlayerComponents.Instance.playerEquipment.boughtArmors.Count; j++)
                    if (PlayerComponents.Instance.playerEquipment.boughtArmors[j].title != null && PlayerComponents.Instance.playerEquipment.boughtArmors[j].title.Contains(armors[i].name))
                        armors[i].SetActive(true);

            List<GameObject[]> armorObjects = new()
            {
                FindObject(pannelsLeather),
                FindObject(pannelsHide),
                FindObject(pannelsRingMail),
                FindObject(pannelsPlate),
            };

            for (int i = 0; i < armorObjects.Count; i++)
                for (int j = 0; j < itemsSO.Length; j++)
                    armorObjects[i][j].SetActive(true);

            leatherContents.position = new Vector3(1000f, leatherContents.position.y);
            hideContents.position = new Vector3(1000f, hideContents.position.y);
            ringMailContents.position = new Vector3(1000f, ringMailContents.position.y);
            plateContents.position = new Vector3(1000f, plateContents.position.y);

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
        
        for (int i = 0; i < armorPannels.Count; i++)
            for (int j = 0; j < itemsSO.Length; j++)
            {
                armorPannels[i][j].titleText.text = itemsSO[j].title;
                armorPannels[i][j].descriptionText.text = itemsSO[j].description;
                armorPannels[i][j].priceText.text = "Price: " + itemsSO[j].price.ToString();

                armorPannels[i][j].counter.fillAmount = playerUpgrades.upgradesLeather.Where(x => x.title == itemsSO[j].title).Count() * 0.067f;

                if (playerUpgrades.upgradesLeather.Where(x => x.title == itemsSO[j].title).Count() == itemsSO[j].max) armorPannels[i][j].lights.SetActive(true);
            }

        pannelsLoaded = true;
    }

    void CheckUpgradesPurchaseable()
    {
        var upgradeMenu = UpgradeMenu.Instance;
        var playerUpgrades = PlayerComponents.Instance.playerUpgrades;

        List<Button[]> armorButtons = new()
        {
            FindButton(pannelsLeather),
            FindButton(pannelsHide),
            FindButton(pannelsRingMail),
            FindButton(pannelsPlate),
        };

        for (int i = 0; i < armorButtons.Count; i++)
            for (int j = 0; j < itemsSO.Length; j++)
                if (upgradeMenu.souls >= itemsSO[j].price && playerUpgrades.upgradesLeather.Where(x => x.title == itemsSO[j].title).Count() < itemsSO[j].max)
                    armorButtons[i][j].interactable = true;
                else armorButtons[i][j].interactable = false;
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

    void PopulateArmorPannels()
    {
        armorPannels.AddRange(new List<UpgradeTemplate[]>
        {
            pannelsLeather,
            pannelsHide,
            pannelsRingMail,
            pannelsPlate,
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
