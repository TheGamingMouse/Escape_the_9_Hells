using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class IronChest : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    [SerializeField] int souls;

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
            if (luckCheck <= PlayerComponents.Instance.playerLevel.luck)
            {
                var sfxManager = SFXAudioManager.Instance;

                souls = Random.Range(10, 26) * 2;
                sfxManager.PlayClip(sfxManager.activateLucky, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod);
            }
            else
            {
                souls = Random.Range(10, 26);
            }
            ExpSoulsManager.Instance.AddSouls(souls, false);

            chestOpened = true;
        }
    }

    #endregion
}
