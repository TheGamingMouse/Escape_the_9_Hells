using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldChest : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
    public RewardType rType;

    [Header("Ints")]
    readonly int exp = 50;

    [Header("Bools")]
    bool chestOpened;

    [Header("Transforms")]
    Transform player;

    [Header("Components")]
    Animator animator;
    PlayerLevel playerLevel;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        playerLevel = player.GetComponent<PlayerLevel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < 2 && !chestOpened)
        {
            animator.SetTrigger("OpenChest");

            if (rType == RewardType.Exp)
            {
                playerLevel.AddExperience(exp);
            }
            else if (rType == RewardType.Level)
            {
                playerLevel.LevelUp(false);
            }

            chestOpened = true;
        }
    }

    #endregion

    #region Enums

    public enum RewardType
    {
        Level,
        Exp
    }

    #endregion
}
