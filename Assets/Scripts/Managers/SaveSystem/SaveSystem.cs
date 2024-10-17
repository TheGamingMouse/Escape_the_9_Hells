using System;
using System.IO;
using System.Linq;
using SaveSystemSpace;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SaveSystemSpace.SaveClasses;

public class SaveSystem : MonoBehaviour
{
    #region Variables

    public static SaveSystem Instance;

    public static readonly string layerDataPath = "Layer Data";
    public static readonly string playerDataPath = "Player Data";
    public static readonly string equipmentDataPath = "Equipment Data";
    public static readonly string soulsDataPath = "Souls Data";
    public static readonly string perksDataPath = "Perks Data";
    public static readonly string settingsDataPath = "Settings Data";
    public static readonly string npcDataPath = "Npc Data";
    public static readonly string persistentDataPath = "Persistant Data";

    public static LayerData loadedLayerData;
    public static PersistentData loadedPersistentData;
    public static PlayerData loadedPlayerData;
    public static EquipmentData loadedEquipmentData;
    public static SoulData loadedSoulData;
    public static PerkData loadedPerkData;
    public static SettingsData loadedSettingsData;
    public static NpcData loadedNpcData;

    #endregion

    #region Methods

    void Awake()
    {
        Instance = this;

        UpdateLoadedData();

        loadedLayerData.layerReached = CheckLayer();
        if (loadedLayerData.layerReached > loadedLayerData.highestLayerReached)
        {
            loadedLayerData.highestLayerReached = loadedLayerData.layerReached;
        }
        Save(loadedLayerData, layerDataPath);

        if (loadedLayerData.lState != LayerData.LayerState.InLayers)
        {
            float musicTime = loadedPersistentData.musicTime;

            loadedPersistentData = new PersistentData
            {
                musicTime = musicTime
            };
        }
        else
        {
            loadedPersistentData.musicTime = 0f;
        }
        loadedPersistentData.reRolls = loadedSoulData.reRollSoulsBought.Count;
        Save(loadedPersistentData, persistentDataPath);
    }

