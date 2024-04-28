using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int x;
    public int y;

    [Header("GameObjects")]
    public GameObject door;
    public GameObject secondDoor;
    public GameObject backDoor;

    [Header("Arrays")]
    public GameObject[] walls;
    public GameObject[] doors;

    #endregion

    #region General Methods

    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
    }

    public void UpdateDoors(bool[] activeDoors)
    {
        for (int i = 0; i < activeDoors.Length; i++)
        {
            if (activeDoors[i] == doors[i])
            {
                door = doors[i];
            }
        }

        for (int i = 0; i < activeDoors.Length; i++)
        {
            if (activeDoors[i] == doors[i] && doors[i] != door)
            {
                secondDoor = doors[i];
            }
        }
    }

    public void UpdateBackDoors(bool[] activeDoors)
    {
        for (int i = 0; i < activeDoors.Length; i++)
        {
            if (activeDoors[i] == doors[i])
            {
                backDoor = doors[i];
            }
        }
    }

    #endregion
}
