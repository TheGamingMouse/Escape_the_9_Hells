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
    public bool boss;

    [Header("Vector3s")]
    Vector3 originalSize;

    [Header("GameObjects")]
    public GameObject explosion;
    public GameObject targetCircle;
    [SerializeField] GameObject instantiatedTargetCircle;

    [Header("LayerMasks")]
    public LayerMask groundMask;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        if (boss == true)
        {
            instantiatedTargetCircle = Instantiate(targetCircle);
            originalSize = targetCircle.transform.localScale;
        }
    }

    void FixedUpdate()
    {
        if (instantiatedTargetCircle != null) UpdateTargetCircle();
    }

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
            else if (coll.TryGetComponent(out LoyalSphereSight _) || coll.TryGetComponent(out AttackSquareCombat _))
                Explode();
        }
        else if (canDamageEnemies)
        {
            if (coll.TryGetComponent(out EnemyHealth eComp))
            {
                eComp.TakeDamage(damage, false);
                Explode();
            }
        }
        
        if (coll.CompareTag("Wall") || coll.CompareTag("Pillar") || coll.CompareTag("Door") || coll.CompareTag("FloorTile")) Explode();
    }

    void Explode()
    {
        var newExplosion = Instantiate(explosion, transform.position, Quaternion.identity).GetComponent<ExplosionComponentStorage>();

        newExplosion.Ground.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.Ground_dark.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.Sphere.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.Impact.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.Fire_up.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.Spark.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);

        newExplosion.GetComponent<SphereCollider>().radius = explosionSize;

        newExplosion.canDamagePlayer = canDamagePlayer;
        newExplosion.canDamageEnemies = canDamageEnemies;

        newExplosion.damage = damage / 2;

        newExplosion.sfxManager = SFXAudioManager.Instance;
        
        Destroy(instantiatedTargetCircle);
        Destroy(newExplosion, 1f);
        Destroy(gameObject);
    }

    void UpdateTargetCircle()
    {
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, float.MaxValue, groundMask))
        {
            instantiatedTargetCircle.transform.position = hit.point;
            if (originalSize.x / hit.distance * 10 > originalSize.x && hit.distance > 10) instantiatedTargetCircle.transform.localScale = originalSize / hit.distance * 10;
            else instantiatedTargetCircle.transform.localScale = originalSize;

            Debug.DrawLine (transform.position, hit.point, Color.cyan);
        }
    }

    void OnDestroy()
    {
        foreach (var source in gameObject.GetComponents<AudioSource>())
            if (SFXAudioManager.Instance.audioSourcePool.Contains(source)) SFXAudioManager.Instance.audioSourcePool.Remove(source);
    }

    #endregion
}
