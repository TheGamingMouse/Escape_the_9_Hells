using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float baseCooldown = 2.5f;
    float cooldown;

    [Header("Bools")]
    bool canSwitch;
    bool primary;
    bool secondary;

    [Header("LoadoutItemsSOs")]
    LoadoutItemsSO primaryWeapon;
    LoadoutItemsSO secondaryWeapon;

    [Header("Components")]
    Weapon weapon;
    Backs back;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var player = PlayerComponents.Instance.player;

        weapon = player.GetComponentInChildren<Weapon>();
        back = player.GetComponentInChildren<Backs>();

        var equipmentData = SaveSystem.loadedEquipmentData;

        primaryWeapon = equipmentData.weaponData.primaryWeapon;
        secondaryWeapon = equipmentData.weaponData.secondaryWeapon;
        primary = equipmentData.backData.backpackPrimary;
        secondary = !primary;

        canSwitch = true;
    }

    // Update is called once per frame
    void Update()
    {
        var playerLoadout = PlayerComponents.Instance.playerLoadout;

        cooldown = baseCooldown / back.abilityCooldownMultiplier;

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
        var sfxManager = SFXAudioManager.Instance;

        if (primaryWeapon.title == "Ulfberht")
        {
            weapon.SwitchToUlfberht();
        }
        else if (primaryWeapon.title == "Pugio")
        {
            weapon.SwitchToPugio();
        }

        PlayerComponents.Instance.playerLoadout.selectedWeapon = primaryWeapon;
        PlayerComponents.Instance.playerUpgrades.weaponUpdated = false;

        sfxManager.PlayClip(sfxManager.backpackActivate, MasterAudioManager.Instance.sBlend2D, sfxManager.backVolumeMod, true);
    }

    void SwitchToSecondary()
    {
        var sfxManager = SFXAudioManager.Instance;

        if (secondaryWeapon.title == "Ulfberht")
        {
            weapon.SwitchToUlfberht();
        }
        else if (secondaryWeapon.title == "Pugio")
        {
            weapon.SwitchToPugio();
        }

        PlayerComponents.Instance.playerLoadout.selectedWeapon = secondaryWeapon;
        PlayerComponents.Instance.playerUpgrades.weaponUpdated = false;

        sfxManager.PlayClip(sfxManager.backpackActivate, MasterAudioManager.Instance.sBlend2D, sfxManager.backVolumeMod, true);
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
