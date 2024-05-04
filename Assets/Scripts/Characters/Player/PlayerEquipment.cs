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
    public List<LoadoutItemsSO> boughtUpperArmors = new();
    public List<LoadoutItemsSO> boughtLowerArmors = new();

    [Header("LoadoutItemsSOs")]
    public LoadoutItemsSO defaultWeapon;
    public LoadoutItemsSO defaultCompanion;
    public LoadoutItemsSO defaultUpperArmor;
    public LoadoutItemsSO defaultLowerArmor;

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
        boughtUpperArmors = saveLoadManager.boughtUpperArmors;
        boughtLowerArmors = saveLoadManager.boughtLowerArmors;

        if (boughtWeapons.Count == 0 || boughtWeapons[0] == null)
        {
            boughtWeapons.Add(defaultWeapon);
        }
        if (boughtCompanions.Count == 0 || boughtCompanions[0] == null)
        {
            boughtCompanions.Add(defaultCompanion);
        }
        if (boughtUpperArmors.Count == 0 || boughtUpperArmors[0] == null)
        {
            boughtUpperArmors.Add(defaultUpperArmor);
        }
        if (boughtLowerArmors.Count == 0 || boughtLowerArmors[0] == null)
        {
            boughtLowerArmors.Add(defaultLowerArmor);
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
    
    public void PurchaseUpperArmor(LoadoutItemsSO upperArmor)
    {
        boughtUpperArmors.Add(upperArmor);
    }
    
    public void PurchaseLowerArmor(LoadoutItemsSO lowerArmor)
    {
        boughtLowerArmors.Add(lowerArmor);
    }

    #endregion
}
