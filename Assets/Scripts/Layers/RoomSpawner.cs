using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    #region Events

    public static event System.Action OnBossSpawn;

    #endregion

    #region Variables

    [Header("Ints")]
    public int maxEnemies = 8;
    public int basicDemonChance;
    public int impChance;

    [Header("Bools")]
    bool layerGenerated;
    // [HideInInspector]
    public bool enemiesDefeated;
    bool nextRoomLoaded;
    bool enemiesSpawned;
    [HideInInspector]
    public bool inArea;
    bool doorCanOpen;
    bool primaryDoorAudioPlayed;
    bool secondaryDoorAudioPlayed;
    bool playerPathfinder;

    [Header("GameObjects")]
    GameObject door;
    GameObject secondDoor;
    GameObject doorHinge;
    GameObject secondDoorHinge;
    public GameObject chest;

    [Header("Transforms")]
    public Transform enemyList;
    Transform chestSpawn;

    [Header("Lists")]
    readonly List<Transform> spawnPoints = new();
    public List<GameObject> enemies = new();
    readonly List<Transform> chestSpawns = new();
    public List<GameObject> enemyTypes = new();
    
    [Header("Arrays")]
    bool[] spawned;

    [Header("Colors")]
    Color mainPathColor = new(0f, 0.5686275f, 1f);

    [Header("Components")]
    LayerGenerator generator;
    StartLevel startLevel;
    LayerManager layerManager;
    RoomBehavior roomBehavior;
    SFXAudioManager sfxManager;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        var managers = GameObject.FindWithTag("Managers");

        generator = GameObject.FindWithTag("Generator").GetComponent<LayerGenerator>();
        startLevel = GameObject.FindWithTag("StartLevel").GetComponent<StartLevel>();
        layerManager = managers.GetComponent<LayerManager>();
        sfxManager = managers.GetComponent<SFXAudioManager>();
        roomBehavior = GetComponent<RoomBehavior>();

        playerPathfinder = roomBehavior.playerPathfinder;

        basicDemonChance = generator.basicDemonChance;
        impChance = generator.impChance;

        if (basicDemonChance + impChance != 100)
        {
            Debug.LogError("Enemy spawn-chances do not match up to 100% basicDemonChance has been changed to match.");
            if (basicDemonChance + impChance < 100)
            {
                basicDemonChance += 100 - (basicDemonChance - impChance);
            }
            else
            {
                basicDemonChance -= 100 - (basicDemonChance - impChance);
            }
        }

        if (gameObject.name.Contains("MazeRoom"))
        {
            for (int i = 0; i < maxEnemies; i++)
            {
                spawnPoints.Add(transform.Find("SpawnPositions").GetChild(i));
            }
        }
        else
        {
            for (int i = 0; i < transform.Find("Floor").childCount; i++)
            {
                spawnPoints.Add(transform.Find("Floor").GetChild(i));
            }
        }
        spawned = new bool[spawnPoints.Count];

        for (int i = 0; i < 4; i++)
        {
            chestSpawns.Add(transform.Find("ChestSpawns").GetChild(i));
        }

        doorCanOpen = true;
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

            if (playerPathfinder)
            {
                CheckLights();
            }
        }

        if (enemiesDefeated && doorCanOpen)
        {
            OpenDoor();
            if (!nextRoomLoaded)
            {
                LoadNextRoom();
                StartCoroutine(OpenDoorTimer());
            }
        }
        
        if (!enemiesSpawned && !layerManager.showroom)
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
            door.GetComponentInChildren<MeshCollider>().enabled = false;
            doorHinge.transform.localRotation = Quaternion.Slerp(doorHinge.transform.localRotation, startLevel.openRot, Time.deltaTime);

            if (!primaryDoorAudioPlayed)
            {
                sfxManager.PlayClip(sfxManager.doorOpen, sfxManager.masterManager.sBlend2D, sfxManager.effectsVolumeMod, true);
                primaryDoorAudioPlayed = true;
            }

            if (secondDoor != null)
            {
                secondDoor.GetComponentInChildren<MeshCollider>().enabled = false;
                secondDoorHinge.transform.localRotation = Quaternion.Slerp(secondDoorHinge.transform.localRotation, startLevel.openRot, Time.deltaTime);

                if (!secondaryDoorAudioPlayed)
                {
                    sfxManager.PlayClip(sfxManager.doorOpen, sfxManager.masterManager.sBlend2D, sfxManager.effectsVolumeMod, true);
                    secondaryDoorAudioPlayed = true;
                }
            }
        }
    }

    void LoadNextRoom()
    {
        if (door != null)
        {
            if (roomBehavior.x == roomBehavior.boardSize.x && roomBehavior.y == roomBehavior.boardSize.y && !layerManager.showroom)
            {
                layerManager.bossRoom.SetActive(true);
                layerManager.bossRoom.GetComponent<RoomBehavior>().active = true;
                OnBossSpawn?.Invoke();
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
                        if (!layerManager.showroom)
                        {
                            r.SetActive(true);
                            r.GetComponent<RoomBehavior>().active = true;
                        }
                        
                        nextRoomLoaded = true;
                    }
                }
                else if (door.name == "Down Door")
                {
                    if (r.GetComponent<RoomBehavior>().x == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y + 1)
                    {
                        r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                        if (!layerManager.showroom)
                        {
                            r.SetActive(true);
                            r.GetComponent<RoomBehavior>().active = true;
                        }
                        
                        nextRoomLoaded = true;
                    }
                }
                else if (door.name == "Right Door")
                {
                    if (r.GetComponent<RoomBehavior>().x == roomBehavior.x + 1 && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                    {
                        r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                        if (!layerManager.showroom)
                        {
                            r.SetActive(true);
                            r.GetComponent<RoomBehavior>().active = true;
                        }
                        
                        nextRoomLoaded = true;
                    }
                }
                else if (door.name == "Left Door")
                {
                    if (r.GetComponent<RoomBehavior>().x == roomBehavior.x - 1 && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                    {
                        r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                        if (!layerManager.showroom)
                        {
                            r.SetActive(true);
                            r.GetComponent<RoomBehavior>().active = true;
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
                            if (!layerManager.showroom)
                            {
                                r.SetActive(true);
                                r.GetComponent<RoomBehavior>().active = true;
                            }

                            nextRoomLoaded = true;
                        }
                    }
                    else if (secondDoor.name == "Down Door")
                    {
                        if (r.GetComponent<RoomBehavior>().x == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y + 1)
                        {
                            r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                            if (!layerManager.showroom)
                            {
                                r.SetActive(true);
                                r.GetComponent<RoomBehavior>().active = true;
                            }

                            nextRoomLoaded = true;
                        }
                    }
                    else if (secondDoor.name == "Right Door")
                    {
                        if (r.GetComponent<RoomBehavior>().x == roomBehavior.x + 1 && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                        {
                            r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                            {
                                r.SetActive(true);
                                r.GetComponent<RoomBehavior>().active = true;
                            }
                            
                            nextRoomLoaded = true;
                        }
                    }
                    else if (secondDoor.name == "Left Door")
                    {
                        if (r.GetComponent<RoomBehavior>().x == roomBehavior.x - 1 && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                        {
                            r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                            {
                                r.SetActive(true);
                                r.GetComponent<RoomBehavior>().active = true;
                            }
                            
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

    IEnumerator OpenDoorTimer()
    {
        yield return new WaitForSeconds(5f);

        doorCanOpen = false;
    }

    #endregion

    #region Spawn Methods

    void SpawnEnemies()
    {
        int enemyAmount = Random.Range(3, maxEnemies + 1);

        for (int i = 0; i < enemyAmount; i++)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Count);
            if (spawned[spawnIndex] == false)
            {
                int j = Random.Range(1, 101);

                int k = -1;
                if (j <= impChance)
                {
                    k = 1;
                }
                else if (j <= basicDemonChance)
                {
                    k = 0;
                }
                else
                {
                    i--;
                    continue;
                }

                if (k == -1)
                {
                    i--;
                    continue;
                }

                var newEnemy = Instantiate(enemyTypes[k], spawnPoints[spawnIndex].position + new Vector3(0f, 1f, 0f), Quaternion.identity, enemyList);
                newEnemy.GetComponent<EnemySight>().roomSpawner = this;
                if (newEnemy.TryGetComponent(out BasicEnemyHealth basicHealth))
                {
                    basicHealth.roomSpawner = this;
                }
                else if (newEnemy.TryGetComponent(out ImpHealth impHealth))
                {
                    impHealth.roomSpawner = this;
                    newEnemy.transform.position -= new Vector3(0f, 0.5f, 0f);
                }
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

    void CheckLights()
    {
        if (door != null)
        {
            if (roomBehavior.x == roomBehavior.boardSize.x && roomBehavior.y == roomBehavior.boardSize.y && !layerManager.showroom)
            {
                foreach(GameObject l in roomBehavior.lights)
                {
                    l.GetComponentInChildren<Light>().color = mainPathColor;
                    l.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);;
                }
                return;
            }

            GameObject[] rooms = layerManager.rooms;

            foreach (GameObject r in rooms)
            {
                if (door.name == "Up Door")
                {
                    if (r.GetComponent<RoomBehavior>().x == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y - 1)
                    {
                        for (int i = 0; i < roomBehavior.lights.Length; i++)
                        {
                            if (r.GetComponent<RoomBehavior>().mainPath && roomBehavior.doors[i] == door)
                            {
                                roomBehavior.lights[i].GetComponentInChildren<Light>().color = mainPathColor;
                                roomBehavior.lights[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
                            }
                        }
                    }
                }
                else if (door.name == "Down Door")
                {
                    if (r.GetComponent<RoomBehavior>().x == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y + 1)
                    {
                        for (int i = 0; i < roomBehavior.lights.Length; i++)
                        {
                            if (r.GetComponent<RoomBehavior>().mainPath && roomBehavior.doors[i] == door)
                            {
                                roomBehavior.lights[i].GetComponentInChildren<Light>().color = mainPathColor;
                                roomBehavior.lights[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
                            }
                        }
                    }
                }
                else if (door.name == "Right Door")
                {
                    if (r.GetComponent<RoomBehavior>().x - 1 == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                    {
                        for (int i = 0; i < roomBehavior.lights.Length; i++)
                        {
                            if (r.GetComponent<RoomBehavior>().mainPath && roomBehavior.doors[i] == door)
                            {
                                roomBehavior.lights[i].GetComponentInChildren<Light>().color = mainPathColor;
                                roomBehavior.lights[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
                            }
                        }
                    }
                }
                else if (door.name == "Left Door")
                {
                    if (r.GetComponent<RoomBehavior>().x + 1 == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                    {
                        for (int i = 0; i < roomBehavior.lights.Length; i++)
                        {
                            if (r.GetComponent<RoomBehavior>().mainPath && roomBehavior.doors[i] == door)
                            {
                                roomBehavior.lights[i].GetComponentInChildren<Light>().color = mainPathColor;
                                roomBehavior.lights[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
                            }
                        }
                    }
                }

                if (secondDoor != null)
                {
                    if (secondDoor.name == "Up Door")
                    {
                        if (r.GetComponent<RoomBehavior>().x == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y - 1)
                        {
                            for (int i = 0; i < roomBehavior.lights.Length; i++)
                            {
                                if (r.GetComponent<RoomBehavior>().mainPath && roomBehavior.doors[i] == secondDoor)
                                {
                                    roomBehavior.lights[i].GetComponentInChildren<Light>().color = mainPathColor;
                                    roomBehavior.lights[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
                                }
                            }
                        }
                    }
                    else if (secondDoor.name == "Down Door")
                    {
                        if (r.GetComponent<RoomBehavior>().x == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y + 1)
                        {
                            for (int i = 0; i < roomBehavior.lights.Length; i++)
                            {
                                if (r.GetComponent<RoomBehavior>().mainPath && roomBehavior.doors[i] == secondDoor)
                                {
                                    roomBehavior.lights[i].GetComponentInChildren<Light>().color = mainPathColor;
                                    roomBehavior.lights[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
                                }
                            }
                        }
                    }
                    else if (secondDoor.name == "Right Door")
                    {
                        if (r.GetComponent<RoomBehavior>().x - 1 == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                        {
                            for (int i = 0; i < roomBehavior.lights.Length; i++)
                            {
                                if (r.GetComponent<RoomBehavior>().mainPath && roomBehavior.doors[i] == secondDoor)
                                {
                                    roomBehavior.lights[i].GetComponentInChildren<Light>().color = mainPathColor;
                                    roomBehavior.lights[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
                                }
                            }
                        }
                    }
                    else if (secondDoor.name == "Left Door")
                    {
                        if (r.GetComponent<RoomBehavior>().x + 1 == roomBehavior.x && r.GetComponent<RoomBehavior>().y == roomBehavior.y)
                        {
                            for (int i = 0; i < roomBehavior.lights.Length; i++)
                            {
                                if (r.GetComponent<RoomBehavior>().mainPath && roomBehavior.doors[i] == secondDoor)
                                {
                                    roomBehavior.lights[i].GetComponentInChildren<Light>().color = mainPathColor;
                                    roomBehavior.lights[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);
                                }
                            }
                        }
                    }
                }
            }
        }
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
