using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerks : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    float defence;
    float attackSpeed;
    float damage;
    float moveSpeed;
    readonly float defenceMod = 0.25f;
    readonly float attackSpeedMod = 0.25f;
    readonly float damageMod = 0.2f;
    readonly float moveSpeedMod = 0.075f;

    [Header("Lists")]
    public List<PerkItemsSO> templatePerks = new();
    public List<PerkItemsSO> defencePerks = new();
    public List<PerkItemsSO> attackSpeedPerks = new();
    public List<PerkItemsSO> damagePerks = new();
    public List<PerkItemsSO> moveSpeedPerks = new();

    [Header("Components")]
    PlayerHealth health;
    Weapon weapon;
    PlayerMovement movement;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        health = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        weapon = GameObject.FindWithTag("Player").GetComponentInChildren<Weapon>();
        movement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        defence = health.resistanceMultiplier;
        attackSpeed = weapon.attackSpeedMultiplier;
        damage = weapon.damageMultiplier;
        moveSpeed = movement.speedMultiplier;
    }

    #endregion

    #region General Methods

    public void AddPerk(PerkItemsSO perk)
    {
        if (perk.title == "Template Perk")
        {
            templatePerks.Add(perk);
            print("player has " + templatePerks.Count + " template perks");
        }
        else if (perk.title == "Defence Perk")
        {
            defencePerks.Add(perk);
            health.resistanceMultiplier = defence + defencePerks.Count * defenceMod;
        }
        else if (perk.title == "Attack Speed Perk")
        {
            attackSpeedPerks.Add(perk);
            weapon.attackSpeedMultiplier = attackSpeed + attackSpeedPerks.Count * attackSpeedMod;
        }
        else if (perk.title == "Damage Perk")
        {
            damagePerks.Add(perk);
            weapon.damageMultiplier = damage + damagePerks.Count * damageMod;
        }
        else if (perk.title == "Movement Speed Perk")
        {
            moveSpeedPerks.Add(perk);
            movement.speedMultiplier = moveSpeed + moveSpeedPerks.Count * moveSpeedMod;
        }
    }

    #endregion
}
