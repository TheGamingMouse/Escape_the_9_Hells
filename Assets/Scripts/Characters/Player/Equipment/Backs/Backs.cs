using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backs : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
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
    readonly Dictionary<Backs.BackActive, GameObject> backs = new();

    [Header("Components")]
    public AngelWings angelWings;
    public SteelWings steelWings;
    public Backpack backpack;
    public CapeOWind capeOWind;
    public SeedBag seedBag;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        angelWingsObj = angelWings.gameObject;
        steelWingsObj = steelWings.gameObject;
        backpackObj = backpack.gameObject;
        capeOWindObj = capeOWind.gameObject;
        seedBagObj = seedBag.gameObject;

        backs.Add(BackActive.AngelWings, angelWingsObj);
        backs.Add(BackActive.SteelWings, steelWingsObj);
        backs.Add(BackActive.Backpack, backpackObj);
        backs.Add(BackActive.CapeOWind, capeOWindObj);
        backs.Add(BackActive.SeedBag, seedBagObj);

        SwitchBack(BackActive.None);
    }

    #endregion

    #region Companion Swap

    public void SwitchBack(BackActive back)
    {
        bActive = back;
        DisableElements();
        if (back != BackActive.None) foreach (GameObject obj in backs.Values) if (obj == backs[back]) obj.SetActive(true);

        switch (back)
        {
            case BackActive.Backpack: PlayerComponents.Instance.playerLoadout.backpackActive = true; break;
            case BackActive.SeedBag: PlayerComponents.Instance.playerLoadout.seedBagActive = true; break;
        }
    }

    void DisableElements()
    {
        foreach (GameObject obj in backs.Values) obj.SetActive(false);
        angelWings.active = false;
        steelWings.active = false;
        PlayerComponents.Instance.playerLoadout.backpackActive = false;
        PlayerComponents.Instance.playerLoadout.seedBagActive = false;
    }

    #endregion

    #region Enums

    public enum BackActive
    {
        None,
        AngelWings,
        SteelWings,
        Backpack,
        CapeOWind,
        SeedBag
    }

    #endregion
}
