using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    # region Variables

    public int maxHealth;
    public int soulAmount;
    int health;
    public int expAmount;

    public bool boss;
    bool canTakeDamage = true;
    [HideInInspector]
    public bool minion;

    public string bossName;

    GameObject healthbar;
    Image healthImage;

    public RoomSpawner roomSpawner;
    EnemySight enemySight;

    #endregion

    void Start()
    {
        enemySight = GetComponent<EnemySight>();

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

    void FixedUpdate()
    {
        if (!boss) healthbar.transform.rotation = new Quaternion(0.707106829f, 0f, 0f, 0.707106829f);
        healthImage.fillAmount = (float)health / maxHealth;
    }

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
            else health -= damage;
            
            if (health <= 0)
            {
                var expSoulsManager = ExpSoulsManager.Instance;
                var sfxManager = SFXAudioManager.Instance;
                var playerLevel = PlayerComponents.Instance.playerLevel;

                if (!boss)
                {
                    if (!minion) roomSpawner.enemies.Remove(gameObject);
                    else MinionSpawner.Instance.minions.Remove(gameObject);

                    expSoulsManager.AddExperience(expAmount, "demon");
                }
                else
                {
                    int luckCheck = Random.Range(1, 10001);
                    if (luckCheck <= playerLevel.luck)
                    {
                        sfxManager.PlayClip(sfxManager.activateLucky, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod);
                        sfxManager.PlayClip(sfxManager.gainLevel, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod);

                        int i = 0;
                        while (i < 3)
                        {
                            playerLevel.LevelUp(false, true, true);
                            i++;
                        }
                    }
                    else
                    {
                        sfxManager.PlayClip(sfxManager.gainLevel, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod);
                        playerLevel.LevelUp(false, true, true);
                    }
                    
                    expSoulsManager.AddSouls(soulAmount, true);
                    BossGenerator.Instance.isBossDead = true;
                }
                foreach (var source in gameObject.GetComponents<AudioSource>())
                    if (sfxManager.audioSourcePool.Contains(source)) sfxManager.audioSourcePool.Remove(source);

                EnemyDeath();
                Destroy(gameObject);
            }
            else EnemyDamage();
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
        var enemyAction = GetComponent<IEnemyAction>();

        int randDeath;
        if (enemyAction.male)
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
        var enemyAction = GetComponent<IEnemyAction>();
        
        int randDamage;
        if (enemyAction.male)
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
}
