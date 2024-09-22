using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSquareSight : MonoBehaviour
{
    #region Variables

    [Header("Transforms")]
    Transform player;
    public Transform target;

    [Header("LayerMasks")]
    public LayerMask enemyMask;
    public LayerMask obstructionMask;

    [Header("Components")]
    RoomSpawner roomSpawner;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerComponents.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        var playerMovement = PlayerComponents.Instance.playerMovement;

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
        var bossGenerator = BossGenerator.Instance;

        if (bossGenerator)
        {
            if (bossGenerator.TryGetComponent(out BasicEnemyHealth beComp) && beComp.boss)
            {
                return bossGenerator.inArea && !bossGenerator.isBossDead;
            }
            else if (bossGenerator.TryGetComponent(out ImpHealth iComp) && iComp.boss)
            {
                return bossGenerator.inArea && !bossGenerator.isBossDead;
            }
        }
        else if (roomSpawner)
        {
            return roomSpawner.inArea && !roomSpawner.enemiesDefeated;
        }
        return false;
    }

    #endregion
}
