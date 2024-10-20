using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSquareCombat : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    readonly int baseDamage = 13;
    int damage;

    [Header("Floats")]
    readonly float baseCooldown = 1.5f;
    float cooldown;

    [Header("Bools")]
    bool canAttack = true;

    [Header("Components")]
    public Companion companion;

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (companion != null)
        {
            damage = (int)(baseDamage * companion.abilityStrengthMultiplier);
            cooldown = baseCooldown / companion.abilityRateMultiplier;
        }
    }

    #endregion

    #region General Methods

    void OnCollisionStay(Collision coll)
    {
        var sfxManager = SFXAudioManager.Instance;

        if (coll.transform.TryGetComponent(out BasicEnemyHealth eComp) && canAttack)
        {
            eComp.TakeDamage(damage, false);
            StartCoroutine(DamageRoutine());

            sfxManager.PlayClip(sfxManager.attackSquareHit, MasterAudioManager.Instance.sBlend3D, sfxManager.effectsVolumeMod, gameObject, "low");
        }
        else if (coll.transform.TryGetComponent(out ImpHealth iComp) && canAttack)
        {
            iComp.TakeDamage(damage, false);

            sfxManager.PlayClip(sfxManager.attackSquareHit, MasterAudioManager.Instance.sBlend3D, sfxManager.effectsVolumeMod, gameObject, "low");
        }
    }

    IEnumerator DamageRoutine()
    {
        canAttack = false;

        yield return new WaitForSeconds(cooldown);

        canAttack = true;
    }

    #endregion
}
