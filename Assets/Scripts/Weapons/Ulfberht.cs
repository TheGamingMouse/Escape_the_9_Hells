using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ulfberht : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    public readonly int damage = 12;

    [Header("Floats")]
    public readonly float slashSpeed = 100f;
    public readonly float cooldown = 0.5f;

    [Header("Bools")]
    public bool slashing;

    #endregion
}
