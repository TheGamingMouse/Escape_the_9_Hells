using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public float moveSpeed = 4f;

    [Header("Bools")]
    public bool targetInRange;
    bool boss;

    [Header("Transforms")]
    Transform player;
    public Transform target;

    [Header("Vector3s")]
    Vector3 moveDirection;

    [Header("Components")]
    Rigidbody rb;
    EnemySight enemySight;
    EnemyAction enemyAction;
    EnemyHealth enemyHealth;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        enemySight = GetComponent<EnemySight>();
        enemyAction = GetComponent<EnemyAction>();
        enemyHealth = GetComponent<EnemyHealth>();

        boss = enemyHealth.boss;

    }

    void FixedUpdate()
    {
        if (target)
        {
            if (boss)
            {
                if (Vector3.Distance(transform.position, player.position) > 4.5f)
                {
                    targetInRange = false;
                }
                else
                {
                    targetInRange = true;
                }

                if (enemyAction.attacking)
                {
                    moveDirection = Vector3.zero;
                    rb.velocity = new Vector3(0f, 0f, 0f);
                }
                else if (Vector3.Distance(transform.position, player.position) > 4.5f)
                {
                    moveDirection = (target.position - transform.position).normalized;
                    rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.z * moveSpeed);
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, player.position) > 1.75f)
                {
                    moveDirection = (target.position - transform.position).normalized;
                    rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.z * moveSpeed);
                    targetInRange = false;
                }
                else if (Vector3.Distance(transform.position, player.position) > 0.75f && Vector3.Distance(transform.position, player.position) < 1.75f)
                {
                    moveDirection = Vector3.zero;
                    rb.velocity = new Vector3(0f, 0f, 0f);
                    targetInRange = true;
                }
                else if (Vector3.Distance(transform.position, player.position) < 0.75f)
                {
                    moveDirection = (target.position - transform.position).normalized;
                    rb.velocity = new Vector3(-moveDirection.x * moveSpeed, 0f, -moveDirection.z * moveSpeed);
                    targetInRange = false;
                }
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            if (boss && !enemyAction.attacking)
            {
                LookAtTarget();
            }
            else if (!boss)
            {
                LookAtTarget();
            }

            if (!enemySight.target)
            {
                target = null;
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }

        if (enemySight.target)
        {
            target = enemySight.target;
        }
    }

    #endregion

    #region Target Methods

    void LookAtTarget()
    {
        transform.forward = target.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
    }

    #endregion

    #region Gizmos

    void OnDrawGizmosSelected()
    {
        if (player)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, player.position);
        }

        if (target)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
    
    #endregion
}
