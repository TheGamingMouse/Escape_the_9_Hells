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
    public LoadoutItemsSO upperArmor;
    public LoadoutItemsSO lowerArmor;

    public List<LoadoutItemsSO> boughtWeapons;
    public List<LoadoutItemsSO> boughtCompanions;
    public List<LoadoutItemsSO> boughtUpperArmors;
    public List<LoadoutItemsSO> boughtLowerArmors;
    
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
        upperArmor = null;
        lowerArmor = null;

        boughtWeapons = new();
        boughtCompanions = new();
        boughtLowerArmors = new();
        boughtUpperArmors = new();
    }
}
