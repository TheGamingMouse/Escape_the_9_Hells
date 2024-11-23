using System;
using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BossGenerator : MonoBehaviour
{
    #region Events

    public static event Action OnBossDeath;
    public static event Action OnBossSpawn;

    #endregion

    #region Variables

    [Header("Instance")]
    public static BossGenerator Instance;

    [Header("Ints")]
    [Range(1, 25)]
    public int obstacleAmount;
    public int obstaclesSpawned;

    [Header("Bools")]
    public bool isBossDead;
    bool treasureSpawned;
    [HideInInspector]
    public bool ready;
    [HideInInspector]
    public bool inArea;
    public bool doRandomObstacles;
    bool canTriggerMusic = true;
    bool bossSpawned;

    [Header("GameObjects")]
    public GameObject boss;
    public GameObject wall;

    [Header("Transforms")]
    public Transform bossSpawn;

    [Header("Door Strings")]
    public const string downDoorString = "Down Door";
    public const string rightDoorString = "Right Door";
    public const string leftDoorString = "Left Door";

    [Header("Arrays")]
    public GameObject[] treasureRooms;
    public GameObject[] floorTiles;
    bool[] floorActive;

    [Header("Quaternions")]
    public Quaternion openRot;

    [Header("Components")]
    RoomBehavior room;
    TreasureRoom treasure;
    
    #endregion

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        room = GetComponent<RoomBehavior>();
        
        GenerateExit();

        TreasureRoom.TreasureType treasureType;
        treasureType = room.door.name switch
        {
            downDoorString => TreasureRoom.TreasureType.Level,
            rightDoorString => TreasureRoom.TreasureType.Souls,
            leftDoorString => TreasureRoom.TreasureType.Exp,

            _ => throw new ArgumentException("Type of door not found.")
        };

        foreach (GameObject t in treasureRooms) 
            if (t.GetComponent<TreasureRoom>().tType == treasureType) treasure = t.GetComponent<TreasureRoom>();
        if (LayerManager.Instance.showroom) isBossDead = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (doRandomObstacles) GenerateObstacles();

        if (!bossSpawned && !isBossDead && room.active)
        {
            var newBoss = Instantiate(boss, bossSpawn);
            OnBossSpawn?.Invoke();

            bossSpawned = true;
        }

        if (treasure.ready && !ready)
        {
            for (int i = 0; i < treasureRooms.Length; i++) treasureRooms[i].SetActive(false);
            ready = true;
        }

        if (isBossDead)
        {
            OnBossDeath?.Invoke();
            OpenDoor();

            if (!treasureSpawned) SpawnTreasure();
        }

        if (PlayerComponents.Instance.playerSouls.playerPathfinder) room.UpdateLights(room.savedStatus);
    }

    void GenerateExit()
    {
        int door = UnityEngine.Random.Range(0, treasureRooms.Length);

        bool[] status = {true, false, false, false};
        switch (door)
        {
            case 0: // Down Door
                status[1] = true;
                room.UpdateRoom(status);
                break;
            
            case 1: // Right Door
                status[2] = true;
                room.UpdateRoom(status);
                break;
            
            case 2: // Left Door
                status[3] = true;
                room.UpdateRoom(status);
                break;
        }

        room.door = room.doors[door + 1];
    }

    void SpawnTreasure()
    {
        var treasureType = room.door.name switch
        {
            downDoorString => TreasureRoom.TreasureType.Level,
            rightDoorString => TreasureRoom.TreasureType.Souls,
            leftDoorString => TreasureRoom.TreasureType.Exp,

            _ => throw new Exception("Door name not recognized.")
        };

        foreach (GameObject obj in treasureRooms)
        {
            if (obj.GetComponent<TreasureRoom>().tType == treasureType) obj.SetActive(true);
        }

        treasure.LoadTreasure();
        treasureSpawned = true;
    }

    void OpenDoor()
    {
        var doorHinge = room.door.transform.Find("DoorHinge");

        room.door.GetComponentInChildren<MeshCollider>().enabled = false;
        doorHinge.localRotation = Quaternion.Slerp(doorHinge.localRotation, openRot, Time.deltaTime);
    }

    void GenerateObstacles()
    {
        if (obstaclesSpawned < obstacleAmount)
        {
            floorActive = new bool[floorTiles.Length];
            while (obstaclesSpawned < obstacleAmount)
            {
                obstaclesSpawned++;
                
                int k = UnityEngine.Random.Range(0, floorTiles.Length);
                if (floorActive[k] || Vector3.Distance(floorTiles[k].transform.position, bossSpawn.position) < 5f)
                {
                    obstaclesSpawned--;
                    continue;
                }

                floorActive[k] = true;
                int j = UnityEngine.Random.Range(0, 4);
                _ = j switch
                {
                    0 => Instantiate(wall, floorTiles[k].transform.position + new Vector3(0f, 0f, 1f), new Quaternion(0f, 0f, 0f, 1f), transform.Find("Walls")),
                    1 => Instantiate(wall, floorTiles[k].transform.position + new Vector3(1f, 0f, 0f), new Quaternion(0f, 0.707106829f, 0f, 0.707106829f), transform.Find("Walls")),
                    2 => Instantiate(wall, floorTiles[k].transform.position + new Vector3(0f, 0f, -1f), new Quaternion(0f, 0f, 0f, 1f), transform.Find("Walls")),
                    3 => Instantiate(wall, floorTiles[k].transform.position + new Vector3(-1f, 0f, 0f), new Quaternion(0f, 0.707106829f, 0f, 0.707106829f), transform.Find("Walls")),

                    _ => throw new Exception("Integer 'j' out of range.")
                };
            }
        }
    }

    IEnumerator TriggerCooldown()
    {
        canTriggerMusic = false;

        yield return new WaitForSeconds(0.5f);
        
        canTriggerMusic = true;
    }

    void OnTriggerEnter(Collider coll)
    {
        var musicManager = MusicAudioManager.Instance;

        if (coll.transform.CompareTag("Player"))
        {
            inArea = true;

            if (!musicManager.inBossRoom && canTriggerMusic)
            {
                musicManager.inBossRoom = true;
                musicManager.PlayMusicTrack();

                StartCoroutine(TriggerCooldown());
            }
        }
    }
    void OnTriggerExit(Collider coll)
    {
        var musicManager = MusicAudioManager.Instance;
        
        if (coll.transform.CompareTag("Player"))
        {
            inArea = false;

            if (musicManager.inBossRoom && canTriggerMusic)
            {
                musicManager.inBossRoom = false;
                musicManager.PlayMusicTrack();

                StartCoroutine(TriggerCooldown());
            }
        }
    }
}
