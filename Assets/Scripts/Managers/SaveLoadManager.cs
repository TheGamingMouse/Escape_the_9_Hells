using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour, IDataPersistence
{
    #region Variables
    
    [Header("Ints")]
    public int layerReached;

    [Header("Bools")]
    public bool rickyStartComp;
    public bool hub;
    public bool mainMenu;
    public bool ready;

    [Header("Lists")]
    public List<LoadoutItemsSO> setWeapons;
    public List<LoadoutItemsSO> setCompanions;
    public List<LoadoutItemsSO> setArmors;
    public List<LoadoutItemsSO> setBacks;

    public List<SoulsItemsSO> setSoulItems;

    public List<UpgradeItemsSO> setWeaponUpgrades;
    public List<UpgradeItemsSO> setCompanionUpgrades;
    public List<UpgradeItemsSO> setArmorUpgrades;
    public List<UpgradeItemsSO> setBackUpgrades;

    public List<LoadoutItemsSO> boughtWeapons = new();
    public List<LoadoutItemsSO> boughtCompanions = new();
    public List<LoadoutItemsSO> boughtArmors = new();
    public List<LoadoutItemsSO> boughtBacks = new();

    public List<SoulsItemsSO> attackSpeedSoulsBought = new();
    public List<SoulsItemsSO> damageSoulsBought = new();
    public List<SoulsItemsSO> defenceSoulsBought = new();
    public List<SoulsItemsSO> movementSpeedSoulsBought = new();
    
    public List<UpgradeItemsSO> pugioUpgrades = new();
    public List<UpgradeItemsSO> ulfberhtUpgrades = new();
    public List<UpgradeItemsSO> loyalSphereUpgrades = new();
    public List<UpgradeItemsSO> attackSquareUpgrades = new();
    public List<UpgradeItemsSO> leatherUpgrades = new();
    public List<UpgradeItemsSO> hideUpgrades = new();
    public List<UpgradeItemsSO> ringMailUpgrades = new();
    public List<UpgradeItemsSO> plateUpgrades = new();
    public List<UpgradeItemsSO> angelWingsUpgrades = new();
    public List<UpgradeItemsSO> steelWingsUpgrades = new();
    public List<UpgradeItemsSO> backpackUpgrades = new();
    public List<UpgradeItemsSO> capeOWindUpgrades = new();

    [Header("LoadoutItemsSOs")]
    public LoadoutItemsSO weapon;
    public LoadoutItemsSO companion;
    public LoadoutItemsSO armor;
    public LoadoutItemsSO back;

    [Header("Components")]
    PlayerLevel playerLevel;
    ExpSoulsManager expSoulsManager;
    PlayerLoadout playerLoadout;
    PlayerEquipment playerEquipment;
    PlayerSouls playerSouls;
    PlayerUpgrades playerUpgrades;
    
    #endregion

    #region StartUpdate Methods

    void Start()
    {
        var player = GameObject.FindWithTag("Player");

        playerLevel = player.GetComponent<PlayerLevel>();
        playerLoadout = player.GetComponent<PlayerLoadout>();
        playerEquipment = player.GetComponent<PlayerEquipment>();
        playerSouls = player.GetComponent<PlayerSouls>();
        playerUpgrades = player.GetComponent<PlayerUpgrades>();

        if (CheckLayer() == -2 && SceneManager.GetActiveScene().buildIndex < 1)
        {
            Debug.LogWarning("Error occured when checking layer");
        }
        else
        {
            layerReached = CheckLayer();
        }
    }

    #endregion

    #region General Methods

    int CheckLayer()
    {
        string layer = SceneManager.GetActiveScene().name.ToLower();
        if (layer == "main menu")
        {
            return -1;
        }
        else if (layer == "hub")
        {
            return 0;
        }
        else if (layer == "9th layer")
        {
            return 1;
        }
        else if (layer == "8th layer")
        {
            return 2;
        }
        else if (layer == "7th layer")
        {
            return 3;
        }
        else if (layer == "6th layer")
        {
            return 4;
        }
        else if (layer == "5th layer")
        {
            return 5;
        }
        else if (layer == "4th layer")
        {
            return 6;
        }
        else if (layer == "3rd layer")
        {
            return 7;
        }
        else if (layer == "2nd layer")
        {
            return 8;
        }
        else if (layer == "1st layer")
        {
            return 9;
        }
        else if (layer == "void layer")
        {
            return 10;
        }
        else if (layer == "golden gates")
        {
            return 11;
        }
        return -2;
    }

    #endregion

    #region SaveLoad Methods

    public void LoadData(GameData data)
    {
        if (!mainMenu)
        {
            // NPC variables
            rickyStartComp = data.rickyStartComp;

            // Souls
            if (hub)
            {
                expSoulsManager = GameObject.FindWithTag("Managers").GetComponent<ExpSoulsManager>();
                expSoulsManager.AddSouls(data.souls, true);
            }

            // Selected Equipment
            foreach (var weaponSelect in setWeapons)
            {
                if (weaponSelect.title == data.weapon)
                {
                    weapon = weaponSelect;
                }
            }
            foreach (var companionSelect in setCompanions)
            {
                if (companionSelect.title == data.companion)
                {
                    companion = companionSelect;
                }
            }
            foreach (var armorSelect in setArmors)
            {
                if (armorSelect.title == data.armor)
                {
                    armor = armorSelect;
                }
            }
            foreach (var backSelect in setBacks)
            {
                if (backSelect.title == data.back)
                {
                    back = backSelect;
                }
            }

            // Bought Equipment
            boughtWeapons.Clear();
            foreach (var setWeaponsBought in setWeapons)
            {
                foreach (var dataWeapon in data.boughtWeapons)
                {
                    if (setWeaponsBought.title == dataWeapon)
                    {
                        boughtWeapons.Add(setWeaponsBought);
                    }
                }
            }
            boughtCompanions.Clear();
            foreach (var setCompanionsBought in setCompanions)
            {
                foreach (var dataCompanion in data.boughtCompanions)
                {
                    if (setCompanionsBought.title == dataCompanion)
                    {
                        boughtCompanions.Add(setCompanionsBought);
                    }
                }
            }
            boughtArmors.Clear();
            foreach (var setArmorsBought in setArmors)
            {
                foreach (var dataArmor in data.boughtArmors)
                {
                    if (setArmorsBought.title == dataArmor)
                    {
                        boughtArmors.Add(setArmorsBought);
                    }
                }
            }
            boughtBacks.Clear();
            foreach (var setBacksBought in setBacks)
            {
                foreach (var dataBack in data.boughtBacks)
                {
                    if (setBacksBought.title == dataBack)
                    {
                        boughtBacks.Add(setBacksBought);
                    }
                }
            }

            // Bought Souls
            SoulsItemsSO attackSpeedSoul = null;
            SoulsItemsSO damageSoul = null;
            SoulsItemsSO defenceSoul = null;
            SoulsItemsSO movementSpeedSoul = null;

            foreach (var soul in setSoulItems)
            {
                if (soul.title == "Attack Speed Soul")
                {
                    attackSpeedSoul = soul;
                }
                else if (soul.title == "Damage Soul")
                {
                    damageSoul = soul;
                }
                else if (soul.title == "Defence Soul")
                {
                    defenceSoul = soul;
                }
                else if (soul.title == "Movement Speed Soul")
                {
                    movementSpeedSoul = soul;
                }
            }

            attackSpeedSoulsBought.Clear();
            for (int i = 0; i < data.attackSpeedSoulsBought; i++)
            {
                attackSpeedSoulsBought.Add(attackSpeedSoul);
            }
            damageSoulsBought.Clear();
            for (int i = 0; i < data.damageSoulsBought; i++)
            {
                damageSoulsBought.Add(damageSoul);
            }
            defenceSoulsBought.Clear();
            for (int i = 0; i < data.defenceSoulsBought; i++)
            {
                defenceSoulsBought.Add(defenceSoul);
            }
            movementSpeedSoulsBought.Clear();
            for (int i = 0; i < data.movementSpeedSoulsBought; i++)
            {
                movementSpeedSoulsBought.Add(movementSpeedSoul);
            }

            // Bought Upgrades
            // Weapons
            pugioUpgrades.Clear();
            foreach (var pugio in setWeaponUpgrades)
            {
                foreach (var dataPugio in data.pugioUpgrades)
                {
                    if (pugio.title == dataPugio)
                    {
                        pugioUpgrades.Add(pugio);
                    }
                }
            }
            ulfberhtUpgrades.Clear();
            foreach (var ulfberht in setWeaponUpgrades)
            {
                foreach (var dataUlfberht in data.ulfberhtUpgrades)
                {
                    if (ulfberht.title == dataUlfberht)
                    {
                        ulfberhtUpgrades.Add(ulfberht);
                    }
                }
            }

            // Companions
            loyalSphereUpgrades.Clear();
            foreach (var loyalSphere in setCompanionUpgrades)
            {
                foreach (var dataLoyalSphere in data.loyalSphereUpgrades)
                {
                    if (loyalSphere.title == dataLoyalSphere)
                    {
                        loyalSphereUpgrades.Add(loyalSphere);
                    }
                }
            }
            attackSquareUpgrades.Clear();
            foreach (var attackSquare in setCompanionUpgrades)
            {
                foreach (var dataAttackSquare in data.attackSquareUpgrades)
                {
                    if (attackSquare.title == dataAttackSquare)
                    {
                        attackSquareUpgrades.Add(attackSquare);
                    }
                }
            }

            // Armors
            leatherUpgrades.Clear();
            foreach (var leather in setArmorUpgrades)
            {
                foreach (var dataLeather in data.leatherUpgrades)
                {
                    if (leather.title == dataLeather)
                    {
                        leatherUpgrades.Add(leather);
                    }
                }
            }hideUpgrades.Clear();
            foreach (var hide in setArmorUpgrades)
            {
                foreach (var dataHide in data.hideUpgrades)
                {
                    if (hide.title == dataHide)
                    {
                        hideUpgrades.Add(hide);
                    }
                }
            }
            ringMailUpgrades.Clear();
            foreach (var ringMail in setArmorUpgrades)
            {
                foreach (var dataRingMail in data.ringMailUpgrades)
                {
                    if (ringMail.title == dataRingMail)
                    {
                        ringMailUpgrades.Add(ringMail);
                    }
                }
            }
            plateUpgrades.Clear();
            foreach (var plate in setArmorUpgrades)
            {
                foreach (var dataPlate in data.plateUpgrades)
                {
                    if (plate.title == dataPlate)
                    {
                        plateUpgrades.Add(plate);
                    }
                }
            }

            // Backs
            angelWingsUpgrades.Clear();
            foreach (var angelWings in setBackUpgrades)
            {
                foreach (var dataAngelWings in data.angelWingsUpgrades)
                {
                    if (angelWings.title == dataAngelWings)
                    {
                        angelWingsUpgrades.Add(angelWings);
                    }
                }
            }
            steelWingsUpgrades.Clear();
            foreach (var steelWing in setBackUpgrades)
            {
                foreach (var dataSteelWing in data.steelWingsUpgrades)
                {
                    if (steelWing.title == dataSteelWing)
                    {
                        steelWingsUpgrades.Add(steelWing);
                    }
                }
            }
            backpackUpgrades.Clear();
            foreach (var backpack in setBackUpgrades)
            {
                foreach (var dataBackpack in data.backpackUpgrades)
                {
                    if (backpack.title == dataBackpack)
                    {
                        backpackUpgrades.Add(backpack);
                    }
                }
            }
            capeOWindUpgrades.Clear();
            foreach (var capeOWind in setBackUpgrades)
            {
                foreach (var dataCapeOWind in data.capeOWindUpgrades)
                {
                    if (capeOWind.title == dataCapeOWind)
                    {
                        capeOWindUpgrades.Add(capeOWind);
                    }
                }
            }
        }
        ready = true;
    }

    public void SaveData(ref GameData data)
    {
        if (!mainMenu)
        {
            // NPC Variables
            data.rickyStartComp = rickyStartComp;

            // In-game Statistics
            data.demonsKilled += playerLevel.demonsKilled;
            data.devilsKilled += playerLevel.devilsKilled;
            if (layerReached > data.highestLayerReached && layerReached >= 0)
            {
                data.highestLayerReached = layerReached;
            }
            data.totalLevelUps += playerLevel.level - 1;
            
            // Souls
            if (hub)
            {
                data.souls = playerLevel.souls;
                data.totalSoulsCollected = playerLevel.souls;
            }
            else
            {
                data.souls += playerLevel.souls;
                data.totalSoulsCollected += playerLevel.souls;
            }

            // Selected Equipment
            data.weapon = playerLoadout.selectedWeapon.title;
            data.companion = playerLoadout.selectedCompanion.title;
            data.armor = playerLoadout.selectedArmor.title;
            data.back = playerLoadout.selectedBack.title;

            // Bought Equipment
            data.boughtWeapons.Clear();
            foreach (var weapon in playerEquipment.boughtWeapons)
            {
                data.boughtWeapons.Add(weapon.title);
            }
            data.boughtCompanions.Clear();
            foreach (var companion in playerEquipment.boughtCompanions)
            {
                data.boughtCompanions.Add(companion.title);
            }
            data.boughtArmors.Clear();
            foreach (var armor in playerEquipment.boughtArmors)
            {
                data.boughtArmors.Add(armor.title);
            }
            data.boughtBacks.Clear();
            foreach (var back in playerEquipment.boughtBacks)
            {
                data.boughtBacks.Add(back.title);
            }

            // Bought Souls
            data.attackSpeedSoulsBought = 0;
            foreach (var _ in playerSouls.attackSpeedSouls)
            {
                data.attackSpeedSoulsBought++;
            }
            data.damageSoulsBought = 0;
            foreach (var _ in playerSouls.damageSouls)
            {
                data.damageSoulsBought++;
            }
            data.defenceSoulsBought = 0;
            foreach (var _ in playerSouls.defenceSouls)
            {
                data.defenceSoulsBought++;
            }
            data.movementSpeedSoulsBought = 0;
            foreach (var _ in playerSouls.movementSpeedSouls)
            {
                data.movementSpeedSoulsBought++;
            }

            // Bought Upgrades
            // Weapons
            data.pugioUpgrades.Clear();
            foreach (var pugio in playerUpgrades.upgradesPugio)
            {
                data.pugioUpgrades.Add(pugio.title);
            }
            data.ulfberhtUpgrades.Clear();
            foreach (var ulfberht in playerUpgrades.upgradesUlfberht)
            {
                data.ulfberhtUpgrades.Add(ulfberht.title);
            }

            // Companions
            data.loyalSphereUpgrades.Clear();
            foreach (var loyalSphere in playerUpgrades.upgradesLoyalSphere)
            {
                data.loyalSphereUpgrades.Add(loyalSphere.title);
            }
            data.attackSquareUpgrades.Clear();
            foreach (var attackSquare in playerUpgrades.upgradesAttackSquare)
            {
                data.attackSquareUpgrades.Add(attackSquare.title);
            }

            // Armors
            data.leatherUpgrades.Clear();
            foreach (var leather in playerUpgrades.upgradesLeather)
            {
                data.leatherUpgrades.Add(leather.title);
            }
            data.hideUpgrades.Clear();
            foreach (var hide in playerUpgrades.upgradesHide)
            {
                data.hideUpgrades.Add(hide.title);
            }
            data.ringMailUpgrades.Clear();
            foreach (var ringMail in playerUpgrades.upgradesRingMail)
            {
                data.ringMailUpgrades.Add(ringMail.title);
            }
            data.plateUpgrades.Clear();
            foreach (var plate in playerUpgrades.upgradesPlate)
            {
                data.plateUpgrades.Add(plate.title);
            }

            // Backs
            data.angelWingsUpgrades.Clear();
            foreach (var angelWing in playerUpgrades.upgradesAngelWings)
            {
                data.angelWingsUpgrades.Add(angelWing.title);
            }
            data.steelWingsUpgrades.Clear();
            foreach (var steelWing in playerUpgrades.upgradesSteelWings)
            {
                data.steelWingsUpgrades.Add(steelWing.title);
            }
            data.backpackUpgrades.Clear();
            foreach (var backpack in playerUpgrades.upgradesBackpacks)
            {
                data.backpackUpgrades.Add(backpack.title);
            }
            data.capeOWindUpgrades.Clear();
            foreach (var capeOWind in playerUpgrades.upgradesCapeOWinds)
            {
                data.capeOWindUpgrades.Add(capeOWind.title);
            }
        }

        // Settings to save
    }

    #endregion
}
