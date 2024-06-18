using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [Header("Components")]
    PlayerHealth playerHealth;
    UIManager uiManager;
    ExpSoulsManager expSoulsManager;
    SaveLoadManager slManager;

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
        var managers = GameObject.FindWithTag("Managers");

        playerHealth = GetComponent<PlayerHealth>();
        uiManager = managers.GetComponent<UIManager>();
        expSoulsManager = managers.GetComponent<ExpSoulsManager>();
        slManager = managers.GetComponent<SaveLoadManager>();

        if (slManager.lState == SaveLoadManager.LayerState.InLayers)
        {
            level = slManager.levelsGainedInLayer;
            exp = slManager.expGainedInLayer;
            expMultiplier = slManager.expMultiplierInLayer;
            souls = slManager.soulsCollectedInLayer;
            demonsKilled = slManager.demonsKilledInLayer;
            devilsKilled = slManager.devilsKilledInLayer;
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
        if (slManager.lState == SaveLoadManager.LayerState.InLayers && uiManager.componentsFound)
        {
            if (level == 0)
            {
                expMultiplier = 250;
                while (startLevel > 0)
                {
                    LevelUp(false, false);
                    startLevel--;
                }
            }
        }

        if (previousSouls != souls)
        {
            uiManager.SoulsCounterValue = souls;
            previousSouls = souls;
        }

        int i = slManager.CheckLayer();
        
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

    public void LevelUp(bool expLoss, bool midLayer)
    {
        timesLeveledUp++;

        playerHealth.health = playerHealth.maxHealth;
        level++;
        if (expLoss)
        {
            exp -= 1f;
        }

        levelUpEffectObj.SetActive(true);
        levelUpEffect.Play();

        if (midLayer)
        {
            expSoulsManager.AddSouls(2 * level, true);
            expMultiplier *= 1.65f;
        }

        OnLevelUp?.Invoke();
    }

    public void AddExperience(float expPercent, bool wasEnemy, string enemyType)
    {
        if (expPercent >= 100f)
        {
            int expOverflow = (int)expPercent / 100;
            expPercent -= expOverflow * 100;

            while (expOverflow > 0)
            {
                LevelUp(false, true);
                expOverflow--;
            }
        }
        
        if (expPercent > 0f && expPercent < 100f)
        {
            exp += expPercent / 100f;
            if (exp >= 1f)
            {
                LevelUp(true, true);
            }
        }

        if (wasEnemy)
        {
            if (enemyType.ToLower() == "demon")
            {
                demonsKilled++;
            }
            else if (enemyType.ToLower() == "devil")
            {
                devilsKilled++;
            }
        }
    }

    #endregion

    #region SubscriptionHandler Methods

    void HandleExpChange(int newExp, string enemyType)
    {
        int luckCheck = Random.Range(1, 101);
        if (luckCheck <= luck)
        {
            exp += newExp / expMultiplier * expLayerMultiplier * 2;
        }
        else
        {
            exp += newExp / expMultiplier * expLayerMultiplier;
        }

        if (exp >= 1f)
        {
            while (exp >= 1f)
            {
                LevelUp(true, true);
            }
        }

        if (enemyType.ToLower() == "demon")
        {
            demonsKilled++;
        }
        else if (enemyType.ToLower() == "devil")
        {
            devilsKilled++;
        }
    }

    void HandleSoulsChange(int newSouls, bool fromLevel)
    {
        souls += newSouls;

        if (!fromLevel)
        {
            gainSoulsEffectObj.SetActive(true);
            gainSoulsEffect.Play();
        }
    }

    #endregion
}
