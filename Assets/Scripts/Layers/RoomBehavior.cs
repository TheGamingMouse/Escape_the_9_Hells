using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int x;
    public int y;

    [Header("Bools")]
    public bool doInterior;
    bool activating;
    public bool completed;

    [Header("GameObjects")]
    public GameObject door;
    public GameObject secondDoor;
    public GameObject backDoor;
    public GameObject interior;

    [Header("Arrays")]
    public GameObject[] walls;
    public GameObject[] doors;

    #endregion

    #region StartUpdate Methods

    void Update()
    {
        if (!activating)
        {
            StartCoroutine(ActivatingRoutine());
            activating = true;
        }
    }

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
            if (door == null && activeDoors[i] == doors[i])
            {
                door = doors[i];
            }
            else if (activeDoors[i] == doors[i])
            {
                secondDoor = doors[i];
                return;
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
                return;
            }
        }
    }

    IEnumerator ActivatingRoutine()
    {
        yield return new WaitForSeconds(3f);

        string errorMsg = "Not all rooms were complete at: " + x + "-" + y;

        if (door)
        {
            if (!door.GetComponentInChildren<FrontEntranceChecker>().active || 
            !door.GetComponentInChildren<BackEntranceChecker>().active)
            {
                if (secondDoor)
                {
                    if (!secondDoor.GetComponentInChildren<FrontEntranceChecker>().active || !secondDoor.GetComponentInChildren<BackEntranceChecker>().active)
                    {
                        Debug.LogError(errorMsg + " Second Door");
                        Application.Quit();
                    }
                    else
                    {
                        completed = true;
                    }
                }
                else
                {
                    Debug.LogError(errorMsg);
                    Application.Quit();
                }
            }
            else if (!secondDoor)
            {
                completed = true;
            }
        }
    }

    #endregion
}
