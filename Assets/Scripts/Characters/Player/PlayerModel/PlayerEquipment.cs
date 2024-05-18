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

    [Header("Components")]
    SaveLoadManager saveLoadManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        saveLoadManager = GameObject.FindWithTag("Managers").GetComponent<SaveLoadManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!equipmentLoaded)
        {
            boughtWeapons = saveLoadManager.boughtWeapons;
            boughtCompanions = saveLoadManager.boughtCompanions;
            boughtArmors = saveLoadManager.boughtArmors;
            boughtBacks = saveLoadManager.boughtBacks;

            // Weapons
            int countWeapons = 0;
            foreach (var weapon in boughtWeapons)
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
            foreach (var companion in boughtCompanions)
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
            foreach (var armor in boughtArmors)
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
            foreach (var back in boughtBacks)
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
    }
    
    public void PurchaseCompanion(LoadoutItemsSO companion)
    {
        boughtCompanions.Add(companion);
    }
    
    public void PurchaseArmor(LoadoutItemsSO armor)
    {
        boughtArmors.Add(armor);
    }
    
    public void PurchaseBack(LoadoutItemsSO back)
    {
        boughtBacks.Add(back);
    }

    #endregion
}
