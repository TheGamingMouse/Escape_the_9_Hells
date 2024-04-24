using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    #region Properties

    [Header("Ints")]
    public int level;

    [Header("Floats")]
    [Range(0f, 1f)]
    public float exp;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (level == 0)
        {
            level = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
