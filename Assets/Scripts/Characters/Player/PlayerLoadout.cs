using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoadout : MonoBehaviour
{
    #region Variables

    [Header("LoadoutItemsSO")]
    public LoadoutItemsSO selectedWeapon;
    public LoadoutItemsSO selectedCompanion;
    public LoadoutItemsSO selectedUpperArmor;
    public LoadoutItemsSO selectedLowerArmor;

    [Header("Components")]
    Weapon playerWeapon;
    SaveLoadManager slManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        playerWeapon = GetComponentInChildren<Weapon>();
        slManager = GameObject.Find("Managers").GetComponent<SaveLoadManager>();

        selectedWeapon = slManager.weapon;
        selectedCompanion = slManager.companion;
        selectedUpperArmor = slManager.upperArmor;
        selectedLowerArmor = slManager.lowerArmor;

        // Weapon
        if (selectedWeapon == null || selectedWeapon.title == "Pugio")
        {
            playerWeapon.SwitchToPugio();
        }
        else if (selectedWeapon.title == "Ulfberht")
        {
            playerWeapon.SwitchToUlfberht();
        }

        // Companion

        // Upper Armor

        // Lower Armor
    }

    #endregion

    #region General Methods

    public void SetLoadout(LoadoutItemsSO weapon, LoadoutItemsSO companion, LoadoutItemsSO upperArmor, LoadoutItemsSO lowerArmor)
    {
        selectedWeapon = weapon;
        selectedCompanion = companion;
        selectedUpperArmor = upperArmor;
        selectedLowerArmor = lowerArmor;

        UpdateWeapon();
        UpdateCompanion();
        UpdateUpperArmor();
        UpdateLowerArmor();
    }

    void UpdateWeapon()
    {
        if (selectedWeapon.title == "Pugio")
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

    }

    void UpdateUpperArmor()
    {
        
    }

    void UpdateLowerArmor()
    {

    }

    #endregion
}
