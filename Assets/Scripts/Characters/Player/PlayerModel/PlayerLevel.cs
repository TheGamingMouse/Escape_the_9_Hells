using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SaveSystemSpace.SaveClasses;
using Random = UnityEngine.Random;

public class PlayerLevel : MonoBehaviour
{
    #region Events

    public static event Action OnLevelUp;

    #endregion

    #region Variables

    [Header("Ints")]
    public int level;
    public int souls;
    int previousSouls;
    public int demonsKilled;
    public int devilsKilled;
    public int timesLeveledUp;
    public int luck;
    public int startLevel;

    [Header("Floats")]
    [Range(0f, 1.5f)]
    public float exp;
    public float expMultiplier;
    [SerializeField] float expLayerMultiplier;

    [Header("Strings")]
    public string layerReached;

    [Header("GameObjects")]
    public GameObject levelUpEffectObj;
    public GameObject gainSoulsEffectObj;

    [Header("Particle Systems")]
    ParticleSystem levelUpEffect;
    ParticleSystem gainSoulsEffect;

    #endregion

    #region Subscription Methods

    void OnEnable()
    {
        ExpSoulsManager.OnExpChange += HandleExpChange;
        ExpSoulsManager.OnSoulsChange += HandleSoulsChange;
    }

    void OnDisable()
    {
        ExpSoulsManager.OnExpChange -= HandleExpChange;
        ExpSoulsManager.OnSoulsChange -= HandleSoulsChange;
    }

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var layerData = SaveSystem.loadedLayerData;
        PersistentData persistentData;

        if (layerData.lState == LayerData.LayerState.InLayers)
        {
            persistentData = SaveSystem.loadedPersistentData;

            level = persistentData.levelsGainedInLayer;
            exp = persistentData.expGainedInLayer;
            expMultiplier = persistentData.expMultiplierInLayer;
            souls = persistentData.soulsCollectedInLayer;
            demonsKilled = persistentData.demonsKilledInLayer;
            devilsKilled = persistentData.devilsKilledInLayer;

            souls = SaveSystem.loadedPlayerData.currentSouls;
        }
        else
        {
            persistentData = new PersistentData();
            SaveSystem.Instance.Save(persistentData, SaveSystem.persistentDataPath);

            var playerData = SaveSystem.loadedPlayerData;
            souls = playerData.currentSouls;
        }

        levelUpEffect = levelUpEffectObj.GetComponent<ParticleSystem>();
        gainSoulsEffect = gainSoulsEffectObj.GetComponent<ParticleSystem>();

        levelUpEffect.Stop();
        gainSoulsEffect.Stop();

        levelUpEffectObj.SetActive(false);
        gainSoulsEffectObj.SetActive(false);

