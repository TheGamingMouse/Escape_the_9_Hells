using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool start;

    [Header("LoadoutItemsSO")]
    public LoadoutItemsSO selectedWeapon;
    public LoadoutItemsSO selectedCompanion;
    public LoadoutItemsSO selectedArmor;
    public LoadoutItemsSO selectedTBD;

    [Header("Components")]
    Weapon playerWeapon;
    SaveLoadManager slManager;
    Companion playerCompanion;
    Armor playerArmor;

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

            selectedWeapon = slManager.weapon;
            selectedCompanion = slManager.companion;
            selectedArmor = slManager.armor;
            selectedTBD = slManager.TBD;

            // Weapon
            if (playerWeapon != null)
            {
                UpdateWeapon();
            }

            // Companion
            if (playerCompanion != null)
            {
                UpdateCompanion();
            }

            // Armor
            if (playerArmor != null)
            {
                UpdateArmor();
            }

            // TBD


            start = true;
        }
    }

    #endregion

    #region General Methods

    public void SetLoadout(LoadoutItemsSO weapon, LoadoutItemsSO companion, LoadoutItemsSO armor, LoadoutItemsSO TBD)
    {
        selectedWeapon = weapon;
        selectedCompanion = companion;
        selectedArmor = armor;
        selectedTBD = TBD;

        UpdateWeapon();
        UpdateCompanion();
        UpdateArmor();
        UpdateTBD();
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

    void UpdateTBD()
    {

    }

    #endregion
}
