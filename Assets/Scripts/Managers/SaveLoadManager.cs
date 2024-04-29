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

    [Header("Components")]
    PlayerLevel playerLevel;
    
    #endregion
    #region StartUpdate Methods

    void Start()
    {
        playerLevel = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>();

        if (CheckLayer() == -2 && SceneManager.GetActiveScene().buildIndex < 1)
        {
            Debug.LogWarning("Error occured when checking layer");
        }
        else
        {
            layerReached = CheckLayer();
        }
    }

    void Update()
    {
        
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
        
    }

    public void SaveData(ref GameData data)
    {
        data.demonsKilled += playerLevel.demonsKilled;
        data.devilsKilled += playerLevel.devilsKilled;
        if (layerReached > data.highestLayerReached && layerReached >= 0)
        {
            data.highestLayerReached = layerReached;
        }
        data.totalLevelUps += playerLevel.level - 1;
        data.souls += playerLevel.souls;
        data.totalSoulsCollected += playerLevel.souls;
    }

    #endregion
}
