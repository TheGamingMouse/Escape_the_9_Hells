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
    LayerManager layerManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        layerManager = GameObject.FindWithTag("Managers").GetComponent<LayerManager>();

        if (!layerManager.showroom)
        {
            animator = GetComponentInChildren<Animator>();
            player = GameObject.FindWithTag("Player").transform;
            playerLevel = player.GetComponent<PlayerLevel>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!layerManager.showroom && Vector3.Distance(player.position, transform.position) < 2 && !chestOpened)
        {
            animator.SetTrigger("OpenChest");

            int luckCheck = Random.Range(1, 101);

            if (rType == RewardType.Exp)
            {
                if (luckCheck <= playerLevel.luck)
                {
                    playerLevel.AddExperience(exp * 2, false, "none");
                }
                else
                {
                    playerLevel.AddExperience(exp, false, "none");
                }
            }
            else if (rType == RewardType.Level)
            {
                if (luckCheck <= playerLevel.luck)
                {
                    playerLevel.LevelUp(false, true);
                    playerLevel.LevelUp(false, true);
                }
                else
                {
                    playerLevel.LevelUp(false, true);
                }
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
