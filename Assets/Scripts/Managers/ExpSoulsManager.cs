using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpSoulsManager : MonoBehaviour
{
    #region Events

    public delegate void ExpChangeHandler(int exp);
    public static event ExpChangeHandler OnExpChange;

    public delegate void SoulsChangeHandler(int souls);
    public static event SoulsChangeHandler OnSoulsChange;

    #endregion

    #region Methods

    public void AddExperience(int exp)
    {
        OnExpChange?.Invoke(exp);
    }

    public void AddSouls(int souls)
    {
        OnSoulsChange?.Invoke(souls);
    }

    #endregion
}
