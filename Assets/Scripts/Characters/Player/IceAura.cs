using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAura : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int damage;

    [Header("Floats")]
    public float speedPenalty = 1f;

    #endregion
    
    #region General Methods

    void OnTriggerStay(Collider coll)
    {
        if (coll.TryGetComponent(out EnemyHealth eComp))
        {
            eComp.TakeDamage(damage, true);
            if (TryGetComponent(out BasicEnemyMovement _) && !eComp.GetComponent<BasicEnemyMovement>().slowed)
                StartCoroutine(eComp.GetComponent<BasicEnemyMovement>().SlowEnemy(speedPenalty));
            else if (TryGetComponent(out ImpMovement _) && !eComp.GetComponent<ImpMovement>().slowed)
                StartCoroutine(eComp.GetComponent<ImpMovement>().SlowEnemy(speedPenalty));
        }
    }

    #endregion
}
