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
    public Transform back1Contents;
    public Transform back2Contents;

    [Header("TMP_Texts")]
    public TMP_Text headerText;

    [Header("Arrays")]
    public UpgradeItemsSO[] itemsSO;
    public UpgradeTemplate[] PannelsAngelWings;
    public UpgradeTemplate[] PannelsSteelWings;
    public UpgradeTemplate[] PannelsBackpack;
    public UpgradeTemplate[] PannelsCapeOWind;
    public UpgradeTemplate[] PannelsBacks1;
    public UpgradeTemplate[] PannelsBacks2;
    public GameObject[] PannelsSOAngelWings;
    public GameObject[] PannelsSOSteelWings;
    public GameObject[] PannelsSOBackpack;
    public GameObject[] PannelsSOCapeOWind;
    public GameObject[] PannelsSOBacks1;
    public GameObject[] PannelsSOBacks2;
    public Button[] ButtonsAngelWings;
    public Button[] ButtonsSteelWings;
    public Button[] ButtonsBackpack;
    public Button[] ButtonsCapeOWind;
    public Button[] ButtonsBacks1;
    public Button[] ButtonsBacks2;
    public GameObject[] backs;

    [Header("Components")]
    PlayerEquipment playerEquipment;
    UpgradeMenu upgradeMenu;
    PlayerUpgrades playerUpgrades;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        playerEquipment = GameObject.FindWithTag("Player").GetComponent<PlayerEquipment>();
        playerUpgrades = GameObject.FindWithTag("Player").GetComponent<PlayerUpgrades>();
        upgradeMenu = GameObject.FindWithTag("Canvas").transform.Find("Menus/npcConversations/Jens").GetComponent<UpgradeMenu>();
    }

    void Update()
    {
        if (!pannelsActivated)
        {
            for (int i = 0; i < backs.Length; i++)
            {
                for (int j = 0; j < playerEquipment.boughtBacks.Count; j++)
                {
                    if (playerEquipment.boughtBacks[j].title.ToLower().Contains(backs[i].name.ToLower()))
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

            // Back1
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOBacks1[i].SetActive(true);
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
            back1Contents.position = new Vector3(1000f, back1Contents.position.y);
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

        // Back1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsBacks1[i].titleText.text = itemsSO[i].title;
            PannelsBacks1[i].descriptionText.text = itemsSO[i].description;
            PannelsBacks1[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsBacks1[i].counter.fillAmount = playerUpgrades.upgradesBacks1.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesBacks1.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsBacks1[i].lights.SetActive(true);
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

        // Back1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesBacks1.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsBacks1[i].interactable = true;
            }
            else
            {
                ButtonsBacks1[i].interactable = false;
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
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddAngelWingsUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }

    public void PurchaseUpgradesSteelWings(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddSteelWingsUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }

    public void PurchaseUpgradesBackpack(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddBackpackUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }
    
    public void PurchaseUpgradesCapeOWind(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddCapeOWindUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }
    
    public void PurchaseUpgradesBack1(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddBack1Upgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }
    
    public void PurchaseUpgradesBack2(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddBack2Upgrade(itemsSO[btnNo]);
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
