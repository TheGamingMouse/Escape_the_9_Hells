using System.Collections;
using UnityEngine;

public class BasicEnemyAction : MonoBehaviour
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
    public bool male;
    bool canLaugh = true;

    [Header("Animator")]
    Animator animator;
    
    [Header("AnimationClips")]
    AnimationClip clip;

    [Header("LayerMasks")]
    public LayerMask playerMask;

    [Header("Components")]
    BasicEnemyMovement enemyMovement;
    BasicEnemyHealth enemyHealth;
    Pugio pugio;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<BasicEnemyMovement>();
        enemyHealth = GetComponent<BasicEnemyHealth>();
        pugio = GetComponentInChildren<Pugio>();
        
        boss = enemyHealth.boss;
        pugio.canDamageEnemies = false;
        canAttack = true;

        if (boss)
        {
            male = true;
        }
        else
        {
            male = Random.Range(0, 10) < 2;
        }

        animator = pugio.GetComponentInParent<Animator>();
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        string clipName;
        if (!boss)
        {
            clipName = "Pugio Pierce";
        }
        else
        {
            clipName = "BasicBoss Slam";
        }
        foreach (AnimationClip c in clips)
        {
            if (c.name == clipName)
            {
                clip = c;
            }
        }

        animator.SetFloat("AttackSpeed", attackSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyMovement.targetInRange && !attacking && !boss)
        {
            StartAttack();
        }
        else if (enemyMovement.targetInRange && canAttack && boss)
        {
            animator.SetBool("Boss", boss);
            BossStartAttack();
        }

        if (canLaugh)
        {
            StartCoroutine(Laugh());
        }
    }

    #endregion

    #region Attack Methods

    void StartAttack()
    {
        pugio.piercing = true;
        animator.SetBool("Piercing", true);

        StartCoroutine(EndAttack());
        StartCoroutine(pugio.PlayNormalAudio());

        attacking = true;
    }

    IEnumerator EndAttack()
    {
        float clipLength = clip.length / animator.GetFloat("AttackSpeed");

        yield return new WaitForSeconds(clipLength);

        pugio.piercing = false;
        animator.SetBool("Piercing", false);

        attacking = false;
    }

    void BossStartAttack()
    {
        pugio.piercing = true;
        animator.SetBool("Piercing", true);
        
        StartCoroutine(BossEndAttack());

        attacking = true;
        canAttack = false;
    }

    IEnumerator BossEndAttack()
    {
        float clipLength = clip.length / animator.GetFloat("AttackSpeed");
        float attackTime = bossAttackTime / animator.GetFloat("AttackSpeed");

        yield return new WaitForSeconds(attackTime);

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
        var sfxManager = SFXAudioManager.Instance;

        int randLaugh;
        if (male)
        {
            randLaugh = Random.Range(0, sfxManager.enemyLaughMale.Count);
            sfxManager.PlayClip(sfxManager.enemyLaughMale[randLaugh], MasterAudioManager.Instance.sBlend3D, sfxManager.enemyVolumeMod, gameObject, "low");
        }
        else
        {
            randLaugh = Random.Range(0, sfxManager.enemyLaughFemale.Count);
            sfxManager.PlayClip(sfxManager.enemyLaughFemale[randLaugh], MasterAudioManager.Instance.sBlend3D, sfxManager.enemyVolumeMod, gameObject, "low");
        }
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
