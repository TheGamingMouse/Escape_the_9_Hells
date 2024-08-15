using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXAudioManager : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public float uiVolumeMod;
    public float enemyVolumeMod;
    public float effectsVolumeMod;
    public float perkEffectsVolumeMod;
    public float npcDialogueVolumeMod;
    public float weaponVolumeMod;
    public float backVolumeMod;
    public float playerVolumeMod;
    public float bossAttackVolumeMod;

    [Header("Lists")]
    public List<AudioSource> audioSourcePool = new(100);

    // General Enemies
    public List<AudioClip> enemyLaughMale = new();
    public List<AudioClip> enemyLaughFemale = new();
    public List<AudioClip> enemyDamageMale = new();
    public List<AudioClip> enemyDamageFemale = new();
    public List<AudioClip> enemyDeathMale = new();
    public List<AudioClip> enemyDeathFemale = new();

    // Flying Imps
    public List<AudioClip> impTargetingMale = new();
    public List<AudioClip> impTargetingFemale = new();
    public List<AudioClip> impAttackMale = new();
    public List<AudioClip> impAttackFemale = new();

    // Souls
    public List<AudioClip> reRollSFX = new();

    // NPC Dialogue
    public List<AudioClip> rickyGreetings = new();
    public List<AudioClip> rickyFarewells = new();
    public List<AudioClip> alexanderGreetings = new();
    public List<AudioClip> alexanderFarewells = new();
    public List<AudioClip> barbaraGreetings = new();
    public List<AudioClip> barbaraFarewells = new();

    // Player Sounds
    public List<AudioClip> playerDamage = new();
    public List<AudioClip> playerWalking = new();

    [Header("AudioClips")]
    // UI
    public AudioClip dialogue;
    public AudioClip onButtonPress;

    // Effects
    public AudioClip doorOpen;
    public AudioClip activateLucky;
    public AudioClip gainSouls;
    public AudioClip gainLevel;

    // Perks
    public AudioClip activeIceAura;
    public AudioClip activeFireAura;
    public AudioClip activeShield;
    public AudioClip defencePerk;
    public AudioClip attSpeedPerk;
    public AudioClip damagePerk;
    public AudioClip moveSpeedPerk;

    // Equipment
    // Backs
    public AudioClip capeOWindActivate;
    public AudioClip angelWingsActivate;
    public AudioClip backpackActivate;
    
    // Companions
    public AudioClip firebolt;
    public AudioClip fireboltExplosion;
    public AudioClip attackSquareHit;
    public AudioClip attackSquareTeleport;

    // Weapons
    public AudioClip pugio;
    public AudioClip ulfberht;

    // Boss Weapons
    public AudioClip cainAttack;

    [Header("AudioSources")]
    public AudioSource dialogueSource;

    [Header("Components")]
    public MasterAudioManager masterManager;
    SettingsManager settingsManager;

    #endregion

    #region StartUpdate Methods

    void Awake()
    {
        Button[] buttons = FindObjectsOfType<Button>();
	
        foreach (var b in buttons)
        {
            b.onClick.AddListener(OnClick);
        }

        PopulateAudioSourcePool();
    }

    // Start is called before the first frame update
    void Start()
    {
        settingsManager = masterManager.settingsManager;

        dialogueSource.clip = dialogue;
        dialogueSource.outputAudioMixerGroup = masterManager.sfxMixer;
        dialogueSource.volume = CalculateVolume(uiVolumeMod);
        dialogueSource.spatialBlend = masterManager.sBlend2D;
    }

    // Update is called once per frame
    void Update()
    {
        if (settingsManager.sfxVolume == -30 || settingsManager.masterVolume == -30)
        {
            foreach (var source in audioSourcePool)
            {
                if (!source.isPlaying)
                {
                    source.volume = 0f;
                }
            }

            dialogueSource.volume = 0f;
        }
        else
        {
            dialogueSource.volume = CalculateVolume(uiVolumeMod);
        }
    }

    #endregion

    #region General Methods

    public void PressButton()
    {
        PlayClip(onButtonPress, masterManager.sBlend2D, uiVolumeMod);
    }

    public void PlayReRoll()
    {
        var randReRoll = Random.Range(0, reRollSFX.Count);
        PlayClip(reRollSFX[randReRoll], masterManager.sBlend2D, effectsVolumeMod);
    }

    public void PlayAlexanderVO(bool isGreeting)
    {
        List<List<AudioClip>> clipList = new()
        {
            alexanderGreetings,
            alexanderFarewells
        };

        if (isGreeting)
        {
            int randInt = Random.Range(0, alexanderGreetings.Count);
            PlayClip(alexanderGreetings[randInt], masterManager.sBlend2D, npcDialogueVolumeMod, false, "none", null, 1, false, clipList);
        }
        else
        {
            int randInt = Random.Range(0, alexanderFarewells.Count);
            PlayClip(alexanderFarewells[randInt], masterManager.sBlend2D, npcDialogueVolumeMod, false, "none", null, 1, false, clipList);
        }
    }

    public void PlayBarbaraVO(bool isGreeting)
    {
        
        List<List<AudioClip>> clipList = new()
        {
            barbaraGreetings,
            barbaraFarewells
        };

        if (isGreeting)
        {
            int randInt = Random.Range(0, barbaraGreetings.Count);
            PlayClip(barbaraGreetings[randInt], masterManager.sBlend2D, npcDialogueVolumeMod, false, "none", null, 1, false, clipList);
        }
        else
        {
            int randInt = Random.Range(0, barbaraFarewells.Count);
            PlayClip(barbaraFarewells[randInt], masterManager.sBlend2D, npcDialogueVolumeMod, false, "none", null, 1, false, clipList);
        }
    }

    public void PlayRickyVO(bool isGreeting)
    {
        List<List<AudioClip>> clipList = new()
        {
            rickyGreetings,
            rickyFarewells
        };

        if (isGreeting)
        {
            int randInt = Random.Range(0, rickyGreetings.Count);
            PlayClip(rickyGreetings[randInt], masterManager.sBlend2D, npcDialogueVolumeMod, false, "none", null, 1, false, clipList);
        }
        else
        {
            int randInt = Random.Range(0, rickyFarewells.Count);
            PlayClip(rickyFarewells[randInt], masterManager.sBlend2D, npcDialogueVolumeMod, false, "none", null, 1, false, clipList);
        }
    }

    #endregion

    #region Audio Methods

    void OnClick()
    {
        PressButton();
    }

    float CalculateVolume(float volumeOverride = 0.15f)
    {
        masterManager.audioMixer.GetFloat("sfxVolume", out float dBSFX);
        float sfxVolume = Mathf.Pow(10.0f, dBSFX / 20.0f);

        masterManager.audioMixer.GetFloat("masterVolume", out float dBMaster);
        float masterVolume = Mathf.Pow(10.0f, dBMaster / 20.0f);
        
        float realVolume = (sfxVolume + masterVolume) / 2;
        if (volumeOverride > 0f)
        {
            realVolume *= volumeOverride;
        }
        else
        {
            Debug.LogError("volumeOverride must be a positive number.");
            realVolume *= masterManager.defaultVolume;
        }

        return realVolume;
    }

    float CalculateRandomPitch(float pitch)
    {
        return Random.Range(pitch - 0.1f, pitch + 0.1f);
    }

    AudioSource AddNewSourceToPool(float blend, float volumeOverride, GameObject objectSource, string priority, float pitchOverride)
    {
        AudioSource newSource;
        if (!objectSource)
        {
            newSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            newSource = objectSource.AddComponent<AudioSource>();
        }

        if (priority != "none")
        {
            if (priority == "high")
            {
                newSource.priority = 1;
            }
            else if (priority == "low")
            {
                newSource.priority = 256;
            }
        }

        newSource.playOnAwake = false;
        newSource.spatialBlend = blend;
        if (settingsManager.sfxVolume == -30 || settingsManager.masterVolume == -30)
        {
            newSource.volume = 0f;
        }
        else
        {
            newSource.volume = CalculateVolume(volumeOverride);
        }
        newSource.pitch = pitchOverride;
        newSource.outputAudioMixerGroup = masterManager.sfxMixer;
        audioSourcePool.Add(newSource);
        return newSource;
    }

    AudioSource GetAvailablePoolSource(float blend, float volumeOverride, GameObject objectSource, string priority, float pitchOverride, bool canPlayIfPlaying, List<List<AudioClip>> clipLists)
    {
        //Fetch the first source in the pool that is not currently playing anything
        foreach (var source in audioSourcePool)
        {
            if (source.IsDestroyed())
            {
                audioSourcePool.Remove(source);
            }

            if (!canPlayIfPlaying)
            {
                if (IsAudioPlaying(clipLists, source))
                {
                    source.Stop();
                }
                return source;
            }

            if (!objectSource)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }
            else
            {
                if (!source.isPlaying && objectSource == source.gameObject)
                {
                    return source;
                }
            }
        }

        //No unused sources. Create and fetch a new source
        return AddNewSourceToPool(blend, volumeOverride, objectSource, priority.ToLower(), pitchOverride);
    }

    AudioSource GetUnavailablePoolSource(AudioClip clip)
    {
        //Fetch the first source in the pool that is not currently playing anything
        foreach (var source in audioSourcePool)
        {
            if (source.isPlaying && source.clip == clip)
            {
                return source;
            }
        }
        return null;
    }

    bool IsAudioPlaying(List<List<AudioClip>> clipLists, AudioSource source)
    {
        foreach (List<AudioClip> list in clipLists)
        {
            if (list.Contains(source.clip) && source.isPlaying)
            {
                return true;
            }
        }

        return false;
    }

    public void PlayClip(AudioClip clip, float blend, float volumeOverride, bool randomPitch = false, string priority = "none", GameObject objectSource = null, float pitchOverride = 1f, bool canPlayIfPlaying = true, List<List<AudioClip>> clipLists = null)
    {
        AudioSource source = GetAvailablePoolSource(blend, volumeOverride, objectSource, priority, pitchOverride, canPlayIfPlaying, clipLists);
        if (source)
        {
            source.clip = clip;
        }
        else
        {
            return;
        }

        if (settingsManager.sfxVolume == -30 || settingsManager.masterVolume == -30)
        {
            source.volume = 0f;
        }
        else
        {
            source.volume = volumeOverride;
        }

        source.spatialBlend = blend;

        if (randomPitch)
        {
            source.pitch = CalculateRandomPitch(pitchOverride);
        }
        else
        {
            source.pitch = pitchOverride;
        }

        source.Play();
    }

    public void StopClip(AudioClip clip)
    {
        AudioSource source = GetUnavailablePoolSource(clip);
        if (source == null)
        {
            return;
        }
        source.clip = clip;
        source.Stop();
    }

    void PopulateAudioSourcePool()
    {
        for (int i = 0; i < 15; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            
            newSource.playOnAwake = false;
            newSource.spatialBlend = masterManager.sBlend2D;
            newSource.volume = 0.15f;
            newSource.outputAudioMixerGroup = masterManager.sfxMixer;
            audioSourcePool.Add(newSource);
        }
    }

    #endregion
}
