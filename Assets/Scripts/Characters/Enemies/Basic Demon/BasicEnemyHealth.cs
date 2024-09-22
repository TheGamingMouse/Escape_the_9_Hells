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
    BasicEnemyAction basicEnemyAction;
    EnemySight enemySight;
    

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        enemySight = GetComponent<EnemySight>();
        basicEnemyAction = GetComponent<BasicEnemyAction>();

        if (!boss)
        {
            healthImage = transform.Find("HealthBarCanvas/Health Bar Fill").GetComponent<Image>();
            healthbar = transform.Find("HealthBarCanvas").gameObject;
        }
        else
        {
            healthImage = GameObject.FindWithTag("Canvas").transform.Find("Boss/BossHealthBar/Health Bar Fill").GetComponent<Image>();
            UIManager.Instance.bossNameString = bossName;
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
                    ExpSoulsManager.Instance.AddExperience(expAmount, "demon");
                }
                else
                {
                    int luckCheck = Random.Range(1, 10001);
                    if (luckCheck <= PlayerComponents.Instance.playerLevel.luck)
                    {
                        var sfxManager = SFXAudioManager.Instance;

                        sfxManager.PlayClip(sfxManager.activateLucky, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod);
                        sfxManager.PlayClip(sfxManager.gainLevel, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod);

                        int i = 0;
                        while (i < 3)
                        {
                            PlayerComponents.Instance.playerLevel.LevelUp(false, true, true);
                            i++;
                        }
                    }
                    else
                    {
                        var sfxManager = SFXAudioManager.Instance;
                        sfxManager.PlayClip(sfxManager.gainLevel, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod);

                        PlayerComponents.Instance.playerLevel.LevelUp(false, true, true);
                    }
                    
                    ExpSoulsManager.Instance.AddSouls(soulAmount, true);
                    BossGenerator.Instance.isBossDead = true;
                }
                
                foreach (var source in gameObject.GetComponents<AudioSource>())
                {
                    var sfxManager = SFXAudioManager.Instance;

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
        var sfxManager = SFXAudioManager.Instance;

        int randDeath;
        if (basicEnemyAction.male)
        {
            randDeath = Random.Range(0, sfxManager.enemyDeathMale.Count);
            sfxManager.PlayClip(sfxManager.enemyDeathMale[randDeath], MasterAudioManager.Instance.sBlend3D, sfxManager.enemyVolumeMod);
        }
        else
        {
            randDeath = Random.Range(0, sfxManager.enemyDeathFemale.Count);
            sfxManager.PlayClip(sfxManager.enemyDeathFemale[randDeath], MasterAudioManager.Instance.sBlend3D, sfxManager.enemyVolumeMod);
        }
    }

    void EnemyDamage()
    {
        var sfxManager = SFXAudioManager.Instance;
        
        int randDamage;
        if (basicEnemyAction.male)
        {
            randDamage = Random.Range(0, sfxManager.enemyDamageMale.Count);
            sfxManager.PlayClip(sfxManager.enemyDamageMale[randDamage], MasterAudioManager.Instance.sBlend3D, sfxManager.enemyVolumeMod, gameObject, "low");
        }
        else
        {
            randDamage = Random.Range(0, sfxManager.enemyDamageFemale.Count);
            sfxManager.PlayClip(sfxManager.enemyDamageFemale[randDamage], MasterAudioManager.Instance.sBlend3D, sfxManager.enemyVolumeMod, gameObject, "low");
        }
    }

    #endregion
}
