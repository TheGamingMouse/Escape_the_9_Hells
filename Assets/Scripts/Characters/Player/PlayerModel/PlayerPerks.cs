using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerks : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    readonly int luckMod = 1;
    readonly int fireAuraMod = 4;
    readonly int iceAuraDamageMod = 2;

    [Header("Floats")]
    readonly float defenceMod = 0.25f;
    readonly float attackSpeedMod = 0.25f;
    readonly float damageMod = 0.2f;
    readonly float moveSpeedMod = 0.075f;
    readonly float shieldMod = 0.2f;
    readonly float iceAuraSpeedMod = 0.05f;

    [Header("Bools")]
    bool fAuraActive;
    bool fAuraPSActive;
    bool iAuraActive;
    bool iAuraPSActive;
    bool shieldActive;
    bool shieldPSActive;

    [Header("Lists")]
    public List<PerkItemsSO> templatePerks = new();
    public List<PerkItemsSO> defencePerks = new();
    public List<PerkItemsSO> attackSpeedPerks = new();
    public List<PerkItemsSO> damagePerks = new();
    public List<PerkItemsSO> moveSpeedPerks = new();
    public List<PerkItemsSO> luckPerks = new();
    public List<PerkItemsSO> fireAuraPerks = new();
    public List<PerkItemsSO> shieldPerks = new();
    public List<PerkItemsSO> iceAuraPerks = new();

    [Header("Components")]
    PlayerHealth health;
    Weapon weapon;
    PlayerMovement movement;
    SaveLoadManager slManager;
    PlayerLevel level;
    public FireAura fireAura;
    public Shield shield;
    public IceAura iceAura;
    SFXAudioManager sfxManager;
    ParticleSystem iAuraPS;
    ParticleSystem fAuraPS;
    ParticleSystem shieldPS;
    ViewPerksMenu viewPerksMenu;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        var managers = GameObject.FindWithTag("Managers");
        var canvas = GameObject.FindWithTag("Canvas").transform;

        health = player.GetComponent<PlayerHealth>();
        weapon = player.GetComponentInChildren<Weapon>();
        movement = player.GetComponent<PlayerMovement>();
        level = player.GetComponent<PlayerLevel>();

        slManager = managers.GetComponent<SaveLoadManager>();
        sfxManager = managers.GetComponent<SFXAudioManager>();
        
        viewPerksMenu = canvas.Find("Menus/PauseMenu/ViewPerksMenu").GetComponent<ViewPerksMenu>();

        iAuraPS = iceAura.GetComponent<ParticleSystem>();
        fAuraPS = fireAura.GetComponent<ParticleSystem>();
        shieldPS = shield.GetComponent<ParticleSystem>();

        if (slManager.lState == SaveLoadManager.LayerState.InLayers)
        {
            defencePerks = slManager.defencePerks;
            attackSpeedPerks = slManager.attackSpeedPerks;
            damagePerks = slManager.damagePerks;
            moveSpeedPerks = slManager.moveSpeedPerks;
            luckPerks = slManager.luckPerks;
            fireAuraPerks = slManager.fireAuraPerks;
            iceAuraPerks = slManager.iceAuraPerks;

            health.resistanceMultiplier += defencePerks.Count * defenceMod;
            weapon.attackSpeedMultiplier += attackSpeedPerks.Count * attackSpeedMod;
            weapon.damageMultiplier += damagePerks.Count * damageMod;
            movement.speedMultiplier += moveSpeedPerks.Count * moveSpeedMod;
            level.luck += luckPerks.Count * luckMod;
            fireAura.damage += fireAuraPerks.Count * fireAuraMod;
            shield.protection += shieldPerks.Count * shieldMod;
            iceAura.damage += iceAuraPerks.Count * iceAuraDamageMod;
            iceAura.speedPenalty += iceAuraPerks.Count * iceAuraSpeedMod;
        }
        else
        {
            defencePerks.Clear();
            attackSpeedPerks.Clear();
            damagePerks.Clear();
            moveSpeedPerks.Clear();
            luckPerks.Clear();
            fireAuraPerks.Clear();
            shieldPerks.Clear();
            iceAuraPerks.Clear();

            slManager.defencePerks.Clear();
            slManager.attackSpeedPerks.Clear();
            slManager.damagePerks.Clear();
            slManager.moveSpeedPerks.Clear();
            slManager.luckPerks.Clear();
            slManager.fireAuraPerks.Clear();
            slManager.shieldPerks.Clear();
            slManager.iceAuraPerks.Clear();
        }

        if (fireAuraPerks.Count > 0 && !fAuraActive)
        {
            fireAura.gameObject.SetActive(true);
            fAuraActive = true;
        }
        else if (fireAuraPerks.Count <= 0)
        {
            fireAura.gameObject.SetActive(false);
            fAuraActive = false;
        }

        if (shieldPerks.Count > 0 && !shieldActive)
        {
            shield.gameObject.SetActive(true);
            shieldActive = true;
        }
        else if (shieldPerks.Count <= 0)
        {
            shield.gameObject.SetActive(false);
            shieldActive = false;
        }

        if (iceAuraPerks.Count > 0 && !iAuraActive)
        {
            iceAura.gameObject.SetActive(true);
            iAuraActive = true;
        }
        else if (iceAuraPerks.Count <= 0)
        {
            iceAura.gameObject.SetActive(false);
            iAuraActive = false;
        }
    }

    void Update()
    {
        if (fireAuraPerks.Count > 0 && !fAuraActive)
        {
            fireAura.gameObject.SetActive(true);
            fAuraActive = true;
        }
        else if (fireAuraPerks.Count <= 0)
        {
            fireAura.gameObject.SetActive(false);
            fAuraActive = false;
        }

        if (fAuraActive)
        {
            if (fAuraPS.time <= 0.1f && !fAuraPSActive)
            {
                PlayFireAuraAudio();

                StartCoroutine(FireAuraActivateCooldown());
            }
        }

        if (shieldPerks.Count > 0 && !shieldActive)
        {
            shield.gameObject.SetActive(true);
            shieldActive = true;
        }
        else if (shieldPerks.Count <= 0)
        {
            shield.gameObject.SetActive(false);
            shieldActive = false;
        }

        if (shieldActive)
        {
            if (shieldPS.time <= 0.1f && !shieldPSActive)
            {
                PlayShieldAudio();

                StartCoroutine(ShieldActivateCooldown());
            }
        }

        if (iceAuraPerks.Count > 0 && !iAuraActive)
        {
            iceAura.gameObject.SetActive(true);
            iAuraActive = true;
        }
        else if (iceAuraPerks.Count <= 0)
        {
            iceAura.gameObject.SetActive(false);
            iAuraActive = false;
        }

        if (iAuraActive)
        {
            if (iAuraPS.time <= 0.1f && !iAuraPSActive)
            {
                PlayIceAuraAudio();

                StartCoroutine(IceAuraActivateCooldown());
            }
        }
    }

    #endregion

    #region General Methods

    public void AddPerk(PerkItemsSO perk)
    {
        if (perk.title == "Template Perk")
        {
            templatePerks.Add(perk);
            print("player has " + templatePerks.Count + " template perks");
        }
        else if (perk.title == "Defence Perk")
        {
            defencePerks.Add(perk);
            health.resistanceMultiplier += defenceMod;

            sfxManager.PlayClip(sfxManager.defencePerk, sfxManager.masterManager.sBlend2D, sfxManager.perkEffectsVolumeMod);
        }
        else if (perk.title == "Attack Speed Perk")
        {
            attackSpeedPerks.Add(perk);
            weapon.attackSpeedMultiplier += attackSpeedMod;

            sfxManager.PlayClip(sfxManager.attSpeedPerk, sfxManager.masterManager.sBlend2D, sfxManager.perkEffectsVolumeMod);
        }
        else if (perk.title == "Damage Perk")
        {
            damagePerks.Add(perk);
            weapon.damageMultiplier += damageMod;

            sfxManager.PlayClip(sfxManager.damagePerk, sfxManager.masterManager.sBlend2D, sfxManager.perkEffectsVolumeMod);
        }
        else if (perk.title == "Movement Speed Perk")
        {
            moveSpeedPerks.Add(perk);
            movement.speedMultiplier += moveSpeedMod;

            sfxManager.PlayClip(sfxManager.moveSpeedPerk, sfxManager.masterManager.sBlend2D, sfxManager.perkEffectsVolumeMod);
        }
        else if (perk.title == "Luck Perk")
        {
            luckPerks.Add(perk);
            level.luck += luckMod;

            sfxManager.PlayClip(sfxManager.activateLucky, sfxManager.masterManager.sBlend2D, sfxManager.perkEffectsVolumeMod);
        }
        else if (perk.title == "Fire Aura Perk")
        {
            fireAuraPerks.Add(perk);
            fireAura.damage += fireAuraMod;

            PlayFireAuraAudio();
        }
        else if (perk.title == "Shield Perk")
        {
            shieldPerks.Add(perk);
            shield.protection += shieldPerks.Count * shieldMod;
            shield.modifierApplied = false;

            PlayShieldAudio();
        }
        else if (perk.title == "Ice Aura Perk")
        {
            iceAuraPerks.Add(perk);
            iceAura.damage += iceAuraDamageMod;
            iceAura.speedPenalty += iceAuraSpeedMod;
            
            PlayIceAuraAudio();
        }

        viewPerksMenu.perksAquired++;
    }

    IEnumerator IceAuraActivateCooldown()
    {
        iAuraPSActive = true;

        yield return new WaitForSeconds(2);

        iAuraPSActive = false;
    }

    void PlayIceAuraAudio()
    {
        sfxManager.PlayClip(sfxManager.activeIceAura, sfxManager.masterManager.sBlend2D, sfxManager.perkEffectsVolumeMod);
    }

    IEnumerator FireAuraActivateCooldown()
    {
        fAuraPSActive = true;

        yield return new WaitForSeconds(2);

        fAuraPSActive = false;
    }

    void PlayFireAuraAudio()
    {
        sfxManager.PlayClip(sfxManager.activeFireAura, sfxManager.masterManager.sBlend2D, sfxManager.perkEffectsVolumeMod/2);
    }

    IEnumerator ShieldActivateCooldown()
    {
        shieldPSActive = true;

        yield return new WaitForSeconds(2);

        shieldPSActive = false;
    }

    void PlayShieldAudio()
    {
        sfxManager.PlayClip(sfxManager.activeShield, sfxManager.masterManager.sBlend2D, sfxManager.perkEffectsVolumeMod*2);
    }

    #endregion
}
