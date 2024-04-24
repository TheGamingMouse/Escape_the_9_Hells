using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSword : MonoBehaviour
{
    #region Properties

    [Header("Enum States")]
    SlashState sState;

    [Header("Ints")]
    public readonly int damage = 12;

    [Header("Floats")]
    readonly float slashSpeed = 100f;

    [Header("Bools")]
    public bool slashing;
    bool nRotBool;
    bool sRot1Bool;
    bool sRot2Bool;
    bool sRotEndBool;

    [Header("Quaternions")]
    public Quaternion nRot, sRot1, sRot2, sRotEnd;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
                    break;
            }
        }
    }

    #endregion

    #region General Methods

    public void Slash()
    {
        slashing = true;
    }

    void BeginSlash()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, sRot1, slashSpeed * Time.deltaTime);

        if (transform.localRotation == sRot1)
        {
            nRotBool = false;
            sRot1Bool = true;
            sRot2Bool = false;
            sRotEndBool = false;
        }
    }

    void MidSlash()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, sRot2, slashSpeed * Time.deltaTime);

        if (transform.localRotation == sRot2)
        {
            nRotBool = false;
            sRot1Bool = false;
            sRot2Bool = true;
            sRotEndBool = false;
        }
    }

    void EndSlash()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, sRotEnd, slashSpeed * Time.deltaTime);

        if (transform.localRotation == sRotEnd)
        {
            nRotBool = false;
            sRot1Bool = false;
            sRot2Bool = false;
            sRotEndBool = true;
        }
    }

    void NoSlash()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, nRot, slashSpeed * Time.deltaTime);

        if (transform.localRotation == nRot)
        {
            nRotBool = true;
            sRot1Bool = false;
            sRot2Bool = false;
            sRotEndBool = false;
        }
    }

    #endregion

    #region Enums

    enum SlashState
    {
        NoSlash,
        StartSlash,
        MidSlash,
        EndSlash
    }

    #endregion
}
