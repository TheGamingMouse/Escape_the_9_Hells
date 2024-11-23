using System.Collections;
using System.Collections.Generic;
using SaveSystemSpace;
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
    public RickyController ricky;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var player = PlayerComponents.Instance.player;

        weapon = player.GetComponentInChildren<Weapon>();

        var soulData = SaveSystem.loadedSoulData;

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
        if (ricky && ricky.daggerGiven) weapon = PlayerComponents.Instance.player.GetComponentInChildren<Weapon>();

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
        var soulData = SaveSystem.loadedSoulData;

        switch (soul.title)
        {
            case SaveClasses.SoulData.attackSpeedString:
                weapon.attackSpeedMultiplier -= attackSpeedSouls.Count * attackSpeedMod;

                soulData.attackSpeedSoulsBought.Add(soul);

                weapon.attackSpeedMultiplier += attackSpeedSouls.Count * attackSpeedMod;
                break;
            
            case SaveClasses.SoulData.damageString:
                weapon.damageMultiplier -= damageSouls.Count * damageMod;

                soulData.damageSoulsBought.Add(soul);

                weapon.damageMultiplier += damageSouls.Count * damageMod;
                break;
            
            case SaveClasses.SoulData.defenceString:
                PlayerComponents.Instance.playerHealth.resistanceMultiplier -= defenceSouls.Count * defenceMod;

                soulData.defenceSoulsBought.Add(soul);
                
                PlayerComponents.Instance.playerHealth.resistanceMultiplier += defenceSouls.Count * defenceMod;
                break;
            
            case SaveClasses.SoulData.movementSpeedString:
                PlayerComponents.Instance.playerMovement.speedMultiplier -= movementSpeedSouls.Count * moveSpeedMod;

                soulData.movementSpeedSoulsBought.Add(soul);
                
                PlayerComponents.Instance.playerMovement.speedMultiplier += movementSpeedSouls.Count * moveSpeedMod;
                break;

            case SaveClasses.SoulData.luckString:
                PlayerComponents.Instance.playerLevel.luck -= luckSouls.Count * luckMod;

                soulData.luckSoulsBought.Add(soul);
                
                PlayerComponents.Instance.playerLevel.luck += luckSouls.Count * luckMod;
                break;

            case SaveClasses.SoulData.startLevelString:
                soulData.startLevelSoulsBought.Add(soul);
            
                PlayerComponents.Instance.playerLevel.startLevel = startLevelSouls.Count + 1;
                break;

            case SaveClasses.SoulData.reRollString:
                soulData.reRollSoulsBought.Add(soul);
                break;

            case SaveClasses.SoulData.pathFinderString:
                soulData.pathFinderSoulsBought.Add(soul);
            
                playerPathfinder = true;
                break;

            default:
                templateSouls.Add(soul);
                print("player has " + templateSouls.Count + " template perks");
                break;
        }

        SaveSystem.Instance.Save(soulData, SaveSystem.soulsDataPath);
    }

    #endregion
}
