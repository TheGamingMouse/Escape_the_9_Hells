using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoyalSphereProjectile : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int damage;

    [Header("Floats")]
    readonly float explostionScale = 0.5f;

    [Header("GameObjects")]
    public GameObject explosion;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region General Methods

    void OnCollisionEnter(Collision coll)
    {
        if (coll.transform.TryGetComponent<EnemyHealth>(out EnemyHealth eComp))
        {
            eComp.TakeDamage(damage);
        }
        GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        newExplosion.GetComponent<ExplosionComponentStorage>().Ground.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.GetComponent<ExplosionComponentStorage>().Ground_dark.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.GetComponent<ExplosionComponentStorage>().Sphere.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.GetComponent<ExplosionComponentStorage>().Impact.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.GetComponent<ExplosionComponentStorage>().Fire_up.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        newExplosion.GetComponent<ExplosionComponentStorage>().Spark.transform.localScale = new Vector3(explostionScale, explostionScale, explostionScale);
        Destroy(newExplosion, 1f);
        Destroy(newExplosion, 1f);
        Destroy(gameObject);
    }

    #endregion
}
