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
    GameObject capeOWindObj;
    GameObject seedBagObj;

    [Header("Lists")]
    readonly List<GameObject> backObjs = new();

    [Header("Components")]
    public AngelWings angelWings;
    public SteelWings steelWings;
    public Backpack backpack;
    PlayerLoadout playerLoadout;
    public CapeOWind capeOWind;
    public SeedBag seedBag;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        playerLoadout = GameObject.FindWithTag("Player").GetComponent<PlayerLoadout>();

        angelWingsObj = angelWings.gameObject;
        steelWingsObj = steelWings.gameObject;
        backpackObj = backpack.gameObject;
        capeOWindObj = capeOWind.gameObject;
        seedBagObj = seedBag.gameObject;

        backObjs.Add(angelWingsObj);
        backObjs.Add(steelWingsObj);
        backObjs.Add(backpackObj);
        backObjs.Add(capeOWindObj);
        backObjs.Add(seedBagObj);

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
        else if (backpackObj.activeInHierarchy)
        {
            bType = BackType.Packs;
        }
        else if (capeOWindObj.activeInHierarchy)
        {
            bType = BackType.Capes;
        }
        else if (seedBagObj.activeInHierarchy)
        {
            bType = BackType.Packs;
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

    public void SwitchToBackpack()
    {
        bActive = BackActive.BackPack;

        DisableElements();

        if (backpackObj)
        {
            backpackObj.SetActive(true);
        }

        playerLoadout.backpackActive = true;
    }

    public void SwitchToCapeOWind()
    {
        bActive = BackActive.CapeOWind;

        DisableElements();

        if (capeOWindObj)
        {
            capeOWindObj.SetActive(true);
        }
    }

    public void SwitchToSeedBag()
    {
        bActive = BackActive.SeedBag;

        DisableElements();

        if (seedBagObj)
        {
            seedBagObj.SetActive(true);
        }

        playerLoadout.seedBagActive = true;
    }

    void DisableElements()
    {
        foreach (GameObject backObj in backObjs)
        {
            backObj.SetActive(false);
        }
        angelWings.active = false;
        steelWings.active = false;
        playerLoadout.backpackActive = false;
        playerLoadout.seedBagActive = false;
    }

    #endregion

    #region Enums

    public enum BackType
    {
        None,
        Wings,
        Packs,
        Capes
    }

    public enum BackActive
    {
        None,
        AngelWings,
        SteelWings,
        BackPack,
        CapeOWind,
        SeedBag
    }

    #endregion
}
