using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BasicEnemyHealth : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int maxHealth = 75;
    public int soulAmount = 50;
    int health;
    public int expAmount = 25;

    [Header("Bools")]
    public bool boss;
    bool canTakeDamage = true;

    [Header("Strings")]
    public string bossName;

    [Header("GameObjects")]
    GameObject healthbar;

    [Header("Images")]
    Image healthImage;

    [Header("Components")]
    public RoomSpawner roomSpawner;
    ExpSoulsManager expSoulsManager;
    EnemySight enemySight;
    public BossGenerator bossGenerator;
    PlayerLevel playerLevel;
    UIManager uiManager;
    SFXAudioManager sfxManager;
    BasicEnemyAction basicEnemyAction;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var managers = GameObject.FindWithTag("Managers");

        expSoulsManager = managers.GetComponent<ExpSoulsManager>();
        enemySight = GetComponent<EnemySight>();
        playerLevel = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>();
        uiManager = managers.GetComponent<UIManager>();
        sfxManager = managers.GetComponent<SFXAudioManager>();
        basicEnemyAction = GetComponent<BasicEnemyAction>();

        if (!boss)
        {
            healthImage = transform.Find("HealthBarCanvas/Health Bar Fill").GetComponent<Image>();
            healthbar = transform.Find("HealthBarCanvas").gameObject;
        }
        else
        {
            healthImage = GameObject.FindWithTag("Canvas").transform.Find("Boss/BossHealthBar/Health Bar Fill").GetComponent<Image>();
            uiManager.bossNameString = bossName;
        }
        
        health = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!boss)
        {
            healthbar.transform.rotation = new Quaternion(0.707106829f, 0f, 0f, 0.707106829f);
        }
        healthImage.fillAmount = (float)health / maxHealth;
    }

    #endregion

    #region General Methods

    public void TakeDamage(int damage, bool aura)
    {
        if (enemySight.target)
        {
            if (aura)
            {
                if (canTakeDamage)
                {
                    health -= damage;
                    StartCoroutine(DamageFromAura());
                }
            }
            else
            {
                health -= damage;
            }

            if (health <= 0)
            {
                if (!boss)
                {
                    roomSpawner.enemies.Remove(gameObject);
                    expSoulsManager.AddExperience(expAmount, "demon");
                }
                else
                {
                    int luckCheck = Random.Range(1, 10001);
                    if (luckCheck <= playerLevel.luck)
                    {
                        sfxManager.PlayClip(sfxManager.activateLucky, sfxManager.masterManager.sBlend2D, sfxManager.effectsVolumeMod);
                        sfxManager.PlayClip(sfxManager.gainLevel, sfxManager.masterManager.sBlend2D, sfxManager.effectsVolumeMod);

                        int i = 0;
                        while (i < 3)
                        {
                            playerLevel.LevelUp(false, true, true);
                            i++;
                        }
                    }
                    else
                    {
                        sfxManager.PlayClip(sfxManager.gainLevel, sfxManager.masterManager.sBlend2D, sfxManager.effectsVolumeMod);

                        playerLevel.LevelUp(false, true, true);
                    }
                    
                    expSoulsManager.AddSouls(soulAmount, true);
                    bossGenerator.isBossDead = true;
                }
                
                foreach (var source in gameObject.GetComponents<AudioSource>())
                {
                    if (sfxManager.audioSourcePool.Contains(source))
                    {
                        sfxManager.audioSourcePool.Remove(source);
                    }
                }
                EnemyDeath();
                Destroy(gameObject);
            }
            else
            {
                EnemyDamage();
            }
        }
    }

    IEnumerator DamageFromAura()
    {
        canTakeDamage = false;

        yield return new WaitForSeconds(0.5f);

        canTakeDamage = true;
    }

    void EnemyDeath()
    {
        int randDeath;
        if (basicEnemyAction.male)
        {
            randDeath = Random.Range(0, sfxManager.enemyDeathMale.Count);
            sfxManager.PlayClip(sfxManager.enemyDeathMale[randDeath], sfxManager.masterManager.sBlend3D, sfxManager.enemyVolumeMod);
        }
        else
        {
            randDeath = Random.Range(0, sfxManager.enemyDeathFemale.Count);
            sfxManager.PlayClip(sfxManager.enemyDeathFemale[randDeath], sfxManager.masterManager.sBlend3D, sfxManager.enemyVolumeMod);
        }
    }

    void EnemyDamage()
    {
        int randDamage;
        if (basicEnemyAction.male)
        {
            randDamage = Random.Range(0, sfxManager.enemyDamageMale.Count);
            sfxManager.PlayClip(sfxManager.enemyDamageMale[randDamage], sfxManager.masterManager.sBlend3D, sfxManager.enemyVolumeMod, gameObject, "low");
        }
        else
        {
            randDamage = Random.Range(0, sfxManager.enemyDamageFemale.Count);
            sfxManager.PlayClip(sfxManager.enemyDamageFemale[randDamage], sfxManager.masterManager.sBlend3D, sfxManager.enemyVolumeMod, gameObject, "low");
        }
    }

    #endregion
}
