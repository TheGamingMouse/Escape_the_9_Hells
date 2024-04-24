using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pugio : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    public readonly int damage = 6;

    [Header("Floats")]
    public float attackSpeed = 75f;

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
        if (coll.transform.CompareTag("Player") && coll.transform.TryGetComponent<PlayerHealth>(out PlayerHealth pComp) && enemyAction.attacking)
        {
            pComp.TakeDamage(damage);
        }
    }

    #endregion
}
