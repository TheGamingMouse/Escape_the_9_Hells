using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Properties

    [Header("Enum States")]
    public RangeState rState;
    public MeleeAttackType mType;

    [Header("Bools")]
    public bool canAttack;

    [Header("GameObjects")]
    GameObject ulfberhtObj;

    [Header("Components")]
    public Ulfberht ulfberht;

    [Header("Ulfberht")]
    SlashState sState;

    public bool slashing;
    bool nRotBool;
    bool sRot1Bool;
    bool sRot2Bool;
    bool sRotEndBool;

    public Quaternion nRot, sRot1, sRot2, sRotEnd;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        ulfberhtObj = ulfberht.gameObject;
    }

    void FixedUpdate()
    {
        // Melee Slash Switch
        if (slashing)
        {
            switch (sState)
            {
                case SlashState.NoSlash:
                    BeginSlash();
                    if (sRot1Bool)
                    {
                        sState = SlashState.StartSlash;
                    }
                    break;
                
                case SlashState.StartSlash:
                    MidSlash();
                    if (sRot2Bool)
                    {
                        sState = SlashState.MidSlash;
                    }
                    break;
                
                case SlashState.MidSlash:
                    EndSlash();
                    if (sRotEndBool)
                    {
                        sState = SlashState.EndSlash;
                    }
                    break;
                
                case SlashState.EndSlash:
                    slashing = false;
                    ulfberht.slashing = false;
                    break;
            }
        }
        else
        {
            switch (sState)
            {
                case SlashState.EndSlash:
                    MidSlash();
                    if (sRot2Bool)
                    {
                        sState = SlashState.MidSlash;
                    }
                    break;
                
                case SlashState.MidSlash:
                    BeginSlash();
                    if (sRot1Bool)
                    {
                        sState = SlashState.StartSlash;
                    }
                    break;
                
                case SlashState.StartSlash:
                    NoSlash();
                    if (nRotBool)
                    {
                        sState = SlashState.NoSlash;
                    }
                    break;
                
                case SlashState.NoSlash:
                    canAttack = true;
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ulfberhtObj.activeInHierarchy)
        {
            rState = RangeState.Melee;
            mType = MeleeAttackType.Slash;
        }
    }

    #endregion

    #region Attack Animations - Melee

    #region Ulberht

    public void Slash()
    {
        slashing = true;
        ulfberht.slashing = true;
        canAttack = false;
    }
    
    void BeginSlash()
    {
        ulfberht.transform.localRotation = Quaternion.Slerp(ulfberht.transform.localRotation, sRot1, ulfberht.slashSpeed * Time.deltaTime);

        if (ulfberht.transform.localRotation == sRot1)
        {
            nRotBool = false;
            sRot1Bool = true;
            sRot2Bool = false;
            sRotEndBool = false;
        }
    }

    void MidSlash()
    {
        ulfberht.transform.localRotation = Quaternion.Slerp(ulfberht.transform.localRotation, sRot2, ulfberht.slashSpeed * Time.deltaTime);

        if (ulfberht.transform.localRotation == sRot2)
        {
            nRotBool = false;
            sRot1Bool = false;
            sRot2Bool = true;
            sRotEndBool = false;
        }
    }

    void EndSlash()
    {
        ulfberht.transform.localRotation = Quaternion.Slerp(ulfberht.transform.localRotation, sRotEnd, ulfberht.slashSpeed * Time.deltaTime);

        if (ulfberht.transform.localRotation == sRotEnd)
        {
            nRotBool = false;
            sRot1Bool = false;
            sRot2Bool = false;
            sRotEndBool = true;
        }
    }

    void NoSlash()
    {
        ulfberht.transform.localRotation = Quaternion.Slerp(ulfberht.transform.localRotation, nRot, ulfberht.slashSpeed * Time.deltaTime);

        if (ulfberht.transform.localRotation == nRot)
        {
            nRotBool = true;
            sRot1Bool = false;
            sRot2Bool = false;
            sRotEndBool = false;
        }
    }

    #endregion Ulberht

    #endregion

    #region Enums

    #region General Enums

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

    #endregion General Enums

    #region Animation Enums

    enum SlashState
    {
        NoSlash,
        StartSlash,
        MidSlash,
        EndSlash
    }

    #endregion Animation Enums

    #endregion
}
