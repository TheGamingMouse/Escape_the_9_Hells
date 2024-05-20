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
    public List<UpgradeItemsSO> upgradesBacks1 = new();
    public List<UpgradeItemsSO> upgradesBacks2 = new();

    [Header("Components")]
    Weapon weapon;
    Companion companion;
    Armor armor;
    Backs backs;
    PlayerLoadout loadout;
    SaveLoadManager slManager;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        weapon = GetComponentInChildren<Weapon>();
        armor = GetComponentInChildren<Armor>();
        backs = GetComponentInChildren<Backs>();
        companion = GameObject.FindWithTag("Companions").GetComponent<Companion>();

        loadout = GetComponent<PlayerLoadout>();
        slManager = GameObject.FindWithTag("Managers").GetComponent<SaveLoadManager>();
    }

    void Update()
    {
        if (loadout.start)
        {
            if (!loaded)
            {
                // Weapons
                upgradesPugio = slManager.pugioUpgrades;
                upgradesUlfberht = slManager.ulfberhtUpgrades;

                // Companions
                upgradesLoyalSphere = slManager.loyalSphereUpgrades;
                upgradesAttackSquare = slManager.attackSquareUpgrades;

                // Armors
                upgradesLeather = slManager.leatherUpgrades;
                upgradesHide = slManager.hideUpgrades;
                upgradesRingMail = slManager.ringMailUpgrades;
                upgradesPlate = slManager.plateUpgrades;

                // Backs
                upgradesAngelWings = slManager.angelWingsUpgrades;
                upgradesSteelWings = slManager.steelWingsUpgrades;
                upgradesBackpacks = slManager.backpackUpgrades;
                upgradesCapeOWinds = slManager.capeOWindUpgrades;

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
        upgradesPugio.Add(upgrade);

        weaponUpdated = false;
    }

    public void AddUlfberhtUpgrade(UpgradeItemsSO upgrade)
    {
        upgradesUlfberht.Add(upgrade);

        weaponUpdated = false;
    }
    
    public void AddWeapon1Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesWeapon1.Add(upgrade);

        weaponUpdated = false;
    }
    
    public void AddWeapon2Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesWeapon2.Add(upgrade);

        weaponUpdated = false;
    }
    
    public void AddWeapon3Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesWeapon3.Add(upgrade);

        weaponUpdated = false;
    }
    
    public void AddWeapon4Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesWeapon4.Add(upgrade);

        weaponUpdated = false;
    }

    #endregion

    #region Companion Methods

    public void AddLoyalSphereUpgrade(UpgradeItemsSO upgrade)
    {
        upgradesLoyalSphere.Add(upgrade);

        companionUpdated = false;
    }

    public void AddAttackSquareUpgrade(UpgradeItemsSO upgrade)
    {
        upgradesAttackSquare.Add(upgrade);

        companionUpdated = false;
    }

    public void AddCompanion1Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesCompanion1.Add(upgrade);

        companionUpdated = false;
    }

    public void AddCompanion2Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesCompanion2.Add(upgrade);

        companionUpdated = false;
    }

    public void AddCompanion3Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesCompanion3.Add(upgrade);

        companionUpdated = false;
    }

    public void AddCompanion4Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesCompanion4.Add(upgrade);

        companionUpdated = false;
    }

    #endregion

    #region Armor Methods

    public void AddLeatherUpgrade(UpgradeItemsSO upgrade)
    {
        upgradesLeather.Add(upgrade);

        armorUpdated = false;
    }

    public void AddHideUpgrade(UpgradeItemsSO upgrade)
    {
        upgradesHide.Add(upgrade);

        armorUpdated = false;
    }

    public void AddRingMailUpgrade(UpgradeItemsSO upgrade)
    {
        upgradesRingMail.Add(upgrade);

        armorUpdated = false;
    }

    public void AddPlateUpgrade(UpgradeItemsSO upgrade)
    {
        upgradesPlate.Add(upgrade);

        armorUpdated = false;
    }

    public void AddArmor1Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesArmor1.Add(upgrade);

        armorUpdated = false;
    }

    public void AddArmor2Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesArmor2.Add(upgrade);

        armorUpdated = false;
    }

    #endregion

    #region Back Methods

    public void AddAngelWingsUpgrade(UpgradeItemsSO upgrade)
    {
        upgradesAngelWings.Add(upgrade);

        backUpdated = false;
    }

    public void AddSteelWingsUpgrade(UpgradeItemsSO upgrade)
    {
        upgradesSteelWings.Add(upgrade);

        backUpdated = false;
    }

    public void AddBackpackUpgrade(UpgradeItemsSO upgrade)
    {
        upgradesBackpacks.Add(upgrade);

        backUpdated = false;
    }

    public void AddCapeOWindUpgrade(UpgradeItemsSO upgrade)
    {
        upgradesCapeOWinds.Add(upgrade);

        backUpdated = false;
    }

    public void AddBack1Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesBacks1.Add(upgrade);

        backUpdated = false;
    }

    public void AddBack2Upgrade(UpgradeItemsSO upgrade)
    {
        upgradesBacks2.Add(upgrade);

        backUpdated = false;
    }

    #endregion
}
