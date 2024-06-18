using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool start;
    public bool backpackActive;
    bool ready;

    [Header("LoadoutItemsSO")]
    public LoadoutItemsSO selectedWeapon;
    public LoadoutItemsSO selectedPrimaryWeapon;
    public LoadoutItemsSO selectedSecondaryWeapon;
    public LoadoutItemsSO selectedCompanion;
    public LoadoutItemsSO selectedArmor;
    public LoadoutItemsSO selectedBack;

    public LoadoutItemsSO defaultWeapon;
    public LoadoutItemsSO defaultCompanion;
    public LoadoutItemsSO defaultArmor;
    public LoadoutItemsSO defaultBack;

    [Header("Components")]
    Weapon playerWeapon;
    SaveLoadManager slManager;
    Companion playerCompanion;
    Armor playerArmor;
    Backs playerBack;
    PlayerUpgrades playerUpgrades;
    NPCSpawner spawner;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        if (GameObject.FindWithTag("NPC") && GameObject.FindWithTag("NPC").TryGetComponent(out NPCSpawner npcSpawner))
        {
            spawner = npcSpawner;
        }
    }

    void Update()
    {
        if (spawner)
        {
            if (spawner.rickyStart)
            {
                ready = true;
            }
        }
        else
        {
            ready = true;
        }
        if (!start && ready)
        {
            playerWeapon = GetComponentInChildren<Weapon>();
            slManager = GameObject.Find("Managers").GetComponent<SaveLoadManager>();
            playerCompanion = GameObject.FindWithTag("Companions").GetComponent<Companion>();
            playerArmor = GetComponentInChildren<Armor>();
            playerBack = GetComponentInChildren<Backs>();
            playerUpgrades = GetComponent<PlayerUpgrades>();

            selectedPrimaryWeapon = slManager.primaryWeapon;
            selectedWeapon = selectedPrimaryWeapon;
            selectedSecondaryWeapon = slManager.secondaryWeapon;

            selectedCompanion = slManager.companion;
            selectedArmor = slManager.armor;
            selectedBack = slManager.back;

            // Weapon
            if (playerWeapon != null)
            {
                UpdateWeapon();
            }
            if (!selectedWeapon)
            {
                selectedWeapon = defaultWeapon;
                UpdateWeapon();
            }

            // Companion
            if (playerCompanion != null)
            {
                UpdateCompanion();
            }
            if (!selectedCompanion)
            {
                selectedCompanion = defaultCompanion;
                UpdateCompanion();
            }

            // Armor
            if (playerArmor != null)
            {
                UpdateArmor();
            }
            if (!selectedArmor)
            {
                selectedArmor = defaultArmor;
                UpdateArmor();
            }

            // Back
            if (playerBack != null)
            {
                UpdateBack();
            }
            if (!selectedBack)
            {
                selectedBack = defaultBack;
                UpdateBack();
            }

            start = true;
        }
    }

    #endregion

    #region General Methods

    #nullable enable
    public void SetLoadout(LoadoutItemsSO primaryWeapon, LoadoutItemsSO? secondaryWeapon, LoadoutItemsSO companion, LoadoutItemsSO armor, LoadoutItemsSO back)
    {
        selectedPrimaryWeapon = primaryWeapon;
        if (secondaryWeapon)
        {
            selectedSecondaryWeapon = secondaryWeapon;
        }
        else
        {
            selectedSecondaryWeapon = primaryWeapon;
        }
        selectedWeapon = selectedPrimaryWeapon;
        selectedCompanion = companion;
        selectedArmor = armor;
        selectedBack = back;

        if (playerWeapon)
        {
            UpdateWeapon();
        }
        if (playerCompanion)
        {
            UpdateCompanion();
        }
        if (playerArmor)
        {
            UpdateArmor();
        }
        if (playerBack)
        {
            UpdateBack();
        }
    }
    #nullable disable

    void UpdateWeapon()
    {
        if (selectedBack && selectedBack.title == "Backpack")
        {
            backpackActive = true;
        }
        else
        {
            backpackActive = false;
        }

        if (!selectedWeapon || selectedWeapon.title == "Pugio")
        {
            playerWeapon.SwitchToPugio();
        }
        else if (selectedWeapon.title == "Ulfberht")
        {
            playerWeapon.SwitchToUlfberht();
        }
        playerUpgrades.weaponUpdated = false;
    }

    void UpdateCompanion()
    {
        if (!selectedCompanion || selectedCompanion.title == "Unequiped")
        {
            playerCompanion.SwitchToNone();
        }
        else if (selectedCompanion.title == "Loyal Sphere")
        {
            playerCompanion.SwitchToLoyalSphere();
        }
        else if (selectedCompanion.title == "Attack Square")
        {
            playerCompanion.SwitchToAttackSquare();
        }
        playerUpgrades.companionUpdated = false;
    }

    void UpdateArmor()
    {
        if (!selectedArmor || selectedArmor.title == "Unequiped")
        {
            playerArmor.SwitchToNone();
        }
        else if (selectedArmor.title == "Leather Armor")
        {
            playerArmor.SwitchToLeather();
        }
        else if (selectedArmor.title == "Hide Armor")
        {
            playerArmor.SwitchToHide();
        }
        else if (selectedArmor.title == "Ring Mail Armor")
        {
            playerArmor.SwitchToRingMail();
        }
        else if (selectedArmor.title == "Plate Armor")
        {
            playerArmor.SwitchToPlate();
        }
        playerUpgrades.armorUpdated = false;
    }

    void UpdateBack()
    {
        if (!selectedBack || selectedBack.title == "Unequiped")
        {
            playerBack.SwitchToNone();
        }
        else if (selectedBack.title == "Angel Wings")
        {
            playerBack.SwitchToAngelWings();
        }
        else if (selectedBack.title == "Steel Wings")
        {
            playerBack.SwitchToSteelWings();
        }
        else if (selectedBack.title == "Backpack")
        {
            playerBack.SwitchToBackpack();
        }
        else if (selectedBack.title == "Cape O' Wind")
        {
            playerBack.SwitchToCapeOWind();
        }
        playerUpgrades.backUpdated = false;
    }

    #endregion
}
