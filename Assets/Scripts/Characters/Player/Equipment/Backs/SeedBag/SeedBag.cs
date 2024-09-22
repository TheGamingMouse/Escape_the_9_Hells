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
    Companion companion;
    Backs back;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var player = PlayerComponents.Instance.player;
        
        companion = GameObject.FindWithTag("Companions").GetComponent<Companion>();
        back = player.GetComponentInChildren<Backs>();

        var equipmentData = SaveSystem.loadedEquipmentData;

        primaryCompanion = equipmentData.companionData.primaryCompanion;
        secondaryCompanion = equipmentData.companionData.secondaryCompanion;

        canSwitch = true;
        primary = equipmentData.backData.seedBagPrimary;
        secondary = !primary;
    }

    // Update is called once per frame
    void Update()
    {
        var playerLoadout = PlayerComponents.Instance.playerLoadout;

        cooldown = baseCooldown / back.abilityCooldownMultiplier;

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
        var sfxManager = SFXAudioManager.Instance;

        if (primaryCompanion.title == "Loyal Sphere")
        {
            companion.SwitchToLoyalSphere();
        }
        else if (primaryCompanion.title == "Attack Square")
        {
            companion.SwitchToAttackSquare();
        }

        PlayerComponents.Instance.playerLoadout.selectedCompanion = primaryCompanion;
        PlayerComponents.Instance.playerUpgrades.companionUpdated = false;

        sfxManager.PlayClip(sfxManager.backpackActivate, MasterAudioManager.Instance.sBlend2D, sfxManager.backVolumeMod, true);
    }

    void SwitchToSecondary()
    {
        var sfxManager = SFXAudioManager.Instance;
        
        if (secondaryCompanion.title == "Loyal Sphere")
        {
            companion.SwitchToLoyalSphere();
        }
        else if (secondaryCompanion.title == "Attack Square")
        {
            companion.SwitchToAttackSquare();
        }

        PlayerComponents.Instance.playerLoadout.selectedCompanion = secondaryCompanion;
        PlayerComponents.Instance.playerUpgrades.companionUpdated = false;

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
