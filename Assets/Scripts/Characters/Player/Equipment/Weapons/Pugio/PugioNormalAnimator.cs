using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PugioNormalAnimator : MonoBehaviour
{
    [Header("Bools")]
    public bool playNormalAudio;
    bool canPlayAudio;

    [Header("Animator")]
    public Animator animator;

    [Header("AnimationClips")]
    AnimationClip lightClip;
    AnimationClip heavyClip;

    [Header("Components")]
    public PugioNormalAttack pugio;
    Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        weapon = pugio.weapon;
        SetAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon != null && PlayerComponents.Instance.playerMovement.startBool == true)
        {
            animator.SetFloat("AttackSpeed", weapon.attackSpeedMultiplier);

            if (Input.GetMouseButton(0) && pugio.canAttack) StartNormal();
            if (Input.GetMouseButton(1) && pugio.canAttack && pugio.canHeavy && weapon.heavyAttack) StartHeavy();
        }
    }
    
    void SetAnimator()
    {
        animator = GetComponent<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip c in clips)
            if (c.name == "Pugio Pierce") lightClip = c;
            else if (c.name == "Pugio Special Attack") heavyClip = c;
    }

    public void StartNormal()
    {
        pugio.piercing = true;
        animator.SetBool("Piercing", true);

        StartCoroutine(EndNormal());
        StartCoroutine(PlayNormalAudio(true));

        pugio.canAttack = false;
    }

    IEnumerator EndNormal()
    {
        float clipLength = lightClip.length / animator.GetFloat("AttackSpeed");

        yield return new WaitForSeconds(clipLength);

        pugio.piercing = false;
        animator.SetBool("Piercing", false);

        pugio.canAttack = true;
    }

    public void StartHeavy()
    {
        pugio.heavyAttacking = true;
        animator.SetBool("Heavy Attack", true);

        StartCoroutine(EndHeavy());
        StartCoroutine(PlayHeavyAudio());

        pugio.canAttack = false;
        pugio.canHeavy = false;
    }

    IEnumerator EndHeavy()
    {
        float clipLength = heavyClip.length * 0.9f;

        yield return new WaitForSeconds(clipLength);

        pugio.heavyAttacking = false;
        animator.SetBool("Heavy Attack", false);

        pugio.canAttack = true;

        yield return new WaitForSeconds(pugio.heavyCooldown);

        pugio.canHeavy = true;
    }

    public IEnumerator PlayNormalAudio(bool isPlayer = false)
    {
        var sfxManager = SFXAudioManager.Instance;

        if (!canPlayAudio) yield return null;

        if (isPlayer) sfxManager.PlayClip(sfxManager.pugio, MasterAudioManager.Instance.sBlend2D, sfxManager.weaponVolumeMod, true, "low");
        else sfxManager.PlayClip(sfxManager.pugio, MasterAudioManager.Instance.sBlend2D, sfxManager.weaponVolumeMod / 2, true, "low");

        canPlayAudio = false;

        yield return new WaitForSeconds(0.1f);

        canPlayAudio = true;
    }

    public IEnumerator PlayHeavyAudio()
    {
        var sfxManager = SFXAudioManager.Instance;
        
        for (int i = 0; i < heavyClip.length * 9; i++)
        {
            sfxManager.PlayClip(sfxManager.pugio, MasterAudioManager.Instance.sBlend2D, sfxManager.weaponVolumeMod, true, "low");
            yield return new WaitForSeconds(0.1f);
        }
    }
}
