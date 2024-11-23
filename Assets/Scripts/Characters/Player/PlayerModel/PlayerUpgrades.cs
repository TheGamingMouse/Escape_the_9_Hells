using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SaveSystemSpace;
using Unity.VisualScripting;
using UnityEngine;
using static SaveSystemSpace.SaveClasses.EquipmentData;

public class PlayerUpgrades : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    // Weapons
    readonly float atSpeed = 0.25f;
    float activeAtSpeed;
    readonly float damage = 0.05f;
    float acitveDamage;
    readonly float spCool = 0.35f;
    float activeSpCool;

    // Companions
    readonly float abRate = 0.25f;
    float activeAbRate;
    readonly float abStrength = 0.2f;
    float activeAbStrength;

    // // Armors
    readonly float resist = 0.04f;
    float activeResist;
    readonly float speedPen = 0.025f;
    float activeSpeedPen;

    // // Backs
    readonly float cool = 0.15f;
    float activeCool;

    [Header("Bools")]
    public bool weaponUpdated;
    public bool companionUpdated;
    public bool backUpdated;
    public bool armorUpdated;
    bool loaded;

    [Header("Lists")]
    // Weapons
    public List<UpgradeItemsSO> upgradesPugio = new();
    public List<UpgradeItemsSO> upgradesUlfberht = new();

    // Companions
    public List<UpgradeItemsSO> upgradesLoyalSphere = new();
    public List<UpgradeItemsSO> upgradesAttackSquare = new();

    // Armors
    public List<UpgradeItemsSO> upgradesLeather = new();
    public List<UpgradeItemsSO> upgradesHide = new();
    public List<UpgradeItemsSO> upgradesRingMail = new();
    public List<UpgradeItemsSO> upgradesPlate = new();

    // Backs
    public List<UpgradeItemsSO> upgradesAngelWings = new();
    public List<UpgradeItemsSO> upgradesSteelWings = new();
    public List<UpgradeItemsSO> upgradesBackpacks = new();
    public List<UpgradeItemsSO> upgradesCapeOWinds = new();
    public List<UpgradeItemsSO> upgradesSeedBags = new();

    [Header("Components")]
    Weapon weapon;
    Companion companion;
    Armor armor;
    Backs backs;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
        armor = GetComponentInChildren<Armor>();
        backs = GetComponentInChildren<Backs>();
        
        if (GameObject.FindWithTag("Companions"))
        {
            companion = GameObject.FindWithTag("Companions").GetComponent<Companion>();
        }
    }

    void Update()
    {
        var loadout = PlayerComponents.Instance.playerLoadout;

        if (loadout.start)
        {
            if (!loaded)
            {
                var equipmentData = SaveSystem.loadedEquipmentData;

                // Weapons
                upgradesPugio = equipmentData.weaponData.pugioUpgrades;
                upgradesUlfberht = equipmentData.weaponData.ulfberhtUpgrades;

                // Companions
                upgradesLoyalSphere = equipmentData.companionData.loyalSphereUpgrades;
                upgradesAttackSquare = equipmentData.companionData.attackSquareUpgrades;

                // Armors
                upgradesLeather = equipmentData.armorData.leatherUpgrades;
                upgradesHide = equipmentData.armorData.hideUpgrades;
                upgradesRingMail = equipmentData.armorData.ringMailUpgrades;
                upgradesPlate = equipmentData.armorData.plateUpgrades;

                // Backs
                upgradesAngelWings = equipmentData.backData.angelWingsUpgrades;
                upgradesSteelWings = equipmentData.backData.steelWingsUpgrades;
                upgradesBackpacks = equipmentData.backData.backpackUpgrades;
                upgradesCapeOWinds = equipmentData.backData.capeOWindUpgrades;
                upgradesSeedBags = equipmentData.backData.seedBagUpgrades;

                loaded = true;
            }

            // Weapons
            if (!weaponUpdated)
            {
                UpdateWeapon(loadout.selectedWeapon.title);
                weaponUpdated = true;
            }

            // Companions
            if (!companionUpdated)
            {
                UpdateCompanion(loadout.selectedCompanion.title);
                companionUpdated = true;
            }

            // Armors
            if (!armorUpdated)
            {
                UpdateArmor(loadout.selectedArmor.title);
                armorUpdated = true;
            }

            // Backs
            if (!backUpdated)
            {
                UpdateBacks(loadout.selectedBack.title);
                backUpdated = true;
            }
        }
    }

    #endregion

    #region General Methods

    void UpdateWeapon(string weaponName)
    {
        var upgrades = weaponName switch
        {
            WeaponData.pugioString => upgradesPugio,
            WeaponData.ulfberhtString => upgradesUlfberht,

            _ => throw new System.Exception($"Weapon name: '{weaponName}' not recognized.")
        };

        // Attack Speed
        weapon.attackSpeedMultiplier -= activeAtSpeed;
        activeAtSpeed = upgrades.Where(x => x.name == WeaponData.attackSpeedUpgradeString).Count() * atSpeed;
        weapon.attackSpeedMultiplier += activeAtSpeed;

        // Damage
        weapon.damageMultiplier -= acitveDamage;
        acitveDamage = upgrades.Where(x => x.name == WeaponData.damageUpgradeString).Count() * damage;
        weapon.damageMultiplier += acitveDamage;

        // Special Attack
        if (upgrades.Where(x => x.name == WeaponData.specialAttackUpgradeString).Count() > 0) weapon.heavyAttack = true;
        else weapon.heavyAttack = false;

        // Special Cooldown
        weapon.heavyCooldownMultiplier -= activeSpCool;
        activeSpCool = upgrades.Where(x => x.name == WeaponData.specialCooldownUpgradeString).Count() * spCool;
        weapon.heavyCooldownMultiplier += activeSpCool;
    }

    void UpdateCompanion(string companionName)
    {
        var upgrades = companionName switch
        {
            CompanionData.loyalSphereString => upgradesLoyalSphere,
            CompanionData.attackSquareString => upgradesAttackSquare,

            _ => new List<UpgradeItemsSO>()
        };

        if (upgrades.Count > 0)
        {
            // Ability Rate
            companion.abilityRateMultiplier -= activeAbRate;

            activeAbRate = upgrades.Where(x => x.name == CompanionData.abilityRateUpgradeString).Count() * abRate;
            companion.abilityRateMultiplier += activeAbRate;
            
            // Ability Strength
            companion.abilityStrengthMultiplier -= activeAbStrength;

            activeAbStrength = upgrades.Where(x => x.name == CompanionData.abilityStrengthUpgradeString).Count() * abStrength;
            companion.abilityStrengthMultiplier += activeAbStrength;
        }
        else
        {
            // Ability Rate
            companion.abilityRateMultiplier -= activeAbRate;
            activeAbRate = 0f;

            // Ability Strength
            companion.abilityStrengthMultiplier -= activeAbStrength;
            activeAbStrength = 0f;
        }
    }

    void UpdateArmor(string armorName)
    {
        var upgrades = armorName switch
        {
            ArmorData.leatherString => upgradesLeather,
            ArmorData.hideString => upgradesHide,
            ArmorData.ringMailString => upgradesRingMail,
            ArmorData.plateString => upgradesPlate,

            _ => new List<UpgradeItemsSO>()
        };

        if (upgrades.Count > 0)
        {
            // Resistance
            armor.leather.resistanceMod -= activeResist;

            activeResist = upgrades.Where(x => x.name == ArmorData.resistanceUpgradeString).Count() * resist;
            armor.leather.resistanceMod += activeResist;

            // Speed
            armor.leather.speedMod -= activeSpeedPen;

            activeSpeedPen = upgrades.Where(x => x.name == ArmorData.speedUpgradeString).Count() * speedPen;
            armor.leather.speedMod += activeSpeedPen;
        }
        else
        {
            // Resistance
            armor.leather.resistanceMod -= activeResist;
            armor.leather.resistanceMod = 0f;

            // Speed
            armor.leather.speedMod -= activeSpeedPen;
            armor.leather.speedMod = 0f;
        }
    }

    void UpdateBacks(string backName)
    {
        var upgrades = backName switch
        {
            BackData.angelWingsString => upgradesAngelWings,
            BackData.steelWingsString => upgradesSteelWings,
            BackData.backpackString => upgradesBackpacks,
            BackData.capeOWindString => upgradesCapeOWinds,
            BackData.seedBagString => upgradesSeedBags,

            _ => new List<UpgradeItemsSO>()
        };

        if (upgrades.Count > 0)
        {
            // Cooldown
            backs.abilityCooldownMultiplier -= activeCool;

            activeCool = upgrades.Where(x => x.name == BackData.cooldownUpgradeString).Count() * cool;
            backs.abilityCooldownMultiplier += activeCool;
        }
        else
        {
            // Cooldown
            backs.abilityCooldownMultiplier -= activeCool;
            activeCool = 0f;
        }
    }

    public void AddUpgrade(UpgradeItemsSO upgrade, string equipment)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;

        var upgrades = equipment switch
        {
            WeaponData.pugioString => equipmentData.weaponData.pugioUpgrades,
            WeaponData.ulfberhtString => equipmentData.weaponData.ulfberhtUpgrades,
            CompanionData.loyalSphereString => equipmentData.companionData.loyalSphereUpgrades,
            CompanionData.attackSquareString => equipmentData.companionData.attackSquareUpgrades,
            ArmorData.leatherString => equipmentData.armorData.leatherUpgrades,
            ArmorData.hideString => equipmentData.armorData.hideUpgrades,
            ArmorData.ringMailString => equipmentData.armorData.ringMailUpgrades,
            ArmorData.plateString => equipmentData.armorData.plateUpgrades,
            BackData.angelWingsString => equipmentData.backData.angelWingsUpgrades,
            BackData.steelWingsString => equipmentData.backData.steelWingsUpgrades,
            BackData.backpackString => equipmentData.backData.backpackUpgrades,
            BackData.capeOWindString => equipmentData.backData.capeOWindUpgrades,
            BackData.seedBagString => equipmentData.backData.seedBagUpgrades,

            _ => throw new System.Exception($"Upgrade: '{upgrade}' was not recognized.")
        };
        upgrades.Add(upgrade);

        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);
        weaponUpdated = false;
    }

    #endregion
}
