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
    
    public GameData()
    {
        souls = 0;
        demonsKilled = 0;
        devilsKilled = 0;
        highestLayerReached = 0;
        totalLevelUps = 0;
        totalSoulsCollected = 0;
    }
}
