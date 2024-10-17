using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<LoadoutItemsSO> boughtBacks = new();

    [Header("LoadoutItemsSOs")]
    public LoadoutItemsSO defaultWeapon;
    public LoadoutItemsSO defaultCompanion;
    public LoadoutItemsSO defaultArmor;
    public LoadoutItemsSO defaultBacks;

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (!equipmentLoaded)
        {
            var equipmentData = SaveSystem.loadedEquipmentData;

            boughtWeapons = equipmentData.weaponData.boughtWeapons;
            boughtCompanions = equipmentData.companionData.boughtCompanions;
            boughtArmors = equipmentData.armorData.boughtArmors;
            boughtBacks = equipmentData.backData.boughtBacks;

            // Weapons
            int countWeapons = 0;
            foreach (var weapon in boughtWeapons.ToList())
            {
                if (!weapon || weapon.equipmentType.ToLower() != "weapon")
                {
                    boughtWeapons.Remove(weapon);
                    continue;
                }
                else if (weapon.title == defaultWeapon.title)
                {
                    countWeapons++;
                    if (countWeapons > 1)
                    {
                        boughtWeapons.Remove(weapon);
                        countWeapons--;
                        continue;
                    }
                } 
            }
            if (countWeapons == 0)
            {
                boughtWeapons.Add(defaultWeapon);
            }

            // Companions
            int countCompanions = 0;
            foreach (var companion in boughtCompanions.ToList())
            {
                if (!companion || companion.equipmentType.ToLower() != "companion")
                {
                    boughtCompanions.Remove(companion);
                    continue;
                }
                else if (companion.title == defaultCompanion.title)
                {
                    countCompanions++;
                    if (countCompanions > 1)
                    {
                        boughtCompanions.Remove(companion);
                        countCompanions--;
                        continue;
                    }
                }
            }
            if (countCompanions == 0)
            {
                boughtCompanions.Add(defaultCompanion);
            }

            // Armors
            int countArmors = 0;
            foreach (var armor in boughtArmors.ToList())
            {
                if (!armor || armor.equipmentType.ToLower() != "armor")
                {
                    boughtArmors.Remove(armor);
                    continue;
                }
                else if (armor.title == defaultArmor.title)
                {
                    countArmors++;
                    if (countArmors > 1)
                    {
                        boughtArmors.Remove(armor);
                        countArmors--;
                        continue;
                    }
                }
            }
            if (countArmors == 0)
            {
                boughtArmors.Add(defaultArmor);
            }

            // Backs
            int countBacks = 0;
            foreach (var back in boughtBacks.ToList())
            {
                if (!back || back.equipmentType.ToLower() != "back")
                {
                    boughtBacks.Remove(back);
                    continue;
                }
                else if (back.title == defaultBacks.title)
                {
                    countBacks++;
                    if (countBacks > 1)
                    {
                        boughtBacks.Remove(back);
                        countBacks--;
                        continue;
                    }
                }
            }
            if (countBacks == 0)
            {
                boughtBacks.Add(defaultBacks);
            }

            equipmentLoaded = true;
        }
    }

    #endregion

    #region General Methods

    public void PurchaseWeapon(LoadoutItemsSO weapon)
    {
        boughtWeapons.Add(weapon);

        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.weaponData.boughtWeapons.Add(weapon);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);
    }
    
    public void PurchaseCompanion(LoadoutItemsSO companion)
    {
        boughtCompanions.Add(companion);

        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.companionData.boughtCompanions.Add(companion);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);
    }
    
    public void PurchaseArmor(LoadoutItemsSO armor)
    {
        boughtArmors.Add(armor);

        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.armorData.boughtArmors.Add(armor);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);
    }
    
    public void PurchaseBack(LoadoutItemsSO back)
    {
        boughtBacks.Add(back);

        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.backData.boughtBacks.Add(back);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);
    }

    #endregion
}
