using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region Poperties

    [Header("Floats")]
    public float moveSpeed = 2.5f;

    [Header("Bools")]
    public bool targetInRange;

    [Header("Transforms")]
    Transform player;
    Transform target;

    [Header("Vector3s")]
    Vector3 moveDirection;

    [Header("LayerMasks")]
    public LayerMask playerMask;

    [Header("Components")]
    Rigidbody rb;
    public RoomSpawner roomSpawner;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (target)
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
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
        {
            Search();
        }
        else
        {
            LookAtTarget();
        }

        if (target && !TargetInArea())
        {
            target = null;
        }
    }

    #endregion

    #region FindPlayer Methods

    void Search()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 100, transform.position, playerMask);
        
        if ((hits.Length > 0) && TargetInArea())
        {
            target = player;
        }
    }

    bool TargetInArea()
    {
        return roomSpawner.inArea;
    }

    #endregion

    #region Target Methods

    void LookAtTarget()
    {
        transform.forward = target.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        transform.rotation = new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w);
    }

    #endregion
}
