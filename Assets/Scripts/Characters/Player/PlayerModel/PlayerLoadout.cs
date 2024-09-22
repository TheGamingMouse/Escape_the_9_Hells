using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool start;
    public bool backpackActive;
    public bool seedBagActive;
    bool ready;

    [Header("LoadoutItemsSO")]
    public LoadoutItemsSO selectedWeapon;
    public LoadoutItemsSO selectedPrimaryWeapon;
    public LoadoutItemsSO selectedSecondaryWeapon;
    public LoadoutItemsSO selectedCompanion;
    public LoadoutItemsSO selectedPrimaryCompanion;
    public LoadoutItemsSO selectedSecondaryCompanion;
    public LoadoutItemsSO selectedArmor;
    public LoadoutItemsSO selectedBack;

    public LoadoutItemsSO defaultWeapon;
    public LoadoutItemsSO defaultCompanion;
    public LoadoutItemsSO defaultArmor;
    public LoadoutItemsSO defaultBack;

    [Header("Components")]
    Weapon playerWeapon;
    Companion playerCompanion;
    Armor playerArmor;
    Backs playerBack;

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (NPCSpawner.Instance)
        {
            if (NPCSpawner.Instance.rickyStart)
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
            playerCompanion = GameObject.FindWithTag("Companions").GetComponent<Companion>();
            playerArmor = GetComponentInChildren<Armor>();
            playerBack = GetComponentInChildren<Backs>();
            
            var equipmentData = SaveSystem.loadedEquipmentData;

            selectedPrimaryWeapon = equipmentData.weaponData.primaryWeapon;
            selectedSecondaryWeapon = equipmentData.weaponData.secondaryWeapon;
            selectedWeapon = equipmentData.weaponData.selectedWeapon;

            selectedPrimaryCompanion = equipmentData.companionData.primaryCompanion;
            selectedSecondaryCompanion = equipmentData.companionData.secondaryCompanion;
            selectedCompanion = equipmentData.companionData.selectedCompanion;

            selectedArmor = equipmentData.armorData.selectedArmor;
            selectedBack = equipmentData.backData.selectedBack;

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
    public void SetLoadout(LoadoutItemsSO primaryWeapon, LoadoutItemsSO secondaryWeapon, LoadoutItemsSO primaryCompanion, LoadoutItemsSO secondaryCompanion, LoadoutItemsSO armor, LoadoutItemsSO back)
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
        selectedPrimaryCompanion = primaryCompanion;
        if (secondaryCompanion)
        {
            selectedSecondaryCompanion = secondaryCompanion;
        }
        else
        {
            selectedSecondaryCompanion = primaryCompanion;
        }
        selectedCompanion = selectedPrimaryCompanion;
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
        PlayerComponents.Instance.playerUpgrades.weaponUpdated = false;
    }

    void UpdateCompanion()
    {
        if (selectedBack && selectedBack.title == "Seed Bag")
        {
            seedBagActive = true;
        }
        else
        {
            seedBagActive = false;
        }

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
        PlayerComponents.Instance.playerUpgrades.companionUpdated = false;
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
        PlayerComponents.Instance.playerUpgrades.armorUpdated = false;
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
        else if (selectedBack.title == "Seed Bag")
        {
            playerBack.SwitchToSeedBag();
        }
        PlayerComponents.Instance.playerUpgrades.backUpdated = false;
    }

    #endregion
}
