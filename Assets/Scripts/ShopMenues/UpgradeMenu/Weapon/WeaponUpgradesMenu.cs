using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradesMenu : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool pannelsLoaded;
    public bool pannelsActivated;

    [Header("Strings")]
    readonly string header = "Weapon Upgrades";

    [Header("Transforms")]
    public Transform contents;
    public Transform pugioContents;
    public Transform ulfberhtContents;
    public Transform weapons1Contents;
    public Transform weapons2Contents;
    public Transform weapons3Contents;
    public Transform weapons4Contents;

    [Header("TMP_Texts")]
    public TMP_Text headerText;

    [Header("Arrays")]
    public UpgradeItemsSO[] itemsSO;
    public UpgradeTemplate[] PannelsPugio;
    public UpgradeTemplate[] PannelsUlfberht;
    public UpgradeTemplate[] PannelsWeapons1;
    public UpgradeTemplate[] PannelsWeapons2;
    public UpgradeTemplate[] PannelsWeapons3;
    public UpgradeTemplate[] PannelsWeapons4;
    public GameObject[] PannelsSOPugio;
    public GameObject[] PannelsSOUlfberht;
    public GameObject[] PannelsSOWeapons1;
    public GameObject[] PannelsSOWeapons2;
    public GameObject[] PannelsSOWeapons3;
    public GameObject[] PannelsSOWeapons4;
    public Button[] ButtonsPugio;
    public Button[] ButtonsUlfberht;
    public Button[] ButtonsWeapons1;
    public Button[] ButtonsWeapons2;
    public Button[] ButtonsWeapons3;
    public Button[] ButtonsWeapons4;
    public GameObject[] weapons;

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
            for (int i = 0; i < weapons.Length; i++)
            {
                for (int j = 0; j < playerEquipment.boughtWeapons.Count; j++)
                {
                    if (playerEquipment.boughtWeapons[j].title.ToLower().Contains(weapons[i].name.ToLower()))
                    {
                        weapons[i].SetActive(true);
                    }
                }
            }

            // Pugio
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOPugio[i].SetActive(true);
            }

            // Ulfberht
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOUlfberht[i].SetActive(true);
            }

            // Weapon1
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOWeapons1[i].SetActive(true);
            }

            // Weapon2
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOWeapons2[i].SetActive(true);
            }

            // Weapon3
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOWeapons3[i].SetActive(true);
            }

            // Weapon4
            for (int i = 0; i < itemsSO.Length; i++)
            {
                PannelsSOWeapons4[i].SetActive(true);
            }

            contents.position = new Vector3(contents.position.x, contents.position.y - 5000f);
            
            pugioContents.position = new Vector3(1000f, pugioContents.position.y);
            ulfberhtContents.position = new Vector3(1000f, ulfberhtContents.position.y);
            weapons1Contents.position = new Vector3(1000f, weapons1Contents.position.y);
            weapons2Contents.position = new Vector3(1000f, weapons2Contents.position.y);
            weapons3Contents.position = new Vector3(1000f, weapons3Contents.position.y);
            weapons4Contents.position = new Vector3(1000f, weapons4Contents.position.y);

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
        // Pugio
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsPugio[i].titleText.text = itemsSO[i].title;
            PannelsPugio[i].descriptionText.text = itemsSO[i].description;
            PannelsPugio[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsPugio[i].counter.fillAmount = playerUpgrades.upgradesPugio.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;
            if (PannelsPugio[i].titleText.text == "Special Attack")
            {
                PannelsPugio[i].border.SetActive(false);
            }

            if (playerUpgrades.upgradesPugio.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsPugio[i].lights.SetActive(true);
            }
        }

        // Ulfberht
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsUlfberht[i].titleText.text = itemsSO[i].title;
            PannelsUlfberht[i].descriptionText.text = itemsSO[i].description;
            PannelsUlfberht[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsUlfberht[i].counter.fillAmount = playerUpgrades.upgradesUlfberht.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;
            if (PannelsUlfberht[i].titleText.text == "Special Attack")
            {
                PannelsUlfberht[i].border.SetActive(false);
            }

            if (playerUpgrades.upgradesUlfberht.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsUlfberht[i].lights.SetActive(true);
            }
        }

        // Weapon1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsWeapons1[i].titleText.text = itemsSO[i].title;
            PannelsWeapons1[i].descriptionText.text = itemsSO[i].description;
            PannelsWeapons1[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsWeapons1[i].counter.fillAmount = playerUpgrades.upgradesWeapon1.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;
            if (PannelsWeapons1[i].titleText.text == "Special Attack")
            {
                PannelsWeapons1[i].border.SetActive(false);
            }

            if (playerUpgrades.upgradesWeapon1.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsWeapons1[i].lights.SetActive(true);
            }
        }

        // Weapon2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsWeapons2[i].titleText.text = itemsSO[i].title;
            PannelsWeapons2[i].descriptionText.text = itemsSO[i].description;
            PannelsWeapons2[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsWeapons2[i].counter.fillAmount = playerUpgrades.upgradesWeapon2.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;
            if (PannelsWeapons2[i].titleText.text == "Special Attack")
            {
                PannelsWeapons2[i].border.SetActive(false);
            }

            if (playerUpgrades.upgradesWeapon2.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsWeapons2[i].lights.SetActive(true);
            }
        }

        // Weapon3
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsWeapons3[i].titleText.text = itemsSO[i].title;
            PannelsWeapons3[i].descriptionText.text = itemsSO[i].description;
            PannelsWeapons3[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsWeapons3[i].counter.fillAmount = playerUpgrades.upgradesWeapon3.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;
            if (PannelsWeapons3[i].titleText.text == "Special Attack")
            {
                PannelsWeapons3[i].border.SetActive(false);
            }

            if (playerUpgrades.upgradesWeapon3.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsWeapons3[i].lights.SetActive(true);
            }
        }

        // Weapon4
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsWeapons4[i].titleText.text = itemsSO[i].title;
            PannelsWeapons4[i].descriptionText.text = itemsSO[i].description;
            PannelsWeapons4[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsWeapons4[i].counter.fillAmount = playerUpgrades.upgradesWeapon4.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;
            if (PannelsWeapons4[i].titleText.text == "Special Attack")
            {
                PannelsWeapons4[i].border.SetActive(false);
            }

            if (playerUpgrades.upgradesWeapon4.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsWeapons4[i].lights.SetActive(true);
            }
        }

        pannelsLoaded = true;
    }

    void CheckUpgradesPurchaseable()
    {
        // Pugio
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (itemsSO[i].title == "Special Cooldown")
            {
                for (int j = 0; j < playerUpgrades.upgradesPugio.Count; j++)
                {
                    if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesPugio[j].title.Contains("Special Attack") && playerUpgrades.upgradesPugio.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
                    {
                        ButtonsPugio[i].interactable = true;
                        break;
                    }
                }
            }
            else if (itemsSO[i].title == "Special Attack")
            {
                if (playerUpgrades.upgradesPugio.Count != 0)
                {
                    for (int j = 0; j < playerUpgrades.upgradesPugio.Count; j++)
                    {
                        if (upgradeMenu.souls >= itemsSO[i].price && !playerUpgrades.upgradesPugio[j].title.Contains(itemsSO[i].title))
                        {
                            ButtonsPugio[i].interactable = true;
                        }
                        else
                        {
                            ButtonsPugio[i].interactable = false;
                            break;
                        }
                    }
                }
                else if (upgradeMenu.souls >= itemsSO[i].price)
                {
                    ButtonsPugio[i].interactable = true;
                }
            }
            else if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesPugio.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsPugio[i].interactable = true;
            }
            else
            {
                ButtonsPugio[i].interactable = false;
            }
        }

        // Ulfberht
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (itemsSO[i].title == "Special Cooldown")
            {
                for (int j = 0; j < playerUpgrades.upgradesUlfberht.Count; j++)
                {
                    if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesUlfberht[j].title.Contains("Special Attack") && playerUpgrades.upgradesUlfberht.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
                    {
                        ButtonsUlfberht[i].interactable = true;
                        break;
                    }
                }
            }
            else if (itemsSO[i].title == "Special Attack")
            {
                if (playerUpgrades.upgradesUlfberht.Count != 0)
                {
                    for (int j = 0; j < playerUpgrades.upgradesUlfberht.Count; j++)
                    {
                        if (upgradeMenu.souls >= itemsSO[i].price && !playerUpgrades.upgradesUlfberht[j].title.Contains(itemsSO[i].title))
                        {
                            ButtonsUlfberht[i].interactable = true;
                        }
                        else
                        {
                            ButtonsUlfberht[i].interactable = false;
                            break;
                        }
                    }
                }
                else if (upgradeMenu.souls >= itemsSO[i].price)
                {
                    ButtonsUlfberht[i].interactable = true;
                }
            }
            else if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesUlfberht.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsUlfberht[i].interactable = true;
            }
            else
            {
                ButtonsUlfberht[i].interactable = false;
            }
        }

        // Weapon1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (itemsSO[i].title == "Special Cooldown")
            {
                for (int j = 0; j < playerUpgrades.upgradesWeapon1.Count; j++)
                {
                    if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesWeapon1[j].title.Contains("Special Attack") && playerUpgrades.upgradesWeapon1.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
                    {
                        ButtonsWeapons1[i].interactable = true;
                        break;
                    }
                }
            }
            else if (itemsSO[i].title == "Special Attack")
            {
                if (playerUpgrades.upgradesWeapon1.Count != 0)
                {
                    for (int j = 0; j < playerUpgrades.upgradesWeapon1.Count; j++)
                    {
                        if (upgradeMenu.souls >= itemsSO[i].price && !playerUpgrades.upgradesWeapon1[j].title.Contains(itemsSO[i].title))
                        {
                            ButtonsWeapons1[i].interactable = true;
                        }
                        else
                        {
                            ButtonsWeapons1[i].interactable = false;
                            break;
                        }
                    }
                }
                else if (upgradeMenu.souls >= itemsSO[i].price)
                {
                    ButtonsWeapons1[i].interactable = true;
                }
            }
            else if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesWeapon1.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsWeapons1[i].interactable = true;
            }
            else
            {
                ButtonsWeapons1[i].interactable = false;
            }
        }

        // Weapon2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (itemsSO[i].title == "Special Cooldown")
            {
                for (int j = 0; j < playerUpgrades.upgradesWeapon2.Count; j++)
                {
                    if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesWeapon2[j].title.Contains("Special Attack") && playerUpgrades.upgradesWeapon2.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
                    {
                        ButtonsWeapons2[i].interactable = true;
                        break;
                    }
                }
            }
            else if (itemsSO[i].title == "Special Attack")
            {
                if (playerUpgrades.upgradesWeapon2.Count != 0)
                {
                    for (int j = 0; j < playerUpgrades.upgradesWeapon2.Count; j++)
                    {
                        if (upgradeMenu.souls >= itemsSO[i].price && !playerUpgrades.upgradesWeapon2[j].title.Contains(itemsSO[i].title))
                        {
                            ButtonsWeapons2[i].interactable = true;
                        }
                        else
                        {
                            ButtonsWeapons2[i].interactable = false;
                            break;
                        }
                    }
                }
                else if (upgradeMenu.souls >= itemsSO[i].price)
                {
                    ButtonsWeapons2[i].interactable = true;
                }
            }
            else if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesWeapon2.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsWeapons2[i].interactable = true;
            }
            else
            {
                ButtonsWeapons2[i].interactable = false;
            }
        }

        // Weapon3
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (itemsSO[i].title == "Special Cooldown")
            {
                for (int j = 0; j < playerUpgrades.upgradesWeapon3.Count; j++)
                {
                    if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesWeapon3[j].title.Contains("Special Attack") && playerUpgrades.upgradesWeapon3.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
                    {
                        ButtonsWeapons3[i].interactable = true;
                        break;
                    }
                }
            }
            else if (itemsSO[i].title == "Special Attack")
            {
                if (playerUpgrades.upgradesWeapon3.Count != 0)
                {
                    for (int j = 0; j < playerUpgrades.upgradesWeapon3.Count; j++)
                    {
                        if (upgradeMenu.souls >= itemsSO[i].price && !playerUpgrades.upgradesWeapon3[j].title.Contains(itemsSO[i].title))
                        {
                            ButtonsWeapons3[i].interactable = true;
                        }
                        else
                        {
                            ButtonsWeapons3[i].interactable = false;
                            break;
                        }
                    }
                }
                else if (upgradeMenu.souls >= itemsSO[i].price)
                {
                    ButtonsWeapons3[i].interactable = true;
                }
            }
            else if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesWeapon3.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsWeapons3[i].interactable = true;
            }
            else
            {
                ButtonsWeapons3[i].interactable = false;
            }
        }

        // Weapon4
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (itemsSO[i].title == "Special Cooldown")
            {
                for (int j = 0; j < playerUpgrades.upgradesWeapon4.Count; j++)
                {
                    if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesWeapon4[j].title.Contains("Special Attack") && playerUpgrades.upgradesWeapon4.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
                    {
                        ButtonsWeapons4[i].interactable = true;
                        break;
                    }
                }
            }
            else if (itemsSO[i].title == "Special Attack")
            {
                if (playerUpgrades.upgradesWeapon4.Count != 0)
                {
                    for (int j = 0; j < playerUpgrades.upgradesWeapon4.Count; j++)
                    {
                        if (upgradeMenu.souls >= itemsSO[i].price && !playerUpgrades.upgradesWeapon4[j].title.Contains(itemsSO[i].title))
                        {
                            ButtonsWeapons4[i].interactable = true;
                        }
                        else
                        {
                            ButtonsWeapons4[i].interactable = false;
                            break;
                        }
                    }
                }
                else if (upgradeMenu.souls >= itemsSO[i].price)
                {
                    ButtonsWeapons4[i].interactable = true;
                }
            }
            else if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesWeapon4.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsWeapons4[i].interactable = true;
            }
            else
            {
                ButtonsWeapons4[i].interactable = false;
            }
        }
    }

    public void PurchaseUpgradesPugio(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddPugioUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }

    public void PurchaseUpgradesUlfberht(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddUlfberhtUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }

    public void PurchaseUpgradesWeapon1(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddWeapon1Upgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }
    
    public void PurchaseUpgradesWeapon2(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddWeapon2Upgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }
    
    public void PurchaseUpgradesWeapon3(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddWeapon3Upgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }
    
    public void PurchaseUpgradesWeapon4(int btnNo)
    {
        if (upgradeMenu.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            playerUpgrades.AddWeapon4Upgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }

    public void ChangeHeader()
    {
        headerText.text = header;
    }

    #endregion
}
