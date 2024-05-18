using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackUpgradesMenu : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool pannelsLoaded;
    public bool pannelsActivated;

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

    // Update is called once per frame
    void Update()
    {
        playerEquipment = GameObject.FindWithTag("Player").GetComponent<PlayerEquipment>();
        playerUpgrades = GameObject.FindWithTag("Player").GetComponent<PlayerUpgrades>();
        upgradeMenu = GameObject.FindWithTag("Canvas").transform.Find("Menus/npcConversations/Jens").GetComponent<UpgradeMenu>();
        
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

            contents.position = new Vector3(contents.position.x, contents.position.y - 5000f);
            
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
        }

        // SteelWings
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsSteelWings[i].titleText.text = itemsSO[i].title;
            PannelsSteelWings[i].descriptionText.text = itemsSO[i].description;
            PannelsSteelWings[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // Backpack
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsBackpack[i].titleText.text = itemsSO[i].title;
            PannelsBackpack[i].descriptionText.text = itemsSO[i].description;
            PannelsBackpack[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // CapeOWind
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsCapeOWind[i].titleText.text = itemsSO[i].title;
            PannelsCapeOWind[i].descriptionText.text = itemsSO[i].description;
            PannelsCapeOWind[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // Back1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsBacks1[i].titleText.text = itemsSO[i].title;
            PannelsBacks1[i].descriptionText.text = itemsSO[i].description;
            PannelsBacks1[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // Back2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsBacks2[i].titleText.text = itemsSO[i].title;
            PannelsBacks2[i].descriptionText.text = itemsSO[i].description;
            PannelsBacks2[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        pannelsLoaded = true;
    }

    void CheckUpgradesPurchaseable()
    {
        // AngelWings
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsAngelWings[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // SteelWings
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsSteelWings[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // Backpack
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsBackpack[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // CapeOWind
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsCapeOWind[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // Back1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsBacks1[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // Back2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsBacks2[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }
    }

    public void PurchaseUpgradesAngelWings(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddAngelWingsUpgrade(itemsSO[btnNo]);
        }
    }

    public void PurchaseUpgradesSteelWings(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddSteelWingsUpgrade(itemsSO[btnNo]);
        }
    }

    public void PurchaseUpgradesBackpack(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddBackpackUpgrade(itemsSO[btnNo]);
        }
    }
    
    public void PurchaseUpgradesCapeOWind(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddCapeOWindUpgrade(itemsSO[btnNo]);
        }
    }
    
    public void PurchaseUpgradesBack1(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddBack1Upgrade(itemsSO[btnNo]);
        }
    }
    
    public void PurchaseUpgradesBack2(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddBack2Upgrade(itemsSO[btnNo]);
        }
    }

    public void ChangeHeader()
    {
        headerText.text = header;
    }

    #endregion
}
