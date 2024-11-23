using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UlfberhtAttack : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public readonly float baseDamage = 12;
    public float damage;
    public float heavyCooldown;

    [Header("Bools")]
    public bool slashing;
    public bool heavyAttacking;
    public bool canAttack;
    public bool canHeavy;

    [Header("Transform")]
    Transform player;

    [Header("Components")]
    public Weapon weapon;
    BladeCollision blade;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (weapon != null)
        {
            damage = baseDamage * weapon.damageMultiplier;
            
            player = PlayerComponents.Instance.player;
            blade = GetComponentInChildren<BladeCollision>();
            blade.player = player;
            blade.damage = damage;

            heavyCooldown = weapon.baseHeavyCooldown / weapon.heavyCooldownMultiplier;

            canAttack = true;
            canHeavy = true;
        }
    }
}
