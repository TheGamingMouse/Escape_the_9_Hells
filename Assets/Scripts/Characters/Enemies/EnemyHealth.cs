using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    #region Events

    public static event Action OnBossSpawn;

    #endregion

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

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        expSoulsManager = GameObject.FindWithTag("Managers").GetComponent<ExpSoulsManager>();
        enemySight = GetComponent<EnemySight>();

        if (!boss)
        {
            healthImage = transform.Find("Healthbar/Background/Foreground").GetComponent<Image>();
            healthbar = transform.Find("Healthbar").gameObject;
        }
        else
        {
            healthImage = GameObject.FindWithTag("Canvas").transform.Find("BossHealthbar/Background/Foreground").GetComponent<Image>();
            OnBossSpawn?.Invoke();
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
                    expSoulsManager.AddExperience(expAmount * 10, "devil");
                    bossGenerator.isBossDead = true;
                }
                Destroy(gameObject);
            }
        }
    }

    #endregion
}
