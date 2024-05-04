using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    float attackSpeed = 0.5f;
    readonly float bossAttackTime = 1.167f;   // Only relevant for boss
    readonly float splashRange = 4f;          // Only relevant for boss
    readonly float cooldown = 0.5f;           // Only relevant for boss

    [Header("Bools")]
    public bool attacking;
    public bool boss;
    bool canAttack;

    [Header("Animator")]
    Animator animator;
    
    [Header("AnimationClips")]
    AnimationClip clip;

    [Header("LayerMasks")]
    public LayerMask playerMask;

    [Header("Components")]
    EnemyMovement enemyMovement;
    Pugio pugio;
    EnemyHealth enemyHealth;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyHealth = GetComponent<EnemyHealth>();
        
        boss = enemyHealth.boss;

        pugio = GetComponentInChildren<Pugio>();
        pugio.canDamageEnemies = false;

        animator = pugio.GetComponentInParent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        if (!boss)
        {
            foreach (AnimationClip c in clips)
            {
                if (c.name == "Pugio Pierce")
                {
                    clip = c;
                }
            }
        }
        else
        {
            foreach (AnimationClip c in clips)
            {
                if (c.name == "BasicBoss Slam")
                {
                    clip = c;
                }
            }

            attackSpeed *= 4;
            pugio.damage *= 10f;
        }

        animator.SetFloat("AttackSpeed", attackSpeed);
        animator.SetBool("Boss", boss);

        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMovement.targetInRange && !attacking && !boss)
        {
            Attack();
        }
        else if (enemyMovement.targetInRange && canAttack)
        {
            BossAttack();
        }
    }

    #endregion

    #region Attack Methods

    void Attack()
    {
        pugio.piercing = true;
        animator.SetBool("Piercing", true);
        StartCoroutine(Unattack());

        attacking = true;
    }

    IEnumerator Unattack()
    {
        float clipLength = clip.length / animator.GetFloat("AttackSpeed");

        yield return new WaitForSeconds(clipLength);

        pugio.piercing = false;
        animator.SetBool("Piercing", false);

        attacking = false;
    }

    void BossAttack()
    {
        pugio.piercing = true;
        animator.SetBool("Piercing", true);
        StartCoroutine(BossUnattack());

        attacking = true;
        canAttack = false;
    }

    IEnumerator BossUnattack()
    {
        float clipLength = clip.length / animator.GetFloat("AttackSpeed");
        float attackTime = bossAttackTime / animator.GetFloat("AttackSpeed");

        yield return new WaitForSeconds(attackTime);

        // Play particle system

        Collider[] colliders = Physics.OverlapSphere(pugio.transform.position, splashRange, playerMask);
        if (colliders.Length > 0 && colliders[0].GetComponent<PlayerHealth>())
        {
            colliders[0].GetComponent<PlayerHealth>().TakeDamage((int)pugio.damage * 5);
        }

        yield return new WaitForSeconds(clipLength - attackTime - clipLength * 0.05f);
        
        pugio.piercing = false;
        animator.SetBool("Piercing", false);
        attacking = false;

        yield return new WaitForSeconds(cooldown);

        canAttack = true;
    }

    #endregion

    #region Gizmos

    void OnDrawGizmosSelected()
    {
        if (pugio)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(pugio.transform.position, splashRange);
        }
    }

    #endregion
}
