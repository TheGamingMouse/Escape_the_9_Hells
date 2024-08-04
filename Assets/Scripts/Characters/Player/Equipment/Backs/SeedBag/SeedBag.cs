using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBag : MonoBehaviour
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
    LoadoutItemsSO primaryCompanion;
    LoadoutItemsSO secondaryCompanion;

    [Header("Components")]
    PlayerUpgrades playerUpgrades;
    PlayerLoadout playerLoadout;
    Companion companion;
    SaveLoadManager slManager;
    Backs playerBacks;
    SFXAudioManager sfxManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        var managers = GameObject.FindWithTag("Managers");

        playerUpgrades = player.GetComponent<PlayerUpgrades>();
        playerLoadout = player.GetComponent<PlayerLoadout>();
        companion = GameObject.FindWithTag("Companions").GetComponent<Companion>();
        playerBacks = player.GetComponentInChildren<Backs>();

        slManager = managers.GetComponent<SaveLoadManager>();
        sfxManager = managers.GetComponent<SFXAudioManager>();

        primaryCompanion = slManager.primaryCompanion;
        secondaryCompanion = slManager.secondaryCompanion;

        canSwitch = true;
        primary = slManager.seedBagPrimary;
        secondary = !primary;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown = baseCooldown / playerBacks.abilityCooldownMultiplier;

        if (playerLoadout.backpackActive)
        {
            primaryCompanion = playerLoadout.selectedPrimaryCompanion;
            secondaryCompanion = playerLoadout.selectedSecondaryCompanion;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && primaryCompanion && canSwitch && secondary)
        {
            SwitchToPrimary();

            StartCoroutine(CooldownRoutine());

            primary = true;
            secondary = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && secondaryCompanion && canSwitch && primary)
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
        if (primaryCompanion.title == "Loyal Sphere")
        {
            companion.SwitchToLoyalSphere();
        }
        else if (primaryCompanion.title == "Attack Square")
        {
            companion.SwitchToAttackSquare();
        }

        playerLoadout.selectedCompanion = primaryCompanion;
        playerUpgrades.companionUpdated = false;

        sfxManager.PlayClip(sfxManager.backpackActivate, sfxManager.masterManager.sBlend2D, sfxManager.backVolumeMod, true);
    }

    void SwitchToSecondary()
    {
        if (secondaryCompanion.title == "Loyal Sphere")
        {
            companion.SwitchToLoyalSphere();
        }
        else if (secondaryCompanion.title == "Attack Square")
        {
            companion.SwitchToAttackSquare();
        }

        playerLoadout.selectedCompanion = secondaryCompanion;
        playerUpgrades.companionUpdated = false;

        sfxManager.PlayClip(sfxManager.backpackActivate, sfxManager.masterManager.sBlend2D, sfxManager.backVolumeMod, true);
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
