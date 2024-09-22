using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ulfberht : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float baseDamage = 12;
    public float damage;

    [Header("Bools")]
    public bool slashing;
    public bool specialAttacking;
    bool canPlayAudio;

    [Header("Transform")]
    public Transform player;

    [Header("Components")]
    public Weapon weapon;

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (weapon != null)
        {
            damage = baseDamage * weapon.damageMultiplier;
            
            GetComponentInChildren<BladeCollision>().player = player;
        }
    }

    #endregion

    #region General Methods

    public IEnumerator PlayNormalAudio(bool isPlayer = false)
    {
        var sfxManager = SFXAudioManager.Instance;

        if (!canPlayAudio)
        {
            yield return null;
        }

        if (isPlayer)
        {
            sfxManager.PlayClip(sfxManager.ulfberht, MasterAudioManager.Instance.sBlend2D, sfxManager.weaponVolumeMod / 3, true, "low");
        }
        else
        {
            sfxManager.PlayClip(sfxManager.ulfberht, MasterAudioManager.Instance.sBlend2D, sfxManager.weaponVolumeMod / 6, true, "low");
        }

        canPlayAudio = false;

        yield return new WaitForSeconds(0.1f);

        canPlayAudio = true;
    }

    public IEnumerator PlaySpecialAudio()
    {
        var sfxManager = SFXAudioManager.Instance;
        
        for (int i = 0; i < 5; i++)
        {
            sfxManager.PlayClip(sfxManager.ulfberht, MasterAudioManager.Instance.sBlend2D, sfxManager.weaponVolumeMod / 3, true, "low");

            yield return new WaitForSeconds(0.1f);
        }
    }

    #endregion
}
