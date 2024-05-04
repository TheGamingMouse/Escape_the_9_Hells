using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSouls : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    float modifier;

    [Header("Lists")]
    public List<SoulsItemsSO> templateSouls = new();

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region General Methods

    public void AddSouls(SoulsItemsSO soul)
    {
        if (soul.title == "Template Soul")
        {
            templateSouls.Add(soul);
            print("player has " + templateSouls.Count + " template perks");
        }
    }

    #endregion
}
