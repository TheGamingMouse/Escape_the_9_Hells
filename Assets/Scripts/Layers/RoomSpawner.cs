using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    #region Variables

    [Header("Bools")]
    bool layerGenerated;
    bool enemiesDefeated;
    bool nextRoomLoaded;
    bool enemiesSpawned;
    public bool inArea;

    [Header("GameObjects")]
    GameObject door;
    GameObject secondDoor;
    GameObject doorHinge;
    GameObject secondDoorHinge;
    public GameObject enemy;
    public GameObject chest;

    [Header("Transforms")]
    public Transform enemyList;
    Transform chestSpawn;

    [Header("Lists")]
    readonly List<Transform> spawnPoints = new();
    public List<GameObject> enemies = new();
    readonly List<Transform> chestSpawns = new();
    
    [Header("Arrays")]
    bool[] spawned;

    [Header("Components")]
    LayerGenerator generator;
    StartLevel startLevel;
    LayerManager layerManager;
    RoomBehavior roomBehavior;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.FindWithTag("Generator").GetComponent<LayerGenerator>();
        startLevel = GameObject.FindWithTag("StartLevel").GetComponent<StartLevel>();
        layerManager = GameObject.FindWithTag("Managers").GetComponent<LayerManager>();
        roomBehavior = GetComponent<RoomBehavior>();

        for (int i = 0; i <= 7; i++)
        {
            if (roomBehavior.interior != null)
            {
                spawnPoints.Add(transform.Find("InteriorWalls/SpawnPositions").GetChild(i));
            }
            else
            {
                spawnPoints.Add(transform.Find("SpawnPositions").GetChild(i));
            }
        }
        spawned = new bool[spawnPoints.Count];

        for (int i = 0; i <= 3; i++)
        {
            chestSpawns.Add(transform.Find("ChestSpawns").GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        layerGenerated = generator.layerGenerated;

        if (layerGenerated)
        {
            if (roomBehavior.door != null)
            {
                door = roomBehavior.door;
                doorHinge = roomBehavior.door.transform.Find("DoorHinge").gameObject;

                if (roomBehavior.secondDoor != null)
                {
                    secondDoor = roomBehavior.secondDoor;
                    secondDoorHinge = roomBehavior.secondDoor.transform.Find("DoorHinge").gameObject;
                }
            }
        }

        if (enemiesDefeated)
        {
            OpenDoor();
            if (!nextRoomLoaded)
            {
                LoadNextRoom();
            }
        }
        
        if (!enemiesSpawned)
        {
            SpawnEnemies();
        }
        else if (enemies.Count == 0)
        {
            enemiesDefeated = true;
        }
    }

    #endregion

    #region Room Methods

    void OpenDoor()
    {
        if (door != null)
        {
            door.GetComponentInChildren<BoxCollider>().enabled = false;
            doorHinge.transform.localRotation = Quaternion.Slerp(doorHinge.transform.localRotation, startLevel.openRot, Time.deltaTime);

            if (secondDoor != null)
            {
                secondDoor.GetComponentInChildren<BoxCollider>().enabled = false;
                secondDoorHinge.transform.localRotation = Quaternion.Slerp(secondDoorHinge.transform.localRotation, startLevel.openRot, Time.deltaTime);
            }
        }
    }

    void LoadNextRoom()
    {
        if (door != null)
        {

            if (roomBehavior.x == 4 && roomBehavior.y == 4)
            {
                layerManager.bossRoom.SetActive(true);
                return;
            }

            GameObject[] rooms = layerManager.rooms;

            foreach (GameObject r in rooms)
            {
                if (door.name == "Up Door")
                {
                    if (r.GetComponent<RoomBehavior>().x == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y - 1)
                    {
                        r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                        r.SetActive(true);
                        
                        nextRoomLoaded = true;
                    }
                }
                else if (door.name == "Down Door")
                {
                    if (r.GetComponent<RoomBehavior>().x == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y + 1)
                    {
                        r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                        if (roomBehavior.x == 4 && roomBehavior.y == 4)
                        {
                            layerManager.bossRoom.SetActive(true);
                        }
                        else
                        {
                            r.SetActive(true);
                        }
                        
                        nextRoomLoaded = true;
                    }
                }
                else if (door.name == "Right Door")
                {
                    if (r.GetComponent<RoomBehavior>().x == roomBehavior.x + 1 && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                    {
                        r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                        if (roomBehavior.x == 4 && roomBehavior.y == 4)
                        {
                            layerManager.bossRoom.SetActive(true);
                        }
                        else
                        {
                            r.SetActive(true);
                        }
                        
                        nextRoomLoaded = true;
                    }
                }
                else if (door.name == "Left Door")
                {
                    if (r.GetComponent<RoomBehavior>().x == roomBehavior.x - 1 && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                    {
                        r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                        if (roomBehavior.x == 4 && roomBehavior.y == 4)
                        {
                            layerManager.bossRoom.SetActive(true);
                        }
                        else
                        {
                            r.SetActive(true);
                        }
                        
                        nextRoomLoaded = true;
                    }
                }

                if (secondDoor != null)
                {
                    if (secondDoor.name == "Up Door")
                    {
                        if (r.GetComponent<RoomBehavior>().x == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y - 1)
                        {
                            r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                            r.SetActive(true);

                            nextRoomLoaded = true;
                        }
                    }
                    else if (secondDoor.name == "Down Door")
                    {
                        if (r.GetComponent<RoomBehavior>().x == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y + 1)
                        {
                            r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                            r.SetActive(true);

                            nextRoomLoaded = true;
                        }
                    }
                    else if (secondDoor.name == "Right Door")
                    {
                        if (r.GetComponent<RoomBehavior>().x == roomBehavior.x + 1 && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                        {
                            r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                            r.SetActive(true);
                            
                            nextRoomLoaded = true;
                        }
                    }
                    else if (secondDoor.name == "Left Door")
                    {
                        if (r.GetComponent<RoomBehavior>().x == roomBehavior.x - 1 && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                        {
                            r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                            r.SetActive(true);
                            
                            nextRoomLoaded = true;
                        }
                    }
                }
            }
        }
        else
        {
            if (roomBehavior.backDoor.name == "Up Door")
            {
                foreach (Transform c in chestSpawns)
                {
                    if (c.name == "Down Chest")
                    {
                        chestSpawn = c;
                    }
                }
            }
            else if (roomBehavior.backDoor.name == "Down Door")
            {
                foreach (Transform c in chestSpawns)
                {
                    if (c.name == "Up Chest")
                    {
                        chestSpawn = c;
                    }
                }
            }
            else if (roomBehavior.backDoor.name == "Right Door")
            {
                foreach (Transform c in chestSpawns)
                {
                    if (c.name == "Left Chest")
                    {
                        chestSpawn = c;
                    }
                }
            }
            else if (roomBehavior.backDoor.name == "Left Door")
            {
                foreach (Transform c in chestSpawns)
                {
                    if (c.name == "Right Chest")
                    {
                        chestSpawn = c;
                    }
                }
            }
            var newChest = Instantiate(chest, chestSpawn.position, Quaternion.identity, chestSpawn);
            newChest.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

            nextRoomLoaded = true;
        }
    }

    #endregion

    #region Spawn Methods

    void SpawnEnemies()
    {
        int enemyAmount = Random.Range(3, 9);

        for (int i = 0; i < enemyAmount; i++)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            if (spawned[spawnIndex] == false)
            {
                var newEnemy = Instantiate(enemy, spawnPoints[spawnIndex].position, Quaternion.identity, enemyList);
                newEnemy.GetComponent<EnemyHealth>().roomSpawner = this;
                newEnemy.GetComponent<EnemySight>().roomSpawner = this;
                enemies.Add(newEnemy);

                spawned[spawnIndex] = true;
            }
            else
            {
                i--;
                continue;
            }
        }

        enemiesSpawned = true;
    }

    #endregion

    #region General Methods

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

    #endregion
}
