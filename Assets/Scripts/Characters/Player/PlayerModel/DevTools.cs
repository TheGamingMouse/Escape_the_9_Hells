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
    Weapon weapon;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
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
            PlayerComponents.Instance.playerHealth.isInvinsible = true;
            PlayerComponents.Instance.playerMovement.speedMultiplier = 1.25f;
            weapon.damageMultiplier = 100f;

            godMode = true;
        }
    }

    void DisableGodMode()
    {
        if (godMode)
        {
            PlayerComponents.Instance.playerHealth.isInvinsible = false;
            PlayerComponents.Instance.playerMovement.speedMultiplier = 1f;
            weapon.damageMultiplier = 1f;

            godMode = false;
        }
    }

    #endregion
}
