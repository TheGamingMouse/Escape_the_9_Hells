using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public float moveSpeed = 3f;
    float bossSpeed = 2f;
    public float distToPlayer;

    [Header("Bools")]
    public bool targetInRange;
    bool boss;
    public bool slowed;

    [Header("Transforms")]
    Transform player;
    public Transform target;

    [Header("Vector3s")]
    Vector3 moveDirection;

    [Header("Components")]
    public Rigidbody rb;
    public EnemySight enemySight;
    public ImpAction enemyAction;
    public ImpHealth enemyHealth;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        boss = enemyHealth.boss;
        if (boss)
        {
            moveSpeed = bossSpeed;
        }
    }

    void FixedUpdate()
    {
        distToPlayer = Vector3.Distance(transform.position, player.position);
        if (target)
        {
            if (distToPlayer > 5.5f)
            {
                moveDirection = (target.position - transform.position).normalized;
                rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.z * moveSpeed);
            }
            else if (distToPlayer > 4.5f && distToPlayer < 5.5f)
            {
                moveDirection = Vector3.zero;
                rb.velocity = new Vector3(0f, 0f, 0f);
            }
            else if (distToPlayer < 4.5f)
            {
                moveDirection = (target.position - transform.position).normalized;
                rb.velocity = new Vector3(-moveDirection.x * moveSpeed, 0f, -moveDirection.z * moveSpeed);
            }

            targetInRange = true;
        }
        else
        {
            moveDirection = Vector3.zero;
            rb.velocity = Vector3.zero;
            targetInRange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            LookAtTarget();

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

    #region General Methods

    public IEnumerator SlowEnemy(float speedPenalty)
    {
        var baseSpeed = moveSpeed;

        moveSpeed /= speedPenalty;
        slowed = true;

        yield return new WaitForSeconds(0.5f);

        moveSpeed = baseSpeed;
        slowed = false;
    }

    #endregion

    #region Target Methods

    void LookAtTarget()
    {
        transform.forward = target.position - transform.position;
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
