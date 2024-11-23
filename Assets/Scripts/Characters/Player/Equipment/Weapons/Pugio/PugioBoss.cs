using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PugioBoss : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float baseDamage = 12;
    public float damage;

    [Header("Bools")]
    public bool bossSlam;
    [SerializeField] bool bossSlamming;

    [Header("Transform")]
    public Transform player;

    [Header("Components")]
    BasicEnemyAction enemyAction;
    public ParticleSystem particles;

    #endregion

    #region StartUpdate Methods

    void Start()
    {
        enemyAction = GetComponentInParent<BasicEnemyAction>();

        if (enemyAction != null) damage = baseDamage / 2;
        if (particles) particles.Stop();
    }

    void Update()
    {
        if (bossSlam && !bossSlamming) StartCoroutine(BossSlamRoutine());
    }

    #endregion

    #region Boss

    IEnumerator BossSlamRoutine()
    {
        var sfxManager = SFXAudioManager.Instance;

        bossSlamming = true;
        particles.Play();

        yield return new WaitForSeconds(1f);

        sfxManager.PlayClip(sfxManager.cainAttack, MasterAudioManager.Instance.sBlend3D, sfxManager.enemyVolumeMod / 1.5f, true, "none", gameObject, 0.6f);

        yield return new WaitForSeconds(2.1f);

        particles.Stop();
        bossSlamming = false;
    }

    #endregion
}
