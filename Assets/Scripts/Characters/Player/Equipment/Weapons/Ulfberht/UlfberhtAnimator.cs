using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UlfberhtAnimator : MonoBehaviour
{
    #region Variables

    bool canPlayAudio;
    AnimationClip ulfClip;
    Animator animator;
    AnimationClip ulfHeavyClip;
    UlfberhtAttack ulfberht;
    Weapon weapon;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        SetAnimator();

        ulfberht = GetComponent<UlfberhtAttack>();
        weapon = ulfberht.weapon;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && ulfberht.canAttack) StartNormal();
        if (Input.GetMouseButton(1) && ulfberht.canAttack && ulfberht.canHeavy && weapon.heavyAttack) StartSpecial();
    }

    void SetAnimator()
    {
        animator = GetComponent<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip c in clips)
            if (c.name == "Ulfberht Slash") ulfClip = c;
            else if (c.name == "Ulfberht Special Attack") ulfHeavyClip = c;
    }

    public void StartNormal()
    {
        animator.SetBool("Slashing", true);

        StartCoroutine(EndNormal());
        StartCoroutine(PlayNormalAudio());

        ulfberht.slashing = true;
        ulfberht.canAttack = false;
    }

    IEnumerator EndNormal()
    {
        float clipLength = ulfClip.length / animator.GetFloat("AttackSpeed");

        yield return new WaitForSeconds(clipLength);

        animator.SetBool("Slashing", false);

        ulfberht.slashing = false;
        ulfberht.canAttack = true;
    }

    public void StartSpecial()
    {
        ulfberht.heavyAttacking = true;
        animator.SetBool("Special Attack", true);

        StartCoroutine(EndSpecial());
        StartCoroutine(PlayHeavyAudio());

        ulfberht.canAttack = false;
        ulfberht.canHeavy = false;
    }

    IEnumerator EndSpecial()
    {
        float clipLength = ulfHeavyClip.length * 0.9f;

        yield return new WaitForSeconds(clipLength * 4f);

        ulfberht.heavyAttacking = false;
        animator.SetBool("Special Attack", false);

        ulfberht.canAttack = true;

        yield return new WaitForSeconds(ulfberht.heavyCooldown);

        ulfberht.canHeavy = true;
    }

    public IEnumerator PlayNormalAudio()
    {
        var sfxManager = SFXAudioManager.Instance;

        if (!canPlayAudio) yield return null;

        sfxManager.PlayClip(sfxManager.ulfberht, MasterAudioManager.Instance.sBlend2D, sfxManager.weaponVolumeMod / 3, true, "low");

        canPlayAudio = false;

        yield return new WaitForSeconds(0.1f);

        canPlayAudio = true;
    }

    public IEnumerator PlayHeavyAudio()
    {
        var sfxManager = SFXAudioManager.Instance;
        
        for (int i = 0; i < 5; i++)
        {
            sfxManager.PlayClip(sfxManager.ulfberht, MasterAudioManager.Instance.sBlend2D, sfxManager.weaponVolumeMod / 3, true, "low");
            yield return new WaitForSeconds(0.1f);
        }
    }
}
