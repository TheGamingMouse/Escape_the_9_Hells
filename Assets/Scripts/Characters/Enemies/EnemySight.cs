using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    #region Variables

    [Header("Enums")]
    public EnemyType eType; 

    [Header("Bools")]
    public bool isObstructed;
    public bool isInArea;
    bool boss;
    [HideInInspector]
    public bool minion;

    [Header("Transforms")]
    public Transform target;
    Transform player;

    [Header("LayerMasks")]
    public LayerMask playerMask;
    public LayerMask obstructionMask;

    [Header("Components")]
    [HideInInspector]
    public RoomSpawner roomSpawner;

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
        isObstructed = TargetObstructed();
        isInArea = TargetInArea();

        if (!target) FindTarget();
        target = (isObstructed || !isInArea) ? null : target;
    }

    #endregion

    #region General Methods

    void FindTarget()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 100, transform.position, playerMask);
        
        target = ((hits.Length > 0) && TargetInArea() && !TargetObstructed()) ? player : null;
    }

    bool TargetInArea()
    {
        if (boss || minion)
        {
            return BossGenerator.Instance.inArea;
        }
        else
        {
            return roomSpawner.inArea;
        }
    }

    bool TargetObstructed()
    {
        return Physics.Linecast(transform.position, player.position, obstructionMask);
    }

    #endregion

    #region Enums

    public enum EnemyType
    {
        basicEnemy,
        imp
    }

    #endregion
}
