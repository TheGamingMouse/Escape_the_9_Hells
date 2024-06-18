using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region Events

    public static event Action OnPlayerDeath;

    #endregion

    #region Variables

    [Header("Ints")]
    [Range(0, 100)]
    public readonly int maxHealth = 100;
    
    [Header("Floats")]
    [Range(0, 100)]
    public float health;
    public float resistanceMultiplier = 1;

    [Header("Bools")]
    public bool playerDead;
    public bool isInvinsible;

    [Header("Components")]
    CapeOWind capeOWind;
    SaveLoadManager slManager;
    public Shield shield;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        capeOWind = transform.GetComponentInChildren<Backs>().capeOWind;
        slManager = GameObject.FindWithTag("Managers").GetComponent<SaveLoadManager>();

        health = maxHealth;

        if (slManager.lState == SaveLoadManager.LayerState.InLayers)
        {
            health = slManager.healthInLayer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !playerDead)
        {
            if (capeOWind.active && capeOWind.canSave)
            {
                health = maxHealth;
                StartCoroutine(capeOWind.Save());
                return;
            }
            
            playerDead = true;
            capeOWind.cooldown = 0;
            OnPlayerDeath?.Invoke();
        }
    }

    #endregion

    #region General Methods

    public void TakeDamage(int damage)
    {
        if (!isInvinsible && !playerDead)
        {
            if ((health - damage) >= 0)
            {
                health -= damage / resistanceMultiplier;
            }
            else if ((health - damage) < 0)
            {
                health = 0;
            }
            
            if (!shield.onCooldown)
            {
                shield.damageTaken = true;
            }
        }
    }

    #endregion
}
