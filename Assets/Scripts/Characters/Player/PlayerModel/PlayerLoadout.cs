using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool start;

    [Header("LoadoutItemsSO")]
    public LoadoutItemsSO selectedWeapon;
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

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Update()
    {
        if (!start)
        {
            playerWeapon = GetComponentInChildren<Weapon>();
            slManager = GameObject.Find("Managers").GetComponent<SaveLoadManager>();
            playerCompanion = GameObject.FindWithTag("Companions").GetComponent<Companion>();
            playerArmor = GetComponentInChildren<Armor>();
            playerBack = GetComponentInChildren<Backs>();
            playerUpgrades = GetComponent<PlayerUpgrades>();

            selectedWeapon = slManager.weapon;
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

    public void SetLoadout(LoadoutItemsSO weapon, LoadoutItemsSO companion, LoadoutItemsSO armor, LoadoutItemsSO back)
    {
        selectedWeapon = weapon;
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

    void UpdateWeapon()
    {
        if (selectedWeapon == null || selectedWeapon.title == "Pugio")
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
        if (selectedCompanion == null || selectedCompanion.title == "Unequiped")
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
        if (selectedArmor == null || selectedArmor.title == "Unequiped")
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
    }

    void UpdateBack()
    {
        if (selectedBack == null || selectedBack.title == "Unequiped")
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
        playerUpgrades.backUpdated = false;
    }

    #endregion
}
