using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SaveSystemSpace.SaveClasses;

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
    Weapon weapon;
    public FireAura fireAura;
    public Shield shield;
    public IceAura iceAura;
    ParticleSystem iAuraPS;
    ParticleSystem fAuraPS;
    ParticleSystem shieldPS;
    ViewPerksMenu viewPerksMenu;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        var player = PlayerComponents.Instance.player;
        var canvas = GameObject.FindWithTag("Canvas").transform;

        weapon = player.GetComponentInChildren<Weapon>();
        
        viewPerksMenu = canvas.Find("Menus/PauseMenu/ViewPerksMenu").GetComponent<ViewPerksMenu>();

        iAuraPS = iceAura.GetComponent<ParticleSystem>();
        fAuraPS = fireAura.GetComponent<ParticleSystem>();
        shieldPS = shield.GetComponent<ParticleSystem>();

        var layerData = SaveSystem.loadedLayerData;
        var perkData = SaveSystem.loadedPerkData;

        if (layerData.lState == LayerData.LayerState.InLayers)
        {
            defencePerks = perkData.defencePerks;
            attackSpeedPerks = perkData.attackSpeedPerks;
            damagePerks = perkData.damagePerks;
            moveSpeedPerks = perkData.moveSpeedPerks;
            luckPerks = perkData.luckPerks;
            fireAuraPerks = perkData.fireAuraPerks;
            iceAuraPerks = perkData.iceAuraPerks;

            PlayerComponents.Instance.playerHealth.resistanceMultiplier += defencePerks.Count * defenceMod;
            weapon.attackSpeedMultiplier += attackSpeedPerks.Count * attackSpeedMod;
            weapon.damageMultiplier += damagePerks.Count * damageMod;
            PlayerComponents.Instance.playerMovement.speedMultiplier += moveSpeedPerks.Count * moveSpeedMod;
            PlayerComponents.Instance.playerLevel.luck += luckPerks.Count * luckMod;
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

            perkData.defencePerks.Clear();
            perkData.attackSpeedPerks.Clear();
            perkData.damagePerks.Clear();
            perkData.moveSpeedPerks.Clear();
            perkData.luckPerks.Clear();
            perkData.fireAuraPerks.Clear();
            perkData.shieldPerks.Clear();
            perkData.iceAuraPerks.Clear();

            SaveSystem.Instance.Save(perkData, SaveSystem.perksDataPath);
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
        var sfxManager = SFXAudioManager.Instance;

        if (perk.title == "Template Perk")
        {
            templatePerks.Add(perk);
            print("player has " + templatePerks.Count + " template perks");
        }
        else if (perk.title == "Defence Perk")
        {
            var perkData = SaveSystem.loadedPerkData;

            perkData.defencePerks.Add(perk);
            defencePerks.Add(perk);
            
            SaveSystem.Instance.Save(perkData, SaveSystem.perksDataPath);
            
            PlayerComponents.Instance.playerHealth.resistanceMultiplier += defenceMod;

            sfxManager.PlayClip(sfxManager.defencePerk, MasterAudioManager.Instance.sBlend2D, sfxManager.perkEffectsVolumeMod);
        }
        else if (perk.title == "Attack Speed Perk")
        {
            var perkData = SaveSystem.loadedPerkData;

            perkData.attackSpeedPerks.Add(perk);
            attackSpeedPerks.Add(perk);
            
            SaveSystem.Instance.Save(perkData, SaveSystem.perksDataPath);

            weapon.attackSpeedMultiplier += attackSpeedMod;

            sfxManager.PlayClip(sfxManager.attSpeedPerk, MasterAudioManager.Instance.sBlend2D, sfxManager.perkEffectsVolumeMod);
        }
        else if (perk.title == "Damage Perk")
        {
            var perkData = SaveSystem.loadedPerkData;

            perkData.damagePerks.Add(perk);
            damagePerks.Add(perk);
            
            weapon.damageMultiplier += damageMod;

            SaveSystem.Instance.Save(perkData, SaveSystem.perksDataPath);

            sfxManager.PlayClip(sfxManager.damagePerk, MasterAudioManager.Instance.sBlend2D, sfxManager.perkEffectsVolumeMod);
        }
        else if (perk.title == "Movement Speed Perk")
        {
            var perkData = SaveSystem.loadedPerkData;

            perkData.damagePerks.Add(perk);
            moveSpeedPerks.Add(perk);
            
            PlayerComponents.Instance.playerMovement.speedMultiplier += moveSpeedMod;

            SaveSystem.Instance.Save(perkData, SaveSystem.perksDataPath);

            sfxManager.PlayClip(sfxManager.moveSpeedPerk, MasterAudioManager.Instance.sBlend2D, sfxManager.perkEffectsVolumeMod);
        }
        else if (perk.title == "Luck Perk")
        {
            var perkData = SaveSystem.loadedPerkData;

            perkData.damagePerks.Add(perk);
            luckPerks.Add(perk);
            
            PlayerComponents.Instance.playerLevel.luck += luckMod;

            SaveSystem.Instance.Save(perkData, SaveSystem.perksDataPath);

            sfxManager.PlayClip(sfxManager.activateLucky, MasterAudioManager.Instance.sBlend2D, sfxManager.perkEffectsVolumeMod);
        }
        else if (perk.title == "Fire Aura Perk")
        {
            var perkData = SaveSystem.loadedPerkData;

            perkData.damagePerks.Add(perk);
            fireAuraPerks.Add(perk);
            
            fireAura.damage += fireAuraMod;

            PlayFireAuraAudio();
        }
        else if (perk.title == "Shield Perk")
        {
            var perkData = SaveSystem.loadedPerkData;

            perkData.damagePerks.Add(perk);
            shieldPerks.Add(perk);
            
            shield.protection += shieldPerks.Count * shieldMod;
            shield.modifierApplied = false;

            SaveSystem.Instance.Save(perkData, SaveSystem.perksDataPath);

            PlayShieldAudio();
        }
        else if (perk.title == "Ice Aura Perk")
        {
            var perkData = SaveSystem.loadedPerkData;

            perkData.damagePerks.Add(perk);
            iceAuraPerks.Add(perk);
            
            iceAura.damage += iceAuraDamageMod;
            iceAura.speedPenalty += iceAuraSpeedMod;

            SaveSystem.Instance.Save(perkData, SaveSystem.perksDataPath);
            
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
        var sfxManager = SFXAudioManager.Instance;

        sfxManager.PlayClip(sfxManager.activeIceAura, MasterAudioManager.Instance.sBlend2D, sfxManager.perkEffectsVolumeMod);
    }

    IEnumerator FireAuraActivateCooldown()
    {
        fAuraPSActive = true;

        yield return new WaitForSeconds(2);

        fAuraPSActive = false;
    }

    void PlayFireAuraAudio()
    {
        var sfxManager = SFXAudioManager.Instance;

        sfxManager.PlayClip(sfxManager.activeFireAura, MasterAudioManager.Instance.sBlend2D, sfxManager.perkEffectsVolumeMod/2);
    }

    IEnumerator ShieldActivateCooldown()
    {
        shieldPSActive = true;

        yield return new WaitForSeconds(2);

        shieldPSActive = false;
    }

    void PlayShieldAudio()
    {
        var sfxManager = SFXAudioManager.Instance;
        
        sfxManager.PlayClip(sfxManager.activeShield, MasterAudioManager.Instance.sBlend2D, sfxManager.perkEffectsVolumeMod*2);
    }

    #endregion
}
