using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [Header("Ints")]
    // Souls
    public int souls;
    public int screenMode;
    public int reRollsSpent;
    
    // In-game Statistics
    public int demonsKilled;
    public int devilsKilled;
    public int highestLayerReached;
    public int totalLevelUps;
    public int totalSoulsCollected;

    // Bought Souls
    public int attackSpeedSoulsBought;
    public int damageSoulsBought;
    public int defenceSoulsBought;
    public int movementSpeedSoulsBought;
    public int luckSoulsBought;
    public int startLevelSoulsBought;
    public int reRollSoulsBought;

    // In-layer
    public int levelsGainedInLayer;
    public int soulsCollectedInLayer;
    public int demonsKilledInLayer;
    public int devilsKilledInLayer;

    [Header("Floats")]
    //Equipment
    public float capeOWindCooldown;

    // Settings
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;

    // In-layer
    public float expGainedInLayer;
    public float expMultiplierInLayer;
    public float healthInLayer;

    // Music Length
    public float hubMenuMusicTime;

    [Header("Bools")]
    // NPC Variables
    public bool rickyStartComp;

    [Header("Strings")]
    // Selected Equipment
    public string primaryWeapon;
    public string secondaryWeapon;
    public string companion;
    public string armor;
    public string back;

    [Header("Lists")]
    // Bought Equipment
    public List<string> boughtWeapons;
    public List<string> boughtCompanions;
    public List<string> boughtArmors;
    public List<string> boughtBacks;

    // Bought Upgrades
    // Weapons
    public List<string> pugioUpgrades;
    public List<string> ulfberhtUpgrades;

    // Companions
    public List<string> loyalSphereUpgrades;
    public List<string> attackSquareUpgrades;

    // Armors
    public List<string> leatherUpgrades;
    public List<string> hideUpgrades;
    public List<string> ringMailUpgrades;
    public List<string> plateUpgrades;

    // Backs
    public List<string> angelWingsUpgrades;
    public List<string> steelWingsUpgrades;
    public List<string> backpackUpgrades;
    public List<string> capeOWindUpgrades;

    // In-layer
    public List<string> perks;
    
    public GameData()
    {
        // NPC Variables
        rickyStartComp = false;

        // Souls
        souls = 0;

        // In-game Statistics
        demonsKilled = 0;
        devilsKilled = 0;
        highestLayerReached = 0;
        totalLevelUps = 0;
        totalSoulsCollected = 0;

        // Selected Equipment
        primaryWeapon = null;
        secondaryWeapon = null;
        companion = null;
        armor = null;
        back = null;

        // Bought Equipment
        boughtWeapons = new();
        boughtCompanions = new();
        boughtBacks = new();
        boughtArmors = new();

        // Bought Souls
        attackSpeedSoulsBought = 0;
        damageSoulsBought = 0;
        defenceSoulsBought = 0;
        movementSpeedSoulsBought = 0;
        luckSoulsBought = 0;
        startLevelSoulsBought = 0;
        reRollSoulsBought = 0;
        reRollsSpent = 0;

        // Bought Upgrades
        // Weapons
        pugioUpgrades = new();
        ulfberhtUpgrades = new();

        // Companions
        loyalSphereUpgrades = new();
        attackSquareUpgrades = new();

        // Armors
        leatherUpgrades = new();
        hideUpgrades = new();
        ringMailUpgrades = new();
        plateUpgrades = new();

        // Backs
        angelWingsUpgrades = new();
        steelWingsUpgrades = new();
        backpackUpgrades = new();
        capeOWindUpgrades = new();

        // Equipment
        capeOWindCooldown = 0f;

        // Settings
        masterVolume = 0f;
        musicVolume = 0f;
        sfxVolume = 0f;
        screenMode = 0;

        // In-layer
        levelsGainedInLayer = 0;
        expGainedInLayer = 0f;
        expMultiplierInLayer = 0f;
        soulsCollectedInLayer = 0;
        demonsKilledInLayer = 0;
        devilsKilledInLayer = 0;
        healthInLayer = 100f;

        perks = new();

        // Music Length
        hubMenuMusicTime = 0f;
    }
}
