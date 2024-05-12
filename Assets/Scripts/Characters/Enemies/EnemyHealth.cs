using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int maxHealth = 75;
    int health;
    public int expAmount = 25;

    [Header("Bools")]
    public bool boss;

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

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        expSoulsManager = GameObject.FindWithTag("Managers").GetComponent<ExpSoulsManager>();
        enemySight = GetComponent<EnemySight>();
        playerLevel = GameObject.FindWithTag("Player").GetComponent<PlayerLevel>();

        if (!boss)
        {
            healthImage = transform.Find("HealthBarCanvas/Health Bar Fill").GetComponent<Image>();
            healthbar = transform.Find("HealthBarCanvas").gameObject;
        }
        else
        {
            healthImage = GameObject.FindWithTag("Canvas").transform.Find("BossHealthBar/Health Bar Fill").GetComponent<Image>();
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

    public void TakeDamage(int damage)
    {
        if (enemySight.target)
        {
            health -= damage;

            if (health <= 0)
            {
                if (!boss)
                {
                    roomSpawner.enemies.Remove(gameObject);
                    expSoulsManager.AddExperience(expAmount, "demon");
                }
                else
                {
                    expSoulsManager.AddExperience((int)playerLevel.expMultiplier * 3, "devil");
                    expSoulsManager.AddSouls(50, false);
                    bossGenerator.isBossDead = true;
                }
                Destroy(gameObject);
            }
        }
    }

    #endregion
}
