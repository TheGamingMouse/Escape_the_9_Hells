using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveClasses : MonoBehaviour
{
    public static SaveClasses Instance;

    void Awake()
    {
        Instance = this;
    }

    #region Ordinary SaveClasses

    public class LayerData
    {
        public LayerState lState = LevelManager.Instance.lState;
        
        public int layerReached = -1;
        public int highestLayerReached = -1;

        public enum LayerState
        {
            InLayers,
            Hub,
            MainMenu
        }
    }

    [Serializable]
    public class PersistentData
    {
        public int soulsCollectedInLayer = 0;
        public int levelsGainedInLayer = 0;
        public int demonsKilledInLayer = 0;
        public int devilsKilledInLayer = 0;
        public int reRolls = 0;
        public float expGainedInLayer = 0f;
        public float expMultiplierInLayer = 0f;
        public float healthInLayer = 0f;
        public float musicTime = 0f;
    }

    [Serializable]
    public class PlayerData
    {
        public int currentSouls = 0;
        public int totalSouls = 0;
        public int totalLevels = 0;
        public int demonsKilled = 0;
        public int devilsKilled = 0;
    }

    public class EquipmentData
    {
        public class WeaponData
        {
            public List<LoadoutItemsSO> boughtWeapons = new()
            {
                SetScriptableObjects.Instance.setWeapons.First()
            };

            public List<UpgradeItemsSO> pugioUpgrades = new();
            public List<UpgradeItemsSO> ulfberhtUpgrades = new();

            public LoadoutItemsSO primaryWeapon = SetScriptableObjects.Instance.setWeapons.First();
            public LoadoutItemsSO secondaryWeapon = SetScriptableObjects.Instance.setWeapons.First();
            public LoadoutItemsSO selectedWeapon = SetScriptableObjects.Instance.setWeapons.First();
        }

        public class CompanionData
        {
            public List<LoadoutItemsSO> boughtCompanions = new();

            public List<UpgradeItemsSO> loyalSphereUpgrades = new();
            public List<UpgradeItemsSO> attackSquareUpgrades = new();

            public LoadoutItemsSO primaryCompanion = SetScriptableObjects.Instance.setCompanions.First();
            public LoadoutItemsSO secondaryCompanion = SetScriptableObjects.Instance.setCompanions.First();
            public LoadoutItemsSO selectedCompanion = SetScriptableObjects.Instance.setCompanions.First();
        }

        public class ArmorData
        {
            public List<LoadoutItemsSO> boughtArmors = new();

            public List<UpgradeItemsSO> leatherUpgrades = new();
            public List<UpgradeItemsSO> hideUpgrades = new();
            public List<UpgradeItemsSO> ringMailUpgrades = new();
            public List<UpgradeItemsSO> plateUpgrades = new();

            public LoadoutItemsSO selectedArmor = SetScriptableObjects.Instance.setArmors.First();
        }

        public class BackData
        {
            public List<LoadoutItemsSO> boughtBacks = new();

            public List<UpgradeItemsSO> angelWingsUpgrades = new();
            public List<UpgradeItemsSO> steelWingsUpgrades = new();
            public List<UpgradeItemsSO> backpackUpgrades = new();
            public List<UpgradeItemsSO> capeOWindUpgrades = new();
            public List<UpgradeItemsSO> seedBagUpgrades = new();

            public LoadoutItemsSO selectedBack = SetScriptableObjects.Instance.setBacks.First();

            public bool backpackPrimary = true;
            public bool seedBagPrimary = true;
            public float capeOWindCooldown = 0f;
        }

        public WeaponData weaponData = new();
        public CompanionData companionData = new();
        public ArmorData armorData = new();
        public BackData backData = new();
    }

    public class SoulData
    {
        public List<SoulsItemsSO> attackSpeedSoulsBought = new();
        public List<SoulsItemsSO> damageSoulsBought = new();
        public List<SoulsItemsSO> defenceSoulsBought = new();
        public List<SoulsItemsSO> movementSpeedSoulsBought = new();
        public List<SoulsItemsSO> luckSoulsBought = new();
        public List<SoulsItemsSO> startLevelSoulsBought = new();
        public List<SoulsItemsSO> reRollSoulsBought = new();
        public List<SoulsItemsSO> pathFinderSoulsBought = new();
    }

    public class PerkData
    {
        public List<PerkItemsSO> defencePerks = new();
        public List<PerkItemsSO> attackSpeedPerks = new();
        public List<PerkItemsSO> damagePerks = new();
        public List<PerkItemsSO> moveSpeedPerks = new();
        public List<PerkItemsSO> luckPerks = new();
        public List<PerkItemsSO> fireAuraPerks = new();
        public List<PerkItemsSO> shieldPerks = new();
        public List<PerkItemsSO> iceAuraPerks = new();
    }

    [Serializable]
    public class SettingsData
    {
        public int screenMode = 0;
        public float masterVolume = 0f;
        public float musicVolume = 0f;
        public float sfxVolume = 0f;
    }

    [Serializable]
    public class NpcData
    {
        public bool rickyStartComp = false;
        public bool ready = false;
        public bool returnedToRicky = false;
    }

    #endregion

    #region JsonFriendly SaveClasses

    [Serializable]
    public class FriendlyLayerData
    {
        public string lState = LevelManager.Instance.lState.ToString();
        public int layerReached = -1;
        public int highestLayerReached = -1;
    }

    [Serializable]
    public class FriendlyEquipmentData
    {
        [Serializable]
        public class WeaponData
        {
            public List<string> boughtWeaponNames = new()
            {
                "Pugio"
            };

            public List<string> pugioUpgradeNames = new();
            public List<string> ulfberhtUpgradeNames = new();

            public string primaryWeaponName = "Pugio";
            public string secondaryWeaponName = "Pugio";
            public string selectedWeaponName = "Pugio";
        }

        [Serializable]
        public class CompanionData
        {
            public List<string> boughtCompanionNames = new();

            public List<string> loyalSphereUpgradeNames = new();
            public List<string> attackSquareUpgradeNames = new();

            public string primaryCompanionName = "";
            public string secondaryCompanionName = "";
            public string selectedCompanionName = "Unequiped";
        }

        [Serializable]
        public class ArmorData
        {
            public List<string> boughtArmorNames = new();

            public List<string> leatherUpgradeNames = new();
            public List<string> hideUpgradeNames = new();
            public List<string> ringMailUpgradeNames = new();
            public List<string> plateUpgradeNames = new();

            public string selectedArmorName = "Unequiped";
        }

        [Serializable]
        public class BackData
        {
            public List<string> boughtBackNames = new();

            public List<string> angelWingsUpgradeNames = new();
            public List<string> steelWingsUpgradeNames = new();
            public List<string> backpackUpgradeNames = new();
            public List<string> capeOWindUpgradeNames = new();
            public List<string> seedBagUpgradeNames = new();

            public string selectedBackName = "Unequiped";

            public bool backpackPrimary = true;
            public bool seedBagPrimary = true;
            public float capeOWindCooldown = 0f;
        }

        public WeaponData weaponData = new();
        public CompanionData companionData = new();
        public ArmorData armorData = new();
        public BackData backData = new();
    }

    [Serializable]
    public class FriendlySoulData
    {
        public int attackSpeedSoulsBought = new();
        public int damageSoulsBought = new();
        public int defenceSoulsBought = new();
        public int movementSpeedSoulsBought = new();
        public int luckSoulsBought = new();
        public int startLevelSoulsBought = new();
        public int reRollSoulsBought = new();
        public int pathFinderSoulsBought = new();
    }

    [Serializable]
    public class FriendlyPerkData
    {
        public int defencePerks = new();
        public int attackSpeedPerks = new();
        public int damagePerks = new();
        public int moveSpeedPerks = new();
        public int luckPerks = new();
        public int fireAuraPerks = new();
        public int shieldPerks = new();
        public int iceAuraPerks = new();
    }

    #endregion
}
