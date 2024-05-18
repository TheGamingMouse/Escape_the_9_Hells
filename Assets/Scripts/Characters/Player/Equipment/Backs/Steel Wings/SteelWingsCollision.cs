using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelWingsCollision : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    public SteelWings wings;

    #endregion

    #region Methods

    void OnTriggerEnter(Collider coll)
    {
        if (wings.canDamage)
        {
            if (coll.TryGetComponent<EnemyHealth>(out EnemyHealth eComp))
            {
                eComp.TakeDamage(wings.damage);
            }
        }
    }

    #endregion
}
