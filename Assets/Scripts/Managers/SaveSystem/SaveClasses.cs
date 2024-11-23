using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SaveSystemSpace
{
    public class SaveClasses : MonoBehaviour
    {
        #region Ordinary SaveClasses

        public class LayerData
        {
            public LayerState lState = LayerState.InLayers;
            
            public int layerReached = -1;
            public int highestLayerReached = -1;

            public enum LayerState
            {
                InLayers,
                Hub,
                MainMenu
            }

            public static LayerData Parse(FriendlyLayerData data)
            {
                LayerData unfriendlyObject = new()
                {
                    lState = Enum.Parse<LayerState>(data.lState),
                    layerReached = data.layerReached,
                    highestLayerReached = data.highestLayerReached
                };

                return unfriendlyObject;
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
            public int healthInLayer = 100;
            public float expGainedInLayer = 0f;
            public float expMultiplierInLayer = 0f;
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
            public bool newGame = true;
        }

        public class EquipmentData
        {
            public class WeaponData
            {
                public List<LoadoutItemsSO> boughtWeapons = new()
                {
                    SetScriptableObjects.Instance.weapons.First()
                };

                public List<UpgradeItemsSO> pugioUpgrades = new();
                public List<UpgradeItemsSO> ulfberhtUpgrades = new();

                public LoadoutItemsSO primaryWeapon = SetScriptableObjects.Instance.pugio;
                public LoadoutItemsSO secondaryWeapon = SetScriptableObjects.Instance.pugio;
                public LoadoutItemsSO selectedWeapon = SetScriptableObjects.Instance.pugio;

                public const string pugioString = "Pugio";
                public const string ulfberhtString = "Ulfberht";

                // Upgrade Strings
                public const string attackSpeedUpgradeString = "Attack Speed Upgrade";
                public const string damageUpgradeString = "Damage Upgrade";
                public const string specialAttackUpgradeString = "Special Attack";
                public const string specialCooldownUpgradeString = "Special Cooldown";

                public static WeaponData Parse(FriendlyEquipmentData data)
                {
                    WeaponData unfriendlyWeaponData = new();
                    var setWeapons = SetScriptableObjects.Instance.weapons;
                    var setWeaponUpgrades = SetScriptableObjects.Instance.weaponUpgrades;

                    foreach (var weapon in data.weaponData.boughtWeaponNames)
                    {
                        foreach (var setWeapon in setWeapons)
                        {
                            if (weapon == setWeapon.title) unfriendlyWeaponData.boughtWeapons.Add(setWeapon);

                            if (setWeapon.title == data.weaponData.primaryWeaponName) unfriendlyWeaponData.primaryWeapon = setWeapon;

                            if (setWeapon.title == data.weaponData.secondaryWeaponName) unfriendlyWeaponData.secondaryWeapon = setWeapon;

                            if (setWeapon.title == data.weaponData.selectedWeaponName) unfriendlyWeaponData.selectedWeapon = setWeapon;
                        }
                    }

                    foreach (var setUpgrade in setWeaponUpgrades)
                    {
                        foreach (var upgrade in data.weaponData.pugioUpgradeNames) if (upgrade == setUpgrade.title) unfriendlyWeaponData.pugioUpgrades.Add(setUpgrade);

                        foreach (var upgrade in data.weaponData.ulfberhtUpgradeNames) if (upgrade == setUpgrade.title) unfriendlyWeaponData.ulfberhtUpgrades.Add(setUpgrade);
                    }

                    return unfriendlyWeaponData;
                }
            }

            public class CompanionData
            {
                public List<LoadoutItemsSO> boughtCompanions = new();

                public List<UpgradeItemsSO> loyalSphereUpgrades = new();
                public List<UpgradeItemsSO> attackSquareUpgrades = new();

                public LoadoutItemsSO primaryCompanion = SetScriptableObjects.Instance.unequipedCompanion;
                public LoadoutItemsSO secondaryCompanion = SetScriptableObjects.Instance.unequipedCompanion;
                public LoadoutItemsSO selectedCompanion = SetScriptableObjects.Instance.unequipedCompanion;

                public const string loyalSphereString = "Loyal Sphere";
                public const string attackSquareString = "Attack Square";

                // Upgrade Strings
                public const string abilityRateUpgradeString = "Ability Rate Upgrade";
                public const string abilityStrengthUpgradeString = "Ability Strength Upgrade";

                public static CompanionData Parse(FriendlyEquipmentData data)
                {
                    CompanionData unfriendlyCompanionData = new();
                    var setCompanions = SetScriptableObjects.Instance.companions;
                    var setCompanionUpgrades = SetScriptableObjects.Instance.companionUpgrades;

                    foreach (var companion in data.companionData.boughtCompanionNames)
                    {
                        foreach (var setCompanion in setCompanions)
                        {
                            if (companion == setCompanion.title) unfriendlyCompanionData.boughtCompanions.Add(setCompanion);

                            if (setCompanion.title == data.companionData.primaryCompanionName) unfriendlyCompanionData.primaryCompanion = setCompanion;

                            if (setCompanion.title == data.companionData.secondaryCompanionName) unfriendlyCompanionData.secondaryCompanion = setCompanion;

                            if (setCompanion.title == data.companionData.selectedCompanionName) unfriendlyCompanionData.selectedCompanion = setCompanion;
                        }
                    }

                    foreach (var setUpgrade in setCompanionUpgrades)
                    {
                        foreach (var upgrade in data.companionData.loyalSphereUpgradeNames)
                            if (upgrade == setUpgrade.title) unfriendlyCompanionData.loyalSphereUpgrades.Add(setUpgrade);

                        foreach (var upgrade in data.companionData.attackSquareUpgradeNames)
                            if (upgrade == setUpgrade.title) unfriendlyCompanionData.attackSquareUpgrades.Add(setUpgrade);
                    }

                    return unfriendlyCompanionData;
                }
            }

            public class ArmorData
            {
                public List<LoadoutItemsSO> boughtArmors = new();

                public List<UpgradeItemsSO> leatherUpgrades = new();
                public List<UpgradeItemsSO> hideUpgrades = new();
                public List<UpgradeItemsSO> ringMailUpgrades = new();
                public List<UpgradeItemsSO> plateUpgrades = new();

                public LoadoutItemsSO selectedArmor = SetScriptableObjects.Instance.unequipedArmor;

                public const string leatherString = "Leather";
                public const string hideString = "Hide";
                public const string ringMailString = "Ring Mail";
                public const string plateString = "Plate";

                // Upgrade Strings
                public const string resistanceUpgradeString = "Resistance Upgrade";
                public const string speedUpgradeString = "Speed Upgrade";

                public static ArmorData Parse(FriendlyEquipmentData data)
                {
                    ArmorData unfriendlyArmorData = new();
                    var setArmors = SetScriptableObjects.Instance.armors;
                    var setArmorsUpgrades = SetScriptableObjects.Instance.armorUpgrades;

                    foreach (var armor in data.armorData.boughtArmorNames)
                    {
                        foreach (var setArmor in setArmors)
                        {
                            if (armor == setArmor.title) unfriendlyArmorData.boughtArmors.Add(setArmor);

                            if (setArmor.title == data.armorData.selectedArmorName) unfriendlyArmorData.selectedArmor = setArmor;
                        }
                    }

                    foreach (var setUpgrade in setArmorsUpgrades)
                    {
                        foreach (var upgrade in data.armorData.leatherUpgradeNames)
                            if (upgrade == setUpgrade.title) unfriendlyArmorData.leatherUpgrades.Add(setUpgrade);

                        foreach (var upgrade in data.armorData.hideUpgradeNames)
                            if (upgrade == setUpgrade.title) unfriendlyArmorData.hideUpgrades.Add(setUpgrade);

                        foreach (var upgrade in data.armorData.ringMailUpgradeNames)
                            if (upgrade == setUpgrade.title) unfriendlyArmorData.ringMailUpgrades.Add(setUpgrade);

                        foreach (var upgrade in data.armorData.plateUpgradeNames)
                            if (upgrade == setUpgrade.title) unfriendlyArmorData.plateUpgrades.Add(setUpgrade);
                    }

                    return unfriendlyArmorData;
                }
            }

            public class BackData
            {
                public List<LoadoutItemsSO> boughtBacks = new();

                public List<UpgradeItemsSO> angelWingsUpgrades = new();
                public List<UpgradeItemsSO> steelWingsUpgrades = new();
                public List<UpgradeItemsSO> backpackUpgrades = new();
                public List<UpgradeItemsSO> capeOWindUpgrades = new();
                public List<UpgradeItemsSO> seedBagUpgrades = new();

                public LoadoutItemsSO selectedBack = SetScriptableObjects.Instance.unequipedBack;

                public bool backpackPrimary = true;
                public bool seedBagPrimary = true;
                public float capeOWindCooldown = 0f;

                public const string angelWingsString = "Angel Wings";
                public const string steelWingsString = "Steel Wings";
                public const string backpackString = "Backpack";
                public const string capeOWindString = "Cape O' Wind";
                public const string seedBagString = "Seed Bag";

                // Upgrade Strings
                public const string cooldownUpgradeString = "Cooldown Upgrade";

                public static BackData Parse(FriendlyEquipmentData data)
                {
                    BackData unfriendlyBackData = new();
                    var setBacks = SetScriptableObjects.Instance.backs;
                    var setBacksUpgrades = SetScriptableObjects.Instance.backUpgrades;

                    foreach (var back in data.backData.boughtBackNames)
                    {
                        foreach (var setBack in setBacks)
                        {
                            if (back == setBack.title) unfriendlyBackData.boughtBacks.Add(setBack);

                            if (setBack.title == data.backData.selectedBackName) unfriendlyBackData.selectedBack = setBack;
                        }
                    }

                    foreach (var setUpgrade in setBacksUpgrades)
                    {
                        foreach (var upgrade in data.backData.angelWingsUpgradeNames)
                            if (upgrade == setUpgrade.title) unfriendlyBackData.angelWingsUpgrades.Add(setUpgrade);

                        foreach (var upgrade in data.backData.steelWingsUpgradeNames)
                            if (upgrade == setUpgrade.title) unfriendlyBackData.steelWingsUpgrades.Add(setUpgrade);

                        foreach (var upgrade in data.backData.backpackUpgradeNames)
                            if (upgrade == setUpgrade.title) unfriendlyBackData.backpackUpgrades.Add(setUpgrade);

                        foreach (var upgrade in data.backData.capeOWindUpgradeNames)
                            if (upgrade == setUpgrade.title) unfriendlyBackData.capeOWindUpgrades.Add(setUpgrade);

                        foreach (var upgrade in data.backData.seedBagUpgradeNames)
                            if (upgrade == setUpgrade.title) unfriendlyBackData.seedBagUpgrades.Add(setUpgrade);
                    }

                    unfriendlyBackData.backpackPrimary = data.backData.backpackPrimary;
                    unfriendlyBackData.seedBagPrimary = data.backData.seedBagPrimary;
                    unfriendlyBackData.capeOWindCooldown = data.backData.capeOWindCooldown;

                    return unfriendlyBackData;
                }
            }

            public WeaponData weaponData = new();
            public CompanionData companionData = new();
            public ArmorData armorData = new();
            public BackData backData = new();

            public const string unequipedString = "Unequiped";

            public static EquipmentData Parse(FriendlyEquipmentData data)
            {
                EquipmentData unfriendlyObject = new()
                {
                    weaponData = WeaponData.Parse(data),
                    companionData = CompanionData.Parse(data),
                    armorData = ArmorData.Parse(data),
                    backData = BackData.Parse(data)
                };

                return unfriendlyObject;
            }
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

            public const string attackSpeedString = "Attack Speed Soul";
            public const string damageString = "Damage Soul";
            public const string defenceString = "Defence Soul";
            public const string movementSpeedString = "Movement Speed Soul";
            public const string luckString = "Luck Soul";
            public const string startLevelString = "Start Level Soul";
            public const string reRollString = "Re Roll Soul";
            public const string pathFinderString = "Path Finder Soul";

            public static SoulData Parse(FriendlySoulData data)
            {
                SoulData unfriendlyObject = new();
                var setSoulItems = SetScriptableObjects.Instance.souls;

                SoulsItemsSO attackSpeedSoul = null;
                SoulsItemsSO damageSoul = null;
                SoulsItemsSO defenceSoul = null;
                SoulsItemsSO movementSpeedSoul = null;
                SoulsItemsSO luckSoul = null;
                SoulsItemsSO startLevelSoul = null;
                SoulsItemsSO reRollSoul = null;
                SoulsItemsSO pathFinderSoul = null;

                foreach (var soul in setSoulItems)
                    _ = soul.title switch
                    {
                        attackSpeedString => attackSpeedSoul = soul,
                        damageString => damageSoul = soul,
                        defenceString => defenceSoul = soul,
                        movementSpeedString => movementSpeedSoul = soul,
                        luckString => luckSoul = soul,
                        startLevelString => startLevelSoul = soul,
                        reRollString => reRollSoul = soul,
                        pathFinderString => pathFinderSoul = soul,

                        _ => throw new ArgumentException($"Name of Soul: '{soul.title}' was not recognized.")
                    };

                for (int i = 0; i < data.attackSpeedSoulsBought; i++)
                    unfriendlyObject.attackSpeedSoulsBought.Add(attackSpeedSoul);

                for (int i = 0; i< data.damageSoulsBought; i++)
                    unfriendlyObject.damageSoulsBought.Add(damageSoul);

                for (int i = 0; i< data.defenceSoulsBought; i++)
                    unfriendlyObject.defenceSoulsBought.Add(defenceSoul);

                for (int i = 0; i< data.movementSpeedSoulsBought; i++)
                    unfriendlyObject.movementSpeedSoulsBought.Add(movementSpeedSoul);

                for (int i = 0; i< data.luckSoulsBought; i++)
                    unfriendlyObject.luckSoulsBought.Add(luckSoul);

                for (int i = 0; i< data.startLevelSoulsBought; i++)
                    unfriendlyObject.startLevelSoulsBought.Add(startLevelSoul);

                for (int i = 0; i< data.reRollSoulsBought; i++)
                    unfriendlyObject.reRollSoulsBought.Add(reRollSoul);

                for (int i = 0; i< data.pathFinderSoulsBought; i++)
                    unfriendlyObject.pathFinderSoulsBought.Add(pathFinderSoul);

                return unfriendlyObject;
            }
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

            public const string defenceString = "Defence Perk";
            public const string attackSpeedString = "Attack Speed Perk";
            public const string damageString = "Damage Perk";
            public const string movementSpeedString = "Movement Speed Perk";
            public const string luckString = "Luck Perk";
            public const string fireAuraString = "Fire Aura Perk";
            public const string shieldString = "Shield Perk";
            public const string iceAuraString = "Ice Aura Perk";

            public static PerkData Parse(FriendlyPerkData data)
            {
                PerkData unfriendlyObject = new();
                var setPerks = SetScriptableObjects.Instance.perks;

                PerkItemsSO defencePerk = null;
                PerkItemsSO attackSpeedPerk = null;
                PerkItemsSO damagePerk = null;
                PerkItemsSO moveSpeedPerk = null;
                PerkItemsSO luckPerk = null;
                PerkItemsSO fireAuraPerk = null;
                PerkItemsSO shieldPerk = null;
                PerkItemsSO iceAuraPerk = null;

                foreach (var perk in setPerks)
                    _ = perk.title switch
                    {
                        defenceString => defencePerk = perk,
                        attackSpeedString => attackSpeedPerk = perk,
                        damageString => damagePerk = perk,
                        movementSpeedString => moveSpeedPerk = perk,
                        luckString => luckPerk = perk,
                        fireAuraString => fireAuraPerk = perk,
                        shieldString => shieldPerk = perk,
                        iceAuraString => iceAuraPerk = perk,

                        _ => throw new ArgumentException($"Name of Soul: '{perk.title}' was not recognized.")
                    };

                for (int i = 0; i< data.defencePerks; i++)
                    unfriendlyObject.defencePerks.Add(defencePerk);

                for (int i = 0; i< data.attackSpeedPerks; i++)
                    unfriendlyObject.attackSpeedPerks.Add(attackSpeedPerk);

                for (int i = 0; i< data.damagePerks; i++)
                    unfriendlyObject.damagePerks.Add(damagePerk);

                for (int i = 0; i< data.moveSpeedPerks; i++)
                    unfriendlyObject.moveSpeedPerks.Add(moveSpeedPerk);

                for (int i = 0; i< data.luckPerks; i++)
                    unfriendlyObject.luckPerks.Add(luckPerk);

                for (int i = 0; i< data.fireAuraPerks; i++)
                    unfriendlyObject.fireAuraPerks.Add(fireAuraPerk);

                for (int i = 0; i< data.shieldPerks; i++)
                    unfriendlyObject.shieldPerks.Add(shieldPerk);

                for (int i = 0; i< data.iceAuraPerks; i++)
                    unfriendlyObject.iceAuraPerks.Add(iceAuraPerk);

                return unfriendlyObject;
            }
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
            public bool returnedToRicky = false;
        }

        #endregion

        #region JsonFriendly SaveClasses

        [Serializable]
        public class FriendlyLayerData
        {
            public string lState = "InLayers";
            public int layerReached = -1;
            public int highestLayerReached = -1;
        }

        public class LayerSupport
        {
            public static FriendlyLayerData Parse(LayerData data)
            {
                FriendlyLayerData friendlyData = new()
                {
                    lState = data.lState.ToString(),
                    layerReached = data.layerReached,
                    highestLayerReached = data.highestLayerReached
                };

                return friendlyData;
            }
        }

        [Serializable]
        public class FriendlyEquipmentData
        {
            [Serializable]
            public class FriendlyWeaponData
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
            public class FriendlyCompanionData
            {
                public List<string> boughtCompanionNames = new();

                public List<string> loyalSphereUpgradeNames = new();
                public List<string> attackSquareUpgradeNames = new();

                public string primaryCompanionName = "";
                public string secondaryCompanionName = "";
                public string selectedCompanionName = "Unequiped";
            }

            [Serializable]
            public class FriendlyArmorData
            {
                public List<string> boughtArmorNames = new();

                public List<string> leatherUpgradeNames = new();
                public List<string> hideUpgradeNames = new();
                public List<string> ringMailUpgradeNames = new();
                public List<string> plateUpgradeNames = new();

                public string selectedArmorName = "Unequiped";
            }

            [Serializable]
            public class FriendlyBackData
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

            public FriendlyWeaponData weaponData = new();
            public FriendlyCompanionData companionData = new();
            public FriendlyArmorData armorData = new();
            public FriendlyBackData backData = new();
        }

        public class EquipmentSupport
        {
            public class WeaponSupport
            {
                public static FriendlyEquipmentData.FriendlyWeaponData Parse(EquipmentData data)
                {
                    FriendlyEquipmentData.FriendlyWeaponData friendlyWeaponData = new();

                    foreach (var weapon in data.weaponData.boughtWeapons)
                        if (!friendlyWeaponData.boughtWeaponNames.Contains(weapon.title)) friendlyWeaponData.boughtWeaponNames.Add(weapon.title);

                    foreach (var upgrade in data.weaponData.pugioUpgrades)
                        friendlyWeaponData.pugioUpgradeNames.Add(upgrade.title);

                    foreach (var upgrade in data.weaponData.ulfberhtUpgrades)
                        friendlyWeaponData.ulfberhtUpgradeNames.Add(upgrade.title);

                    if (data.weaponData.selectedWeapon != null)
                    {
                        friendlyWeaponData.primaryWeaponName = data.weaponData.primaryWeapon.name;
                        friendlyWeaponData.secondaryWeaponName = data.weaponData.secondaryWeapon.name;
                    }
                    friendlyWeaponData.selectedWeaponName = data.weaponData.selectedWeapon.name;

                    return friendlyWeaponData;
                }
            }

            public class CompanionSupport
            {
                public static FriendlyEquipmentData.FriendlyCompanionData Parse(EquipmentData data)
                {
                    FriendlyEquipmentData.FriendlyCompanionData friendlyCompanionData = new();

                    foreach (var companion in data.companionData.boughtCompanions)
                        if (!friendlyCompanionData.boughtCompanionNames.Contains(companion.title)) friendlyCompanionData.boughtCompanionNames.Add(companion.title);

                    foreach (var upgrade in data.companionData.loyalSphereUpgrades)
                        friendlyCompanionData.loyalSphereUpgradeNames.Add(upgrade.title);

                    foreach (var upgrade in data.companionData.attackSquareUpgrades)
                        friendlyCompanionData.attackSquareUpgradeNames.Add(upgrade.title);

                    if (data.companionData.selectedCompanion != null)
                    {
                        friendlyCompanionData.primaryCompanionName = data.companionData.primaryCompanion.name;
                        friendlyCompanionData.secondaryCompanionName = data.companionData.secondaryCompanion.name;
                        friendlyCompanionData.selectedCompanionName = data.companionData.selectedCompanion.name;
                    }

                    return friendlyCompanionData;
                }
            }

            public class ArmorSupport
            {
                public static FriendlyEquipmentData.FriendlyArmorData Parse(EquipmentData data)
                {
                    FriendlyEquipmentData.FriendlyArmorData friendlyArmorData = new();

                    foreach (var armor in data.armorData.boughtArmors)
                        if (!friendlyArmorData.boughtArmorNames.Contains(armor.title)) friendlyArmorData.boughtArmorNames.Add(armor.title);

                    foreach (var upgrade in data.armorData.leatherUpgrades)
                        friendlyArmorData.leatherUpgradeNames.Add(upgrade.title);

                    foreach (var upgrade in data.armorData.hideUpgrades)
                        friendlyArmorData.hideUpgradeNames.Add(upgrade.title);

                    foreach (var upgrade in data.armorData.ringMailUpgrades)
                        friendlyArmorData.ringMailUpgradeNames.Add(upgrade.title);

                    foreach (var upgrade in data.armorData.plateUpgrades)
                        friendlyArmorData.plateUpgradeNames.Add(upgrade.title);

                    if (data.armorData.selectedArmor != null) friendlyArmorData.selectedArmorName = data.armorData.selectedArmor.title;

                    return friendlyArmorData;
                }
            }

            public class BackSupport
            {
                public static FriendlyEquipmentData.FriendlyBackData Parse(EquipmentData data)
                {
                    FriendlyEquipmentData.FriendlyBackData friendlyBackData = new();

                    foreach (var back in data.backData.boughtBacks)
                        if (!friendlyBackData.boughtBackNames.Contains(back.title)) friendlyBackData.boughtBackNames.Add(back.title);

                    foreach (var upgrade in data.backData.angelWingsUpgrades)
                        friendlyBackData.angelWingsUpgradeNames.Add(upgrade.title);

                    foreach (var upgrade in data.backData.steelWingsUpgrades)
                        friendlyBackData.steelWingsUpgradeNames.Add(upgrade.title);

                    foreach (var upgrade in data.backData.backpackUpgrades)
                        friendlyBackData.backpackUpgradeNames.Add(upgrade.title);

                    foreach (var upgrade in data.backData.capeOWindUpgrades)
                        friendlyBackData.capeOWindUpgradeNames.Add(upgrade.title);

                    foreach (var upgrade in data.backData.seedBagUpgrades)
                        friendlyBackData.seedBagUpgradeNames.Add(upgrade.title);

                    
                    if (data.backData.selectedBack != null) friendlyBackData.selectedBackName = data.backData.selectedBack.title;

                    friendlyBackData.backpackPrimary = data.backData.backpackPrimary;
                    friendlyBackData.seedBagPrimary = data.backData.seedBagPrimary;
                    friendlyBackData.capeOWindCooldown = data.backData.capeOWindCooldown;

                    return friendlyBackData;
                }
            }

            public static FriendlyEquipmentData Parse(EquipmentData data)
            {
                FriendlyEquipmentData friendlyObject = new()
                {
                    weaponData = WeaponSupport.Parse(data),
                    companionData = CompanionSupport.Parse(data),
                    armorData = ArmorSupport.Parse(data),
                    backData = BackSupport.Parse(data)
                };

                return friendlyObject;
            }
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

        public class SoulSupport
        {
            public static FriendlySoulData Parse(SoulData data)
            {
                FriendlySoulData friendlyObject = new()
                {
                    attackSpeedSoulsBought = data.attackSpeedSoulsBought.Count,
                    damageSoulsBought = data.damageSoulsBought.Count,
                    defenceSoulsBought = data.defenceSoulsBought.Count,
                    movementSpeedSoulsBought = data.movementSpeedSoulsBought.Count,
                    luckSoulsBought = data.luckSoulsBought.Count,
                    startLevelSoulsBought = data.startLevelSoulsBought.Count,
                    reRollSoulsBought = data.reRollSoulsBought.Count,
                    pathFinderSoulsBought = data.pathFinderSoulsBought.Count
                };

                return friendlyObject;
            }
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

        public class PerkSupport
        {
            public static FriendlyPerkData Parse(PerkData data)
            {
                FriendlyPerkData friendlyObject = new()
                {
                    defencePerks = data.defencePerks.Count,
                    attackSpeedPerks = data.attackSpeedPerks.Count,
                    damagePerks = data.damagePerks.Count,
                    moveSpeedPerks = data.moveSpeedPerks.Count,
                    luckPerks = data.luckPerks.Count,
                    fireAuraPerks = data.fireAuraPerks.Count,
                    shieldPerks = data.shieldPerks.Count,
                    iceAuraPerks = data.iceAuraPerks.Count
                };

                return friendlyObject;
            }
        }

        #endregion
    }
}