using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSquareMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    readonly float speed = 10f;

    [Header("Transforms")]
    Transform target;
    Transform player;

    [Header("Vector3s")]
    Vector3  moveDirection;

    [Header("Componensts")]
    Rigidbody rb;
    AttackSquareSight sight;
    public ParticleSystem teleportEffect;
    SFXAudioManager sfxManager;
    SaveLoadManager slManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var managers = GameObject.FindWithTag("Managers");

        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        sight = GetComponent<AttackSquareSight>();
        sfxManager = managers.GetComponent<SFXAudioManager>();
        slManager = managers.GetComponent<SaveLoadManager>();

        teleportEffect.Stop();
        teleportEffect.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            if (sight.target)
            {
                target = sight.target;
            }

            if (Vector3.Distance(transform.position, player.position) > 1.75f)
            {
                moveDirection = (player.position - transform.position).normalized;
                rb.velocity = new Vector3(moveDirection.x * speed, 0f, moveDirection.z * speed);
            }
            else if (Vector3.Distance(transform.position, player.position) > 0.75f && Vector3.Distance(transform.position, player.position) < 1.75f)
            {
                moveDirection = Vector3.zero;
                rb.velocity = new Vector3(0f, 0f, 0f);
            }
            else if (Vector3.Distance(transform.position, player.position) < 0.75f)
            {
                moveDirection = (player.position - transform.position).normalized;
                rb.velocity = new Vector3(-moveDirection.x * speed, 0f, -moveDirection.z * speed);
            }
        }
        else
        {
            if (!sight.target)
            {
                target = null;
                return;
            }

            if (Vector3.Distance(transform.position, target.position) > 0.25f)
            {
                moveDirection = (target.position - transform.position).normalized;
                rb.velocity = new Vector3(moveDirection.x * speed, 0f, moveDirection.z * speed);
            }
            else if (Vector3.Distance(transform.position, target.position) < 0.25f)
            {
                moveDirection = Vector3.zero;
                rb.velocity = new Vector3(0f, 0f, 0f);
            }
        }

        if (Vector3.Distance(player.position, transform.position) > 10f && slManager.lState == SaveLoadManager.LayerState.InLayers)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);

            StartCoroutine(Teleport());
        }
    }

    #endregion

    #region General Methods

    IEnumerator Teleport()
    {
        teleportEffect.gameObject.SetActive(true);
        teleportEffect.Play();

        sfxManager.PlayClip(sfxManager.attackSquareTeleport, sfxManager.masterManager.sBlend3D, sfxManager.effectsVolumeMod, gameObject);

        yield return new WaitForSeconds(2f);

        teleportEffect.Stop();
        teleportEffect.gameObject.SetActive(false);
    }

    #endregion
}
