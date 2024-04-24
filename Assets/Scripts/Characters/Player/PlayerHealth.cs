using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    [Range(0, 100)]
    readonly int maxHealth = 100;
    
    [Range(0, 100)]
    public int health;

    [Header("Bools")]
    bool playerDead;

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
        if (health <= 0 && !playerDead)
        {
            print("Player is dead");
            playerDead = true;
        }
    }

    #endregion

    #region General Methods

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    #endregion
}
