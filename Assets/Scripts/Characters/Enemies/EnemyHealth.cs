using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    readonly int maxHealth = 75;
    int health;

    [Header("Components")]
    public RoomSpawner roomSpawner;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
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
        health -= damage;

        if (health <= 0)
        {
            roomSpawner.enemies.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    #endregion
}
