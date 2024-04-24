using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    #region Properties

    [Header("Bools")]
    bool layerGenerated;
    bool enemiesDefeated;
    bool nextRoomLoaded;
    [SerializeField] bool enemiesSpawned;

    [Header("GameObjects")]
    GameObject door;
    GameObject doorHinge;
    public GameObject normalEnemy;

    [Header("Transforms")]
    public Transform enemyList;

    [Header("Lists")]
    readonly List<Transform> spawnPoints = new();
    public List<GameObject> enemies = new();

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
            spawnPoints.Add(transform.Find("SpawnPositions").GetChild(i));
        }
    }

    // Update is called once per frame
    void Update()
    {
        layerGenerated = generator.layerGenerated;

        if (layerGenerated)
        {
            door = roomBehavior.door;
            doorHinge = roomBehavior.door.transform.Find("DoorHinge").gameObject;
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
        door.GetComponentInChildren<BoxCollider>().enabled = false;
        doorHinge.transform.localRotation = Quaternion.Slerp(doorHinge.transform.localRotation, startLevel.openRot, Time.deltaTime);
    }

    void LoadNextRoom()
    {
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
                    r.SetActive(true);

                    nextRoomLoaded = true;
                }
            }
            else if (door.name == "Right Door")
            {
                if (r.GetComponent<RoomBehavior>().x == roomBehavior.x + 1 && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                {
                    r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                    r.SetActive(true);
                    
                    nextRoomLoaded = true;
                }
            }
            else if (door.name == "Left Door")
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

    #endregion

    #region Spawn Methods

    void SpawnEnemies()
    {
        int enemyAmount = Random.Range(0, 6);

        for (int i = 0; i < enemyAmount + 1; i++)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            
            var newEnemy = Instantiate(normalEnemy, spawnPoints[spawnIndex].position, Quaternion.identity, enemyList);
            newEnemy.GetComponent<EnemyHealth>().roomSpawner = this;
            enemies.Add(newEnemy);
        }

        enemiesSpawned = true;
    }

    #endregion
}
