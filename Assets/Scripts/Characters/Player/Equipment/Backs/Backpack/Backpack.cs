using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float baseCooldown = 10f;
    float cooldown;

    [Header("Bools")]
    bool canSwitch;
    bool primary;
    bool secondary;

    [Header("LoadoutItemsSOs")]
    [SerializeField] LoadoutItemsSO primaryWeapon;
    [SerializeField] LoadoutItemsSO secondaryWeapon;

    [Header("Components")]
    PlayerUpgrades playerUpgrades;
    PlayerLoadout playerLoadout;
    Weapon playerWeapon;
    SaveLoadManager slManager;
    Backs playerBacks;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindWithTag("Player");

        playerUpgrades = player.GetComponent<PlayerUpgrades>();
        playerLoadout = player.GetComponent<PlayerLoadout>();
        playerWeapon = player.GetComponentInChildren<Weapon>();
        playerBacks = player.GetComponentInChildren<Backs>();

        slManager = GameObject.FindWithTag("Managers").GetComponent<SaveLoadManager>();

        primaryWeapon = slManager.primaryWeapon;
        secondaryWeapon = slManager.secondaryWeapon;

        canSwitch = true;
        primary = true;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = baseCooldown / playerBacks.abilityCooldownMultiplier;

        if (playerLoadout.backpackActive)
        {
            primaryWeapon = playerLoadout.selectedPrimaryWeapon;
            secondaryWeapon = playerLoadout.selectedSecondaryWeapon;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && primaryWeapon && canSwitch && secondary)
        {
            SwitchToPrimary();

            StartCoroutine(CooldownRoutine());

            primary = true;
            secondary = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && secondaryWeapon && canSwitch && primary)
        {
            SwitchToSecondary();

            StartCoroutine(CooldownRoutine());

            primary = false;
            secondary = true;
        }
    }

    #endregion

    #region General Methods

    void SwitchToPrimary()
    {
        if (primaryWeapon.title == "Ulfberht")
        {
            playerWeapon.SwitchToUlfberht();
        }
        else if (primaryWeapon.title == "Pugio")
        {
            playerWeapon.SwitchToPugio();
        }

        playerLoadout.selectedWeapon = primaryWeapon;
        playerUpgrades.weaponUpdated = false;
    }

    void SwitchToSecondary()
    {
        if (secondaryWeapon.title == "Ulfberht")
        {
            playerWeapon.SwitchToUlfberht();
        }
        else if (secondaryWeapon.title == "Pugio")
        {
            playerWeapon.SwitchToPugio();
        }

        playerLoadout.selectedWeapon = secondaryWeapon;
        playerUpgrades.weaponUpdated = false;
    }

    IEnumerator CooldownRoutine()
    {
        canSwitch = false;
        GetComponent<MeshRenderer>().material.color = new Color(1f, 0.39f, 0.39f, 1f);

        yield return new WaitForSeconds(cooldown);

        canSwitch = true;
        GetComponent<MeshRenderer>().material.color = Color.white;
    }

    #endregion
}
