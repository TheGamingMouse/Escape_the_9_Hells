using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public List<UpgradeItemsSO> upgradesWeapon1 = new();
    public List<UpgradeItemsSO> upgradesWeapon2 = new();
    public List<UpgradeItemsSO> upgradesWeapon3 = new();
    public List<UpgradeItemsSO> upgradesWeapon4 = new();

    // Companions
    public List<UpgradeItemsSO> upgradesLoyalSphere = new();
    public List<UpgradeItemsSO> upgradesAttackSquare = new();
    public List<UpgradeItemsSO> upgradesCompanion1 = new();
    public List<UpgradeItemsSO> upgradesCompanion2 = new();
    public List<UpgradeItemsSO> upgradesCompanion3 = new();
    public List<UpgradeItemsSO> upgradesCompanion4 = new();

    // Armors
    public List<UpgradeItemsSO> upgradesLeather = new();
    public List<UpgradeItemsSO> upgradesHide = new();
    public List<UpgradeItemsSO> upgradesRingMail = new();
    public List<UpgradeItemsSO> upgradesPlate = new();
    public List<UpgradeItemsSO> upgradesArmor1 = new();
    public List<UpgradeItemsSO> upgradesArmor2 = new();

    // Backs
    public List<UpgradeItemsSO> upgradesAngelWings = new();
    public List<UpgradeItemsSO> upgradesSteelWings = new();
    public List<UpgradeItemsSO> upgradesBackpacks = new();
    public List<UpgradeItemsSO> upgradesCapeOWinds = new();
    public List<UpgradeItemsSO> upgradesSeedBag = new();
    public List<UpgradeItemsSO> upgradesBacks2 = new();

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
                upgradesSeedBag = equipmentData.backData.seedBagUpgrades;

                loaded = true;
            }

            // Weapons
            if (!weaponUpdated)
            {
                if (loadout.selectedWeapon.title.ToLower() == "pugio")
                {
                    // Attack Speed
                    weapon.attackSpeedMultiplier -= activeAtSpeed;

                    activeAtSpeed = upgradesPugio.Where(x => x.name == "Attack Speed Upgrade").Count() * atSpeed;
                    weapon.attackSpeedMultiplier += activeAtSpeed;

                    // Damage
                    weapon.damageMultiplier -= acitveDamage;

                    acitveDamage = upgradesPugio.Where(x => x.name == "Damage Upgrade").Count() * damage;
                    weapon.damageMultiplier += acitveDamage;

                    // Special Attack
                    if (upgradesPugio.Where(x => x.name == "Special Attack").Count() > 0)
                    {
                        weapon.specialAttack = true;
                    }
                    else
                    {
                        weapon.specialAttack = false;
                    }

                    // Special Cooldown
                    weapon.specialCooldownMultiplier -= activeSpCool;

                    activeSpCool = upgradesPugio.Where(x => x.name == "Special Cooldown Upgrade").Count() * spCool;
                    weapon.specialCooldownMultiplier += activeSpCool;
                }
                else if (loadout.selectedWeapon.title.ToLower() == "ulfberht")
                {
                    // Attack Speed
                    weapon.attackSpeedMultiplier -= activeAtSpeed;

                    activeAtSpeed = upgradesUlfberht.Where(x => x.name == "Attack Speed Upgrade").Count() * atSpeed;
                    weapon.attackSpeedMultiplier += activeAtSpeed;

                    // Damage
                    weapon.damageMultiplier -= acitveDamage;

                    acitveDamage = upgradesUlfberht.Where(x => x.name == "Damage Upgrade").Count() * damage;
                    weapon.damageMultiplier += acitveDamage;

                    // Special Attack
                    if (upgradesUlfberht.Where(x => x.name == "Special Attack").Count() > 0)
                    {
                        weapon.specialAttack = true;
                    }
                    else
                    {
                        weapon.specialAttack = false;
                    }

                    // Special Cooldown
                    weapon.specialCooldownMultiplier -= activeSpCool;

                    activeSpCool = upgradesUlfberht.Where(x => x.name == "Special Cooldown Upgrade").Count() * spCool;
                    weapon.specialCooldownMultiplier += activeSpCool;
                }
                else if (loadout.selectedWeapon.title.ToLower() == "weapon1")
                {
                    // Attack Speed
                    weapon.attackSpeedMultiplier -= activeAtSpeed;

                    activeAtSpeed = upgradesWeapon1.Where(x => x.name == "Attack Speed Upgrade").Count() * atSpeed;
                    weapon.attackSpeedMultiplier += activeAtSpeed;

                    // Damage
                    weapon.damageMultiplier -= acitveDamage;

                    acitveDamage = upgradesWeapon1.Where(x => x.name == "Damage Upgrade").Count() * damage;
                    weapon.damageMultiplier += acitveDamage;

                    // Special Attack
                    if (upgradesWeapon1.Where(x => x.name == "Special Attack").Count() > 0)
                    {
                        weapon.specialAttack = true;
                    }
                    else
                    {
                        weapon.specialAttack = false;
                    }

                    // Special Cooldown
                    weapon.specialCooldownMultiplier -= activeSpCool;

                    activeSpCool = upgradesWeapon1.Where(x => x.name == "Special Cooldown Upgrade").Count() * spCool;
                    weapon.specialCooldownMultiplier += activeSpCool;
                }
                else if (loadout.selectedWeapon.title.ToLower() == "weapon2")
                {
                    // Attack Speed
                    weapon.attackSpeedMultiplier -= activeAtSpeed;

                    activeAtSpeed = upgradesWeapon2.Where(x => x.name == "Attack Speed Upgrade").Count() * atSpeed;
                    weapon.attackSpeedMultiplier += activeAtSpeed;

                    // Damage
                    weapon.damageMultiplier -= acitveDamage;

                    acitveDamage = upgradesWeapon2.Where(x => x.name == "Damage Upgrade").Count() * damage;
                    weapon.damageMultiplier += acitveDamage;

                    // Special Attack
                    if (upgradesWeapon2.Where(x => x.name == "Special Cooldown").Count() > 0)
                    {
                        weapon.specialAttack = true;
                    }
                    else
                    {
                        weapon.specialAttack = false;
                    }

                    // Special Cooldown
                    weapon.specialCooldownMultiplier -= activeSpCool;

                    activeSpCool = upgradesWeapon2.Where(x => x.name == "Special Cooldown Upgrade").Count() * spCool;
                    weapon.specialCooldownMultiplier += activeSpCool;
                }
                else if (loadout.selectedWeapon.title.ToLower() == "weapon3")
                {
                    // Attack Speed
                    weapon.attackSpeedMultiplier -= activeAtSpeed;

                    activeAtSpeed = upgradesWeapon3.Where(x => x.name == "Attack Speed Upgrade").Count() * atSpeed;
                    weapon.attackSpeedMultiplier += activeAtSpeed;

                    // Damage
                    weapon.damageMultiplier -= acitveDamage;

                    acitveDamage = upgradesWeapon3.Where(x => x.name == "Damage Upgrade").Count() * damage;
                    weapon.damageMultiplier += acitveDamage;

                    // Special Attack
                    if (upgradesWeapon3.Where(x => x.name == "Special Attack").Count() > 0)
                    {
                        weapon.specialAttack = true;
                    }
                    else
                    {
                        weapon.specialAttack = false;
                    }

                    // Special Cooldown
                    weapon.specialCooldownMultiplier -= activeSpCool;

                    activeSpCool = upgradesWeapon3.Where(x => x.name == "Special Cooldown Upgrade").Count() * spCool;
                    weapon.specialCooldownMultiplier += activeSpCool;
                }
                else if (loadout.selectedWeapon.title.ToLower() == "weapon4")
                {
                    // Attack Speed
                    weapon.attackSpeedMultiplier -= activeAtSpeed;

                    activeAtSpeed = upgradesWeapon4.Where(x => x.name == "Attack Speed Upgrade").Count() * atSpeed;
                    weapon.attackSpeedMultiplier += activeAtSpeed;

                    // Damage
                    weapon.damageMultiplier -= acitveDamage;

                    acitveDamage = upgradesWeapon4.Where(x => x.name == "Damage Upgrade").Count() * damage;
                    weapon.damageMultiplier += acitveDamage;

                    // Special Attack
                    if (upgradesWeapon4.Where(x => x.name == "Special Attack").Count() > 0)
                    {
                        weapon.specialAttack = true;
                    }
                    else
                    {
                        weapon.specialAttack = false;
                    }

                    // Special Cooldown
                    weapon.specialCooldownMultiplier -= activeSpCool;

                    activeSpCool = upgradesWeapon4.Where(x => x.name == "Special Cooldown Upgrade").Count() * spCool;
                    weapon.specialCooldownMultiplier += activeSpCool;
                }
                else
                {
                    // Attack Speed
                    weapon.attackSpeedMultiplier -= activeAtSpeed;
                    activeAtSpeed = 0f;

                    // Damage
                    weapon.damageMultiplier -= acitveDamage;
                    acitveDamage = 0f;

                    // Special Attack
                    weapon.specialAttack = false;

                    // Special Cooldown
                    weapon.specialCooldownMultiplier -= activeSpCool;
                    activeSpCool = 0f;

                }

                weaponUpdated = true;
            }

            // Companions
            if (!companionUpdated)
            {
                if (loadout.selectedCompanion.title.ToLower() == "loyal sphere")
                {
                    // Ability Rate
                    companion.abilityRateMultiplier -= activeAbRate;

                    activeAbRate = upgradesLoyalSphere.Where(x => x.name == "Ability Rate Upgrade").Count() * abRate;
                    companion.abilityRateMultiplier += activeAbRate;
                    
                    // Ability Strength
                    companion.abilityStrengthMultiplier -= activeAbStrength;

                    activeAbStrength = upgradesLoyalSphere.Where(x => x.name == "Ability Strength Upgrade").Count() * abStrength;
                    companion.abilityStrengthMultiplier += activeAbStrength;
                    
                }
                else if (loadout.selectedCompanion.title.ToLower() == "attack square")
                {
                    // Ability Rate
                    companion.abilityRateMultiplier -= activeAbRate;

                    activeAbRate = upgradesAttackSquare.Where(x => x.name == "Ability Rate Upgrade").Count() * abRate;
                    companion.abilityRateMultiplier += activeAbRate;
                    
                    // Ability Strength
                    companion.abilityStrengthMultiplier -= activeAbStrength;

                    activeAbStrength = upgradesAttackSquare.Where(x => x.name == "Ability Strength Upgrade").Count() * abStrength;
                    companion.abilityStrengthMultiplier += activeAbStrength;
                }
                else if (loadout.selectedCompanion.title.ToLower() == "companion1")
                {
                    // Ability Rate
                    companion.abilityRateMultiplier -= activeAbRate;

                    activeAbRate = upgradesCompanion1.Where(x => x.name == "Ability Rate Upgrade").Count() * abRate;
                    companion.abilityRateMultiplier += activeAbRate;
                    
                    // Ability Strength
                    companion.abilityStrengthMultiplier -= activeAbStrength;

                    activeAbStrength = upgradesCompanion1.Where(x => x.name == "Ability Strength Upgrade").Count() * abStrength;
                    companion.abilityStrengthMultiplier += activeAbStrength;
                }
                else if (loadout.selectedCompanion.title.ToLower() == "companion2")
                {
                    // Ability Rate
                    companion.abilityRateMultiplier -= activeAbRate;

                    activeAbRate = upgradesCompanion2.Where(x => x.name == "Ability Rate Upgrade").Count() * abRate;
                    companion.abilityRateMultiplier += activeAbRate;
                    
                    // Ability Strength
                    companion.abilityStrengthMultiplier -= activeAbStrength;

                    activeAbStrength = upgradesCompanion2.Where(x => x.name == "Ability Strength Upgrade").Count() * abStrength;
                    companion.abilityStrengthMultiplier += activeAbStrength;
                }
                else if (loadout.selectedCompanion.title.ToLower() == "companion3")
                {
                    // Ability Rate
                    companion.abilityRateMultiplier -= activeAbRate;

                    activeAbRate = upgradesCompanion3.Where(x => x.name == "Ability Rate Upgrade").Count() * abRate;
                    companion.abilityRateMultiplier += activeAbRate;
                    
                    // Ability Strength
                    companion.abilityStrengthMultiplier -= activeAbStrength;

                    activeAbStrength = upgradesCompanion3.Where(x => x.name == "Ability Strength Upgrade").Count() * abStrength;
                    companion.abilityStrengthMultiplier += activeAbStrength;
                }
                else if (loadout.selectedCompanion.title.ToLower() == "companion4")
                {
                    // Ability Rate
                    companion.abilityRateMultiplier -= activeAbRate;

                    activeAbRate = upgradesCompanion4.Where(x => x.name == "Ability Rate Upgrade").Count() * abRate;
                    companion.abilityRateMultiplier += activeAbRate;
                    
                    // Ability Strength
                    companion.abilityStrengthMultiplier -= activeAbStrength;

                    activeAbStrength = upgradesCompanion4.Where(x => x.name == "Ability Strength Upgrade").Count() * abStrength;
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

                companionUpdated = true;
            }

            // Armors
            if (!armorUpdated)
            {
                if (loadout.selectedArmor.title.ToLower() == "leather armor")
                {
                    // Resistance
                    armor.leather.resistanceMod -= activeResist;

                    activeResist = upgradesLeather.Where(x => x.name == "Resistance Upgrade").Count() * resist;
                    armor.leather.resistanceMod += activeResist;

                    // Speed
                    armor.leather.speedMod -= activeSpeedPen;

                    activeSpeedPen = upgradesLeather.Where(x => x.name == "Speed Upgrade").Count() * speedPen;
                    armor.leather.speedMod += activeSpeedPen;
                }
                else if (loadout.selectedArmor.title.ToLower() == "hide armor")
                {
                    // Resistance
                    armor.leather.resistanceMod -= activeResist;

                    activeResist = upgradesLeather.Where(x => x.name == "Resistance Upgrade").Count() * resist;
                    armor.leather.resistanceMod += activeResist;

                    // Speed
                    armor.leather.speedMod -= activeSpeedPen;

                    activeSpeedPen = upgradesLeather.Where(x => x.name == "Speed Upgrade").Count() * speedPen;
                    armor.leather.speedMod += activeSpeedPen;
                }
                else if (loadout.selectedArmor.title.ToLower() == "ring mail armor")
                {
                    // Resistance
                    armor.leather.resistanceMod -= activeResist;

                    activeResist = upgradesLeather.Where(x => x.name == "Resistance Upgrade").Count() * resist;
                    armor.leather.resistanceMod += activeResist;

                    // Speed
                    armor.leather.speedMod -= activeSpeedPen;

                    activeSpeedPen = upgradesLeather.Where(x => x.name == "Speed Upgrade").Count() * speedPen;
                    armor.leather.speedMod += activeSpeedPen;
                }
                else if (loadout.selectedArmor.title.ToLower() == "plate armor")
                {
                    // Resistance
                    armor.leather.resistanceMod -= activeResist;

                    activeResist = upgradesLeather.Where(x => x.name == "Resistance Upgrade").Count() * resist;
                    armor.leather.resistanceMod += activeResist;

                    // Speed
                    armor.leather.speedMod -= activeSpeedPen;

                    activeSpeedPen = upgradesLeather.Where(x => x.name == "Speed Upgrade").Count() * speedPen;
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

                armorUpdated = true;
            }

            // Backs
            if (!backUpdated)
            {
                if (loadout.selectedBack.title.ToLower() == "angel wings")
                {
                    // Cooldown
                    backs.abilityCooldownMultiplier -= activeCool;

                    activeCool = upgradesAngelWings.Where(x => x.name == "Cooldown").Count() * cool;
                    backs.abilityCooldownMultiplier += activeCool;
                }
                else if (loadout.selectedBack.title.ToLower() == "steel wings")
                {
                    // Cooldown
                    backs.abilityCooldownMultiplier -= activeCool;

                    activeCool = upgradesSteelWings.Where(x => x.name == "Cooldown").Count() * cool;
                    backs.abilityCooldownMultiplier += activeCool;
                }
                else if (loadout.selectedBack.title.ToLower() == "backpack")
                {
                    // Cooldown
                    backs.abilityCooldownMultiplier -= activeCool;

                    activeCool = upgradesBackpacks.Where(x => x.name == "Cooldown").Count() * cool;
                    backs.abilityCooldownMultiplier += activeCool;
                }
                else if (loadout.selectedBack.title.ToLower() == "cape o' wind")
                {
                    // Cooldown
                    backs.abilityCooldownMultiplier -= activeCool;

                    activeCool = upgradesCapeOWinds.Where(x => x.name == "Cooldown").Count() * cool;
                    backs.abilityCooldownMultiplier += activeCool;
                }
                else if (loadout.selectedBack.title.ToLower() == "seed bag")
                {
                    // Cooldown
                    backs.abilityCooldownMultiplier -= activeCool;

                    activeCool = upgradesSeedBag.Where(x => x.name == "Cooldown").Count() * cool;
                    backs.abilityCooldownMultiplier += activeCool;
                }
                else
                {
                    // Cooldown
                    backs.abilityCooldownMultiplier -= activeCool;
                    activeCool = 0f;
                }

                backUpdated = true;
            }
        }
    }

    #endregion

    #region Weapon Methods

    public void AddPugioUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.weaponData.pugioUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    public void AddUlfberhtUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.weaponData.ulfberhtUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    #endregion

    #region Companion Methods

    public void AddLoyalSphereUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.companionData.loyalSphereUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    public void AddAttackSquareUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.companionData.attackSquareUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    #endregion

    #region Armor Methods

    public void AddLeatherUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.armorData.leatherUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    public void AddHideUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.armorData.hideUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    public void AddRingMailUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.armorData.ringMailUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    public void AddPlateUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.armorData.plateUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    #endregion

    #region Back Methods

    public void AddAngelWingsUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.backData.angelWingsUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    public void AddSteelWingsUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.backData.steelWingsUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    public void AddBackpackUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.backData.backpackUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    public void AddCapeOWindUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.backData.capeOWindUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    public void AddSeedBagUpgrade(UpgradeItemsSO upgrade)
    {
        var equipmentData = SaveSystem.loadedEquipmentData;
        equipmentData.backData.seedBagUpgrades.Add(upgrade);
        
        SaveSystem.Instance.Save(equipmentData, SaveSystem.equipmentDataPath);

        weaponUpdated = false;
    }

    #endregion
}
