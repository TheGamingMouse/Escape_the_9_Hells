using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Floats")]
    [Range(0f, 1.5f)]
    public float exp;
    float expMultiplier;
    float preExpMultiplier;

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
        playerHealth = GetComponent<PlayerHealth>();
        uiManager = GameObject.FindWithTag("Managers").GetComponent<UIManager>();

        if (level == 0)
        {
            level = 1;
            expMultiplier = 100;
            preExpMultiplier = expMultiplier;
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
        if (previousSouls != souls)
        {
            uiManager.SoulsCounterValue = souls;
            previousSouls = souls;
        }
    }

    #endregion

    #region General Methods

    public void LevelUp(bool expLoss)
    {
        // Chose upgrade

        playerHealth.health = playerHealth.maxHealth;
        level++;
        if (expLoss)
        {
            exp -= 1f;
        }
        preExpMultiplier = expMultiplier;

        expMultiplier = preExpMultiplier * level;

        levelUpEffectObj.SetActive(true);
        levelUpEffect.Play();

        OnLevelUp?.Invoke();
    }

    public void AddExperience(float expPercent)
    {
        if (expPercent > 0 && expPercent < 100)
        {
            exp += (float)expPercent / 100;
            if (exp >= 1f)
            {
                LevelUp(true);
            }
        }
    }

    #endregion

    #region SubscriptionHandler Methods

    void HandleExpChange(int newExp, string enemyType)
    {
        exp += newExp / expMultiplier;
        if (exp >= 1f)
        {
            LevelUp(true);
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

    void HandleSoulsChange(int newSouls)
    {
        souls += newSouls;

        gainSoulsEffectObj.SetActive(true);
        gainSoulsEffect.Play();
    }

    #endregion
}
