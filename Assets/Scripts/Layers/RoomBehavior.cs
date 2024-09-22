using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    public int x;
    public int y;
    public int index;

    [Header("Bools")]
    public bool doInterior;
    bool activating;
    public bool completed;
    public bool active;
    public bool mainPath;
    public bool playerPathfinder;

    [Header("GameObjects")]
    public GameObject door;
    public GameObject secondDoor;
    public GameObject backDoor;
    public GameObject interior;

    [Header("Arrays")]
    public GameObject[] walls;
    public GameObject[] doors;
    public GameObject[] lights;

    [Header("Vector2")]
    public Vector2 boardSize;

    [Header("Colors")]
    Color mainPathColor = new(0f, 0.5686275f, 1f);

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
            lights[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);

            if (mainPath && doors[i] == backDoor && playerPathfinder)
            {
                lights[i].GetComponentInChildren<Light>().color = mainPathColor;
                lights[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
            }

            if (TryGetComponent(out BossGenerator _))
            {
                foreach (GameObject l in lights)
                {
                    l.GetComponentInChildren<Light>().color = mainPathColor;
                }
            }
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

        if (door && LayerManager.Instance.showroom)
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
