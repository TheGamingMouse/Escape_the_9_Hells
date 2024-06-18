using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionComponentStorage : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int damage;

    [Header("Bools")]
    public bool canDamagePlayer;
    public bool canDamageEnemies;

    [Header("GameObjects")]
    public GameObject Ground;
    public GameObject Ground_dark;
    public GameObject Sphere;
    public GameObject Impact;
    public GameObject Fire_up;
    public GameObject Spark;

    #endregion

    #region Methods

    void OnTriggerEnter(Collider coll)
    {
        if (canDamagePlayer)
        {
            if (coll.TryGetComponent(out PlayerHealth playerComp))
            {
                playerComp.TakeDamage(damage);
            }
        }
        else if (canDamageEnemies)
        {
            if (coll.TryGetComponent(out BasicEnemyHealth basicComp))
            {
                basicComp.TakeDamage(damage, false);
            }
            else if (coll.TryGetComponent(out ImpHealth impComp))
            {
                impComp.TakeDamage(damage, false);
            }
        }
    }

    #endregion
}
