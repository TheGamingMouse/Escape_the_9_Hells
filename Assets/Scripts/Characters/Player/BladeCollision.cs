using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BladeCollision : MonoBehaviour
{
    int damage;
    Ulfberht ulfberht;

    void Start()
    {
        damage = GetComponentInParent<Ulfberht>().damage;
        ulfberht = GetComponentInParent<Ulfberht>();
    }

    void OnTriggerStay(Collider coll)
    {
        if (ulfberht.slashing)
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
    }
}
