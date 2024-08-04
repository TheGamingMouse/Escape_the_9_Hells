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
    public float attackSpeedMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float specialCooldownMultiplier = 1f;
    readonly float baseSpecialCooldown = 10f;
    float specialCooldown;

    [Header("Bools")]
    public bool canAttack;
    public bool specialAttack;
    public bool canSpecial;

    [Header("GameObjects")]
    GameObject ulfberhtObj;
    GameObject pugioObj;

    [Header("Transform")]
    Transform player;

    [Header("Lists")]
    readonly List<GameObject> weaponObjs = new();

    [Header("Animator")]
    Animator ulfAnimator;
    Animator pugAnimator;

    [Header("AnimationClips")]
    AnimationClip ulfClip;
    AnimationClip pugClip;
    public AnimationClip pugSpecialClip;
    AnimationClip ulfSpecialClip;

    [Header("Components")]
    public Ulfberht ulfberht;
    public Pugio pugio;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        ulfberhtObj = ulfberht.gameObject;
        pugioObj = pugio.gameObject;

        weaponObjs.Add(ulfberhtObj);
        weaponObjs.Add(pugioObj);

        if (wActive == WeaponActive.Ulfberht)
        {
            SwitchToUlfberht();
        }
        else if (wActive == WeaponActive.Pugio)
        {
            SwitchToPugio();
        }

        if (ulfberhtObj.activeInHierarchy)
        {
            SetUlfberhtAnimator();
        }
        else if (pugioObj.activeInHierarchy)
        {
            SetPugioAnimator();
        }

        canAttack = true;
        canSpecial = true;
    }

    // Update is called once per frame
    void Update()
    {
        specialCooldown = baseSpecialCooldown / specialCooldownMultiplier;

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

    // Ulfberht
    void SetUlfberhtAnimator()
    {
        ulfAnimator = ulfberhtObj.GetComponent<Animator>();

        AnimationClip[] clips = ulfAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip c in clips)
        {
            if (c.name == "Ulfberht Slash")
            {
                ulfClip = c;
            }
            else if (c.name == "Ulfberht Special Attack")
            {
                ulfSpecialClip = c;
            }
        }
    }

    public void UlfberhtStartNormal()
    {
        if (wActive == WeaponActive.Ulfberht)
        {
            ulfAnimator.SetBool("Slashing", true);

            StartCoroutine(UlfberhtEndNormal());
            StartCoroutine(ulfberht.PlayNormalAudio(true));

            ulfberht.slashing = true;
            canAttack = false;
        }
    }

    IEnumerator UlfberhtEndNormal()
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

    public void UlfberhtStartSpecial()
    {
        if (wActive == WeaponActive.Ulfberht)
        {
            ulfberht.specialAttacking = true;
            ulfAnimator.SetBool("Special Attack", true);

            StartCoroutine(UlfberhtEndSpecial());
            StartCoroutine(ulfberht.PlaySpecialAudio());

            canAttack = false;
            canSpecial = false;
        }
    }

    IEnumerator UlfberhtEndSpecial()
    {
        if (wActive == WeaponActive.Ulfberht)
        {
            float clipLength = ulfSpecialClip.length * 0.9f;

            yield return new WaitForSeconds(clipLength * 4f);

            ulfberht.specialAttacking = false;
            ulfAnimator.SetBool("Special Attack", false);

            canAttack = true;

            yield return new WaitForSeconds(specialCooldown);

            canSpecial = true;
        }
    }

    // Pugio
    void SetPugioAnimator()
    {
        pugAnimator = pugioObj.GetComponent<Animator>();

        AnimationClip[] clips = pugAnimator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip c in clips)
        {
            if (c.name == "Pugio Pierce")
            {
                pugClip = c;
            }
            else if (c.name == "Pugio Special Attack")
            {
                pugSpecialClip = c;
            }
        }
    }
    
    public void PugioStartNormal()
    {
        if (wActive == WeaponActive.Pugio)
        {
            pugio.piercing = true;
            pugAnimator.SetBool("Piercing", true);

            StartCoroutine(PugioEndNormal());
            StartCoroutine(pugio.PlayNormalAudio(true));

            canAttack = false;
        }
    }

    IEnumerator PugioEndNormal()
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

    public void PugioStartSpecial()
    {
        if (wActive == WeaponActive.Pugio)
        {
            pugio.specialAttacking = true;
            pugAnimator.SetBool("Special Attack", true);

            StartCoroutine(PugioEndSpecial());
            StartCoroutine(pugio.PlaySpecialAudio());

            canAttack = false;
            canSpecial = false;
        }
    }

    IEnumerator PugioEndSpecial()
    {
        if (wActive == WeaponActive.Pugio)
        {
            float clipLength = pugSpecialClip.length * 0.9f;

            yield return new WaitForSeconds(clipLength);

            pugio.specialAttacking = false;
            pugAnimator.SetBool("Special Attack", false);

            canAttack = true;

            yield return new WaitForSeconds(specialCooldown);

            canSpecial = true;
        }
    }

    #endregion

    #region Weapon Swap

    public void SwitchToUlfberht()
    {
        if (ulfberhtObj)
        {
            ulfberht.player = player;
            wActive = WeaponActive.Ulfberht;

            foreach (GameObject weaponObj in weaponObjs)
            {
                weaponObj.SetActive(false);
            }
            ulfberhtObj.SetActive(true);
            
            pugio.canDamageEnemies = false;

            ulfAnimator = ulfberhtObj.GetComponent<Animator>();

            SetUlfberhtAnimator();
        }
    }

    public void SwitchToPugio()
    {
        if (pugioObj)
        {
            pugio.player = player;
            wActive = WeaponActive.Pugio;

            foreach (GameObject weaponObj in weaponObjs)
            {
                weaponObj.SetActive(false);
            }
            pugioObj.SetActive(true);
            pugio.canDamageEnemies = true;

            pugAnimator = pugioObj.GetComponent<Animator>();

            SetPugioAnimator();
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
