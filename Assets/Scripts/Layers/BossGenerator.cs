using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossGenerator : MonoBehaviour
{
    #region Events

    public static event System.Action OnBossDeath;

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

    [Header("GameObjects")]
    public GameObject boss;
    public GameObject wall;

    [Header("Transforms")]
    public Transform bossSpawn;

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

        if (room.door.name == "Down Door")
        {
            foreach (GameObject t in treasureRooms)
            {
                if (t.GetComponent<TreasureRoom>().tType == TreasureRoom.TreasureType.Level)
                {
                    treasure = t.GetComponent<TreasureRoom>();
                }
            }
        }
        else if (room.door.name == "Right Door")
        {
            foreach (GameObject t in treasureRooms)
            {
                if (t.GetComponent<TreasureRoom>().tType == TreasureRoom.TreasureType.Souls)
                {
                    treasure = t.GetComponent<TreasureRoom>();
                }
            }
        }
        else if (room.door.name == "Left Door")
        {
            foreach (GameObject t in treasureRooms)
            {
                if (t.GetComponent<TreasureRoom>().tType == TreasureRoom.TreasureType.Exp)
                {
                    treasure = t.GetComponent<TreasureRoom>();
                }
            }
        }

        if (doRandomObstacles)
        {
            GenerateObstacles();
        }

        if (LayerManager.Instance.showroom)
        {
            isBossDead = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (doRandomObstacles)
        {
            GenerateObstacles();
        }

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
            foreach (GameObject t in treasureRooms)
            {
                if (t.GetComponent<TreasureRoom>().tType == TreasureRoom.TreasureType.Level)
                {
                    t.SetActive(true);
                }
            }
        }
        else if (room.door.name == "Right Door")
        {
            foreach (GameObject t in treasureRooms)
            {
                if (t.GetComponent<TreasureRoom>().tType == TreasureRoom.TreasureType.Souls)
                {
                    t.SetActive(true);
                }
            }
        }
        else if (room.door.name == "Left Door")
        {
            foreach (GameObject t in treasureRooms)
            {
                if (t.GetComponent<TreasureRoom>().tType == TreasureRoom.TreasureType.Exp)
                {
                    t.SetActive(true);
                }
            }
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
                
                int k = Random.Range(0, floorTiles.Length);
                if (floorActive[k] || Vector3.Distance(floorTiles[k].transform.position, bossSpawn.position) < 5f)
                {
                    obstaclesSpawned--;
                    continue;
                }

                floorActive[k] = true;
                int j = Random.Range(0, 4);
                switch (j)
                {
                    case 0:
                        Instantiate(wall, floorTiles[k].transform.position + new Vector3(0f, 0f, 1f), new Quaternion(0f, 0f, 0f, 1f), transform.Find("Walls"));
                        break;
                    
                    case 1:
                        Instantiate(wall, floorTiles[k].transform.position + new Vector3(1f, 0f, 0f), new Quaternion(0f, 0.707106829f, 0f, 0.707106829f), transform.Find("Walls"));
                        break;
                    
                    case 2:
                        Instantiate(wall, floorTiles[k].transform.position + new Vector3(0f, 0f, -1f), new Quaternion(0f, 0f, 0f, 1f), transform.Find("Walls"));
                        break;
                    
                    case 3:
                        Instantiate(wall, floorTiles[k].transform.position + new Vector3(-1f, 0f, 0f), new Quaternion(0f, 0.707106829f, 0f, 0.707106829f), transform.Find("Walls"));
                        break;
                }
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
                musicManager.CheckMusicTrack();

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
                musicManager.CheckMusicTrack();

                StartCoroutine(TriggerCooldown());
            }
        }
    }
}
