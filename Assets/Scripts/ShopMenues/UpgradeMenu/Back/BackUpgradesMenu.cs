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
    public Transform back2Contents;

    [Header("TMP_Texts")]
    public TMP_Text headerText;

    [Header("Arrays")]
    public UpgradeItemsSO[] itemsSO;
    public UpgradeTemplate[] PannelsAngelWings;
    public UpgradeTemplate[] PannelsSteelWings;
    public UpgradeTemplate[] PannelsBackpack;
    public UpgradeTemplate[] PannelsCapeOWind;
    public UpgradeTemplate[] PannelsSeedBag;
    public UpgradeTemplate[] PannelsBacks2;
    public GameObject[] PannelsSOAngelWings;
    public GameObject[] PannelsSOSteelWings;
    public GameObject[] PannelsSOBackpack;
    public GameObject[] PannelsSOCapeOWind;
    public GameObject[] PannelsSOSeedBag;
    public GameObject[] PannelsSOBacks2;
    public Button[] ButtonsAngelWings;
    public Button[] ButtonsSteelWings;
    public Button[] ButtonsBackpack;
    public Button[] ButtonsCapeOWind;
    public Button[] ButtonsSeedBag;
    public Button[] ButtonsBacks2;
    public GameObject[] backs;

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (!pannelsActivated)
        {
            for (int i = 0; i < backs.Length; i++)
            {
                for (int j = 0; j < PlayerComponents.Instance.playerEquipment.boughtBacks.Count; j++)
                {
                    if (PlayerComponents.Instance.playerEquipment.boughtBacks[j].title.ToLower().Contains(backs[i].name.ToLower()))
                    {
                        backs[i].SetActive(true);
                    }
                }
            }

            // AngelWings
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOAngelWings[i].SetActive(true);
            }

            // SteelWings
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOSteelWings[i].SetActive(true);
            }

            // Backpack
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOBackpack[i].SetActive(true);
            }

            // CapeOWind
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOCapeOWind[i].SetActive(true);
            }

            // SeedBag
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOSeedBag[i].SetActive(true);
            }

            // Back2
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOBacks2[i].SetActive(true);
            }
            
            angelWingsContents.position = new Vector3(1000f, angelWingsContents.position.y);
            steelWingsContents.position = new Vector3(1000f, steelWingsContents.position.y);
            backpackContents.position = new Vector3(1000f, backpackContents.position.y);
            capeOWindContents.position = new Vector3(1000f, capeOWindContents.position.y);
            seedBagContents.position = new Vector3(1000f, seedBagContents.position.y);
            back2Contents.position = new Vector3(1000f, back2Contents.position.y);

            CheckUpgradesPurchaseable();

            pannelsActivated = true;
        }
        
        if (!pannelsLoaded)
        {
            LoadUpgradePannels();
        }
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

        // AngelWings
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsAngelWings[i].titleText.text = itemsSO[i].title;
            PannelsAngelWings[i].descriptionText.text = itemsSO[i].description;
            PannelsAngelWings[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsAngelWings[i].counter.fillAmount = playerUpgrades.upgradesAngelWings.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesAngelWings.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsAngelWings[i].lights.SetActive(true);
            }
        }

        // SteelWings
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsSteelWings[i].titleText.text = itemsSO[i].title;
            PannelsSteelWings[i].descriptionText.text = itemsSO[i].description;
            PannelsSteelWings[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsSteelWings[i].counter.fillAmount = playerUpgrades.upgradesSteelWings.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesSteelWings.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsSteelWings[i].lights.SetActive(true);
            }
        }

        // Backpack
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsBackpack[i].titleText.text = itemsSO[i].title;
            PannelsBackpack[i].descriptionText.text = itemsSO[i].description;
            PannelsBackpack[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsBackpack[i].counter.fillAmount = playerUpgrades.upgradesBackpacks.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesBackpacks.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsBackpack[i].lights.SetActive(true);
            }
        }

        // CapeOWind
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsCapeOWind[i].titleText.text = itemsSO[i].title;
            PannelsCapeOWind[i].descriptionText.text = itemsSO[i].description;
            PannelsCapeOWind[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsCapeOWind[i].counter.fillAmount = playerUpgrades.upgradesCapeOWinds.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesCapeOWinds.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsCapeOWind[i].lights.SetActive(true);
            }
        }

        // SeedBag
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsSeedBag[i].titleText.text = itemsSO[i].title;
            PannelsSeedBag[i].descriptionText.text = itemsSO[i].description;
            PannelsSeedBag[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsSeedBag[i].counter.fillAmount = playerUpgrades.upgradesSeedBag.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesSeedBag.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsSeedBag[i].lights.SetActive(true);
            }
        }

        // Back2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsBacks2[i].titleText.text = itemsSO[i].title;
            PannelsBacks2[i].descriptionText.text = itemsSO[i].description;
            PannelsBacks2[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsBacks2[i].counter.fillAmount = playerUpgrades.upgradesBacks2.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesBacks2.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsBacks2[i].lights.SetActive(true);
            }
        }

        pannelsLoaded = true;
    }

    void CheckUpgradesPurchaseable()
    {
        var upgradeMenu = UpgradeMenu.Instance;
        var playerUpgrades = PlayerComponents.Instance.playerUpgrades;

        // AngelWings
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesAngelWings.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsAngelWings[i].interactable = true;
            }
            else
            {
                ButtonsAngelWings[i].interactable = false;
            }
        }

        // SteelWings
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesSteelWings.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsSteelWings[i].interactable = true;
            }
            else
            {
                ButtonsSteelWings[i].interactable = false;
            }
        }

        // Backpack
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesBackpacks.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsBackpack[i].interactable = true;
            }
            else
            {
                ButtonsBackpack[i].interactable = false;
            }
        }

        // CapeOWind
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesCapeOWinds.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsCapeOWind[i].interactable = true;
            }
            else
            {
                ButtonsCapeOWind[i].interactable = false;
            }
        }

        // SeedBag
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesSeedBag.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsSeedBag[i].interactable = true;
            }
            else
            {
                ButtonsSeedBag[i].interactable = false;
            }
        }

        // Back2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesBacks2.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsBacks2[i].interactable = true;
            }
            else
            {
                ButtonsBacks2[i].interactable = false;
            }
        }
    }

    public void PurchaseUpgradesAngelWings(int btnNo)
    {
        if (UpgradeMenu.Instance.souls >= itemsSO[btnNo].price)
        {
            PlayerComponents.Instance.playerLevel.souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            PlayerComponents.Instance.playerUpgrades.AddAngelWingsUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }

    public void PurchaseUpgradesSteelWings(int btnNo)
    {
        if (UpgradeMenu.Instance.souls >= itemsSO[btnNo].price)
        {
            PlayerComponents.Instance.playerLevel.souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            PlayerComponents.Instance.playerUpgrades.AddSteelWingsUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }

    public void PurchaseUpgradesBackpack(int btnNo)
    {
        if (UpgradeMenu.Instance.souls >= itemsSO[btnNo].price)
        {
            PlayerComponents.Instance.playerLevel.souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            PlayerComponents.Instance.playerUpgrades.AddBackpackUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }
    
    public void PurchaseUpgradesCapeOWind(int btnNo)
    {
        if (UpgradeMenu.Instance.souls >= itemsSO[btnNo].price)
        {
            PlayerComponents.Instance.playerLevel.souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            PlayerComponents.Instance.playerUpgrades.AddCapeOWindUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }
    
    public void PurchaseUpgradesSeedBag(int btnNo)
    {
        if (UpgradeMenu.Instance.souls >= itemsSO[btnNo].price)
        {
            PlayerComponents.Instance.playerLevel.souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            PlayerComponents.Instance.playerUpgrades.AddSeedBagUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }

    public void ChangeHeader()
    {
        headerText.text = header;

        contents.position = new Vector3(contents.position.x, contents.position.y - 5000f);
    }

    #endregion
}
