using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorUpgradesMenu : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool pannelsLoaded;
    public bool pannelsActivated;

    [Header("Strings")]
    readonly string header = "Armor Upgrades";

    [Header("Transforms")]
    public Transform contents;
    public Transform leatherContents;
    public Transform hideContents;
    public Transform ringMailContents;
    public Transform plateContents;
    public Transform armor1Contents;
    public Transform armor2Contents;

    [Header("TMP_Texts")]
    public TMP_Text headerText;

    [Header("Arrays")]
    public UpgradeItemsSO[] itemsSO;
    public UpgradeTemplate[] PannelsLeather;
    public UpgradeTemplate[] PannelsHide;
    public UpgradeTemplate[] PannelsRingMail;
    public UpgradeTemplate[] PannelsPlate;
    public UpgradeTemplate[] PannelsArmors1;
    public UpgradeTemplate[] PannelsArmors2;
    public GameObject[] PannelsSOLeather;
    public GameObject[] PannelsSOHide;
    public GameObject[] PannelsSORingMail;
    public GameObject[] PannelsSOPlate;
    public GameObject[] PannelsSOArmors1;
    public GameObject[] PannelsSOArmors2;
    public Button[] ButtonsLeather;
    public Button[] ButtonsHide;
    public Button[] ButtonsRingMail;
    public Button[] ButtonsPlate;
    public Button[] ButtonsArmors1;
    public Button[] ButtonsArmors2;
    public GameObject[] armors;

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
            for (int i = 0; i < armors.Length; i++)
            {
                for (int j = 0; j < playerEquipment.boughtArmors.Count; j++)
                {
                    if (playerEquipment.boughtArmors[j].title.ToLower().Contains(armors[i].name.ToLower()))
                    {
                        armors[i].SetActive(true);
                    }
                }
            }

            // Leather
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOLeather[i].SetActive(true);
            }

            // Hide
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOHide[i].SetActive(true);
            }

            // RingMail
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSORingMail[i].SetActive(true);
            }

            // Plate
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOPlate[i].SetActive(true);
            }

            // Armor1
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOArmors1[i].SetActive(true);
            }

            // Armor2
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOArmors2[i].SetActive(true);
            }

            contents.position = new Vector3(contents.position.x, contents.position.y - 5000f);
            
            leatherContents.position = new Vector3(1000f, leatherContents.position.y);
            hideContents.position = new Vector3(1000f, hideContents.position.y);
            ringMailContents.position = new Vector3(1000f, ringMailContents.position.y);
            plateContents.position = new Vector3(1000f, plateContents.position.y);
            armor1Contents.position = new Vector3(1000f, armor1Contents.position.y);
            armor2Contents.position = new Vector3(1000f, armor2Contents.position.y);

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
        // Leather
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsLeather[i].titleText.text = itemsSO[i].title;
            PannelsLeather[i].descriptionText.text = itemsSO[i].description;
            PannelsLeather[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // Hide
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsHide[i].titleText.text = itemsSO[i].title;
            PannelsHide[i].descriptionText.text = itemsSO[i].description;
            PannelsHide[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // RingMail
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsRingMail[i].titleText.text = itemsSO[i].title;
            PannelsRingMail[i].descriptionText.text = itemsSO[i].description;
            PannelsRingMail[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // Plate
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsPlate[i].titleText.text = itemsSO[i].title;
            PannelsPlate[i].descriptionText.text = itemsSO[i].description;
            PannelsPlate[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // Armor1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsArmors1[i].titleText.text = itemsSO[i].title;
            PannelsArmors1[i].descriptionText.text = itemsSO[i].description;
            PannelsArmors1[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // Armor2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsArmors2[i].titleText.text = itemsSO[i].title;
            PannelsArmors2[i].descriptionText.text = itemsSO[i].description;
            PannelsArmors2[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        pannelsLoaded = true;
    }

    void CheckUpgradesPurchaseable()
    {
        // Leather
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsLeather[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // Hide
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsHide[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // RingMail
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsRingMail[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // Plate
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsPlate[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // Armor1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsArmors1[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // Armor2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsArmors2[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }
    }

    public void PurchaseUpgradesLeather(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddLeatherUpgrade(itemsSO[btnNo]);
        }
    }

    public void PurchaseUpgradesHide(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddHideUpgrade(itemsSO[btnNo]);
        }
    }

    public void PurchaseUpgradesRingMail(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddRingMailUpgrade(itemsSO[btnNo]);
        }
    }
    
    public void PurchaseUpgradesPlate(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddPlateUpgrade(itemsSO[btnNo]);
        }
    }
    
    public void PurchaseUpgradesArmor1(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddArmor1Upgrade(itemsSO[btnNo]);
        }
    }
    
    public void PurchaseUpgradesArmor2(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddArmor2Upgrade(itemsSO[btnNo]);
        }
    }

    public void ChangeHeader()
    {
        headerText.text = header;
    }

    #endregion
}
