using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pugio : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float baseDamage = 12;
    public float damage;

    [Header("Bools")]
    public bool piercing;
    public bool canDamageEnemies;
    public bool specialAttacking;

    [Header("Components")]
    BasicEnemyAction enemyAction;
    public Weapon weapon;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        enemyAction = GetComponentInParent<BasicEnemyAction>();
    }

    void Update()
    {
        if (enemyAction != null)
        {
            damage = baseDamage / 2;
        }
        else if (weapon != null)
        {
            damage = baseDamage * weapon.damageMultiplier;
        }
    }

    #endregion

    #region Colliders

    void OnTriggerEnter(Collider coll)
    {
        if (piercing)
        {
            if (canDamageEnemies)
            {
                if (coll.TryGetComponent(out Ricky ricky))
                {
                    ricky.TakeDamage((int)damage);
                }
                
                if (coll.TryGetComponent(out BasicEnemyHealth eComp))
                {
                    eComp.TakeDamage((int)damage, false);
                }
                else if (coll.TryGetComponent(out ImpHealth iComp))
                {
                    iComp.TakeDamage((int)damage, false);
                }
            }

            if (coll.transform.CompareTag("Player") && coll.transform.TryGetComponent(out PlayerHealth pComp) && enemyAction.attacking)
            {
                pComp.TakeDamage((int)damage);
            }
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (specialAttacking)
        {
            if (canDamageEnemies)
            {
                if (coll.TryGetComponent(out Ricky ricky))
                {
                    ricky.TakeDamage((int)damage);
                }
                
                if (coll.TryGetComponent(out BasicEnemyHealth eComp))
                {
                    eComp.TakeDamage((int)damage, false);
                }
                else if (coll.TryGetComponent(out ImpHealth iComp))
                {
                    iComp.TakeDamage((int)damage, false);
                }
            }

            if (coll.transform.CompareTag("Player") && coll.transform.TryGetComponent(out PlayerHealth pComp) && enemyAction.attacking)
            {
                pComp.TakeDamage((int)damage);
            }
        }
    }

    #endregion
}
