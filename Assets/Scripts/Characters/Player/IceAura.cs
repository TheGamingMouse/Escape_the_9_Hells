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
        if (coll.TryGetComponent(out BasicEnemyHealth beComp))
        {
            beComp.TakeDamage(damage, true);
            if (!beComp.GetComponent<BasicEnemyMovement>().slowed)
            {
                StartCoroutine(beComp.GetComponent<BasicEnemyMovement>().SlowEnemy(speedPenalty));
            }
        }
        else if (coll.TryGetComponent(out ImpHealth iComp))
        {
            iComp.TakeDamage(damage, true);
            if (!iComp.GetComponent<ImpMovement>().slowed)
            {
                StartCoroutine(iComp.GetComponent<ImpMovement>().SlowEnemy(speedPenalty));
            }
        }
    }

    #endregion
}
