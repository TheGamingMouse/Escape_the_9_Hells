using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartLevel : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool doorOpening;
    bool doorOpened;
    bool firstRoomLoaded;

    [Header("GameObjects")]
    public GameObject door;
    public GameObject startWalls;

    [Header("Arrays")]
    GameObject[] rooms;

    [Header("Quaternions")]
    public Quaternion openRot;

    [Header("Components")]
    LayerManager layerManager;
    public BoxCollider startWallCollider;

    #endregion
    
    #region StartUpdate Methods

    void Start()
    {
        layerManager = GameObject.FindWithTag("Managers").GetComponent<LayerManager>();
    }

    void Update()
    {
        if (doorOpening)
        {
            OpenDoor();
            if (!firstRoomLoaded)
            {
                rooms = layerManager.rooms;

                foreach (GameObject r in rooms)
                {
                    if (r.GetComponent<RoomBehavior>().x == 0 && r.GetComponent<RoomBehavior>().y == 0)
                    {
                        r.SetActive(true);
                        r.GetComponent<RoomBehavior>().backDoor.transform.Find("DoorHinge").gameObject.SetActive(false);
                    }
                }

                startWalls.SetActive(false);
                
                firstRoomLoaded = true;
            }
        }
    }

    #endregion

    #region Genereal Methods

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("Player") && !doorOpening && !doorOpened)
        {
            doorOpening = true;
        }
    }

    void OpenDoor()
    {
        door.GetComponentInChildren<BoxCollider>().enabled = false;
        startWallCollider.enabled = false;
        
        door.transform.localRotation = Quaternion.Slerp(door.transform.localRotation, openRot, Time.deltaTime);
        if (door.transform.rotation == openRot)
        {
            doorOpened = true;
            doorOpening = false;
        }
    }

    #endregion
}
