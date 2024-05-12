using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BladeCollision : MonoBehaviour
{
    [Header("Floats")]
    float damage;

    [Header("Components")]
    Ulfberht ulfberht;
    EnemyAction enemyAction;

    void Start()
    {
        ulfberht = GetComponentInParent<Ulfberht>();
        enemyAction = GetComponentInParent<EnemyAction>();
    }

    void Update()
    {
        damage = ulfberht.damage;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (ulfberht.slashing)
        {
            if (ulfberht.canDamageEnemies)
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
}
