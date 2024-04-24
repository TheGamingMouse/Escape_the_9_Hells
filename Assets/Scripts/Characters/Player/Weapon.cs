using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Properties

    [Header("Enum States")]
    public RangeState rState;
    public MeleeAttackType mType;

    [Header("GameObjects")]
    GameObject ulfSword;

    [Header("Components")]
    public SlashSword slashSword;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        ulfSword = slashSword.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (ulfSword.activeInHierarchy)
        {
            rState = RangeState.Melee;
            mType = MeleeAttackType.Slash;
        }
    }

    #endregion

    #region Enums

    public enum RangeState
    {
        Melee,
        ShortRange,
        LongRange
    }

    public enum MeleeAttackType
    {
        Slash,
        Pierce
    }

    #endregion
}
