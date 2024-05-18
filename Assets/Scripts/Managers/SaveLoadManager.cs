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

    [Header("Lists")]
    public List<LoadoutItemsSO> boughtWeapons = new();
    public List<LoadoutItemsSO> boughtCompanions = new();
    public List<LoadoutItemsSO> boughtArmors = new();
    public List<LoadoutItemsSO> boughtBacks = new();

    public List<SoulsItemsSO> attackSpeedSoulsBought = new();
    public List<SoulsItemsSO> damageSoulsBought = new();
    public List<SoulsItemsSO> defenceSoulsBought = new();
    public List<SoulsItemsSO> movementSpeedSoulsBought = new();

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
    
    #endregion

    #region StartUpdate Methods

    void Start()
    {
        playerLevel = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>();
        playerLoadout = GameObject.FindWithTag("Player").GetComponent<PlayerLoadout>();
        playerEquipment = GameObject.FindWithTag("Player").GetComponent<PlayerEquipment>();
        playerSouls = GameObject.FindWithTag("Player").GetComponent<PlayerSouls>();

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
            rickyStartComp = data.rickyStartComp;

            if (hub)
            {
                expSoulsManager = GameObject.FindWithTag("Managers").GetComponent<ExpSoulsManager>();
                expSoulsManager.AddSouls(data.souls, true);
            }

            weapon = data.weapon;
            companion = data.companion;
            armor = data.armor;
            back = data.back;

            boughtWeapons = data.boughtWeapons;
            boughtCompanions = data.boughtCompanions;
            boughtArmors = data.boughtArmors;
            boughtBacks = data.boughtBacks;

            attackSpeedSoulsBought = data.attackSpeedSoulsBought;
            damageSoulsBought = data.damageSoulsBought;
            defenceSoulsBought = data.defenceSoulsBought;
            movementSpeedSoulsBought = data.movementSpeedSoulsBought;
        }
    }

    public void SaveData(ref GameData data)
    {
        if (!mainMenu)
        {
            data.demonsKilled += playerLevel.demonsKilled;
            data.devilsKilled += playerLevel.devilsKilled;
            if (layerReached > data.highestLayerReached && layerReached >= 0)
            {
                data.highestLayerReached = layerReached;
            }
            data.totalLevelUps += playerLevel.level - 1;
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

            data.rickyStartComp = rickyStartComp;

            data.weapon = playerLoadout.selectedWeapon;
            data.companion = playerLoadout.selectedCompanion;
            data.armor = playerLoadout.selectedArmor;
            data.back = playerLoadout.selectedBack;

            data.boughtWeapons = playerEquipment.boughtWeapons;
            data.boughtCompanions = playerEquipment.boughtCompanions;
            data.boughtArmors = playerEquipment.boughtArmors;
            data.boughtBacks = playerEquipment.boughtBacks;

            data.attackSpeedSoulsBought = playerSouls.attackSpeedSouls;
            data.damageSoulsBought = playerSouls.damageSouls;
            data.defenceSoulsBought = playerSouls.defenceSouls;
            data.movementSpeedSoulsBought = playerSouls.movementSpeedSouls;
        }

        // Settings to save
    }

    #endregion
}
