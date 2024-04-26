using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class IronChest : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    [SerializeField] int souls;

    [Header("Bools")]
    bool chestOpened;

    [Header("Transforms")]
    Transform player;

    [Header("Components")]
    ExpSoulsManager expSoulsManager;
    Animator animator;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        expSoulsManager = GameObject.FindWithTag("Managers").GetComponent<ExpSoulsManager>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player").transform;

        souls = Random.Range(5, 16);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < 2 && !chestOpened)
        {
            animator.SetTrigger("OpenChest");
            expSoulsManager.AddSouls(souls);

            chestOpened = true;
        }
    }

    #endregion
}
