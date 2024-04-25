using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class IronChest : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    int souls;

    [Header("Components")]
    ExpSoulsManager expSoulsManager;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        expSoulsManager = GameObject.FindWithTag("Managers").GetComponent<ExpSoulsManager>();

        souls = Random.Range(5, 16);
        expSoulsManager.AddSouls(souls);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
