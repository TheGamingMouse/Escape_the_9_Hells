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

    void Start()
    {
        ulfberht = GetComponentInParent<Ulfberht>();
    }

    void Update()
    {
        damage = ulfberht.damage;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (ulfberht.slashing)
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
    }

    void OnTriggerStay(Collider coll)
    {
        if (ulfberht.specialAttacking)
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
    }
}
