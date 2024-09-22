using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (!pannelsActivated)
        {
            for (int i = 0; i < armors.Length; i++)
            {
                for (int j = 0; j < PlayerComponents.Instance.playerEquipment.boughtArmors.Count; j++)
                {
                    if (PlayerComponents.Instance.playerEquipment.boughtArmors[j].title.ToLower().Contains(armors[i].name.ToLower()))
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
        
        // Leather
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsLeather[i].titleText.text = itemsSO[i].title;
            PannelsLeather[i].descriptionText.text = itemsSO[i].description;
            PannelsLeather[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsLeather[i].counter.fillAmount = playerUpgrades.upgradesLeather.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesLeather.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsLeather[i].lights.SetActive(true);
            }
        }

        // Hide
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsHide[i].titleText.text = itemsSO[i].title;
            PannelsHide[i].descriptionText.text = itemsSO[i].description;
            PannelsHide[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsHide[i].counter.fillAmount = playerUpgrades.upgradesHide.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesHide.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsHide[i].lights.SetActive(true);
            }
        }

        // RingMail
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsRingMail[i].titleText.text = itemsSO[i].title;
            PannelsRingMail[i].descriptionText.text = itemsSO[i].description;
            PannelsRingMail[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsRingMail[i].counter.fillAmount = playerUpgrades.upgradesRingMail.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesRingMail.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsRingMail[i].lights.SetActive(true);
            }
        }

        // Plate
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsPlate[i].titleText.text = itemsSO[i].title;
            PannelsPlate[i].descriptionText.text = itemsSO[i].description;
            PannelsPlate[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsPlate[i].counter.fillAmount = playerUpgrades.upgradesPlate.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesPlate.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsPlate[i].lights.SetActive(true);
            }
        }

        // Armor1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsArmors1[i].titleText.text = itemsSO[i].title;
            PannelsArmors1[i].descriptionText.text = itemsSO[i].description;
            PannelsArmors1[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsArmors1[i].counter.fillAmount = playerUpgrades.upgradesArmor1.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesArmor1.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsArmors1[i].lights.SetActive(true);
            }
        }

        // Armor2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            PannelsArmors2[i].titleText.text = itemsSO[i].title;
            PannelsArmors2[i].descriptionText.text = itemsSO[i].description;
            PannelsArmors2[i].priceText.text = "Price: " + itemsSO[i].price.ToString();

            PannelsArmors2[i].counter.fillAmount = playerUpgrades.upgradesArmor2.Where(x => x.title == itemsSO[i].title).Count() * 0.067f;

            if (playerUpgrades.upgradesArmor2.Where(x => x.title == itemsSO[i].title).Count() == itemsSO[i].max)
            {
                PannelsArmors2[i].lights.SetActive(true);
            }
        }

        pannelsLoaded = true;
    }

    void CheckUpgradesPurchaseable()
    {
        var upgradeMenu = UpgradeMenu.Instance;
        var playerUpgrades = PlayerComponents.Instance.playerUpgrades;
        
        // Leather
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesLeather.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsLeather[i].interactable = true;
            }
            else
            {
                ButtonsLeather[i].interactable = false;
            }
        }

        // Hide
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesHide.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsHide[i].interactable = true;
            }
            else
            {
                ButtonsHide[i].interactable = false;
            }
        }

        // RingMail
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesRingMail.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsRingMail[i].interactable = true;
            }
            else
            {
                ButtonsRingMail[i].interactable = false;
            }
        }

        // Plate
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesPlate.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsPlate[i].interactable = true;
            }
            else
            {
                ButtonsPlate[i].interactable = false;
            }
        }

        // Armor1
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesArmor1.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsArmors1[i].interactable = true;
            }
            else
            {
                ButtonsArmors1[i].interactable = false;
            }
        }

        // Armor2
        for (int i = 0; i < itemsSO.Length; i++)
        {
            if (upgradeMenu.souls >= itemsSO[i].price && playerUpgrades.upgradesArmor2.Where(x => x.title == itemsSO[i].title).Count() < itemsSO[i].max)
            {
                ButtonsArmors2[i].interactable = true;
            }
            else
            {
                ButtonsArmors2[i].interactable = false;
            }
        }
    }

    public void PurchaseUpgradesLeather(int btnNo)
    {
        if (UpgradeMenu.Instance.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            PlayerComponents.Instance.playerUpgrades.AddLeatherUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }

    public void PurchaseUpgradesHide(int btnNo)
    {
        if (UpgradeMenu.Instance.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            PlayerComponents.Instance.playerUpgrades.AddHideUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }

    public void PurchaseUpgradesRingMail(int btnNo)
    {
        if (UpgradeMenu.Instance.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            PlayerComponents.Instance.playerUpgrades.AddRingMailUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }
    
    public void PurchaseUpgradesPlate(int btnNo)
    {
        if (UpgradeMenu.Instance.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            PlayerComponents.Instance.playerUpgrades.AddPlateUpgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }
    
    public void PurchaseUpgradesArmor1(int btnNo)
    {
        if (UpgradeMenu.Instance.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            PlayerComponents.Instance.playerUpgrades.AddArmor1Upgrade(itemsSO[btnNo]);
            pannelsLoaded = false;
        }
    }
    
    public void PurchaseUpgradesArmor2(int btnNo)
    {
        if (UpgradeMenu.Instance.souls >= itemsSO[btnNo].price)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerLevel>().souls -= itemsSO[btnNo].price;

            //Unlock purchased item.
            PlayerComponents.Instance.playerUpgrades.AddArmor2Upgrade(itemsSO[btnNo]);
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
