using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    public int level;
    public int souls;

    [Header("Floats")]
    [Range(0f, 1.5f)]
    public float exp;
    [SerializeField] float expMultiplier;
    [SerializeField] float preExpMultiplier;

    [Header("Components")]
    PlayerHealth playerHealth;

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

        if (level == 0)
        {
            level = 1;
            expMultiplier = 100;
            preExpMultiplier = expMultiplier;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region General Methods

    void LevelUp()
    {
        // Chose upgrade

        playerHealth.health = playerHealth.maxHealth;
        level++;
        exp -= 1f;
        preExpMultiplier = expMultiplier;

        expMultiplier = preExpMultiplier * level;
    }

    #endregion

    #region SubscriptionHandler Methods

    void HandleExpChange(int newExp)
    {
        exp += newExp / expMultiplier;
        if (exp >= 1f)
        {
            LevelUp();
        }
    }

    void HandleSoulsChange(int newSouls)
    {
        souls += newSouls;
    }

    #endregion
}
