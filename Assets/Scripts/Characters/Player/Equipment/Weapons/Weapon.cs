using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
    public WeaponActive wActive;

    [Header("Floats")]
    public float attackSpeedMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float heavyCooldownMultiplier = 1f;
    public float baseHeavyCooldown = 10f;

    [Header("Bools")]
    public bool heavyAttack;

    [Header("GameObjects")]
    public GameObject ulfberhtObj;
    public GameObject pugioObj;

    [Header("Lists")]
    readonly Dictionary<WeaponActive, GameObject> weapons = new();

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        weapons.Add(WeaponActive.Ulfberht, ulfberhtObj);
        weapons.Add(WeaponActive.Pugio, pugioObj);

        SwitchWeapon(wActive);
    }

    #endregion

    #region General Methods

    public void SwitchWeapon(WeaponActive weapon)
    {
        wActive = weapon;

        foreach (GameObject obj in weapons.Values) 
            if (obj != weapons[weapon]) obj.SetActive(false);
            else obj.SetActive(true);
    }

    #endregion

    #region Enums

    public enum WeaponActive
    {
        Ulfberht,
        Pugio
    }

    #endregion
}
