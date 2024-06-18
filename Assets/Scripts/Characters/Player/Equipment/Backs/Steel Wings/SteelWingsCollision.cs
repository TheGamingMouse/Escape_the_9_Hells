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
            if (coll.TryGetComponent(out BasicEnemyHealth eComp))
            {
                eComp.TakeDamage(wings.damage, false);
            }
            else if (coll.TryGetComponent(out ImpHealth iComp))
            {
                iComp.TakeDamage(wings.damage, false);
            }
        }
    }

    #endregion
}