    public void Save(object saveData, string dataPath)
    {
        var updateData = saveData;

        if (saveData.GetType() == typeof(LayerData))
        {
            saveData = FriendlifyLayerData((LayerData)saveData);
        }
        
        else if (saveData.GetType() == typeof(EquipmentData))
        {
            saveData = FriendlifyEquipmentData((EquipmentData)saveData);
        }
        
        else if (saveData.GetType() == typeof(SoulData))
        {
            saveData = FriendlifySoulsData((SoulData)saveData);
        }
        
        else if (saveData.GetType() == typeof(PerkData))
        {
            saveData = FriendlifyPerksData((PerkData)saveData);
        }

        string fullPath = Application.persistentDataPath + "/Saved Files/" + dataPath + ".Json";

        if (!Directory.Exists(Application.persistentDataPath + "/Saved Files"))
        {
            Debug.Log($"Creating new directory at: {Application.persistentDataPath}/Saved Files.");
            Directory.CreateDirectory(Application.persistentDataPath + "/Saved Files");
        }

        try
        {
            string JsonString = JsonUtility.ToJson(saveData, true);
            
            File.WriteAllText(fullPath, JsonString);
            UpdateLoadedData(updateData);
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.GetType()} while saving: {e.Message}\n{e.StackTrace}.");
        }
    }

    public object Load(string dataPath)
    {
        var fullPath = $"{Application.persistentDataPath}/Saved Files/{dataPath}.Json";

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"File could not be found at {fullPath}.");
            return NewObject(dataPath);
        }

        try
        {
            if (dataPath == layerDataPath)
            {
                var friendlyObject = JsonUtility.FromJson<FriendlyLayerData>(File.ReadAllText(fullPath));

                var unfriendlyObject = UnfriendlifyLayerData(friendlyObject);
                return unfriendlyObject;
            }
            
            else if (dataPath == equipmentDataPath)
            {
                var friendlyObject = JsonUtility.FromJson<FriendlyEquipmentData>(File.ReadAllText(fullPath));

                var unfriendlyObject = UnfriendlifyEquipmentData(friendlyObject);
                return unfriendlyObject;
            }
            
            else if (dataPath == soulsDataPath)
            {
                var friendlyObject = JsonUtility.FromJson<FriendlySoulData>(File.ReadAllText(fullPath));

                var unfriendlyObject = UnfriendlifySoulsData(friendlyObject);
                return unfriendlyObject;
            }
            
            else if (dataPath == perksDataPath)
            {
                var friendlyObject = JsonUtility.FromJson<FriendlyPerkData>(File.ReadAllText(fullPath));

                var unfriendlyObject = UnfriendlifyPerksData(friendlyObject);
                return unfriendlyObject;
            }
            
            else if (dataPath == persistentDataPath)
            {
                return JsonUtility.FromJson<PersistentData>(File.ReadAllText(fullPath));
            }
            
            else if (dataPath == playerDataPath)
            {
                return JsonUtility.FromJson<PlayerData>(File.ReadAllText(fullPath));
            }
            
            else if (dataPath == settingsDataPath)
            {
                return JsonUtility.FromJson<SettingsData>(File.ReadAllText(fullPath));
            }
            
            else if (dataPath == npcDataPath)
            {
                return JsonUtility.FromJson<NpcData>(File.ReadAllText(fullPath));
            }

            else
            {
                return new ArgumentException();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"{e.GetType()} while loading: {e.Message}\n{e.StackTrace}.");
            return NewObject(dataPath);
        }
    }

    public void StartNewGame()
    {
        Save(loadedLayerData = new LayerData(), layerDataPath);
        Save(loadedPersistentData = new PersistentData(), persistentDataPath);
        Save(loadedPlayerData = new PlayerData(), playerDataPath);
        Save(loadedEquipmentData = new EquipmentData(), equipmentDataPath);
        Save(loadedSoulData = new SoulData(), soulsDataPath);
        Save(loadedPerkData = new PerkData(), perksDataPath);
        Save(loadedSettingsData = new SettingsData(), settingsDataPath);
        Save(loadedNpcData = new NpcData(), npcDataPath);
    }

    public int CheckLayer()
    {
        int layer = SceneManager.GetActiveScene().buildIndex;
        
        return layer - 1;
    }

    void UpdateLoadedData()
    {
        loadedLayerData = (LayerData)Load(layerDataPath);
        loadedPersistentData = (PersistentData)Load(persistentDataPath);
        loadedPlayerData = (PlayerData)Load(playerDataPath);
        loadedEquipmentData = (EquipmentData)Load(equipmentDataPath);
        loadedSoulData = (SoulData)Load(soulsDataPath);
        loadedPerkData = (PerkData)Load(perksDataPath);
        loadedSettingsData = (SettingsData)Load(settingsDataPath);
        loadedNpcData = (NpcData)Load(npcDataPath);
    }

    void UpdateLoadedData(object data)
    {
        if (data.GetType() == typeof(LayerData))
        {
            loadedLayerData = (LayerData)data;
        }
        
        else if (data.GetType() == typeof(PersistentData))
        {
            loadedPersistentData = (PersistentData)data;
        }
        
        else if (data.GetType() == typeof(PlayerData))
        {
            loadedPlayerData = (PlayerData)data;
        }
        
        else if (data.GetType() == typeof(EquipmentData))
        {
            loadedEquipmentData = (EquipmentData)data;
        }
        
        else if (data.GetType() == typeof(SoulData))
        {
            loadedSoulData = (SoulData)data;
        }
        
        else if (data.GetType() == typeof(PerkData))
        {
            loadedPerkData = (PerkData)data;
        }
        
        else if (data.GetType() == typeof(SettingsData))
        {
            loadedSettingsData = (SettingsData)data;
        }
        
        else if (data.GetType() == typeof(NpcData))
        {
            loadedNpcData = (NpcData)data;
        }

        else
        {
            Debug.LogError("Data object not recognized.");
        }
    }

    object NewObject(string dataPath)
    {
        if (dataPath == layerDataPath)
        {
            Save(new LayerData(), dataPath);
            return new LayerData();
        }
        
        else if (dataPath == persistentDataPath)
        {
            Save(new PersistentData(), dataPath);
            return new PersistentData();
        }
        
        else if (dataPath == playerDataPath)
        {
            Save(new PlayerData(), dataPath);
            return new PlayerData();
        }
        
        else if (dataPath == equipmentDataPath)
        {
            Save(new EquipmentData(), dataPath);
            return new EquipmentData();
        }
        
        else if (dataPath == soulsDataPath)
        {
            Save(new SoulData(), dataPath);
            return new SoulData();
        }
        
        else if (dataPath == perksDataPath)
        {
            Save(new PerkData(), dataPath);
            return new PerkData();
        }
        
        else if (dataPath == settingsDataPath)
        {
            Save(new SettingsData(), dataPath);
            return new SettingsData();
        }
        
        else if (dataPath == npcDataPath)
        {
            Save(new NpcData(), dataPath);
            return new NpcData();
        }

        else
        {
            Debug.LogError("Data path not recognized.");
            return null;
        }
    }

    #endregion

    #region Friendlify Methods

    FriendlyLayerData FriendlifyLayerData(LayerData data)
    {
        var friendlyObject = new FriendlyLayerData();

        friendlyObject.lState = data.lState.ToString();
        friendlyObject.layerReached = data.layerReached;
        friendlyObject.highestLayerReached = data.highestLayerReached;

        return friendlyObject;
    }

    FriendlyEquipmentData FriendlifyEquipmentData(EquipmentData data)
    {
        var friendlyObject = new FriendlyEquipmentData();

        // Weapons
        foreach (var weapon in data.weaponData.boughtWeapons)
        {
            if (!friendlyObject.weaponData.boughtWeaponNames.Contains(weapon.title))
            {
                friendlyObject.weaponData.boughtWeaponNames.Add(weapon.title);
            }
        }

        foreach (var upgrade in data.weaponData.pugioUpgrades)
        {
            friendlyObject.weaponData.pugioUpgradeNames.Add(upgrade.title);
        }

        foreach (var upgrade in data.weaponData.ulfberhtUpgrades)
        {
            friendlyObject.weaponData.ulfberhtUpgradeNames.Add(upgrade.title);
        }

        if (data.weaponData.selectedWeapon != null)
        {
            friendlyObject.weaponData.primaryWeaponName = data.weaponData.primaryWeapon.name;
            friendlyObject.weaponData.secondaryWeaponName = data.weaponData.secondaryWeapon.name;
        }
        friendlyObject.weaponData.selectedWeaponName = data.weaponData.selectedWeapon.name;

        // Companions
        foreach (var companion in data.companionData.boughtCompanions)
        {
            if (!friendlyObject.companionData.boughtCompanionNames.Contains(companion.title))
            {
                friendlyObject.companionData.boughtCompanionNames.Add(companion.title);
            }
        }

        foreach (var upgrade in data.companionData.loyalSphereUpgrades)
        {
            friendlyObject.companionData.loyalSphereUpgradeNames.Add(upgrade.title);
        }

        foreach (var upgrade in data.companionData.attackSquareUpgrades)
        {
            friendlyObject.companionData.attackSquareUpgradeNames.Add(upgrade.title);
        }

        if (data.companionData.selectedCompanion != null)
        {
            friendlyObject.companionData.primaryCompanionName = data.companionData.primaryCompanion.name;
            friendlyObject.companionData.secondaryCompanionName = data.companionData.secondaryCompanion.name;
            friendlyObject.companionData.selectedCompanionName = data.companionData.selectedCompanion.name;
        }

        // Armors
        foreach (var armor in data.armorData.boughtArmors)
        {
            if (!friendlyObject.armorData.boughtArmorNames.Contains(armor.title))
            {
                friendlyObject.armorData.boughtArmorNames.Add(armor.title);
            }
        }

        foreach (var upgrade in data.armorData.leatherUpgrades)
        {
            friendlyObject.armorData.leatherUpgradeNames.Add(upgrade.title);
        }

        foreach (var upgrade in data.armorData.hideUpgrades)
        {
            friendlyObject.armorData.hideUpgradeNames.Add(upgrade.title);
        }

        foreach (var upgrade in data.armorData.ringMailUpgrades)
        {
            friendlyObject.armorData.ringMailUpgradeNames.Add(upgrade.title);
        }

        foreach (var upgrade in data.armorData.plateUpgrades)
        {
            friendlyObject.armorData.plateUpgradeNames.Add(upgrade.title);
        }

        if (data.armorData.selectedArmor != null)
        friendlyObject.armorData.selectedArmorName = data.armorData.selectedArmor.title;

        // Backs
        foreach (var back in data.backData.boughtBacks)
        {
            if (!friendlyObject.backData.boughtBackNames.Contains(back.title))
            {
                friendlyObject.backData.boughtBackNames.Add(back.title);
            }
        }

        foreach (var upgrade in data.backData.angelWingsUpgrades)
        {
            friendlyObject.backData.angelWingsUpgradeNames.Add(upgrade.title);
        }

        foreach (var upgrade in data.backData.steelWingsUpgrades)
        {
            friendlyObject.backData.steelWingsUpgradeNames.Add(upgrade.title);
        }

        foreach (var upgrade in data.backData.backpackUpgrades)
        {
            friendlyObject.backData.backpackUpgradeNames.Add(upgrade.title);
        }

        foreach (var upgrade in data.backData.capeOWindUpgrades)
        {
            friendlyObject.backData.capeOWindUpgradeNames.Add(upgrade.title);
        }

        foreach (var upgrade in data.backData.seedBagUpgrades)
        {
            friendlyObject.backData.seedBagUpgradeNames.Add(upgrade.title);
        }

        
        if (data.backData.selectedBack != null)
        friendlyObject.backData.selectedBackName = data.backData.selectedBack.title;

        friendlyObject.backData.backpackPrimary = data.backData.backpackPrimary;
        friendlyObject.backData.seedBagPrimary = data.backData.seedBagPrimary;
        friendlyObject.backData.capeOWindCooldown = data.backData.capeOWindCooldown;

        return friendlyObject;
    }

    FriendlySoulData FriendlifySoulsData(SoulData data)
    {
        var friendlyObject = new FriendlySoulData();

        friendlyObject.attackSpeedSoulsBought = data.attackSpeedSoulsBought.Count;
        friendlyObject.damageSoulsBought = data.damageSoulsBought.Count;
        friendlyObject.defenceSoulsBought = data.defenceSoulsBought.Count;
        friendlyObject.movementSpeedSoulsBought = data.movementSpeedSoulsBought.Count;
        friendlyObject.luckSoulsBought = data.luckSoulsBought.Count;
        friendlyObject.startLevelSoulsBought = data.startLevelSoulsBought.Count;
        friendlyObject.reRollSoulsBought = data.reRollSoulsBought.Count;
        friendlyObject.pathFinderSoulsBought = data.pathFinderSoulsBought.Count;

        return friendlyObject;
    }

    FriendlyPerkData FriendlifyPerksData(PerkData data)
    {
        var friendlyObject = new FriendlyPerkData();

        friendlyObject.defencePerks = data.defencePerks.Count;
        friendlyObject.attackSpeedPerks = data.attackSpeedPerks.Count;
        friendlyObject.damagePerks = data.damagePerks.Count;
        friendlyObject.moveSpeedPerks = data.moveSpeedPerks.Count;
        friendlyObject.luckPerks = data.luckPerks.Count;
        friendlyObject.fireAuraPerks = data.fireAuraPerks.Count;
        friendlyObject.shieldPerks = data.shieldPerks.Count;
        friendlyObject.iceAuraPerks = data.iceAuraPerks.Count;

        return friendlyObject;
    }

    #endregion

    #region Unfriendlify Methods

    LayerData UnfriendlifyLayerData(FriendlyLayerData data)
    {
        var unfriendlyObject = new LayerData();

        unfriendlyObject.lState = Enum.Parse<LayerData.LayerState>(data.lState);
        unfriendlyObject.layerReached = data.layerReached;
        unfriendlyObject.highestLayerReached = data.highestLayerReached;

        return unfriendlyObject;
    }

    EquipmentData UnfriendlifyEquipmentData(FriendlyEquipmentData data)
    {
        var unfriendlyObject = new EquipmentData();
        var setObjects = SetScriptableObjects.Instance;

        var setWeapons = setObjects.setWeapons;
        var setCompanions = setObjects.setCompanions;
        var setArmors = setObjects.setArmors;
        var setBacks = setObjects.setBacks;

        var setWeaponUpgrades = setObjects.setWeaponUpgrades;
        var setCompanionUpgrades = setObjects.setCompanionUpgrades;
        var setArmorsUpgrades = setObjects.setArmorUpgrades;
        var setBacksUpgrades = setObjects.setBackUpgrades;

        // Weapons
        foreach (var weapon in data.weaponData.boughtWeaponNames)
        {
            foreach (var setWeapon in setWeapons)
            {
                if (weapon == setWeapon.title)
                {
                    unfriendlyObject.weaponData.boughtWeapons.Add(setWeapon);
                }

                if (setWeapon.title == data.weaponData.primaryWeaponName)
                {
                    unfriendlyObject.weaponData.primaryWeapon = setWeapon;
                }

                if (setWeapon.title == data.weaponData.secondaryWeaponName)
                {
                    unfriendlyObject.weaponData.secondaryWeapon = setWeapon;
                }

                if (setWeapon.title == data.weaponData.selectedWeaponName)
                {
                    unfriendlyObject.weaponData.selectedWeapon = setWeapon;
                }
            }
        }

        foreach (var setUpgrade in setWeaponUpgrades)
        {
            foreach (var upgrade in data.weaponData.pugioUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.weaponData.pugioUpgrades.Add(setUpgrade);
                }
            }

            foreach (var upgrade in data.weaponData.ulfberhtUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.weaponData.ulfberhtUpgrades.Add(setUpgrade);
                }
            }
        }

        // Companions
        foreach (var companion in data.companionData.boughtCompanionNames)
        {
            foreach (var setCompanion in setCompanions)
            {
                if (companion == setCompanion.title)
                {
                    unfriendlyObject.companionData.boughtCompanions.Add(setCompanion);
                }

                if (setCompanion.title == data.companionData.primaryCompanionName)
                {
                    unfriendlyObject.companionData.primaryCompanion = setCompanion;
                }

                if (setCompanion.title == data.companionData.secondaryCompanionName)
                {
                    unfriendlyObject.companionData.secondaryCompanion = setCompanion;
                }

                if (setCompanion.title == data.companionData.selectedCompanionName)
                {
                    unfriendlyObject.companionData.selectedCompanion = setCompanion;
                }
            }
        }

        foreach (var setUpgrade in setCompanionUpgrades)
        {
            foreach (var upgrade in data.companionData.loyalSphereUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.companionData.loyalSphereUpgrades.Add(setUpgrade);
                }
            }

            foreach (var upgrade in data.companionData.attackSquareUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.companionData.attackSquareUpgrades.Add(setUpgrade);
                }
            }
        }

        // Armors
        foreach (var armor in data.armorData.boughtArmorNames)
        {
            foreach (var setArmor in setArmors)
            {
                if (armor == setArmor.title)
                {
                    unfriendlyObject.armorData.boughtArmors.Add(setArmor);
                }

                if (setArmor.title == data.armorData.selectedArmorName)
                {
                    unfriendlyObject.armorData.selectedArmor = setArmor;
                }
            }
        }

        foreach (var setUpgrade in setArmorsUpgrades)
        {
            foreach (var upgrade in data.armorData.leatherUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.armorData.leatherUpgrades.Add(setUpgrade);
                }
            }

            foreach (var upgrade in data.armorData.hideUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.armorData.hideUpgrades.Add(setUpgrade);
                }
            }

            foreach (var upgrade in data.armorData.ringMailUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.armorData.ringMailUpgrades.Add(setUpgrade);
                }
            }

            foreach (var upgrade in data.armorData.plateUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.armorData.plateUpgrades.Add(setUpgrade);
                }
            }
        }

        // Backs
        foreach (var back in data.backData.boughtBackNames)
        {
            foreach (var setBack in setBacks)
            {
                if (back == setBack.title)
                {
                    unfriendlyObject.backData.boughtBacks.Add(setBack);
                }

                if (setBack.title == data.backData.selectedBackName)
                {
                    unfriendlyObject.backData.selectedBack = setBack;
                }
            }
        }

        foreach (var setUpgrade in setBacksUpgrades)
        {
            foreach (var upgrade in data.backData.angelWingsUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.backData.angelWingsUpgrades.Add(setUpgrade);
                }
            }

            foreach (var upgrade in data.backData.steelWingsUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.backData.steelWingsUpgrades.Add(setUpgrade);
                }
            }

            foreach (var upgrade in data.backData.backpackUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.backData.backpackUpgrades.Add(setUpgrade);
                }
            }

            foreach (var upgrade in data.backData.capeOWindUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.backData.capeOWindUpgrades.Add(setUpgrade);
                }
            }

            foreach (var upgrade in data.backData.seedBagUpgradeNames)
            {
                if (upgrade == setUpgrade.title)
                {
                    unfriendlyObject.backData.seedBagUpgrades.Add(setUpgrade);
                }
            }
        }

        unfriendlyObject.backData.backpackPrimary = data.backData.backpackPrimary;
        unfriendlyObject.backData.seedBagPrimary = data.backData.seedBagPrimary;
        unfriendlyObject.backData.capeOWindCooldown = data.backData.capeOWindCooldown;

        return unfriendlyObject;
    }
    
    SoulData UnfriendlifySoulsData(FriendlySoulData data)
    {
        var unfriendlyObject = new SoulData();
        var setSoulItems = SetScriptableObjects.Instance.setSoulItems;

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

        for (int i = 0; i < data.attackSpeedSoulsBought; i++)
        {
            unfriendlyObject.attackSpeedSoulsBought.Add(attackSpeedSoul);
        }

        for (int i = 0; i< data.damageSoulsBought; i++)
        {
            unfriendlyObject.damageSoulsBought.Add(damageSoul);
        }

        for (int i = 0; i< data.defenceSoulsBought; i++)
        {
            unfriendlyObject.defenceSoulsBought.Add(defenceSoul);
        }

        for (int i = 0; i< data.movementSpeedSoulsBought; i++)
        {
            unfriendlyObject.movementSpeedSoulsBought.Add(movementSpeedSoul);
        }

        for (int i = 0; i< data.luckSoulsBought; i++)
        {
            unfriendlyObject.luckSoulsBought.Add(luckSoul);
        }

        for (int i = 0; i< data.startLevelSoulsBought; i++)
        {
            unfriendlyObject.startLevelSoulsBought.Add(startLevelSoul);
        }

        for (int i = 0; i< data.reRollSoulsBought; i++)
        {
            unfriendlyObject.reRollSoulsBought.Add(reRollSoul);
        }

        for (int i = 0; i< data.pathFinderSoulsBought; i++)
        {
            unfriendlyObject.pathFinderSoulsBought.Add(pathFinderSoul);
        }

        return unfriendlyObject;
    }

    PerkData UnfriendlifyPerksData(FriendlyPerkData data)
    {
        var unfriendlyObject = new PerkData();
        var setPerks = SetScriptableObjects.Instance.setPerks;

        PerkItemsSO defencePerk = null;
        PerkItemsSO attackSpeedPerk = null;
        PerkItemsSO damagePerk = null;
        PerkItemsSO moveSpeedPerk = null;
        PerkItemsSO luckPerk = null;
        PerkItemsSO fireAuraPerk = null;
        PerkItemsSO shieldPerk = null;
        PerkItemsSO iceAuraPerk = null;

        foreach (var perk in setPerks)
        {
            if (perk.title == "Defence Perk")
            {
                defencePerk = perk;
            }
            else if (perk.title == "Attack Speed Perk")
            {
                attackSpeedPerk = perk;
            }
            else if (perk.title == "Damage Perk")
            {
                damagePerk = perk;
            }
            else if (perk.title == "Movement Speed Perk")
            {
                moveSpeedPerk = perk;
            }
            else if (perk.title == "Luck Perk")
            {
                luckPerk = perk;
            }
            else if (perk.title == "Fire Aura Perk")
            {
                fireAuraPerk = perk;
            }
            else if (perk.title == "Shield Perk")
            {
                shieldPerk = perk;
            }
            else if (perk.title == "Ice Aura Perk")
            {
                iceAuraPerk = perk;
            }
        }

        for (int i = 0; i< data.defencePerks; i++)
        {
            unfriendlyObject.defencePerks.Add(defencePerk);
        }

        for (int i = 0; i< data.attackSpeedPerks; i++)
        {
            unfriendlyObject.attackSpeedPerks.Add(attackSpeedPerk);
        }

        for (int i = 0; i< data.damagePerks; i++)
        {
            unfriendlyObject.damagePerks.Add(damagePerk);
        }

        for (int i = 0; i< data.moveSpeedPerks; i++)
        {
            unfriendlyObject.moveSpeedPerks.Add(moveSpeedPerk);
        }

        for (int i = 0; i< data.luckPerks; i++)
        {
            unfriendlyObject.luckPerks.Add(luckPerk);
        }

        for (int i = 0; i< data.fireAuraPerks; i++)
        {
            unfriendlyObject.fireAuraPerks.Add(fireAuraPerk);
        }

        for (int i = 0; i< data.shieldPerks; i++)
        {
            unfriendlyObject.shieldPerks.Add(shieldPerk);
        }

        for (int i = 0; i< data.iceAuraPerks; i++)
        {
            unfriendlyObject.iceAuraPerks.Add(iceAuraPerk);
        }

        return unfriendlyObject;
    }

    #endregion
}