using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool equipmentLoaded;

    [Header("Lists")]
    public List<LoadoutItemsSO> boughtWeapons = new();
    public List<LoadoutItemsSO> boughtCompanions = new();
    public List<LoadoutItemsSO> boughtArmors = new();
    public List<LoadoutItemsSO> boughtTBDs = new();

    [Header("LoadoutItemsSOs")]
    public LoadoutItemsSO defaultWeapon;
    public LoadoutItemsSO defaultCompanion;
    public LoadoutItemsSO defaultArmor;
    public LoadoutItemsSO defaultTBD;

    [Header("Components")]
    SaveLoadManager saveLoadManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        saveLoadManager = GameObject.FindWithTag("Managers").GetComponent<SaveLoadManager>();

        boughtWeapons = saveLoadManager.boughtWeapons;
        boughtCompanions = saveLoadManager.boughtCompanions;
        boughtArmors = saveLoadManager.boughtArmors;
        boughtTBDs = saveLoadManager.boughtTBDs;

        if (boughtWeapons.Count == 0 || boughtWeapons[0] == null)
        {
            boughtWeapons.Add(defaultWeapon);
        }
        if (boughtCompanions.Count == 0 || boughtCompanions[0] == null)
        {
            boughtCompanions.Add(defaultCompanion);
        }
        if (boughtArmors.Count == 0 || boughtArmors[0] == null)
        {
            boughtArmors.Add(defaultArmor);
        }
        if (boughtTBDs.Count == 0 || boughtTBDs[0] == null)
        {
            boughtTBDs.Add(defaultTBD);
        }

        equipmentLoaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region General Methods

    public void PurchaseWeapon(LoadoutItemsSO weapon)
    {
        boughtWeapons.Add(weapon);
    }
    
    public void PurchaseCompanion(LoadoutItemsSO companion)
    {
        boughtCompanions.Add(companion);
    }
    
    public void PurchaseArmor(LoadoutItemsSO armor)
    {
        boughtArmors.Add(armor);
    }
    
    public void PurchaseTBD(LoadoutItemsSO TBD)
    {
        boughtTBDs.Add(TBD);
    }

    #endregion
}
