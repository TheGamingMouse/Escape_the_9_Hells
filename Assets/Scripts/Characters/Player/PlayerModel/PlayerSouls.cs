using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSouls : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float attackSpeedMod = 0.1f;
    readonly float damageMod = 0.1f;
    readonly float defenceMod = 0.15f;
    readonly float moveSpeedMod = 0.05f;

    [Header("Bools")]
    bool soulsUpdated;

    [Header("Lists")]
    public List<SoulsItemsSO> templateSouls = new();
    public List<SoulsItemsSO> attackSpeedSouls = new();
    public List<SoulsItemsSO> damageSouls = new();
    public List<SoulsItemsSO> defenceSouls = new();
    public List<SoulsItemsSO> movementSpeedSouls = new();

    [Header("Components")]
    SaveLoadManager slManager;
    PlayerHealth health;
    Weapon weapon;
    PlayerMovement movement;
    Ricky ricky;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        slManager = GameObject.FindWithTag("Managers").GetComponent<SaveLoadManager>();
        health = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        weapon = GameObject.FindWithTag("Player").GetComponentInChildren<Weapon>();
        movement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        if (GameObject.FindWithTag("NPC") != null && GameObject.FindWithTag("NPC").transform.Find("Ricky").TryGetComponent<Ricky>(out Ricky rickyCmop))
        {
            ricky = rickyCmop;
        }

        attackSpeedSouls = slManager.attackSpeedSoulsBought;
        damageSouls = slManager.damageSoulsBought;
        defenceSouls = slManager.defenceSoulsBought;
        movementSpeedSouls = slManager.movementSpeedSoulsBought;
    }

    // Update is called once per frame
    void Update()
    {
        if (ricky != null && ricky.daggerGiven)
        {
            health = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
            weapon = GameObject.FindWithTag("Player").GetComponentInChildren<Weapon>();
            movement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        }

        if (!soulsUpdated)
        {
            weapon.attackSpeedMultiplier += attackSpeedSouls.Count * attackSpeedMod;
            weapon.damageMultiplier += damageSouls.Count * damageMod;
            health.resistanceMultiplier += defenceSouls.Count * defenceMod;
            movement.speedMultiplier += movementSpeedSouls.Count * moveSpeedMod;

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

            attackSpeedSouls.Add(soul);
            weapon.attackSpeedMultiplier += attackSpeedSouls.Count * attackSpeedMod;
        }
        else if (soul.title == "Damage Soul")
        {
            weapon.damageMultiplier -= damageSouls.Count * damageMod;

            damageSouls.Add(soul);
            weapon.damageMultiplier += damageSouls.Count * damageMod;
        }
        else if (soul.title == "Defence Soul")
        {
            health.resistanceMultiplier -= defenceSouls.Count * defenceMod;

            defenceSouls.Add(soul);
            health.resistanceMultiplier += defenceSouls.Count * defenceMod;
        }
        else if (soul.title == "Movement Speed Soul")
        {
            movement.speedMultiplier -= movementSpeedSouls.Count * moveSpeedMod;

            movementSpeedSouls.Add(soul);
            movement.speedMultiplier += movementSpeedSouls.Count * moveSpeedMod;
        }
    }

    #endregion
}
