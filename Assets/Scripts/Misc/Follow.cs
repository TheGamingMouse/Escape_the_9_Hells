using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
    public FollowState fState;

    [Header("Floats")]
    readonly float speed = 5f;

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
            transform.position = Vector3.Lerp(transform.position, player.position + offset, speed);
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
