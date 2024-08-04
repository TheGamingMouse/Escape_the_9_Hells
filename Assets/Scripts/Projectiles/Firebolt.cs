using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebolt : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int damage;

    [Header("Floats")]
    public float explostionScale = 0.5f;
    public float explosionSize = 1.5f;

    [Header("Bools")]
    public bool canDamagePlayer;
    public bool canDamageEnemies;

    [Header("GameObjects")]
    public GameObject explosion;
    public SFXAudioManager sfxManager;

    #endregion

    #region General Methods

    void OnTriggerEnter(Collider coll)
    {
        if (canDamagePlayer)
        {
            if (coll.TryGetComponent(out PlayerHealth pComp))
            {
                pComp.TakeDamage(damage);
                Explode();
            }
            else if (coll.TryGetComponent(out LoyalSphereSight _))
            {
                Explode();
            }
            else if (coll.TryGetComponent(out AttackSquareCombat _))
            {
                Explode();
            }
        }
        else if (canDamageEnemies)
        {
            if (coll.TryGetComponent(out BasicEnemyHealth eComp))
            {
                eComp.TakeDamage(damage, false);
                Explode();
            }
            else if (coll.TryGetComponent(out ImpHealth iComp))
            {
                iComp.TakeDamage(damage, false);
                Explode();
            }
        }
        
        if (coll.CompareTag("Wall") || coll.CompareTag("Pillar") || coll.CompareTag("Door") || coll.CompareTag("FloorTile"))
        {
            Explode();
        }
    }

    void Explode()
    {
        GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);

        newExplosion.GetComponent<ExplosionComponentStorage>().Ground.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.GetComponent<ExplosionComponentStorage>().Ground_dark.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.GetComponent<ExplosionComponentStorage>().Sphere.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.GetComponent<ExplosionComponentStorage>().Impact.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.GetComponent<ExplosionComponentStorage>().Fire_up.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.GetComponent<ExplosionComponentStorage>().Spark.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);

        newExplosion.GetComponent<SphereCollider>().radius = explosionSize;

        newExplosion.GetComponent<ExplosionComponentStorage>().canDamagePlayer = canDamagePlayer;
        newExplosion.GetComponent<ExplosionComponentStorage>().canDamageEnemies = canDamageEnemies;

        newExplosion.GetComponent<ExplosionComponentStorage>().damage = damage / 2;

        newExplosion.GetComponent<ExplosionComponentStorage>().sfxManager = sfxManager;
        
        Destroy(newExplosion, 1f);
        Destroy(gameObject);
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
