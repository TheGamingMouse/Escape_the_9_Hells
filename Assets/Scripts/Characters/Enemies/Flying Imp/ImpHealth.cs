using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImpHealth : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int maxHealth = 75;
    public int soulAmount = 60;
    int health;
    public int expAmount = 25;

    [Header("Bools")]
    public bool boss;
    [HideInInspector]
    public bool minion;
    bool canTakeDamage = true;

    [Header("Strings")]
    public string bossName;

    [Header("GameObjects")]
    GameObject healthbar;

    [Header("Images")]
    Image healthImage;

    [Header("Components")]
    [HideInInspector]
    public RoomSpawner roomSpawner;
    ExpSoulsManager expSoulsManager;
    EnemySight enemySight;
    [HideInInspector]
    public BossGenerator bossGenerator;
    PlayerLevel playerLevel;
    [HideInInspector]
    public MinionSpawner minionSpawner;
    UIManager uiManager;

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
                    if (!minion)
                    {
                        roomSpawner.enemies.Remove(gameObject);
                    }
                    else
                    {
                        minionSpawner.minions.Remove(gameObject);
                    }
                    expSoulsManager.AddExperience(expAmount, "demon");
                }
                else
                {
                    int luckCheck = Random.Range(1, 1001);
                    if (luckCheck <= playerLevel.luck)
                    {
                        playerLevel.AddExperience(300f * 2, true, "devil");
                    }
                    else
                    {
                        playerLevel.AddExperience(300f, true, "devil");
                    }

                    expSoulsManager.AddSouls(soulAmount, false);
                    bossGenerator.isBossDead = true;
                }
                Destroy(gameObject);
            }

            if (aura)
            {
                StartCoroutine(DamageFromAura());
            }
        }
    }

    IEnumerator DamageFromAura()
    {
        canTakeDamage = false;

        yield return new WaitForSeconds(0.5f);

        canTakeDamage = true;
    }

    #endregion
}
