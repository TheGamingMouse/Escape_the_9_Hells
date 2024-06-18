using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    [HideInInspector]
    public bool godMode;
    public bool isDev = false;

    [Header("Transforms")]
    public Transform toBossRoom;

    [Header("Components")]
    PlayerHealth playerHealth;
    PlayerMovement playerMovement;
    Weapon weapon;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerMovement = GetComponent<PlayerMovement>();
        weapon = GetComponentInChildren<Weapon>();

        godMode = false;
    }

    void Update()
    {
        if (isDev)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (!godMode)
                {
                    EnableGodMode();
                }
                else
                {
                    DisableGodMode();
                }
            }
        }
    }

    #endregion

    #region General Methods

    void EnableGodMode()
    {
        if (!godMode)
        {
            playerHealth.isInvinsible = true;
            playerMovement.speedMultiplier = 1.25f;
            weapon.damageMultiplier = 100f;

            godMode = true;
        }
    }

    void DisableGodMode()
    {
        if (godMode)
        {
            playerHealth.isInvinsible = false;
            playerMovement.speedMultiplier = 1f;
            weapon.damageMultiplier = 1f;

            godMode = false;
        }
    }

    #endregion
}
