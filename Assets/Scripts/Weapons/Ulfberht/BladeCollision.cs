using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BladeCollision : MonoBehaviour
{
    [Header("Ints")]
    int damage;

    [Header("Components")]
    Ulfberht ulfberht;
    EnemyAction enemyAction;

    void Start()
    {
        damage = GetComponentInParent<Ulfberht>().damage;
        ulfberht = GetComponentInParent<Ulfberht>();
        enemyAction = GetComponentInParent<EnemyAction>();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (ulfberht.slashing)
        {
            if (ulfberht.canDamageEnemies)
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
}
