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
    SFXAudioManager sfxManager;
    public ParticleSystem capeOWindPS;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var managers = GameObject.FindWithTag("Managers");

        capeOWind = transform.GetComponentInChildren<Backs>().capeOWind;
        slManager = managers.GetComponent<SaveLoadManager>();
        sfxManager = managers.GetComponent<SFXAudioManager>();

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
                StartCoroutine(CapeSave());

                sfxManager.PlayClip(sfxManager.capeOWindActivate, sfxManager.masterManager.sBlend2D, sfxManager.backVolumeMod / 2, false, "high");

                return;
            }
            
            Die();
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

                int i = UnityEngine.Random.Range(0, sfxManager.playerDamage.Count);
                sfxManager.PlayClip(sfxManager.playerDamage[i], sfxManager.masterManager.sBlend2D, sfxManager.playerVolumeMod);
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

    public void Die()
    {
        health = 0;
        playerDead = true;
        capeOWind.cooldown = 0;
        OnPlayerDeath?.Invoke();
    }

    IEnumerator CapeSave()
    {
        capeOWindPS.gameObject.SetActive(true);
        capeOWindPS.Play();

        yield return new WaitForSeconds(0.85f);

        capeOWindPS.Stop();
        capeOWindPS.gameObject.SetActive(false);
    }

    #endregion
}
