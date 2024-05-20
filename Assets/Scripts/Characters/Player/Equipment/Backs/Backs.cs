using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backs : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
    public BackType bType;
    public BackActive bActive;

    [Header("Floats")]
    public float abilityCooldownMultiplier = 1f;

    [Header("GameObjects")]
    GameObject angelWingsObj;
    GameObject steelWingsObj;

    [Header("Lists")]
    readonly List<GameObject> backObjs = new();

    [Header("Components")]
    public AngelWings angelWings;
    public SteelWings steelWings;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        angelWingsObj = angelWings.gameObject;
        steelWingsObj = steelWings.gameObject;

        backObjs.Add(angelWingsObj);
        backObjs.Add(steelWingsObj);

        SwitchToNone();
    }

    // Update is called once per frame
    void Update()
    {
        if (angelWingsObj.activeInHierarchy)
        {
            bType = BackType.Wings;
        }
        else if (steelWingsObj.activeInHierarchy)
        {
            bType = BackType.Wings;
        }
        else
        {
            bType = BackType.None;
        }
    }

    #endregion

    #region Companion Swap

    public void SwitchToNone()
    {
        bActive = BackActive.None;

        DisableElements();
    }

    public void SwitchToAngelWings()
    {
        bActive = BackActive.AngelWings;

        DisableElements();
        
        if (angelWingsObj)
        {
            angelWingsObj.SetActive(true);
        }
    }

    public void SwitchToSteelWings()
    {
        bActive = BackActive.SteelWings;

        DisableElements();

        if (steelWingsObj)
        {
            steelWingsObj.SetActive(true);
        }
    }

    void DisableElements()
    {
        foreach (GameObject backObj in backObjs)
        {
            backObj.SetActive(false);
        }
        angelWings.active = false;
    }

    #endregion

    #region Enums

    public enum BackType
    {
        None,
        Wings,
        Packs
    }

    public enum BackActive
    {
        None,
        AngelWings,
        SteelWings,
        BackPack
    }

    #endregion
}
