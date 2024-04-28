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

    [Header("GameObjects")]
    GameObject healthbar;

    [Header("Images")]
    Image healthImage;

    [Header("Components")]
    public RoomSpawner roomSpawner;
    ExpSoulsManager expSoulsManager;
    EnemySight enemySight;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        expSoulsManager = GameObject.FindWithTag("Managers").GetComponent<ExpSoulsManager>();
        healthImage = transform.Find("Healthbar/Background/Foreground").GetComponent<Image>();
        healthbar = transform.Find("Healthbar").gameObject;
        enemySight = GetComponent<EnemySight>();
        
        health = maxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        healthbar.transform.rotation = new Quaternion(0.707106829f, 0f, 0f, 0.707106829f);
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
                roomSpawner.enemies.Remove(gameObject);
                expSoulsManager.AddExperience(expAmount, "demon");
                Destroy(gameObject);
            }
        }
    }

    #endregion
}
