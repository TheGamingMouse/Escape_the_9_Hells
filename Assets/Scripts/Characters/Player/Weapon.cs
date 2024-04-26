using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Properties

    [Header("Enum States")]
    public RangeState rState;
    public MeleeAttackType mType;
    public WeaponActive wActive;

    [Header("Ints")]
    public int attackSpeedMultiplier = 1;

    [Header("Bools")]
    public bool canAttack;

    [Header("GameObjects")]
    GameObject ulfberhtObj;
    GameObject pugioObj;

    [Header("Animator")]
    Animator ulfAnimator;
    Animator pugAnimator;

    [Header("AnimationClips")]
    AnimationClip ulfClip;
    AnimationClip pugClip;

    [Header("Components")]
    public Ulfberht ulfberht;
    public Pugio pugio;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        ulfberhtObj = ulfberht.gameObject;
        pugioObj = pugio.gameObject;

        if (wActive == WeaponActive.Ulfberht)
        {
            pugioObj.SetActive(false);
            ulfberht.canDamageEnemies = true;
        }
        else if (wActive == WeaponActive.Pugio)
        {
            ulfberhtObj.SetActive(false);
            pugio.canDamageEnemies = true;
        }

        if (ulfberhtObj.activeInHierarchy)
        {
            ulfAnimator = ulfberhtObj.GetComponent<Animator>();

            AnimationClip[] clips = ulfAnimator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip c in clips)
            {
                if (c.name == "Ulfberht Slash")
                {
                    ulfClip = c;
                }
            }
        }
        else if (pugioObj.activeInHierarchy)
        {
            pugAnimator = pugioObj.GetComponent<Animator>();

            AnimationClip[] clips = pugAnimator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip c in clips)
            {
                if (c.name == "Pugio Pierce")
                {
                    pugClip = c;
                }
            }
        }

        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ulfberhtObj.activeInHierarchy)
        {
            rState = RangeState.Melee;
            mType = MeleeAttackType.Slash;
        }
        else if (pugioObj.activeInHierarchy)
        {
            rState = RangeState.Melee;
            mType = MeleeAttackType.Pierce;
        }
    }

    #endregion

    #region Attack - Melee

    public void Slash()
    {
        if (wActive == WeaponActive.Ulfberht)
        {
            ulfAnimator.SetBool("Slashing", true);
            StartCoroutine(Unslash());

            ulfberht.slashing = true;
            canAttack = false;
        }
    }

    IEnumerator Unslash()
    {
        if (wActive == WeaponActive.Ulfberht)
        {
            float clipLength = ulfClip.length / ulfAnimator.GetFloat("AttackSpeed");

            yield return new WaitForSeconds(clipLength);

            ulfAnimator.SetBool("Slashing", false);

            ulfberht.slashing = false;
            canAttack = true;
        }
    }

    public void Pierce()
    {
        if (wActive == WeaponActive.Pugio)
        {
            pugio.piercing = true;
            pugAnimator.SetBool("Piercing", true);
            StartCoroutine(Unpierce());

            canAttack = false;
        }
    }

    IEnumerator Unpierce()
    {
        if (wActive == WeaponActive.Pugio)
        {
            float clipLength = pugClip.length / pugAnimator.GetFloat("AttackSpeed");

            yield return new WaitForSeconds(clipLength);

            pugio.piercing = false;
            pugAnimator.SetBool("Piercing", false);

            canAttack = true;
        }
    }

    public void IncreaseAttackMultiplier(int newValue)
    {
        attackSpeedMultiplier += newValue;
        Mathf.Clamp(attackSpeedMultiplier, 1, 10);

        if (wActive == WeaponActive.Ulfberht)
        {
            ulfAnimator.SetFloat("AttackSpeed", attackSpeedMultiplier);
        }
        else if (wActive == WeaponActive.Pugio)
        {
            pugAnimator.SetFloat("AttackSpeed", attackSpeedMultiplier);
        }
    }

    #endregion

    #region Enums

    public enum RangeState
    {
        Melee,
        ShortRange,
        LongRange
    }

    public enum MeleeAttackType
    {
        Slash,
        Pierce
    }

    public enum WeaponActive
    {
        Ulfberht,
        Pugio
    }

    #endregion
}
