using System.Collections.Generic;
using UnityEngine;

public class SetScriptableObjects : MonoBehaviour
{
    public static SetScriptableObjects Instance;

    public List<LoadoutItemsSO> setWeapons;
    public List<LoadoutItemsSO> setCompanions;
    public List<LoadoutItemsSO> setArmors;
    public List<LoadoutItemsSO> setBacks;

    public List<UpgradeItemsSO> setWeaponUpgrades;
    public List<UpgradeItemsSO> setCompanionUpgrades;
    public List<UpgradeItemsSO> setArmorUpgrades;
    public List<UpgradeItemsSO> setBackUpgrades;

    public List<SoulsItemsSO> setSoulItems;
    public List<PerkItemsSO> setPerks;

    void Awake()
    {
        Instance = this;
    }
}
