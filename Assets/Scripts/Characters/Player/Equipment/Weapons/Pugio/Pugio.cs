using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pugio : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float baseDamage = 12;
    public float damage;

    [Header("Bools")]
    public bool piercing;
    public bool canDamageEnemies;
    public bool specialAttacking;
    public bool playNormalAudio;
    bool canPlayAudio;
    public bool bossSlam;
    bool bossSlamming;

    [Header("Transform")]
    public Transform player;

    [Header("Components")]
    BasicEnemyAction enemyAction;
    public Weapon weapon;
    public ParticleSystem particles;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        enemyAction = GetComponentInParent<BasicEnemyAction>();

        if (particles)
        {
            particles.Stop();
        }
    }

    void Update()
    {
        if (enemyAction != null)
        {
            damage = baseDamage / 2;
        }
        else if (weapon != null)
        {
            damage = baseDamage * weapon.damageMultiplier;
        }

        if (bossSlam && !bossSlamming)
        {
            StartCoroutine(BossSlamRoutine());
        }
    }

    #endregion

    #region Boss

    IEnumerator BossSlamRoutine()
    {
        var sfxManager = SFXAudioManager.Instance;

        bossSlamming = true;
        particles.Play();

        yield return new WaitForSeconds(1f);

        sfxManager.PlayClip(sfxManager.cainAttack, MasterAudioManager.Instance.sBlend3D, sfxManager.enemyVolumeMod / 1.5f, true, "none", gameObject, 0.6f);

        yield return new WaitForSeconds(0.8f);

        particles.Stop();
        bossSlamming = false;
    }

    #endregion

    #region Colliders

    void OnTriggerEnter(Collider coll)
    {
        if (piercing)
        {
            if (canDamageEnemies)
            {
                if (coll.TryGetComponent(out Ricky ricky))
                {
                    ricky.TakeDamage((int)damage);
                }
                
                if (coll.TryGetComponent(out BasicEnemyHealth eComp))
                {
                    eComp.TakeDamage((int)damage, false);
                }
                else if (coll.TryGetComponent(out ImpHealth iComp))
                {
                    iComp.TakeDamage((int)damage, false);
                }
            }

            if (coll.transform.CompareTag("Player") && coll.transform.TryGetComponent(out PlayerHealth pComp) && enemyAction.attacking)
            {
                pComp.TakeDamage((int)damage);
            }
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (specialAttacking)
        {
            if (canDamageEnemies)
            {
                if (coll.TryGetComponent(out Ricky ricky))
                {
                    ricky.TakeDamage((int)damage);
                }
                
                if (coll.TryGetComponent(out BasicEnemyHealth eComp))
                {
                    eComp.TakeDamage((int)damage, false);
                }
                else if (coll.TryGetComponent(out ImpHealth iComp))
                {
                    iComp.TakeDamage((int)damage, false);
                }
            }
        }

        if (piercing && player && Vector3.Distance(coll.transform.position, player.position) < 1.3f)
        {
            if (canDamageEnemies)
            {
                if (coll.TryGetComponent(out Ricky ricky))
                {
                    ricky.TakeDamage((int)damage / 2);
                }
                
                if (coll.TryGetComponent(out BasicEnemyHealth eComp))
                {
                    eComp.TakeDamage((int)damage / 2, false);
                }
                else if (coll.TryGetComponent(out ImpHealth iComp))
                {
                    iComp.TakeDamage((int)damage / 2, false);
                }
            }
        }
    }

    public IEnumerator PlayNormalAudio(bool isPlayer = false)
    {
        var sfxManager = SFXAudioManager.Instance;

        if (!canPlayAudio)
        {
            yield return null;
        }

        if (isPlayer)
        {
            sfxManager.PlayClip(sfxManager.pugio, MasterAudioManager.Instance.sBlend2D, sfxManager.weaponVolumeMod, true, "low");
        }
        else
        {
            sfxManager.PlayClip(sfxManager.pugio, MasterAudioManager.Instance.sBlend2D, sfxManager.weaponVolumeMod / 2, true, "low");
        }

        canPlayAudio = false;

        yield return new WaitForSeconds(0.1f);

        canPlayAudio = true;
    }

    public IEnumerator PlaySpecialAudio()
    {
        var sfxManager = SFXAudioManager.Instance;
        
        for (int i = 0; i < weapon.pugSpecialClip.length * 9; i++)
        {
            sfxManager.PlayClip(sfxManager.pugio, MasterAudioManager.Instance.sBlend2D, sfxManager.weaponVolumeMod, true, "low");

            yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion
}
