using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PugioNormalAttack : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    [NonSerialized] public readonly float baseDamage = 12;
    [NonSerialized] public float damage;
    [NonSerialized] public float heavyCooldown;

    [Header("Bools")]
    public bool piercing;
    public bool canDamageEnemies;
    public bool heavyAttacking;
    public bool canAttack;
    public bool canHeavy;

    [Header("Transform")]
    Transform player;

    [Header("Components")]
    BasicEnemyAction enemyAction;
    public Weapon weapon;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        enemyAction = GetComponentInParent<BasicEnemyAction>();
        player = PlayerComponents.Instance.player;
        
        if (enemyAction != null) damage = baseDamage / 2;
        else if (weapon != null)
        {
            damage = baseDamage * weapon.damageMultiplier;
            heavyCooldown = weapon.baseHeavyCooldown / weapon.heavyCooldownMultiplier;
        }

        canAttack = true;
        canHeavy = true;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (piercing)
        {
            if (canDamageEnemies)
            {
                if (coll.TryGetComponent(out RickyController ricky)) ricky.rickyCombat.TakeDamage((int)damage);
                
                if (coll.TryGetComponent(out EnemyHealth eComp)) eComp.TakeDamage((int)damage, false);
            }

            if (coll.transform.CompareTag("Player") && coll.transform.TryGetComponent(out PlayerHealth pComp) && enemyAction.attacking) pComp.TakeDamage((int)damage);
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (heavyAttacking)
            if (canDamageEnemies)
            {
                if (coll.TryGetComponent(out RickyController ricky)) ricky.rickyCombat.TakeDamage((int)damage);
                
                if (coll.TryGetComponent(out EnemyHealth eComp)) eComp.TakeDamage((int)damage, false);
            }

        if (piercing && player && Vector3.Distance(coll.transform.position, player.position) < 1.3f)
            if (canDamageEnemies)
            {
                if (coll.TryGetComponent(out RickyController ricky)) ricky.rickyCombat.TakeDamage((int)damage / 2);
                
                if (coll.TryGetComponent(out EnemyHealth eComp)) eComp.TakeDamage((int)damage / 2, false);
            }
    }

    #endregion
}
