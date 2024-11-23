using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackUpgradesMenu : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool pannelsLoaded;
    public bool pannelsActivated;
    bool atTop;

    [Header("Strings")]
    readonly string header = "Back Upgrades";

    [Header("Transforms")]
    public Transform contents;
    public Transform angelWingsContents;
    public Transform steelWingsContents;
    public Transform backpackContents;
    public Transform capeOWindContents;
    public Transform seedBagContents;

    [Header("TMP_Texts")]
    public TMP_Text headerText;

    [Header("Arrays")]
    public UpgradeItemsSO[] itemsSO;
    public UpgradeTemplate[] pannelsAngelWings;
    public UpgradeTemplate[] pannelsSteelWings;
    public UpgradeTemplate[] pannelsBackpack;
    public UpgradeTemplate[] pannelsCapeOWind;
    public UpgradeTemplate[] pannelsSeedBag;
    public GameObject[] backs;

    [Header("Lists")]
    readonly List<UpgradeTemplate[]> backPannels = new();

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
            for (int i = 0; i < backs.Length; i++)
                for (int j = 0; j < PlayerComponents.Instance.playerEquipment.boughtBacks.Count; j++)
                    if (PlayerComponents.Instance.playerEquipment.boughtBacks[j].title != null && PlayerComponents.Instance.playerEquipment.boughtBacks[j].title.Contains(backs[i].name))
                        backs[i].SetActive(true);

            List<GameObject[]> backObjects = new()
            {
                FindObject(pannelsAngelWings),
                FindObject(pannelsSteelWings),
                FindObject(pannelsBackpack),
                FindObject(pannelsCapeOWind),
                FindObject(pannelsSeedBag),
            };

            for (int i = 0; i < backObjects.Count; i++)
                for (int j = 0; j < itemsSO.Length; j++)
                    backObjects[i][j].SetActive(true);
            
            angelWingsContents.position = new Vector3(1000f, angelWingsContents.position.y);
            steelWingsContents.position = new Vector3(1000f, steelWingsContents.position.y);
            backpackContents.position = new Vector3(1000f, backpackContents.position.y);
            capeOWindContents.position = new Vector3(1000f, capeOWindContents.position.y);
            seedBagContents.position = new Vector3(1000f, seedBagContents.position.y);

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

        for (int i = 0; i < backPannels.Count; i++)
            for (int j = 0; j < itemsSO.Length; j++)
            {
                backPannels[i][j].titleText.text = itemsSO[j].title;
                backPannels[i][j].descriptionText.text = itemsSO[j].description;
                backPannels[i][j].priceText.text = "Price: " + itemsSO[j].price.ToString();

                backPannels[i][j].counter.fillAmount = playerUpgrades.upgradesAngelWings.Where(x => x.title == itemsSO[j].title).Count() * 0.067f;

                if (playerUpgrades.upgradesAngelWings.Where(x => x.title == itemsSO[j].title).Count() == itemsSO[j].max) backPannels[i][j].lights.SetActive(true);
            }

        pannelsLoaded = true;
    }

    void CheckUpgradesPurchaseable()
    {
        var upgradeMenu = UpgradeMenu.Instance;
        var playerUpgrades = PlayerComponents.Instance.playerUpgrades;

        List<Button[]> backButtons = new()
        {
            FindButton(pannelsAngelWings),
            FindButton(pannelsSteelWings),
            FindButton(pannelsBackpack),
            FindButton(pannelsCapeOWind),
            FindButton(pannelsSeedBag),
        };

        for (int i = 0; i < backButtons.Count; i++)
            for (int j = 0; j < itemsSO.Length; j++)
                if (upgradeMenu.souls >= itemsSO[j].price && playerUpgrades.upgradesAngelWings.Where(x => x.title == itemsSO[j].title).Count() < itemsSO[j].max)
                    backButtons[i][j].interactable = true;
                else backButtons[i][j].interactable = false;
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
        backPannels.AddRange(new List<UpgradeTemplate[]>
        {
            pannelsAngelWings,
            pannelsSteelWings,
            pannelsBackpack,
            pannelsCapeOWind,
            pannelsSeedBag,
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
