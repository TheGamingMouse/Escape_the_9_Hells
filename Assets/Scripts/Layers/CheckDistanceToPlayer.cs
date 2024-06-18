using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDistanceToPlayer : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public float distToPlayer;
    public float maxDist = 25f;

    [Header("Bools")]
    bool nameUpdated;

    [Header("GameObjects")]
    public GameObject room;

    #endregion
    
    #region StartUpdate Methods

    void Update()
    {
        if (!nameUpdated)
        {
            gameObject.name = room.name;
            nameUpdated = true;
        }

        if (GameObject.FindWithTag("Player") && room.GetComponentInChildren<RoomBehavior>().active)
        {
            distToPlayer = Vector3.Distance(GameObject.FindWithTag("Player").transform.position, transform.position);
            if (distToPlayer <= maxDist)
            {
                room.SetActive(true);
            }
            else
            {
                room.SetActive(false);
            }
        }
    }

    #endregion
}
