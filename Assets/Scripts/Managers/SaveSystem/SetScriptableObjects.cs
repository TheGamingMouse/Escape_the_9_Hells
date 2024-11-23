using System.Collections.Generic;
using UnityEngine;
using static SaveSystemSpace.SaveClasses;

namespace SaveSystemSpace
{
    public class SetScriptableObjects : MonoBehaviour
    {
        [Header("Equipment - Weapons")]
        public LoadoutItemsSO pugio;
        public LoadoutItemsSO ulfberht;

        [Header("Equipment - Companions")]
        public LoadoutItemsSO unequipedCompanion;
        public LoadoutItemsSO loyalSphere;
        public LoadoutItemsSO attackSquare;

        [Header("Equipment - Armors")]
        public LoadoutItemsSO unequipedArmor;
        public LoadoutItemsSO leather;
        public LoadoutItemsSO hide;
        public LoadoutItemsSO ringMail;
        public LoadoutItemsSO plate;

        [Header("Equipment - Backs")]
        public LoadoutItemsSO unequipedBack;
        public LoadoutItemsSO angelWings;
        public LoadoutItemsSO steelWings;
        public LoadoutItemsSO backpack;
        public LoadoutItemsSO capeOWind;
        public LoadoutItemsSO seedBag;

        [Header("Upgrades - Weapons")]
        public UpgradeItemsSO attackSpeedUpgrade;
        public UpgradeItemsSO damageUpgrade;
        public UpgradeItemsSO specialAttackUpgrade;
        public UpgradeItemsSO specialCooldownUpgrade;

        [Header("Upgrades - Companions")]
        public UpgradeItemsSO abilityRateUpgrade;
        public UpgradeItemsSO abilityStrengthUpgrade;
        
        [Header("Upgrades - Armors")]
        public UpgradeItemsSO resistanceUpgrade;
        public UpgradeItemsSO speedUpgrade;

        [Header("Upgrades - Backs")]
        public UpgradeItemsSO cooldownUpgrade;

        [Header("Souls")]
        public SoulsItemsSO attackSpeedSoul;
        public SoulsItemsSO damageSoul;
        public SoulsItemsSO defenceSoul;
        public SoulsItemsSO luckSoul;
        public SoulsItemsSO movementSpeedSoul;
        public SoulsItemsSO pathFinderSoul;
        public SoulsItemsSO reRollSoul;
        public SoulsItemsSO startLevelSoul;

        [Header("Perks")]
        public PerkItemsSO attackSpeedPerk;
        public PerkItemsSO damagePerk;
        public PerkItemsSO defencePerk;
        public PerkItemsSO fireAuraPerk;
        public PerkItemsSO iceAuraPerk;
        public PerkItemsSO luckPerk;
        public PerkItemsSO movementSpeedPerk;
        public PerkItemsSO shieldPerk;

        public static SetScriptableObjects Instance;

        public readonly List<LoadoutItemsSO> weapons = new();
        public readonly List<LoadoutItemsSO> companions = new();
        public readonly List<LoadoutItemsSO> armors = new();
        public readonly List<LoadoutItemsSO> backs = new();

        public readonly List<UpgradeItemsSO> weaponUpgrades = new();
        public readonly List<UpgradeItemsSO> companionUpgrades = new();
        public readonly List<UpgradeItemsSO> armorUpgrades = new();
        public readonly List<UpgradeItemsSO> backUpgrades = new();

        public readonly List<SoulsItemsSO> souls = new();
        public readonly List<PerkItemsSO> perks = new();

        void Awake()
        {
            Instance = this;
            
            PopulateLists();
            UpdateTitles();
        }

