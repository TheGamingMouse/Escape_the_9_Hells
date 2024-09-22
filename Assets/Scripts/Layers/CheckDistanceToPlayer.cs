using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDistanceToPlayer : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    public float distToPlayer;
    readonly float maxDist = 22.5f;

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

        if (PlayerComponents.Instance.player && room.GetComponentInChildren<RoomBehavior>().active)
        {
            distToPlayer = Vector3.Distance(PlayerComponents.Instance.player.position, transform.position);
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
