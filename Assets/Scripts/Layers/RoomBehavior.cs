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

    [Header("GameObjects")]
    public GameObject door;
    public GameObject secondDoor;
    public GameObject backDoor;
    public GameObject interior;

    [Header("Arrays")]
    public GameObject[] walls;
    public GameObject[] doors;
    public GameObject[] lights;
    public bool[] savedStatus = new bool[4];

    [Header("Vector2")]
    public Vector2 boardSize;

    [Header("Colors")]
    public static Color mainPathColor = new(0f, 0.2843137f, 0.5f);

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
        savedStatus = status;
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
            lights[i].SetActive(status[i]);
            walls[i].SetActive(!status[i]);
        }
        
        if (LayerManager.Instance.showroom == false)
        {
            if (PlayerComponents.Instance.playerSouls.playerPathfinder == true) UpdateLights(status);
        }
        else UpdateLights(status);
    }

    public void UpdateLights(bool[] status)
    {
        if (LayerManager.Instance.showroom == true)
        {
            foreach (var light in lights)
            {
                light.GetComponentInChildren<Light>().color = mainPathColor;
                light.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
            }
        }
        else if (PlayerComponents.Instance.playerSouls.playerPathfinder == true)
            for (int i = 0; i < status.Length; i++)
            {
                if (mainPath && doors[i] == backDoor)
                {
                    lights[i].GetComponentInChildren<Light>().color = mainPathColor;
                    lights[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
                }

                if (TryGetComponent(out BossGenerator _))
                    foreach (GameObject light in lights)
                    {
                        light.GetComponentInChildren<Light>().color = mainPathColor;
                        light.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
                    }
            }
    }

    public void UpdateDoors(bool[] activeDoors)
    {
        for (int i = 0; i < activeDoors.Length; i++)
            if (door == null && activeDoors[i] == doors[i]) 
                door = doors[i];
            else if (activeDoors[i] == doors[i])
            {
                secondDoor = doors[i];
                return;
            }
    }

    public void UpdateBackDoors(bool[] activeDoors)
    {
        for (int i = 0; i < activeDoors.Length; i++)
            if (activeDoors[i] == doors[i])
            {
                backDoor = doors[i];
                return;
            }
    }

    IEnumerator ActivatingRoutine()
    {
        yield return new WaitForSeconds(3f);

        string errorMsg = $"Not all rooms were complete at: {x} - {y}";

        if (door && LayerManager.Instance.showroom)
            if (!door.GetComponentInChildren<FrontEntranceChecker>().active || !door.GetComponentInChildren<BackEntranceChecker>().active)
                if (secondDoor)
                {
                    if (!secondDoor.GetComponentInChildren<FrontEntranceChecker>().active || !secondDoor.GetComponentInChildren<BackEntranceChecker>().active)
                    {
                        Debug.LogError(errorMsg + " Second Door");
                        Application.Quit();
                    }
                    else completed = true;
                }
                else
                {
                    Debug.LogError(errorMsg);
                    Application.Quit();
                }
            else if (!secondDoor) completed = true;
    }

    #endregion
}
