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
    ExpSoulsManager expSoulsManager;
    Animator animator;
    LayerManager layerManager;
    PlayerLevel playerLevel;
    SFXAudioManager sfxManager;
    

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var managers = GameObject.FindWithTag("Managers");
        layerManager = managers.GetComponent<LayerManager>();

        if (!layerManager.showroom)
        {
            expSoulsManager = managers.GetComponent<ExpSoulsManager>();
            animator = GetComponentInChildren<Animator>();
            player = GameObject.FindWithTag("Player").transform;
            playerLevel = player.GetComponent<PlayerLevel>();
            sfxManager = managers.GetComponent<SFXAudioManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!layerManager.showroom && Vector3.Distance(player.position, transform.position) < 2 && !chestOpened)
        {
            animator.SetTrigger("OpenChest");
            
            int luckCheck = Random.Range(1, 101);
            if (luckCheck <= playerLevel.luck)
            {
                souls = Random.Range(10, 26) * 2;
                sfxManager.PlayClip(sfxManager.activateLucky, sfxManager.masterManager.sBlend2D, sfxManager.effectsVolumeMod);
            }
            else
            {
                souls = Random.Range(10, 26);
            }
            expSoulsManager.AddSouls(souls, false);

            chestOpened = true;
        }
    }

    #endregion
}
