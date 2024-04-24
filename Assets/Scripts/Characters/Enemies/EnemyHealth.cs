using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    public int maxHealth = 75;
    int health;
    public int expAmount = 50;

    [Header("GameObjects")]
    GameObject healthbar;

    [Header("Images")]
    Image healthImage;

    [Header("Components")]
    public RoomSpawner roomSpawner;
    ExpSoulsManager expSoulsManager;
    Camera cam;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        expSoulsManager = GameObject.FindWithTag("Managers").GetComponent<ExpSoulsManager>();
        healthImage = transform.Find("Healthbar/Background/Foreground").GetComponent<Image>();
        healthbar = transform.Find("Healthbar").gameObject;
        cam = Camera.main;
        
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

            healthbar.transform.rotation = Quaternion.LookRotation(healthbar.transform.position - cam.transform.position);
            healthImage.fillAmount = (float)health / maxHealth;

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
