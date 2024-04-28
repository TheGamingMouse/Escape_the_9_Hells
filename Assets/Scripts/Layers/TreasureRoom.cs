using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : MonoBehaviour
{
    #region Variables

    [Header("Enum States")]
    public TreasureType tType;

    [Header("Bools")]
    public bool ready;

    [Header("Arrays")]
    public GameObject[] chests;
    public Transform[] spawns;
    bool[] spawnable;

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (!ready)
        {
            if (tType == TreasureType.Souls || tType == TreasureType.Exp)
            {
                spawnable = new bool[4];
            }
            else if (tType == TreasureType.Level)
            {
                spawnable = new bool[2];
            }

            ready = true;
        }
    }

    #endregion

    #region General Methods

    public void LoadTreasure()
    {
        int chestAmount = Random.Range(1, spawns.Length + 1);
        
        for (int i = chestAmount; i > 0; i--)
        {
            int spawn = Random.Range(0, spawns.Length);
            if (spawnable[spawn])
            {
                i += 1;
                continue;
            }

            int chest = -1;
            if (tType == TreasureType.Souls)
            {
                chest = 0;
            }
            else if (tType == TreasureType.Exp)
            {
                chest = 1;
            }
            else if (tType == TreasureType.Level)
            {
                chest = 2;
            }

            if (chest == -1)
            {
                i++;
                continue;
            }

            Instantiate(chests[chest], spawns[spawn].position, new Quaternion(0f, 0f, 0f, 0f), spawns[spawn]);
            spawnable[spawn] = true;
        }
    }

    #endregion

    #region Enums

    public enum TreasureType
    {
        Souls,
        Level,
        Exp
    }

    #endregion
}
