using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompanionUpgradesMenu : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool pannelsLoaded;
    public bool pannelsActivated;
    bool atTop;

    [Header("Strings")]
    readonly string header = "Companion Upgrades";

    [Header("Transforms")]
    public Transform contents;
    public Transform loyalSphereContents;
    public Transform attackSquareContents;

    [Header("TMP_Texts")]
    public TMP_Text headerText;

    [Header("Arrays")]
    public UpgradeItemsSO[] itemsSO;
    public UpgradeTemplate[] pannelsLoyalSphere;
    public UpgradeTemplate[] pannelsAttackSquare;
    public GameObject[] companions;

    [Header("Lists")]
    readonly List<UpgradeTemplate[]> companionPannels = new();

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
            for (int i = 0; i < companions.Length; i++)
                for (int j = 0; j < PlayerComponents.Instance.playerEquipment.boughtCompanions.Count; j++)
                    if (PlayerComponents.Instance.playerEquipment.boughtCompanions[j].title != null && PlayerComponents.Instance.playerEquipment.boughtCompanions[j].title.Contains(companions[i].name))
                        companions[i].SetActive(true);
            
            List<GameObject[]> companionObjects = new()
            {
                FindObject(pannelsLoyalSphere),
                FindObject(pannelsAttackSquare),
            };
            
            for (int i = 0; i < companionObjects.Count; i++)
                for (int j = 0; j < itemsSO.Length; j++)
                    companionObjects[i][j].SetActive(true);

            loyalSphereContents.position = new Vector3(1000f, loyalSphereContents.position.y);
            attackSquareContents.position = new Vector3(1000f, attackSquareContents.position.y);

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

        for (int i = 0; i < companionPannels.Count; i++)
            for (int j = 0; j < itemsSO.Length; j++)
            {
                companionPannels[i][j].titleText.text = itemsSO[j].title;
                companionPannels[i][j].descriptionText.text = itemsSO[j].description;
                companionPannels[i][j].priceText.text = "Price: " + itemsSO[j].price.ToString();

                companionPannels[i][j].counter.fillAmount = playerUpgrades.upgradesLoyalSphere.Where(x => x.title == itemsSO[j].title).Count() * 0.067f;

                if (playerUpgrades.upgradesLoyalSphere.Where(x => x.title == itemsSO[j].title).Count() == itemsSO[j].max) companionPannels[i][j].lights.SetActive(true);
            }

        pannelsLoaded = true;
    }

    void CheckUpgradesPurchaseable()
    {
        var upgradeMenu = UpgradeMenu.Instance;
        var playerUpgrades = PlayerComponents.Instance.playerUpgrades;

        List<Button[]> companionButtons = new()
        {
            FindButton(pannelsLoyalSphere),
            FindButton(pannelsAttackSquare),
        };

        for (int i = 0; i < companionButtons.Count; i++)
            for (int j = 0; j < itemsSO.Length; j++)
                if (upgradeMenu.souls >= itemsSO[j].price && playerUpgrades.upgradesLoyalSphere.Where(x => x.title == itemsSO[j].title).Count() < itemsSO[j].max)
                    companionButtons[i][j].interactable = true;
                else companionButtons[i][j].interactable = false;
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
        companionPannels.AddRange(new List<UpgradeTemplate[]>
        {
            pannelsLoyalSphere,
            pannelsAttackSquare,
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
