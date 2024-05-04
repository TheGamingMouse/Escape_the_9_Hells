using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
    public RangeState rState;
    public AttackType aType;
    public WeaponActive wActive;

    [Header("Floats")]
    public float attackSpeedMultiplier = 1;
    public float damageMultiplier = 1;

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
            ulfberhtObj.SetActive(true);
            ulfberht.canDamageEnemies = true;
            
            pugio.canDamageEnemies = false;
            pugioObj.SetActive(false);
        }
        else if (wActive == WeaponActive.Pugio)
        {
            pugioObj.SetActive(true);
            pugio.canDamageEnemies = true;
            
            ulfberht.canDamageEnemies = false;
            ulfberhtObj.SetActive(false);
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
            aType = AttackType.Slash;

            ulfberht.weapon = this;
            ulfAnimator.SetFloat("AttackSpeed", attackSpeedMultiplier);
        }
        else if (pugioObj.activeInHierarchy)
        {
            rState = RangeState.Melee;
            aType = AttackType.Pierce;

            pugio.weapon = this;
            pugAnimator.SetFloat("AttackSpeed", attackSpeedMultiplier);
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

    #endregion

    #region Weapon Swap

    public void SwitchToUlfberht()
    {
        wActive = WeaponActive.Ulfberht;

        ulfberhtObj.SetActive(true);
        ulfberht.canDamageEnemies = true;
        
        pugio.canDamageEnemies = false;
        pugioObj.SetActive(false);

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

    public void SwitchToPugio()
    {
        wActive = WeaponActive.Pugio;
        
        pugioObj.SetActive(true);
        pugio.canDamageEnemies = true;
        
        ulfberht.canDamageEnemies = false;
        ulfberhtObj.SetActive(false);

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

    #endregion

    #region Enums

    public enum RangeState
    {
        Melee,
        ShortRange,
        LongRange
    }

    public enum AttackType
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
