using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LoyalSphereCombat : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    readonly int baseDamage = 12;
    [SerializeField] int damage;

    [Header("Floats")]
    readonly float baseCooldown = 2.5f;
    [SerializeField] float cooldown;
    readonly float force = 10f;

    [Header("Bools")]
    bool shooting;

    [Header("GameObjects")]
    public GameObject projectile;

    [Header("Components")]
    LoyalSphereSight sight;
    public Companion companion;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        sight = GetComponent<LoyalSphereSight>();
    }

    // Update is called once per frame
    void Update()
    {
        if (companion != null)
        {
            damage = (int)(baseDamage * companion.abilityStrengthMultiplier);
            cooldown = baseCooldown * companion.abilityRateMultiplier;
        }

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
        var sfxManager = SFXAudioManager.Instance;

        GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity, GameObject.FindWithTag("PrefabStorage").transform);
        newProjectile.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
        newProjectile.GetComponent<Firebolt>().damage = damage;
        newProjectile.GetComponent<Firebolt>().explostionScale = 0.5f;
        newProjectile.GetComponent<Firebolt>().canDamagePlayer = false;
        newProjectile.GetComponent<Firebolt>().canDamageEnemies = true;

        sfxManager.PlayClip(sfxManager.firebolt, MasterAudioManager.Instance.sBlend3D, sfxManager.effectsVolumeMod, gameObject);

        Destroy(newProjectile, 3f);
    }

    void OnDestroy()
    {
        var sfxManager = SFXAudioManager.Instance;
        
        foreach (var source in gameObject.GetComponents<AudioSource>())
        {
            if (sfxManager.audioSourcePool.Contains(source))
            {
                sfxManager.audioSourcePool.Remove(source);
            }
        }
    }
}
