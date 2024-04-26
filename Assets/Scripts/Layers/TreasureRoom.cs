using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : MonoBehaviour
{
    #region Properties

    [Header("Enum States")]
    public TreasureType tType;

    [Header("GameObjects")]
    public GameObject soulChest;
    public GameObject levelChest;
    public GameObject expChest;

    [Header("Lists")]
    public List<Transform> spawns = new();
    readonly List<bool> spawnable = new();

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        if (tType == TreasureType.Souls)
        {
            for (int i = 0; i < 4; i++)
            {
                spawns.Add(transform.Find("Treasure").GetChild(i));
                spawnable.Add(false);
            }
        }
        else if (tType == TreasureType.Exp)
        {
            for (int i = 0; i < 4; i++)
            {
                spawns.Add(transform.Find("Treasure").GetChild(i));
                spawnable.Add(false);
            }
        }
        else if (tType == TreasureType.Level)
        {
            for (int i = 0; i < 2; i++)
            {
                spawns.Add(transform.Find("Treasure").GetChild(i));
                spawnable.Add(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region General Methods

    public void LoadTreasure()
    {
        int chestAmount = 0;
        if (tType == TreasureType.Souls || tType == TreasureType.Exp)
        {
            chestAmount = Random.Range(1, 5);
        }
        else if (tType == TreasureType.Level)
        {
            chestAmount = Random.Range(1, 3);
        }

        for (int i = chestAmount; i > 0; i--)
        {
            int spawn = 0;
            if (tType == TreasureType.Souls || tType == TreasureType.Exp)
            {
                spawn = Random.Range(0, 4);
                if (spawnable[spawn])
                {
                    i += 1;
                    continue;
                }
            }
            else if (tType == TreasureType.Level)
            {
                spawn = Random.Range(0, 2);
                if (spawnable[spawn])
                {
                    i += 1;
                    continue;
                }
            }

            if (tType == TreasureType.Souls)
            {
                Instantiate(soulChest, spawns[spawn].position, new Quaternion(0f, 0f, 0f, 0f), spawns[spawn]);
                spawnable[spawn] = true;
            }
            else if (tType == TreasureType.Exp)
            {
                Instantiate(expChest, spawns[spawn].position, new Quaternion(0f, 0f, 0f, 0f), spawns[spawn]);
                spawnable[spawn] = true;
            }
            else if (tType == TreasureType.Level)
            {
                Instantiate(levelChest, spawns[spawn].position, new Quaternion(0f, 0f, 0f, 0f), spawns[spawn]);
                spawnable[spawn] = true;
            }
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
