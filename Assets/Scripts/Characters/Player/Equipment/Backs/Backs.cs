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
    GameObject backpackObj;

    [Header("Lists")]
    readonly List<GameObject> backObjs = new();

    [Header("Components")]
    public AngelWings angelWings;
    public SteelWings steelWings;
    // public BackPack backpack;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        angelWingsObj = angelWings.gameObject;
        steelWingsObj = steelWings.gameObject;
        // backpackObj = backpack.gameObject;

        backObjs.Add(angelWingsObj);
        backObjs.Add(steelWingsObj);
        // backObjs.Add(backpackObj);

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
        // else if (backpackObj.activeInHierarchy)
        // {
        //     bType = BackType.Packs;
        // }
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

    public void SwitchToBackPack()
    {
        bActive = BackActive.BackPack;

        DisableElements();

        if (backpackObj)
        {
            backpackObj.SetActive(true);
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
