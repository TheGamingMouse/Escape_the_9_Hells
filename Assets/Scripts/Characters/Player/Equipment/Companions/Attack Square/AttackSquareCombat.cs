using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSquareCombat : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    readonly int damage = 5;

    [Header("Floats")]
    readonly float cooldown = 1.5f;

    [Header("Bools")]
    bool canAttack = true;

    #endregion

    #region General Methods

    void OnCollisionStay(Collision coll)
    {
        if (coll.transform.TryGetComponent<EnemyHealth>(out EnemyHealth eComp) && canAttack)
        {
            eComp.TakeDamage(damage);
            StartCoroutine(DamageRoutine());
        }
    }

    IEnumerator DamageRoutine()
    {
        canAttack = false;

        yield return new WaitForSeconds(cooldown);

        canAttack = true;
    }

    #endregion
}
