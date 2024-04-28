using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    public bool isBossDead;
    bool treasureSpawned;
    public bool ready;

    [Header("Arrays")]
    public GameObject[] treasureRooms;

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

        if (room.door.name == "Down Door")
        {
            treasure = transform.Find("TreasureRooms/BossTreasureRoom1.1 Levels").GetComponent<TreasureRoom>();
        }
        else if (room.door.name == "Right Door")
        {
            treasure = transform.Find("TreasureRooms/BossTreasureRoom1.1 Souls").GetComponent<TreasureRoom>();
        }
        else if (room.door.name == "Left Door")
        {
            treasure = transform.Find("TreasureRooms/BossTreasureRoom1.1 Exp").GetComponent<TreasureRoom>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (treasure.ready && !ready)
        {
            for (int i = 0; i < treasureRooms.Length; i++)
            {
                treasureRooms[i].SetActive(false);
            }

            ready = true;
        }

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
        int door = Random.Range(0, treasureRooms.Length);

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
            treasureRooms[0].SetActive(true);
        }
        else if (room.door.name == "Right Door")
        {
            treasureRooms[1].SetActive(true);
        }
        else if (room.door.name == "Left Door")
        {
            treasureRooms[2].SetActive(true);
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
