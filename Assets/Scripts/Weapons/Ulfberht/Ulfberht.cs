using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ulfberht : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float baseDamage = 12;
    public float damage;

    [Header("Bools")]
    public bool slashing;
    public bool canDamageEnemies;

    [Header("Components")]
    public Weapon weapon;

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (weapon != null)
        {
            damage = baseDamage * weapon.damageMultiplier;
        }
    }

    #endregion
}
