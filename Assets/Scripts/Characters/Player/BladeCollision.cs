using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BladeCollision : MonoBehaviour
{
    int damage;
    SlashSword slashSword;

    void Start()
    {
        damage = GetComponentInParent<SlashSword>().damage;
        slashSword = GetComponentInParent<SlashSword>();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (slashSword.slashing)
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
