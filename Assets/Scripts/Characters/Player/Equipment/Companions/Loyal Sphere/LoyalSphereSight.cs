using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoyalSphereSight : MonoBehaviour
{
    #region Variables

    [Header("Transforms")]
    public Transform target;
    Transform player;

    [Header("LayerMasks")]
    public LayerMask enemyMask;
    public LayerMask obstructionMask;

    [Header("Components")]
    BossGenerator bossGenerator;
    RoomSpawner roomSpawner;
    PlayerMovement playerMovement;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.bossGenerator)
        {
            bossGenerator = playerMovement.bossGenerator;
        }
        if (playerMovement.roomSpawner)
        {
            roomSpawner = playerMovement.roomSpawner;
        }

        if (!target)
        {
            FindTarget();
        }
        else if (!PlayerInArea() || Physics.Linecast(transform.position, target.position, obstructionMask))
        {
            target = null;
        }
    }

    #endregion

    #region General Methods

    void FindTarget()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 100, transform.position, enemyMask);
        if (hits.Length > 0 && PlayerInArea())
        {
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.CompareTag("Enemy") && !Physics.Linecast(transform.position, hit.transform.position, obstructionMask))
                {
                    target = hit.transform;
                    return;
                }
            }
        }
    }

    bool PlayerInArea()
    {
        if (bossGenerator && bossGenerator.boss.GetComponent<EnemyHealth>().boss)
        {
            return bossGenerator.inArea && !bossGenerator.isBossDead;
        }
        else if (roomSpawner)
        {
            return roomSpawner.inArea && !roomSpawner.enemiesDefeated;
        }
        return false;
    }

    #endregion
}
