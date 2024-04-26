using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pugio : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    public readonly int damage = 6;

    [Header("Bools")]
    public bool piercing;
    public bool canDamageEnemies;

    [Header("Components")]
    EnemyAction enemyAction;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        enemyAction = GetComponentInParent<EnemyAction>();
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
                    ricky.TakeDamage(damage);
                }
                
                if (coll.TryGetComponent<EnemyHealth>(out EnemyHealth eComp))
                {
                    eComp.TakeDamage(damage);
                }
            }

            if (coll.transform.CompareTag("Player") && coll.transform.TryGetComponent<PlayerHealth>(out PlayerHealth pComp) && enemyAction.attacking)
            {
                pComp.TakeDamage(damage);
            }
        }
    }

    #endregion
}
