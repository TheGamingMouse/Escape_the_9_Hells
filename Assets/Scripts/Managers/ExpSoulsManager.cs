using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpSoulsManager : MonoBehaviour
{
    #region Instance
    
    public static ExpSoulsManager Instance;
    
    #endregion

    #region Events

    public delegate void ExpChangeHandler(int exp, string type);
    public static event ExpChangeHandler OnExpChange;

    public delegate void SoulsChangeHandler(int souls, bool fromLevel);
    public static event SoulsChangeHandler OnSoulsChange;

    #endregion

    #region Methods

    void Awake()
    {
        Instance = this;
    }

    public void AddExperience(int exp, string type)
    {
        OnExpChange?.Invoke(exp, type);
    }

    public void AddSouls(int souls, bool fromLevel)
    {
        OnSoulsChange?.Invoke(souls, fromLevel);
    }

    #endregion
}
