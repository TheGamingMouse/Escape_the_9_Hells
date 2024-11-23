using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    [HideInInspector]
    public bool godMode = false;
    public bool isDev = false;

    [Header("Components")]
    public Weapon weapon;

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (isDev && Input.GetKeyDown(KeyCode.P))
        {
            if (!godMode) EnableGodMode();
            else DisableGodMode();
        }
    }

    #endregion

    #region General Methods

    void EnableGodMode()
    {
        PlayerComponents.Instance.playerHealth.isInvinsible = true;
        PlayerComponents.Instance.playerMovement.speedMultiplier += 0.5f;
        weapon.damageMultiplier += 100f;

        UpdateWeaponDamage();

        godMode = true;
    }

    void DisableGodMode()
    {
        PlayerComponents.Instance.playerHealth.isInvinsible = false;
        PlayerComponents.Instance.playerMovement.speedMultiplier -= 0.5f;
        weapon.damageMultiplier -= 100f;

        UpdateWeaponDamage();

        godMode = false;
    }

    void UpdateWeaponDamage()
    {
        weapon.pugioObj.transform.GetChild(0).TryGetComponent(out PugioNormalAttack pugio);
        if (pugio != null)
        {
            pugio.damage = pugio.baseDamage * weapon.damageMultiplier;
            return;
        }

        weapon.ulfberhtObj.transform.GetChild(0).TryGetComponent(out UlfberhtAttack ulfberht);
        if (ulfberht != null)
        {
            ulfberht.damage = ulfberht.baseDamage * weapon.damageMultiplier;
            return;
        }
    }

    #endregion
}
