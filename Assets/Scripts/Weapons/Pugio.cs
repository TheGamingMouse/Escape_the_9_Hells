using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pugio : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float baseDamage = 6;
    public float damage;

    [Header("Bools")]
    public bool piercing;
    public bool canDamageEnemies;

    [Header("Components")]
    EnemyAction enemyAction;
    public Weapon weapon;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        enemyAction = GetComponentInParent<EnemyAction>();
    }

    void Update()
    {
        if (enemyAction != null)
        {
            damage = baseDamage;
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
                if (coll.TryGetComponent<Ricky>(out Ricky ricky))
                {
                    ricky.TakeDamage((int)damage);
                }
                
                if (coll.TryGetComponent<EnemyHealth>(out EnemyHealth eComp))
                {
                    eComp.TakeDamage((int)damage);
                }
            }

            if (coll.transform.CompareTag("Player") && coll.transform.TryGetComponent<PlayerHealth>(out PlayerHealth pComp) && enemyAction.attacking)
            {
                pComp.TakeDamage((int)damage);
            }
        }
    }

    #endregion
}
