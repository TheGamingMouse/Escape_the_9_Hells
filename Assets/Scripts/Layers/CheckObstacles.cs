using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObstacles : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    public BossGenerator bossGenerator;

    #endregion
    
    #region Methods

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("ObstacleWall"))
        {
            Destroy(coll.gameObject);
            bossGenerator.obstaclesSpawned--;
        }
    }

    #endregion
}
