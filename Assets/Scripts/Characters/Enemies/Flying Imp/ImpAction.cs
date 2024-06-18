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

    [Header("GameObjects")]
    public GameObject firebolt;
    public GameObject fireball;

    [Header("LayerMasks")]
    public LayerMask playerMask;

    [Header("Components")]
    ImpMovement enemyMovement;
    ImpHealth enemyHealth;
    EnemySight enemySight;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<ImpMovement>();
        enemyHealth = GetComponent<ImpHealth>();
        enemySight = GetComponent<EnemySight>();
        
        boss = enemyHealth.boss;
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

        Destroy(newProjectile, 10f);

        StartCoroutine(BeginCooldown(attackSpeed));

        attacking = true;
        canAttack = false;
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
        yield return new WaitForSeconds(attackSpeed / 2f);

        canAttack = true;
    }

    #endregion
}
