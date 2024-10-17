using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSouls : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    readonly int luckMod = 1;

    [Header("Floats")]
    readonly float attackSpeedMod = 0.1f;
    readonly float damageMod = 0.1f;
    readonly float defenceMod = 0.15f;
    readonly float moveSpeedMod = 0.05f;

    [Header("Bools")]
    bool soulsUpdated;
    public bool playerPathfinder;

    [Header("Lists")]
    public List<SoulsItemsSO> templateSouls = new();
    public List<SoulsItemsSO> attackSpeedSouls = new();
    public List<SoulsItemsSO> damageSouls = new();
    public List<SoulsItemsSO> defenceSouls = new();
    public List<SoulsItemsSO> movementSpeedSouls = new();
    public List<SoulsItemsSO> luckSouls = new();
    public List<SoulsItemsSO> startLevelSouls = new();
    public List<SoulsItemsSO> reRollSouls = new();

    [Header("Components")]
    Weapon weapon;
    public Ricky ricky;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var player = PlayerComponents.Instance.player;

        weapon = player.GetComponentInChildren<Weapon>();

        var soulData = SaveSystem.loadedSoulData;
        var persistentData = SaveSystem.loadedPersistentData;

        attackSpeedSouls = soulData.attackSpeedSoulsBought;
        damageSouls = soulData.damageSoulsBought;
        defenceSouls = soulData.defenceSoulsBought;
        movementSpeedSouls = soulData.movementSpeedSoulsBought;
        luckSouls = soulData.luckSoulsBought;
        startLevelSouls = soulData.startLevelSoulsBought;
        reRollSouls = soulData.reRollSoulsBought;
        playerPathfinder = soulData.pathFinderSoulsBought.Count == 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (ricky && ricky.daggerGiven)
        {
            weapon = PlayerComponents.Instance.player.GetComponentInChildren<Weapon>();
        }

        if (!soulsUpdated && weapon)
        {
            weapon.attackSpeedMultiplier += attackSpeedSouls.Count * attackSpeedMod;
            weapon.damageMultiplier += damageSouls.Count * damageMod;
            PlayerComponents.Instance.playerHealth.resistanceMultiplier += defenceSouls.Count * defenceMod;
            PlayerComponents.Instance.playerMovement.speedMultiplier += movementSpeedSouls.Count * moveSpeedMod;
            PlayerComponents.Instance.playerLevel.luck += luckSouls.Count * luckMod;
            PlayerComponents.Instance.playerLevel.startLevel = startLevelSouls.Count + 1;

            soulsUpdated = true;
        }
    }

    #endregion

    #region General Methods

    public void AddSouls(SoulsItemsSO soul)
    {
        if (soul.title == "Template Soul")
        {
            templateSouls.Add(soul);
            print("player has " + templateSouls.Count + " template perks");
        }
        else if (soul.title == "Attack Speed Soul")
        {
            weapon.attackSpeedMultiplier -= attackSpeedSouls.Count * attackSpeedMod;

            var soulData = SaveSystem.loadedSoulData;
            soulData.attackSpeedSoulsBought.Add(soul);
            
            SaveSystem.Instance.Save(soulData, SaveSystem.soulsDataPath);

            weapon.attackSpeedMultiplier += attackSpeedSouls.Count * attackSpeedMod;
        }
        else if (soul.title == "Damage Soul")
        {
            weapon.damageMultiplier -= damageSouls.Count * damageMod;

            var soulData = SaveSystem.loadedSoulData;
            soulData.attackSpeedSoulsBought.Add(soul);
            
            SaveSystem.Instance.Save(soulData, SaveSystem.soulsDataPath);

            weapon.damageMultiplier += damageSouls.Count * damageMod;

            SaveSystem.Instance.Save(soulData, SaveSystem.soulsDataPath);
        }
        else if (soul.title == "Defence Soul")
        {
            PlayerComponents.Instance.playerHealth.resistanceMultiplier -= defenceSouls.Count * defenceMod;

            var soulData = SaveSystem.loadedSoulData;
            soulData.defenceSoulsBought.Add(soul);
            
            PlayerComponents.Instance.playerHealth.resistanceMultiplier += defenceSouls.Count * defenceMod;

            SaveSystem.Instance.Save(soulData, SaveSystem.soulsDataPath);
        }
        else if (soul.title == "Movement Speed Soul")
        {
            PlayerComponents.Instance.playerMovement.speedMultiplier -= movementSpeedSouls.Count * moveSpeedMod;

            var soulData = SaveSystem.loadedSoulData;
            soulData.movementSpeedSoulsBought.Add(soul);
            
            PlayerComponents.Instance.playerMovement.speedMultiplier += movementSpeedSouls.Count * moveSpeedMod;

            SaveSystem.Instance.Save(soulData, SaveSystem.soulsDataPath);
        }
        else if (soul.title == "Luck Soul")
        {
            PlayerComponents.Instance.playerLevel.luck -= luckSouls.Count * luckMod;

            var soulData = SaveSystem.loadedSoulData;
            soulData.luckSoulsBought.Add(soul);
            
            PlayerComponents.Instance.playerLevel.luck += luckSouls.Count * luckMod;

            SaveSystem.Instance.Save(soulData, SaveSystem.soulsDataPath);
        }
        else if (soul.title == "Start Level Soul")
        {
            var soulData = SaveSystem.loadedSoulData;
            soulData.startLevelSoulsBought.Add(soul);
            
            PlayerComponents.Instance.playerLevel.startLevel = startLevelSouls.Count + 1;

            SaveSystem.Instance.Save(soulData, SaveSystem.soulsDataPath);
        }
        else if (soul.title == "Re Roll Soul")
        {
            var soulData = SaveSystem.loadedSoulData;
            soulData.reRollSoulsBought.Add(soul);
            

            SaveSystem.Instance.Save(soulData, SaveSystem.soulsDataPath);
        }
        else if (soul.title == "Path Finder Soul")
        {
            var soulData = SaveSystem.loadedSoulData;
            soulData.pathFinderSoulsBought.Add(soul);
            
            SaveSystem.Instance.Save(soulData, SaveSystem.soulsDataPath);
            
            playerPathfinder = true;
        }
    }

    #endregion
}
