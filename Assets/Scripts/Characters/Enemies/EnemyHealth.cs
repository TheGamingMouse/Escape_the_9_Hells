using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    public int maxHealth = 75;
    int health;
    public int expAmount = 50;

    [Header("Components")]
    public RoomSpawner roomSpawner;
    ExpSoulsManager expSoulsManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        expSoulsManager = GameObject.FindWithTag("Managers").GetComponent<ExpSoulsManager>();
        
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region General Methods

    public void TakeDamage(int damage)
    {
        if (roomSpawner.inArea)
        {
            health -= damage;

            if (health <= 0)
            {
                roomSpawner.enemies.Remove(gameObject);
                expSoulsManager.AddExperience(expAmount);
                Destroy(gameObject);
            }
        }
    }

    #endregion
}
