using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAura : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int damage;

    #endregion
    
    #region General Methods

    void OnTriggerStay(Collider coll)
    {
        if (coll.TryGetComponent(out EnemyHealth eComp))
        {
            eComp.TakeDamage(damage, true);
        }
    }

    #endregion
}
