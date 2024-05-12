using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoyalSphereCombat : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    readonly int damage = 12;

    [Header("Floats")]
    readonly float cooldown = 2.5f;
    readonly float force = 10f;

    [Header("Bools")]
    bool shooting;

    [Header("GameObjects")]
    public GameObject projectile;

    [Header("Components")]
    LoyalSphereSight sight;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        sight = GetComponent<LoyalSphereSight>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sight.target && !shooting)
        {
            StartCoroutine(ShootRoutine());
        }
    }

    IEnumerator ShootRoutine()
    {
        shooting = true;

        Shoot();

        yield return new WaitForSeconds(cooldown);

        shooting = false;
    }

    void Shoot()
    {
        GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity, GameObject.FindWithTag("PrefabStorage").transform);
        newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
        newProjectile.GetComponent<LoyalSphereProjectile>().damage = damage;

        Destroy(newProjectile, 3f);
    }
}
