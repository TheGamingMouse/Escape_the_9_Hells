using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour, IDataPersistence
{
    #region Variables

    [Header("Enums")]
    public LayerState lState;
    
    [Header("Ints")]
    [HideInInspector]
    public int layerReached;
    [HideInInspector]
    public int screenMode;
    [HideInInspector]
    public int higestLayerReached;
    // [HideInInspector]
    public int currentSouls;
    [HideInInspector]
    public int totalSouls;
    [HideInInspector]
    public int totalLevels;
    [HideInInspector]
    public int demonsKilled;
    [HideInInspector]
    public int devilsKilled;
    [HideInInspector]
    public int reRolls;

    // In-layer
    [HideInInspector]
    public int soulsCollectedInLayer;
    [HideInInspector]
    public int levelsGainedInLayer;
    [HideInInspector]
    public int demonsKilledInLayer;
    [HideInInspector]
    public int devilsKilledInLayer;

    [Header("Floats")]
    [HideInInspector]
    public float capeOWindCooldown;
    [HideInInspector]
    public float masterVolume;
    [HideInInspector]
    public float musicVolume;
    [HideInInspector]
    public float sfxVolume;
    [HideInInspector]
    public float musicTime;

    // In-layer
    [HideInInspector]
    public float expGainedInLayer;
    [HideInInspector]
    public float expMultiplierInLayer;
    [HideInInspector]
    public float healthInLayer;

    [Header("Bools")]
    [HideInInspector]
    public bool rickyStartComp;
    [HideInInspector]
    public bool ready;
    [HideInInspector]
    public bool backpackPrimary;
    [HideInInspector]
    public bool seedBagPrimary;
    [HideInInspector]
    public bool returnedToRicky;

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

    public List<PerkItemsSO> setPerks;

    [HideInInspector]
    public List<LoadoutItemsSO> boughtWeapons = new();
    [HideInInspector]
    public List<LoadoutItemsSO> boughtCompanions = new();
    [HideInInspector]
    public List<LoadoutItemsSO> boughtArmors = new();
    [HideInInspector]
    public List<LoadoutItemsSO> boughtBacks = new();

    [HideInInspector]
    public List<SoulsItemsSO> attackSpeedSoulsBought = new();
    [HideInInspector]
    public List<SoulsItemsSO> damageSoulsBought = new();
    [HideInInspector]
    public List<SoulsItemsSO> defenceSoulsBought = new();
    [HideInInspector]
    public List<SoulsItemsSO> movementSpeedSoulsBought = new();
    [HideInInspector]
    public List<SoulsItemsSO> luckSoulsBought = new();
    [HideInInspector]
    public List<SoulsItemsSO> startLevelSoulsBought = new();
    [HideInInspector]
    public List<SoulsItemsSO> reRollSoulsBought = new();
    [HideInInspector]
    public List<SoulsItemsSO> pathFinderSoulsBought = new();
    
    [HideInInspector]
    public List<UpgradeItemsSO> pugioUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> ulfberhtUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> loyalSphereUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> attackSquareUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> leatherUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> hideUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> ringMailUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> plateUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> angelWingsUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> steelWingsUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> backpackUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> capeOWindUpgrades = new();
    [HideInInspector]
    public List<UpgradeItemsSO> seedBagUpgrades = new();

    // In-layer
    [HideInInspector]
    public List<PerkItemsSO> defencePerks = new();
    [HideInInspector]
    public List<PerkItemsSO> attackSpeedPerks = new();
    [HideInInspector]
    public List<PerkItemsSO> damagePerks = new();
    [HideInInspector]
    public List<PerkItemsSO> moveSpeedPerks = new();
    [HideInInspector]
    public List<PerkItemsSO> luckPerks = new();
    [HideInInspector]
    public List<PerkItemsSO> fireAuraPerks = new();
    [HideInInspector]
    public List<PerkItemsSO> shieldPerks = new();
    [HideInInspector]
    public List<PerkItemsSO> iceAuraPerks = new();

    [Header("LoadoutItemsSOs")]
    [HideInInspector]
    public LoadoutItemsSO primaryWeapon;
    [HideInInspector]
    public LoadoutItemsSO secondaryWeapon;
    [HideInInspector]
    public LoadoutItemsSO selectedWeapon;
    [HideInInspector]
    public LoadoutItemsSO primaryCompanion;
    [HideInInspector]
    public LoadoutItemsSO secondaryCompanion;
    [HideInInspector]
    public LoadoutItemsSO selectedCompanion;
    [HideInInspector]
    public LoadoutItemsSO armor;
    [HideInInspector]
    public LoadoutItemsSO back;

    [Header("Components")]
    PlayerLevel playerLevel;
    PlayerLoadout playerLoadout;
    PlayerEquipment playerEquipment;
    PlayerSouls playerSouls;
    PlayerUpgrades playerUpgrades;
    CapeOWind capeOWind;
    public MainMenuManager mainMenu;
    SettingsManager settingsManager;
    PlayerPerks playerPerks;
    PlayerHealth playerHealth;
    PerkMenu perkMenu;
    MusicAudioManager musicManager;
    
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
        playerPerks = player.GetComponent<PlayerPerks>();
        playerHealth = player.GetComponent<PlayerHealth>();
        
        if (lState != LayerState.MainMenu)
        {
            capeOWind = player.GetComponentInChildren<Backs>().capeOWind;
            settingsManager = GameObject.FindWithTag("Managers").GetComponent<SettingsManager>();
            perkMenu = GameObject.FindWithTag("Canvas").transform.Find("Menus/PerkMenu").GetComponent<PerkMenu>();
        }

        if (lState == LayerState.MainMenu || lState == LayerState.Hub)
        {
            musicManager = GetComponent<MusicAudioManager>();
        }

        if (CheckLayer() == -2)
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

    public int CheckLayer()
    {
        int layer = SceneManager.GetActiveScene().buildIndex;
        
        return layer - 1;
    }

    #endregion

    #region SaveLoad Methods

    public void LoadData(GameData data)
    {
        currentSouls = data.souls;
        if (lState != LayerState.MainMenu)
        {
            if (lState == LayerState.Hub)
            {
                // Souls
                GameObject.FindWithTag("Managers").GetComponent<ExpSoulsManager>().AddSouls(data.souls, true);
            }

            // Total Souls
            totalSouls = data.totalSoulsCollected;

            // NPC variables
            rickyStartComp = data.rickyStartComp;
            returnedToRicky = data.returnedToRicky;

            // Selected Equipment
            foreach (var weapon in setWeapons)
            {
                if (weapon.title == data.primaryWeapon)
                {
                    primaryWeapon = weapon;
                }
            }
            foreach (var weapon in setWeapons)
            {
                if (weapon.title == data.secondaryWeapon)
                {
                    secondaryWeapon = weapon;
                }
            }
            foreach (var weapon in setWeapons)
            {
                if (weapon.title == data.selectedWeapon)
                {
                    selectedWeapon = weapon;
                }
            }
            if (selectedWeapon && selectedWeapon.title == data.primaryWeapon)
            {
                backpackPrimary = true;
            }
            else if (!selectedWeapon)
            {
                backpackPrimary = false;
            }
            else if (selectedWeapon.title == data.secondaryWeapon)
            {
                backpackPrimary = false;
            }
            foreach (var companion in setCompanions)
            {
                if (companion.title == data.primaryCompanion)
                {
                    primaryCompanion = companion;
                }
            }
            foreach (var companion in setCompanions)
            {
                if (companion.title == data.secondaryCompanion)
                {
                    secondaryCompanion = companion;
                }
            }
            foreach (var companion in setCompanions)
            {
                if (companion.title == data.selectedCompanion)
                {
                    selectedCompanion = companion;
                }
            }
            if (selectedCompanion && selectedCompanion.title == data.primaryCompanion)
            {
                seedBagPrimary = true;
            }
            else if (!selectedCompanion)
            {
                seedBagPrimary = false;
            }
            else if (selectedCompanion.title == data.secondaryCompanion)
            {
                seedBagPrimary = false;
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
            SoulsItemsSO luckSoul = null;
            SoulsItemsSO startLevelSoul = null;
            SoulsItemsSO reRollSoul = null;
            SoulsItemsSO pathFinderSoul = null;

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
                else if (soul.title == "Luck Soul")
                {
                    luckSoul = soul;
                }
                else if (soul.title == "Start Level Soul")
                {
                    startLevelSoul = soul;
                }
                else if (soul.title == "Re Roll Soul")
                {
                    reRollSoul = soul;
                }
                else if (soul.title == "Path Finder Soul")
                {
                    pathFinderSoul = soul;
                }
            }
            if (!attackSpeedSoul)
            {
                Debug.LogError("Attack Speed Soul was not found");
            }
            if (!damageSoul)
            {
                Debug.LogError("Damage Soul was not found");
            }
            if (!defenceSoul)
            {
                Debug.LogError("Defence Soul was not found");
            }
            if (!movementSpeedSoul)
            {
                Debug.LogError("Movement Speed Soul was not found");
            }
            if (!luckSoul)
            {
                Debug.LogError("Luck Soul was not found");
            }
            if (!startLevelSoul)
            {
                Debug.LogError("Start Soul was not found");
            }
            if (!reRollSoul)
            {
                Debug.LogError("Re Roll Soul was not found");
            }
            if (!pathFinderSoul)
            {
                Debug.LogError("Path Finder Soul was not found");
            }

            attackSpeedSoulsBought.Clear();
            if (data.attackSpeedSoulsBought > 6)
            {
                data.attackSpeedSoulsBought = 0;
                Debug.LogWarning("More than 6 Souls found in attackSpeedSouls");
            }
            for (int i = 0; i < data.attackSpeedSoulsBought; i++)
            {
                attackSpeedSoulsBought.Add(attackSpeedSoul);
            }

            damageSoulsBought.Clear();
            if (data.damageSoulsBought > 6)
            {
                data.damageSoulsBought = 0;
                Debug.LogWarning("More than 6 Souls found in damageSouls");
            }
            for (int i = 0; i < data.damageSoulsBought; i++)
            {
                damageSoulsBought.Add(damageSoul);
            }

            defenceSoulsBought.Clear();
            if (data.defenceSoulsBought > 6)
            {
                data.defenceSoulsBought = 0;
                Debug.LogWarning("More than 6 Souls found in defenceSouls");
            }
            for (int i = 0; i < data.defenceSoulsBought; i++)
            {
                defenceSoulsBought.Add(defenceSoul);
            }

            movementSpeedSoulsBought.Clear();
            if (data.movementSpeedSoulsBought > 6)
            {
                data.movementSpeedSoulsBought = 0;
                Debug.LogWarning("More than 6 Souls found in movementSpeedSouls");
            }
            for (int i = 0; i < data.movementSpeedSoulsBought; i++)
            {
                movementSpeedSoulsBought.Add(movementSpeedSoul);
            }
            
            luckSoulsBought.Clear();
            if (data.luckSoulsBought > 6)
            {
                data.luckSoulsBought = 0;
                Debug.LogWarning("More than 6 Souls found in luckSouls");
            }
            for (int i = 0; i < data.luckSoulsBought; i++)
            {
                luckSoulsBought.Add(luckSoul);
            }

            startLevelSoulsBought.Clear();
            if (data.startLevelSoulsBought > 6)
            {
                data.startLevelSoulsBought = 0;
                Debug.LogWarning("More than 6 Souls found in startLevelSouls");
            }
            for (int i = 0; i < data.startLevelSoulsBought; i++)
            {
                startLevelSoulsBought.Add(startLevelSoul);
            }

            reRollSoulsBought.Clear();
            if (data.reRollSoulsBought > 6)
            {
                data.reRollSoulsBought = 0;
                Debug.LogWarning("More than 6 Souls found in reRollSouls");
            }
            for (int i = 0; i < data.reRollSoulsBought; i++)
            {
                reRollSoulsBought.Add(reRollSoul);
                reRolls++;
            }
            reRolls -= data.reRollsSpent;

            pathFinderSoulsBought.Clear();
            if (data.pathFinderSoulsBought > 1)
            {
                data.pathFinderSoulsBought = 0;
                Debug.LogWarning("More than 1 Souls found in pathFinderSouls");
            }
            for (int i = 0; i < data.pathFinderSoulsBought; i++)
            {
                pathFinderSoulsBought.Add(pathFinderSoul);
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
            seedBagUpgrades.Clear();
            foreach (var seedBag in setBackUpgrades)
            {
                foreach (var dataSeedBag in data.seedBagUpgrades)
                {
                    if (seedBag.title == dataSeedBag)
                    {
                        seedBagUpgrades.Add(seedBag);
                    }
                }
            }

            // Equipment
            capeOWindCooldown = data.capeOWindCooldown;

            if (lState == LayerState.InLayers)
            {
                // In-layer
                levelsGainedInLayer = data.levelsGainedInLayer;
                expGainedInLayer = data.expGainedInLayer;
                expMultiplierInLayer = data.expMultiplierInLayer;
                soulsCollectedInLayer = data.soulsCollectedInLayer;
                demonsKilledInLayer = data.demonsKilledInLayer;
                devilsKilledInLayer = data.devilsKilledInLayer;
                healthInLayer = data.healthInLayer;
                
                foreach (var perk in data.perks)
                {
                    if (perk == "Defence Perk")
                    {
                        defencePerks.Add(setPerks.Where(t => t.title == perk).First());
                    }
                    else if (perk == "Attack Speed Perk")
                    {
                        attackSpeedPerks.Add(setPerks.Where(t => t.title == perk).First());
                    }
                    else if (perk == "Damage Perk")
                    {
                        damagePerks.Add(setPerks.Where(t => t.title == perk).First());
                    }
                    else if (perk == "Movement Speed Perk")
                    {
                        moveSpeedPerks.Add(setPerks.Where(t => t.title == perk).First());
                    }
                    else if (perk == "Luck Perk")
                    {
                        luckPerks.Add(setPerks.Where(t => t.title == perk).First());
                    }
                    else if (perk == "Fire Aura Perk")
                    {
                        fireAuraPerks.Add(setPerks.Where(t => t.title == perk).First());
                    }
                    else if (perk == "Shield Perk")
                    {
                        shieldPerks.Add(setPerks.Where(t => t.title == perk).First());
                    }
                    else if (perk == "Ice Aura Perk")
                    {
                        iceAuraPerks.Add(setPerks.Where(t => t.title == perk).First());
                    }
                }
            }
        }
        else
        {
            demonsKilled = data.demonsKilled;
            devilsKilled = data.devilsKilled;
            higestLayerReached = data.highestLayerReached;
            totalLevels = data.totalLevelUps;
            totalSouls = data.totalSoulsCollected;
        }

        if (lState == LayerState.MainMenu || lState == LayerState.Hub)
        {
            musicTime = data.musicTime;
        }
        else
        {
            musicTime = 0f;
        }

        // Settings
        masterVolume = data.masterVolume;
        musicVolume = data.musicVolume;
        sfxVolume = data.sfxVolume;
        screenMode = data.screenMode;

        ready = true;
    }

    public void SaveData(ref GameData data)
    {
        if (lState != LayerState.MainMenu)
        {
            if (lState == LayerState.Hub)
            {
                // Souls
                data.souls = playerLevel.souls;
            }
            else
            {
                data.souls += playerLevel.souls;
                data.totalSoulsCollected += playerLevel.souls;
            }

            // NPC Variables
            data.rickyStartComp = rickyStartComp;
            data.returnedToRicky = returnedToRicky;

            // In-game Statistics
            data.demonsKilled += playerLevel.demonsKilled;
            data.devilsKilled += playerLevel.devilsKilled;
            if (layerReached > data.highestLayerReached && layerReached >= 0)
            {
                data.highestLayerReached = layerReached;
            }
            data.totalLevelUps += playerLevel.level - 1;

            // Selected Equipment
            if (playerLoadout.selectedWeapon != null)
            {
                data.selectedWeapon = playerLoadout.selectedWeapon.title;

                if (playerLoadout.selectedPrimaryWeapon)
                {
                    data.primaryWeapon = playerLoadout.selectedPrimaryWeapon.title;
                }
                
                if (playerLoadout.selectedSecondaryWeapon)
                {
                    data.secondaryWeapon = playerLoadout.selectedSecondaryWeapon.title;
                }
                else
                {
                    data.secondaryWeapon = null;
                }
            }
            else
            {
                data.primaryWeapon = null;
                data.secondaryWeapon = null;
                data.selectedWeapon = null;
            }
            if (playerLoadout.selectedCompanion != null)
            {
                data.selectedCompanion = playerLoadout.selectedCompanion.title;

                if (playerLoadout.selectedPrimaryCompanion)
                {
                    data.primaryCompanion = playerLoadout.selectedPrimaryCompanion.title;
                }
                
                if (playerLoadout.selectedSecondaryCompanion)
                {
                    data.secondaryCompanion = playerLoadout.selectedSecondaryCompanion.title;
                }
                else
                {
                    data.secondaryCompanion = null;
                }
            }
            else
            {
                data.primaryCompanion = null;
                data.secondaryCompanion = null;
                data.selectedCompanion = null;
            }
            if (playerLoadout.selectedArmor != null)
            {
                data.armor = playerLoadout.selectedArmor.title;
            }
            else
            {
                data.armor = null;
            }
            if (playerLoadout.selectedBack)
            {
                data.back = playerLoadout.selectedBack.title;
            }
            else
            {
                data.back = null;
            }

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
            data.luckSoulsBought = 0;
            foreach (var _ in playerSouls.luckSouls)
            {
                data.luckSoulsBought++;
            }
            data.startLevelSoulsBought = 0;
            foreach (var _ in playerSouls.startLevelSouls)
            {
                data.startLevelSoulsBought++;
            }
            data.reRollSoulsBought = 0;
            foreach (var _ in playerSouls.reRollSouls)
            {
                data.reRollSoulsBought++;
            }
            data.reRollsSpent = perkMenu.reRollsSpent;
            data.pathFinderSoulsBought = 0;
            if (playerSouls.playerPathfinder)
            {
                data.pathFinderSoulsBought = 1;
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
            data.seedBagUpgrades.Clear();
            foreach (var seedBag in playerUpgrades.upgradesSeedBag)
            {
                data.seedBagUpgrades.Add(seedBag.title);
            }

            // Equipment
            if (!capeOWind)
            {
                data.capeOWindCooldown = capeOWind.cooldown;
            }
            else
            {
                data.capeOWindCooldown = 0f;
            }

            // In-layer
            if (lState == LayerState.InLayers)
            {
                data.soulsCollectedInLayer = playerLevel.souls;
                data.levelsGainedInLayer = playerLevel.level;
                data.expGainedInLayer = playerLevel.exp;
                data.expMultiplierInLayer = playerLevel.expMultiplier;
                data.demonsKilledInLayer = playerLevel.demonsKilled;
                data.devilsKilledInLayer = playerLevel.devilsKilled;
                data.healthInLayer = playerHealth.health;
            
                data.perks.Clear();
                foreach (var defence in playerPerks.defencePerks)
                {
                    data.perks.Add(defence.title);
                }

                foreach (var attack in playerPerks.attackSpeedPerks)
                {
                    data.perks.Add(attack.title);
                }

                foreach (var damage in playerPerks.damagePerks)
                {
                    data.perks.Add(damage.title);
                }

                foreach (var moveSpeed in playerPerks.moveSpeedPerks)
                {
                    data.perks.Add(moveSpeed.title);
                }

                foreach (var luck in playerPerks.luckPerks)
                {
                    data.perks.Add(luck.title);
                }

                foreach (var fireAura in playerPerks.fireAuraPerks)
                {
                    data.perks.Add(fireAura.title);
                }

                foreach (var shield in playerPerks.shieldPerks)
                {
                    data.perks.Add(shield.title);
                }

                foreach (var iceAura in playerPerks.iceAuraPerks)
                {
                    data.perks.Add(iceAura.title);
                }
            }
            else
            {
                data.soulsCollectedInLayer = 0;
                data.levelsGainedInLayer = 0;
                data.expGainedInLayer = 0f;
                data.expMultiplierInLayer = 0f;
                data.demonsKilledInLayer = 0;
                data.devilsKilledInLayer = 0;
                data.healthInLayer = playerHealth.maxHealth;
            
                data.perks = new();
            }
        }

        // Settings
        if (lState == LayerState.MainMenu)
        {
            data.masterVolume = mainMenu.masterVolume;
            data.musicVolume = mainMenu.musicVolume;
            data.sfxVolume = mainMenu.sfxVolume;
            data.screenMode = mainMenu.screenMode;
        }
        else
        {
            data.masterVolume = settingsManager.masterVolume;
            data.musicVolume = settingsManager.musicVolume;
            data.sfxVolume = settingsManager.sfxVolume;
            data.screenMode = settingsManager.screenMode;
        }

        if (lState == LayerState.MainMenu || lState == LayerState.Hub)
        {
            data.musicTime = musicManager.musicTime;
        }
        else
        {
            data.musicTime = 0f;
        }
    }

    #endregion

    #region Enums

    public enum LayerState
    {
        InLayers,
        Hub,
        MainMenu
    }

    #endregion
}