        previousSouls = souls;
        layerReached = SceneManager.GetActiveScene().name.ToLower();
    }

    // Update is called once per frame
    void Update()
    {
        if (timesLeveledUp < 0)
        {
            timesLeveledUp = 0;
        }

        var layerData = SaveSystem.loadedLayerData;
        if (layerData.lState == LayerData.LayerState.InLayers && UIManager.Instance.componentsFound)
        {
            if (level == 0)
            {
                expMultiplier = 250;

                var persistentData = SaveSystem.loadedPersistentData;
                persistentData.expMultiplierInLayer = expMultiplier;

                SaveSystem.Instance.Save(persistentData, SaveSystem.persistentDataPath);
                
                while (startLevel > 0)
                {
                    LevelUp(false, false);
                    startLevel--;
                }
            }
        }

        if (previousSouls != souls)
        {
            UIManager.Instance.SoulsCounterValue = souls;
            previousSouls = souls;
        }

        int i = SaveSystem.Instance.CheckLayer();
        
        if (i > 0)
        {
            expLayerMultiplier = Mathf.Pow(7, i - 1);
        }
        else
        {
            expLayerMultiplier = 1f;
        }
    }

    #endregion

    #region General Methods

    public void LevelUp(bool expLoss, bool midLayer, bool boss = false)
    {
        timesLeveledUp++;

        PlayerComponents.Instance.playerHealth.health = PlayerComponents.Instance.playerHealth.maxHealth;
        level++;
        if (expLoss)
        {
            exp -= 1f;

            var persistentDataLocal = SaveSystem.loadedPersistentData;
            persistentDataLocal.expGainedInLayer = exp;

            SaveSystem.Instance.Save(persistentDataLocal, SaveSystem.persistentDataPath);
        }

        levelUpEffectObj.SetActive(true);
        levelUpEffect.Play();

        if (midLayer)
        {
            ExpSoulsManager.Instance.AddSouls(2 * level, true);
            expMultiplier *= 1.65f;

            var persistentDataLocal = SaveSystem.loadedPersistentData;
            persistentDataLocal.expMultiplierInLayer = expMultiplier;

            SaveSystem.Instance.Save(persistentDataLocal, SaveSystem.persistentDataPath);
        }

        if (boss)
        {
            devilsKilled++;

            var persistentDataLocal = SaveSystem.loadedPersistentData;
            persistentDataLocal.devilsKilledInLayer++;

            SaveSystem.Instance.Save(persistentDataLocal, SaveSystem.persistentDataPath);
        }

        var persistentData = SaveSystem.loadedPersistentData;
        persistentData.levelsGainedInLayer++;

        SaveSystem.Instance.Save(persistentData, SaveSystem.persistentDataPath);

        OnLevelUp?.Invoke();
    }

    public void AddExperience(float expPercent, bool wasEnemy, string enemyType)
    {
        var sfxManager = SFXAudioManager.Instance;
        
        if (expPercent >= 100f)
        {
            int expOverflow = (int)expPercent / 100;
            expPercent -= expOverflow * 100;

            while (expOverflow > 0)
            {
                LevelUp(false, true);
                expOverflow--;

                sfxManager.PlayClip(sfxManager.gainLevel, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod / 2);
            }
        }
        
        if (expPercent > 0f && expPercent < 100f)
        {
            exp += expPercent / 100f;
            if (exp >= 1f)
            {
                LevelUp(true, true);

                var persistentData = SaveSystem.loadedPersistentData;
                persistentData.expGainedInLayer = exp;

                SaveSystem.Instance.Save(persistentData, SaveSystem.persistentDataPath);

                sfxManager.PlayClip(sfxManager.gainLevel, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod / 2);
            }
        }

        if (wasEnemy)
        {
            if (enemyType.ToLower() == "demon")
            {
                demonsKilled++;

                var persistentData = SaveSystem.loadedPersistentData;
                persistentData.demonsKilledInLayer++;

                SaveSystem.Instance.Save(persistentData, SaveSystem.persistentDataPath);
            }
            else if (enemyType.ToLower() == "devil")
            {
                devilsKilled++;

                var persistentData = SaveSystem.loadedPersistentData;
                persistentData.devilsKilledInLayer++;

                SaveSystem.Instance.Save(persistentData, SaveSystem.persistentDataPath);
            }
        }
    }

    #endregion

    #region SubscriptionHandler Methods

    void HandleExpChange(int newExp, string enemyType)
    {
        var sfxManager = SFXAudioManager.Instance;

        int luckCheck = Random.Range(1, 501);
        if (luckCheck <= luck)
        {
            exp += newExp / expMultiplier * expLayerMultiplier * 2;
            sfxManager.PlayClip(sfxManager.activateLucky, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod / 2);
        }
        else
        {
            exp += newExp / expMultiplier * expLayerMultiplier;
        }

        var persistentData = SaveSystem.loadedPersistentData;
        persistentData.expGainedInLayer = exp;

        SaveSystem.Instance.Save(persistentData, SaveSystem.persistentDataPath);

        if (exp >= 1f)
        {
            sfxManager.PlayClip(sfxManager.gainLevel, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod / 2);

            while (exp >= 1f)
            {
                LevelUp(true, true);
            }
        }

        if (enemyType.ToLower() == "demon")
        {
            demonsKilled++;

            var persistentDataLocal = SaveSystem.loadedPersistentData;
            persistentDataLocal.demonsKilledInLayer++;

            SaveSystem.Instance.Save(persistentDataLocal, SaveSystem.persistentDataPath);
        }
    }

    void HandleSoulsChange(int newSouls, bool fromLevel)
    {
        var sfxManager = SFXAudioManager.Instance;
        var persistentData = SaveSystem.loadedPersistentData;
        
        souls += newSouls;

        persistentData.soulsCollectedInLayer = souls;
        SaveSystem.Instance.Save(persistentData, SaveSystem.persistentDataPath);

        if (!fromLevel)
        {
            gainSoulsEffectObj.SetActive(true);
            gainSoulsEffect.Play();

            sfxManager.PlayClip(sfxManager.gainSouls, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod, true);
        }
    }

    #endregion
}
