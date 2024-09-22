using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckObstacles : MonoBehaviour
{
    #region Methods

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("ObstacleWall"))
        {
            Destroy(coll.gameObject);
            BossGenerator.Instance.obstaclesSpawned--;
        }
    }

    #endregion
}
