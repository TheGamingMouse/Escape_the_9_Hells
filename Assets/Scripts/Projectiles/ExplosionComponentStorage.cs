using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionComponentStorage : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int damage;

    [Header("Bools")]
    public bool canDamagePlayer;
    public bool canDamageEnemies;
    bool firstColl;

    [Header("GameObjects")]
    public GameObject Ground;
    public GameObject Ground_dark;
    public GameObject Sphere;
    public GameObject Impact;
    public GameObject Fire_up;
    public GameObject Spark;
    
    [Header("Components")]
    public SFXAudioManager sfxManager;

    #endregion

    #region Methods

    void OnTriggerEnter(Collider coll)
    {
        if (canDamagePlayer)
        {
            if (coll.TryGetComponent(out PlayerHealth playerComp))
            {
                playerComp.TakeDamage(damage);
            }
        }
        else if (canDamageEnemies)
        {
            if (coll.TryGetComponent(out BasicEnemyHealth basicComp))
            {
                basicComp.TakeDamage(damage, false);
            }
            else if (coll.TryGetComponent(out ImpHealth impComp))
            {
                impComp.TakeDamage(damage, false);
            }
        }

        if (!firstColl)
        {
            sfxManager.PlayClip(sfxManager.fireboltExplosion, sfxManager.masterManager.sBlend3D, sfxManager.effectsVolumeMod / 2, gameObject);

            firstColl = true;
        }
    }

    void OnDestroy()
    {
        foreach (var source in gameObject.GetComponents<AudioSource>())
        {
            if (sfxManager.audioSourcePool.Contains(source))
            {
                sfxManager.audioSourcePool.Remove(source);
            }
        }
    }

    #endregion
}
