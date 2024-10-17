using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SaveSystemSpace.SaveClasses;

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
    public Shield shield;
    public ParticleSystem capeOWindParticleSystem;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        capeOWind = transform.GetComponentInChildren<Backs>().capeOWind;

        health = maxHealth;

        var layerData = SaveSystem.loadedLayerData;
        var persistentData = SaveSystem.loadedPersistentData;

        if (layerData.lState == LayerData.LayerState.InLayers)
        {
            health = persistentData.healthInLayer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !playerDead)
        {
            if (capeOWind.active && capeOWind.canSave)
            {
                var sfxManager = SFXAudioManager.Instance;

                health = maxHealth;
                StartCoroutine(capeOWind.Save());
                StartCoroutine(CapeSave());

                sfxManager.PlayClip(sfxManager.capeOWindActivate, MasterAudioManager.Instance.sBlend2D, sfxManager.backVolumeMod / 2, false, "high");

                var persistentData = SaveSystem.loadedPersistentData;
                persistentData.healthInLayer = health;

                SaveSystem.Instance.Save(persistentData, SaveSystem.persistentDataPath);

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
                var sfxManager = SFXAudioManager.Instance;
                
                health -= damage / resistanceMultiplier;

                int i = UnityEngine.Random.Range(0, sfxManager.playerDamage.Count);
                sfxManager.PlayClip(sfxManager.playerDamage[i], MasterAudioManager.Instance.sBlend2D, sfxManager.playerVolumeMod);
            }
            else if ((health - damage) < 0)
            {
                health = 0;
            }
            
            if (!shield.onCooldown)
            {
                shield.damageTaken = true;
            }

            var persistentData = SaveSystem.loadedPersistentData;
            persistentData.healthInLayer = health;

            SaveSystem.Instance.Save(persistentData, SaveSystem.persistentDataPath);
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
        capeOWindParticleSystem.gameObject.SetActive(true);
        capeOWindParticleSystem.Play();

        yield return new WaitForSeconds(0.85f);

        capeOWindParticleSystem.Stop();
        capeOWindParticleSystem.gameObject.SetActive(false);
    }

    #endregion
}
