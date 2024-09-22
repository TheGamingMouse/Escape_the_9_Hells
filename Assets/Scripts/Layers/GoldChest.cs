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

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
       if (!LayerManager.Instance.showroom)
        {
            animator = GetComponentInChildren<Animator>();
            player = PlayerComponents.Instance.player;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!LayerManager.Instance.showroom && Vector3.Distance(player.position, transform.position) < 2 && !chestOpened)
        {
            animator.SetTrigger("OpenChest");

            int luckCheck = Random.Range(1, 101);

            if (rType == RewardType.Exp)
            {
                if (luckCheck <= PlayerComponents.Instance.playerLevel.luck)
                {
                    var sfxManager = SFXAudioManager.Instance;

                    PlayerComponents.Instance.playerLevel.AddExperience(exp * 2, false, "none");
                    sfxManager.PlayClip(sfxManager.activateLucky, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod);
                }
                else
                {
                    PlayerComponents.Instance.playerLevel.AddExperience(exp, false, "none");
                }
            }
            else if (rType == RewardType.Level)
            {
                var playerLevel = PlayerComponents.Instance.playerLevel;
                var sfxManager = SFXAudioManager.Instance;
                
                if (luckCheck <= playerLevel.luck)
                {
                    playerLevel.LevelUp(false, true);
                    playerLevel.LevelUp(false, true);

                    sfxManager.PlayClip(sfxManager.activateLucky, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod);
                    sfxManager.PlayClip(sfxManager.gainLevel, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod);
                }
                else
                {
                    playerLevel.LevelUp(false, true);

                    sfxManager.PlayClip(sfxManager.gainLevel, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod);
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
