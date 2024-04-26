using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    #region Properties

    [Header("Bools")]
    public bool isBossDead;
    bool treasureSpawned;

    [Header("GameObjects")]
    public GameObject downTreasure;
    public GameObject rightTreasure;
    public GameObject leftTreasure;

    [Header("Quaternions")]
    public Quaternion openRot;

    [Header("Components")]
    RoomBehavior room;
    TreasureRoom treasure;
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        room = GetComponent<RoomBehavior>();
        
        GenerateExit();

        downTreasure.SetActive(false);
        rightTreasure.SetActive(false);
        leftTreasure.SetActive(false);

        if (room.door.name == "Down Door")
        {
            treasure = transform.Find("TreasureRooms/BossTreasureRoom1.1 Down").GetComponent<TreasureRoom>();
        }
        else if (room.door.name == "Left Door")
        {
            treasure = transform.Find("TreasureRooms/BossTreasureRoom1.1 Left").GetComponent<TreasureRoom>();
        }
        else if (room.door.name == "Right Door")
        {
            treasure = transform.Find("TreasureRooms/BossTreasureRoom1.1 Right").GetComponent<TreasureRoom>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBossDead)
        {
            OpenDoor();
            if (!treasureSpawned)
            {
                SpawnTreasure();
            }
        }
    }

    void GenerateExit()
    {
        int door = Random.Range(0, 3);

        if (door == 0) // Down Door
        {
            bool[] status = {true, true, false, false};
            room.UpdateRoom(status);
        }
        if (door == 1) // Right Door
        {
            bool[] status = {true, false, true, false};
            room.UpdateRoom(status);
        }
        if (door == 2) // Left Door
        {
            bool[] status = {true, false, false, true};
            room.UpdateRoom(status);
        }

        room.door = room.doors[door + 1];
    }

    void SpawnTreasure()
    {
        if (room.door.name == "Down Door")
        {
            downTreasure.SetActive(true);
        }
        else if (room.door.name == "Right Door")
        {
            rightTreasure.SetActive(true);
        }
        else if (room.door.name == "Left Door")
        {
            leftTreasure.SetActive(true);
        }

        treasure.LoadTreasure();
        treasureSpawned = true;
    }

    void OpenDoor()
    {
        var doorHinge = room.door.transform.Find("DoorHinge");

        room.door.GetComponentInChildren<BoxCollider>().enabled = false;
        doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, openRot, Time.deltaTime);
    }
}
