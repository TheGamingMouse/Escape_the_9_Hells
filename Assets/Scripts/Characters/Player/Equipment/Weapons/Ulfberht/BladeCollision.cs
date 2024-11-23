using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BladeCollision : MonoBehaviour
{
    [Header("Floats")]
    public float damage;

    [Header("Transform")]
    public Transform player;

    [Header("Components")]
    UlfberhtAttack ulfberht;

    void Start()
    {
        ulfberht = GetComponentInParent<UlfberhtAttack>();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (ulfberht.slashing)
        {
            if (coll.TryGetComponent(out RickyController ricky)) ricky.rickyCombat.TakeDamage((int)damage);

            if (coll.TryGetComponent(out EnemyHealth eComp)) eComp.TakeDamage((int)damage, false);
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (ulfberht.heavyAttacking)
        {
            if (coll.TryGetComponent(out RickyController ricky)) ricky.rickyCombat.TakeDamage((int)damage);

            if (coll.TryGetComponent(out EnemyHealth eComp)) eComp.TakeDamage((int)damage, false);
        }

        if (ulfberht.slashing && Vector3.Distance(coll.transform.position, player.position) < 1.1f)
        {
            if (coll.TryGetComponent(out RickyController ricky)) ricky.rickyCombat.TakeDamage((int)damage / 2);

            if (coll.TryGetComponent(out EnemyHealth eComp)) eComp.TakeDamage((int)damage / 2, false);
        }
    }
}
