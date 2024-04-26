using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    #region Events

    public static event Action OnLevelUp;

    #endregion

    #region Properties

    [Header("Ints")]
    public int level;
    public int souls;
    int previousSouls;

    [Header("Floats")]
    [Range(0f, 1.5f)]
    public float exp;
    float expMultiplier;
    float preExpMultiplier;

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

    void HandleExpChange(int newExp)
    {
        exp += newExp / expMultiplier;
        if (exp >= 1f)
        {
            LevelUp(true);
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
