using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGenerator : MonoBehaviour
{
    #region Events

    public static event System.Action OnBossDeath;

    #endregion

    #region Variables

    [Header("Bools")]
    public bool isBossDead;
    bool treasureSpawned;
    public bool ready;
    public bool inArea;
    bool bossSpawned;

    [Header("GameObjects")]
    public GameObject boss;

    [Header("Arrays")]
    public GameObject[] treasureRooms;

    [Header("Vector3s")]
    public Vector3 bossPos;

    [Header("Quaternions")]
    public Quaternion openRot;

    [Header("Components")]
    RoomBehavior room;
    TreasureRoom treasure;
    UIManager uiManager;
    
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

        uiManager = GameObject.FindWithTag("Managers").GetComponent<UIManager>();
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

        if (!bossSpawned)
        {
            var newBoss = Instantiate(boss, bossPos, Quaternion.identity, transform).GetComponent<EnemySight>();
            newBoss.bossGenerator = this;
            newBoss.GetComponent<EnemyHealth>().bossGenerator = this;
            
            uiManager.bossGenerator = this;
            
            bossSpawned = true;
        }

        if (isBossDead)
        {
            OnBossDeath?.Invoke();
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

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("Player"))
        {
            inArea = true;
        }
    }
    void OnTriggerExit(Collider coll)
    {
        if (coll.transform.CompareTag("Player"))
        {
            inArea = false;
        }
    }
}
