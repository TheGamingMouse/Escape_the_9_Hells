using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : MonoBehaviour
{
    #region Properties

    [Header("Floats")]
    readonly float attackSpeed = 0.5f;

    [Header("Bools")]
    public bool attacking;

    [Header("Animator")]
    Animator animator;
    
    [Header("AnimationClips")]
    AnimationClip clip;

    [Header("Components")]
    EnemyMovement enemyMovement;
    Pugio pugio;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        pugio = GetComponentInChildren<Pugio>();
        pugio.canDamageEnemies = false;

        animator = pugio.GetComponentInParent<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip c in clips)
        {
            if (c.name == "Pugio Pierce")
            {
                clip = c;
            }
        }

        animator.SetFloat("AttackSpeed", attackSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMovement.targetInRange && !attacking)
        {
            Attack();
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

    #endregion
}
