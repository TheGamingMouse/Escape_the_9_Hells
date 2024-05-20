using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    [Header("Ints")]
    // Souls
    public int souls;
    
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

    [Header("Bools")]
    // NPC Variables
    public bool rickyStartComp;

    [Header("Strings")]
    // Selected Equipment
    public string weapon;
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
        weapon = null;
        companion = null;
        armor = null;
        back = null;

        // Bought Equipment
        boughtWeapons = new();
        boughtCompanions = new();
        boughtBacks = new();
        boughtArmors = new();

        // Bought Souls
        attackSpeedSoulsBought = new();
        damageSoulsBought = new();
        defenceSoulsBought = new();
        movementSpeedSoulsBought = new();

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
    }
}
