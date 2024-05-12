using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int souls;
    public int demonsKilled;
    public int devilsKilled;
    public int highestLayerReached;
    public int totalLevelUps;
    public int totalSoulsCollected;

    public bool rickyStartComp;

    public LoadoutItemsSO weapon;
    public LoadoutItemsSO companion;
    public LoadoutItemsSO armor;
    public LoadoutItemsSO TBD;

    public List<LoadoutItemsSO> boughtWeapons;
    public List<LoadoutItemsSO> boughtCompanions;
    public List<LoadoutItemsSO> boughtArmors;
    public List<LoadoutItemsSO> boughtTBDs;

    public List<SoulsItemsSO> attackSpeedSoulsBought;
    public List<SoulsItemsSO> damageSoulsBought;
    public List<SoulsItemsSO> defenceSoulsBought;
    public List<SoulsItemsSO> movementSpeedSoulsBought;
    
    public GameData()
    {
        souls = 0;
        demonsKilled = 0;
        devilsKilled = 0;
        highestLayerReached = 0;
        totalLevelUps = 0;
        totalSoulsCollected = 0;

        rickyStartComp = false;

        weapon = null;
        companion = null;
        armor = null;
        TBD = null;

        boughtWeapons = new();
        boughtCompanions = new();
        boughtTBDs = new();
        boughtArmors = new();

        attackSpeedSoulsBought = new();
        damageSoulsBought = new();
        defenceSoulsBought = new();
        movementSpeedSoulsBought = new();
    }
}
