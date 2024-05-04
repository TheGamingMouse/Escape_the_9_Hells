using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
    public FollowState fState;

    [Header("Transforms")]
    Transform player;

    [Header("Vector3s")]
    public Vector3 offset;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (fState == FollowState.Player)
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, player.position.z + offset.z);
        }
    }

    #endregion

    #region EnumMethods

    public enum FollowState
    {
        Player
    }

    #endregion
}