        void PopulateLists()
        {
            weapons.AddRange(new List<LoadoutItemsSO>
            {
                pugio,
                ulfberht,
            });

            companions.AddRange(new List<LoadoutItemsSO>
            {
                unequipedCompanion,
                loyalSphere,
                attackSquare,
            });

            armors.AddRange(new List<LoadoutItemsSO>
            {
                unequipedArmor,
                leather,
                hide,
                ringMail,
                plate,
            });

            backs.AddRange(new List<LoadoutItemsSO>
            {
                unequipedBack,
                angelWings,
                steelWings,
                backpack,
                capeOWind,
                seedBag,
            });

            weaponUpgrades.AddRange(new List<UpgradeItemsSO>
            {
                attackSpeedUpgrade,
                damageUpgrade,
                specialAttackUpgrade,
                specialCooldownUpgrade,
            });

            companionUpgrades.AddRange(new List<UpgradeItemsSO>
            {
                abilityRateUpgrade,
                abilityStrengthUpgrade,
            });

            armorUpgrades.AddRange(new List<UpgradeItemsSO>
            {
                resistanceUpgrade,
                speedUpgrade,
            });

            backUpgrades.AddRange(new List<UpgradeItemsSO>
            {
                cooldownUpgrade,
            });

            souls.AddRange(new List<SoulsItemsSO>
            {
                attackSpeedSoul,
                damageSoul,
                luckSoul,
                movementSpeedSoul,
                pathFinderSoul,
                reRollSoul,
                startLevelSoul,
            });

            perks.AddRange(new List<PerkItemsSO>
            {
                attackSpeedPerk,
                damagePerk,
                defencePerk,
                fireAuraPerk,
                iceAuraPerk,
                luckPerk,
                movementSpeedPerk,
                shieldPerk,
            });
        }

        void UpdateTitles()
        {
            // Weapons
            pugio.title = EquipmentData.WeaponData.pugioString;
            ulfberht.title = EquipmentData.WeaponData.ulfberhtString;

            // Companions
            loyalSphere.title = EquipmentData.CompanionData.loyalSphereString;
            attackSquare.title = EquipmentData.CompanionData.attackSquareString;

            // Armors
            leather.title = EquipmentData.ArmorData.leatherString;
            hide.title = EquipmentData.ArmorData.hideString;
            ringMail.title = EquipmentData.ArmorData.ringMailString;
            plate.title = EquipmentData.ArmorData.plateString;

            // Backs
            angelWings.title = EquipmentData.BackData.angelWingsString;
            steelWings.title = EquipmentData.BackData.steelWingsString;
            backpack.title = EquipmentData.BackData.backpackString;
            capeOWind.title = EquipmentData.BackData.capeOWindString;
            seedBag.title = EquipmentData.BackData.seedBagString;

            // Weapon Upgrades
            attackSpeedUpgrade.title = EquipmentData.WeaponData.attackSpeedUpgradeString;
            damageUpgrade.title = EquipmentData.WeaponData.damageUpgradeString;
            specialAttackUpgrade.title = EquipmentData.WeaponData.specialAttackUpgradeString;
            specialCooldownUpgrade.title = EquipmentData.WeaponData.specialCooldownUpgradeString;

            // Companion Upgrades
            abilityRateUpgrade.title = EquipmentData.CompanionData.abilityRateUpgradeString;
            abilityStrengthUpgrade.title = EquipmentData.CompanionData.abilityStrengthUpgradeString;

            // Armor Upgrades
            resistanceUpgrade.title = EquipmentData.ArmorData.resistanceUpgradeString;
            speedUpgrade.title = EquipmentData.ArmorData.speedUpgradeString;

            // Back Upgrades
            cooldownUpgrade.title = EquipmentData.BackData.cooldownUpgradeString;

            // Souls
            attackSpeedSoul.title = SoulData.attackSpeedString;
            damageSoul.title = SoulData.damageString;
            defenceSoul.title = SoulData.defenceString;
            luckSoul.title = SoulData.luckString;
            movementSpeedSoul.title = SoulData.movementSpeedString;
            pathFinderSoul.title = SoulData.pathFinderString;
            reRollSoul.title = SoulData.reRollString;
            startLevelSoul.title = SoulData.startLevelString;

            // Perks
            attackSpeedPerk.title = PerkData.attackSpeedString;
            damagePerk.title = PerkData.damageString;
            defencePerk.title = PerkData.defenceString;
            fireAuraPerk.title = PerkData.fireAuraString;
            iceAuraPerk.title = PerkData.iceAuraString;
            luckPerk.title = PerkData.luckString;
            movementSpeedPerk.title = PerkData.movementSpeedString;
            shieldPerk.title = PerkData.shieldString;
        }
    }
}
