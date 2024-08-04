using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpAction : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    readonly int damage = 7;

    [Header("Floats")]
    readonly float attackSpeed = 2.5f;
    readonly float bossCooldown = 4.5f;         // Only relevant for boss
    readonly float force = 10f;
    readonly float bossUpForce = 30f;           // Only relevant for boss
    readonly float bossFrontForce = 1f;         // Only relevant for boss

    [Header("Bools")]
    [HideInInspector]
    public bool attacking;
    [HideInInspector]
    public bool boss;
    bool canAttack;
    bool cooling;
    public bool male;
    bool canLaugh = true;

    [Header("GameObjects")]
    public GameObject firebolt;
    public GameObject fireball;

    [Header("LayerMasks")]
    public LayerMask playerMask;

    [Header("Components")]
    ImpMovement enemyMovement;
    ImpHealth enemyHealth;
    EnemySight enemySight;
    SFXAudioManager sfxManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<ImpMovement>();
        enemyHealth = GetComponent<ImpHealth>();
        enemySight = GetComponent<EnemySight>();

        sfxManager = GameObject.FindWithTag("Managers").GetComponent<SFXAudioManager>();
        
        boss = enemyHealth.boss;

        if (boss)
        {
            male = true;
        }
        else
        {
            male = Random.Range(0, 10) < 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMovement.targetInRange && canAttack)
        {
            if (boss)
            {
                BossAttack();
            }
            else
            {
                Attack();
            }
        }
        
        if (!cooling && enemySight.target)
        {
            StartCoroutine(CoolingRoutine());
            cooling = true;
        }

        if (!enemySight.target)
        {
            cooling = false;
            canAttack = false;
            StopAllCoroutines();
        }

        if (canLaugh)
        {
            StartCoroutine(Laugh());
        }
    }

    #endregion

    #region Attack Methods

    void Attack()
    {
        GameObject newProjectile = Instantiate(firebolt, transform.position, Quaternion.identity, GameObject.FindWithTag("PrefabStorage").transform);
        
        newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
        newProjectile.GetComponent<Firebolt>().damage = damage;
        newProjectile.GetComponent<Firebolt>().explostionScale = 0.75f;
        newProjectile.GetComponent<Firebolt>().canDamagePlayer = true;
        newProjectile.GetComponent<Firebolt>().canDamageEnemies = false;
        newProjectile.GetComponent<Firebolt>().sfxManager = sfxManager;

        Destroy(newProjectile, 10f);

        StartCoroutine(BeginCooldown(attackSpeed));

        attacking = true;
        canAttack = false;

        AttackAudio();
    }

    void BossAttack()
    {
        GameObject newProjectile = Instantiate(fireball, transform.position, Quaternion.identity, GameObject.FindWithTag("PrefabStorage").transform);
        
        newProjectile.GetComponent<Rigidbody>().AddForce(transform.up * bossUpForce, ForceMode.Impulse);
        newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * bossFrontForce, ForceMode.Impulse);
        newProjectile.GetComponent<Firebolt>().damage = damage * 2;
        newProjectile.GetComponent<Firebolt>().explostionScale = 3.5f;
        newProjectile.GetComponent<Firebolt>().canDamagePlayer = true;
        newProjectile.GetComponent<Firebolt>().canDamageEnemies = false;
        newProjectile.GetComponent<Firebolt>().sfxManager = sfxManager;

        Destroy(newProjectile, 10f);

        StartCoroutine(BeginCooldown(bossCooldown));

        attacking = true;
        canAttack = false;
    }

    IEnumerator BeginCooldown(float cooldownTimer)
    {
        attacking = false;

        yield return new WaitForSeconds(cooldownTimer);

        canAttack = true;
    }

    IEnumerator CoolingRoutine()
    {
        TargetingAudio();

        yield return new WaitForSeconds(attackSpeed / 2f);

        canAttack = true;
    }

    IEnumerator Laugh()
    {
        canLaugh = false;

        yield return new WaitForSeconds(5f);

        if (Random.Range(0, 3) == 0)
        {
            EnemyLaugh();
        }
        canLaugh = true;
    }

    void EnemyLaugh()
    {
        int randLaugh;
        if (male)
        {
            randLaugh = Random.Range(0, sfxManager.enemyLaughMale.Count);
            sfxManager.PlayClip(sfxManager.enemyLaughMale[randLaugh], sfxManager.masterManager.sBlend3D, sfxManager.enemyVolumeMod, gameObject, "low");
        }
        else
        {
            randLaugh = Random.Range(0, sfxManager.enemyLaughFemale.Count);
            sfxManager.PlayClip(sfxManager.enemyLaughFemale[randLaugh], sfxManager.masterManager.sBlend3D, sfxManager.enemyVolumeMod, gameObject, "low");
        }
    }

    void TargetingAudio()
    {
        int randTarget;
        if (male)
        {
            randTarget = Random.Range(0, sfxManager.impTargetingMale.Count);
            sfxManager.PlayClip(sfxManager.impTargetingMale[randTarget], sfxManager.masterManager.sBlend3D, sfxManager.enemyVolumeMod * 2, gameObject);
        }
        else
        {
            randTarget = Random.Range(0, sfxManager.impTargetingFemale.Count);
            sfxManager.PlayClip(sfxManager.impTargetingFemale[randTarget], sfxManager.masterManager.sBlend3D, sfxManager.enemyVolumeMod * 2, gameObject);
        }
    }

    void AttackAudio()
    {
        int randAttack;
        if (male)
        {
            randAttack = Random.Range(0, sfxManager.impAttackMale.Count);
            sfxManager.PlayClip(sfxManager.impAttackMale[randAttack], sfxManager.masterManager.sBlend3D, sfxManager.enemyVolumeMod, gameObject);
        }
        else
        {
            randAttack = Random.Range(0, sfxManager.impAttackFemale.Count);
            sfxManager.PlayClip(sfxManager.impAttackFemale[randAttack], sfxManager.masterManager.sBlend3D, sfxManager.enemyVolumeMod, gameObject);
        }

        sfxManager.PlayClip(sfxManager.firebolt, sfxManager.masterManager.sBlend3D, sfxManager.effectsVolumeMod, gameObject);
    }

    #endregion
}
