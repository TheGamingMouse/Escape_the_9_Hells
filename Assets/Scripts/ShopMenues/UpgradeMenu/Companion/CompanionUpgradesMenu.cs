using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompanionUpgradesMenu : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool pannelsLoaded;
    public bool pannelsActivated;

    [Header("Strings")]
    readonly string header = "Companion Upgrades";

    [Header("Transforms")]
    public Transform contents;
    public Transform loyalSphereContents;
    public Transform attackSquareContents;
    public Transform companions1Contents;
    public Transform companions2Contents;
    public Transform companions3Contents;
    public Transform companions4Contents;

    [Header("TMP_Texts")]
    public TMP_Text headerText;

    [Header("Arrays")]
    public UpgradeItemsSO[] itemsSO;
    public UpgradeTemplate[] PannelsLoyalSphere;
    public UpgradeTemplate[] PannelsAttackSquare;
    public UpgradeTemplate[] PannelsCompanions1;
    public UpgradeTemplate[] PannelsCompanions2;
    public UpgradeTemplate[] PannelsCompanions3;
    public UpgradeTemplate[] PannelsCompanions4;
    public GameObject[] PannelsSOLoyalSphere;
    public GameObject[] PannelsSOAttackSquare;
    public GameObject[] PannelsSOCompanions1;
    public GameObject[] PannelsSOCompanions2;
    public GameObject[] PannelsSOCompanions3;
    public GameObject[] PannelsSOCompanions4;
    public Button[] ButtonsLoyalSphere;
    public Button[] ButtonsAttackSquare;
    public Button[] ButtonsCompanions1;
    public Button[] ButtonsCompanions2;
    public Button[] ButtonsCompanions3;
    public Button[] ButtonsCompanions4;
    public GameObject[] companions;

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
            for (int i = 0; i < companions.Length; i++)
            {
                for (int j = 0; j < playerEquipment.boughtCompanions.Count; j++)
                {
                    if (playerEquipment.boughtCompanions[j].title.ToLower().Contains(companions[i].name.ToLower()))
                    {
                        companions[i].SetActive(true);
                    }
                }
            }

            // LoyalSphere
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOLoyalSphere[i].SetActive(true);
            }

            // AttackSquare
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOAttackSquare[i].SetActive(true);
            }

            // Companion1
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOCompanions1[i].SetActive(true);
            }

            // Companion2
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOCompanions2[i].SetActive(true);
            }

            // Companion3
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOCompanions3[i].SetActive(true);
            }

            // Companion4
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOCompanions4[i].SetActive(true);
            }

            contents.position = new Vector3(contents.position.x, contents.position.y - 5000f);
            
            loyalSphereContents.position = new Vector3(1000f, loyalSphereContents.position.y);
            attackSquareContents.position = new Vector3(1000f, attackSquareContents.position.y);
            companions1Contents.position = new Vector3(1000f, companions1Contents.position.y);
            companions2Contents.position = new Vector3(1000f, companions2Contents.position.y);
            companions3Contents.position = new Vector3(1000f, companions3Contents.position.y);
            companions4Contents.position = new Vector3(1000f, companions4Contents.position.y);

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
        // LoyalSphere
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsLoyalSphere[i].titleText.text = itemsSO[i].title;
            PannelsLoyalSphere[i].descriptionText.text = itemsSO[i].description;
            PannelsLoyalSphere[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // AttackSquare
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsAttackSquare[i].titleText.text = itemsSO[i].title;
            PannelsAttackSquare[i].descriptionText.text = itemsSO[i].description;
            PannelsAttackSquare[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // Companion1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsCompanions1[i].titleText.text = itemsSO[i].title;
            PannelsCompanions1[i].descriptionText.text = itemsSO[i].description;
            PannelsCompanions1[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // Companion2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsCompanions2[i].titleText.text = itemsSO[i].title;
            PannelsCompanions2[i].descriptionText.text = itemsSO[i].description;
            PannelsCompanions2[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // Companion3
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsCompanions3[i].titleText.text = itemsSO[i].title;
            PannelsCompanions3[i].descriptionText.text = itemsSO[i].description;
            PannelsCompanions3[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        // Companion4
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsCompanions4[i].titleText.text = itemsSO[i].title;
            PannelsCompanions4[i].descriptionText.text = itemsSO[i].description;
            PannelsCompanions4[i].priceText.text = "Price: " + itemsSO[i].price.ToString();
        }

        pannelsLoaded = true;
    }

    void CheckUpgradesPurchaseable()
    {
        // LoyalSphere
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsLoyalSphere[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // AttackSquare
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsAttackSquare[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // Companion1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsCompanions1[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // Companion2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsCompanions2[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // Companion3
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsCompanions3[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }

        // Companion4
        for (int i = 0; i < itemsSO.Length; i++)
        {
            ButtonsCompanions4[i].interactable = upgradeMenu.souls >= itemsSO[i].price;
        }
    }

    public void PurchaseUpgradesLoyalSphere(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddLoyalSphereUpgrade(itemsSO[btnNo]);
        }
    }

    public void PurchaseUpgradesAttackSquare(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddAttackSquareUpgrade(itemsSO[btnNo]);
        }
    }

    public void PurchaseUpgradesCompanion1(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddCompanion1Upgrade(itemsSO[btnNo]);
        }
    }
    
    public void PurchaseUpgradesCompanion2(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddCompanion2Upgrade(itemsSO[btnNo]);
        }
    }
    
    public void PurchaseUpgradesCompanion3(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddCompanion3Upgrade(itemsSO[btnNo]);
        }
    }
    
    public void PurchaseUpgradesCompanion4(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddCompanion4Upgrade(itemsSO[btnNo]);
        }
    }

    public void ChangeHeader()
    {
        headerText.text = header;
    }

    #endregion
}
