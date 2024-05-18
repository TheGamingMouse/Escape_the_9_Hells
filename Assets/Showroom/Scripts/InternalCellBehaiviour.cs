using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalCellBehaiviour : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int cellNumber;

    [Header("Arrays")]
    public GameObject[] walls;

    #endregion

    #region General Methods

    public void UpdateCell(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            walls[i].SetActive(!status[i]);
        }
    }

    #endregion
}
