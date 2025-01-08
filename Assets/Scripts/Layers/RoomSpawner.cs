using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
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

    [Header("GameObjects")]
    GameObject door;
    GameObject secondDoor;
    GameObject doorHinge;
    GameObject secondDoorHinge;
    public GameObject chest;

    [Header("Transforms")]
    public Transform enemyList;
    Transform chestSpawn;

    [Header("Door Strings")]
    public const string downDoorString = "Down Door";
    public const string upDoorString = "Up Door";
    public const string rightDoorString = "Right Door";
    public const string leftDoorString = "Left Door";

    [Header("Lists")]
    readonly List<Transform> spawnPoints = new();
    public List<GameObject> enemies = new();
    readonly List<Transform> chestSpawns = new();
    public List<GameObject> enemyTypes = new();
    
    [Header("Arrays")]
    bool[] spawned;

    [Header("Colors")]
    Color mainPathColor;

    [Header("Components")]
    RoomBehavior roomBehavior;

    #endregion

    #region StartUpdate Methods

    // Start is called before the first frame update
    void Start()
    {
        roomBehavior = GetComponent<RoomBehavior>();

        mainPathColor = RoomBehavior.mainPathColor;

        basicDemonChance = LayerGenerator.Instance.basicDemonChance;
        impChance = LayerGenerator.Instance.impChance;

        if (basicDemonChance + impChance != 100)
        {
            Debug.LogError("Enemy spawn-chances do not match up to 100% basicDemonChance has been changed to match.");

            if (basicDemonChance + impChance < 100) basicDemonChance += 100 - (basicDemonChance - impChance);
            else basicDemonChance -= 100 - (basicDemonChance - impChance);
        }

        if (gameObject.name.Contains("MazeRoom"))
            for (int i = 0; i < maxEnemies; i++) spawnPoints.Add(transform.Find("SpawnPositions").GetChild(i));
        else
            for (int i = 0; i < transform.Find("Floor").childCount; i++) spawnPoints.Add(transform.Find("Floor").GetChild(i));

        spawned = new bool[spawnPoints.Count];

        for (int i = 0; i < 4; i++) chestSpawns.Add(transform.Find("ChestSpawns").GetChild(i));

        doorCanOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        layerGenerated = LayerGenerator.Instance.layerGenerated;

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

            if (PlayerComponents.Instance && PlayerComponents.Instance.playerSouls.playerPathfinder) UpdateLights();
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
        
        if (!enemiesSpawned && !LayerManager.Instance.showroom) SpawnEnemies();
        else if (enemies.Count == 0) enemiesDefeated = true;
    }

    #endregion

    #region Room Methods

    void OpenDoor()
    {
        if (door != null)
        {
            door.GetComponentInChildren<MeshCollider>().enabled = false;
            doorHinge.transform.localRotation = Quaternion.Slerp(doorHinge.transform.localRotation, StartLevel.Instance.openRot, Time.deltaTime);

            var sfxManager = SFXAudioManager.Instance;

            if (!primaryDoorAudioPlayed)
            {
                if (LayerManager.Instance.showroom == false) sfxManager.PlayClip(sfxManager.doorOpen, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod, true);
                primaryDoorAudioPlayed = true;
            }

            if (secondDoor != null)
            {
                secondDoor.GetComponentInChildren<MeshCollider>().enabled = false;
                secondDoorHinge.transform.localRotation = Quaternion.Slerp(secondDoorHinge.transform.localRotation, StartLevel.Instance.openRot, Time.deltaTime);

                if (!secondaryDoorAudioPlayed)
                {
                    if (LayerManager.Instance.showroom == false) sfxManager.PlayClip(sfxManager.doorOpen, MasterAudioManager.Instance.sBlend2D, sfxManager.effectsVolumeMod, true);
                    secondaryDoorAudioPlayed = true;
                }
            }
        }
    }

    void LoadNextRoom()
    {
        if (door != null)
        {
            if (roomBehavior.x == roomBehavior.boardSize.x && roomBehavior.y == roomBehavior.boardSize.y && !LayerManager.Instance.showroom)
            {
                BossGenerator.Instance.gameObject.SetActive(true);
                BossGenerator.Instance.GetComponent<RoomBehavior>().active = true;
                return;
            }

            GameObject[] rooms = LayerManager.Instance.rooms;

            foreach (GameObject r in rooms)
            {
                var doorMods = door.name switch
                {
                    upDoorString => new Vector2(roomBehavior.x, roomBehavior.y - 1),
                    downDoorString => new Vector2(roomBehavior.x, roomBehavior.y + 1),
                    rightDoorString => new Vector2(roomBehavior.x + 1, roomBehavior.y),
                    leftDoorString => new Vector2(roomBehavior.x - 1, roomBehavior.y),

                    _ => throw new System.Exception("Door name was not recognized.")
                };

                if (r.GetComponent<RoomBehavior>().x == doorMods.x && r.GetComponent<RoomBehavior>().y == doorMods.y)
                {
                    r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                    if (!LayerManager.Instance.showroom)
                    {
                        r.SetActive(true);
                        r.GetComponent<RoomBehavior>().active = true;
                    }
                    
                    nextRoomLoaded = true;
                }

                if (secondDoor != null)
                {
                    var secondDoorMods = secondDoor.name switch
                    {
                        upDoorString => new Vector2(roomBehavior.x, roomBehavior.y - 1),
                        downDoorString => new Vector2(roomBehavior.x, roomBehavior.y + 1),
                        rightDoorString => new Vector2(roomBehavior.x + 1, roomBehavior.y),
                        leftDoorString => new Vector2(roomBehavior.x - 1, roomBehavior.y),

                        _ => throw new System.Exception("Door name was not recognized.")
                    };

                    if (r.GetComponent<RoomBehavior>().x == secondDoorMods.x && r.GetComponent<RoomBehavior>().y == secondDoorMods.y)
                    {
                        r.GetComponent<RoomBehavior>().backDoor.SetActive(false);
                        if (!LayerManager.Instance.showroom)
                        {
                            r.SetActive(true);
                            r.GetComponent<RoomBehavior>().active = true;
                        }

                        nextRoomLoaded = true;
                    }
                }
            }
        }
        else
        {
            var chestName = roomBehavior.backDoor.name switch
            {
                upDoorString => "Down",
                downDoorString => "Up",
                rightDoorString => "Left",
                leftDoorString => "Right",

                _ => throw new System.Exception("Door name was not recognized.")
            };
            chestName += " Chest";

            foreach (Transform c in chestSpawns)
                if (c.name == chestName) chestSpawn = c;

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
                int k;

                if (j <= impChance) k = 1;
                else if (j <= basicDemonChance) k = 0;
                else
                {
                    i--;
                    continue;
                }

                var newEnemy = Instantiate(enemyTypes[k], spawnPoints[spawnIndex].position + new Vector3(0f, 1f, 0f), Quaternion.identity, enemyList);
                newEnemy.GetComponent<EnemySight>().roomSpawner = this;
                newEnemy.GetComponent<EnemyHealth>().roomSpawner = this;
                if (newEnemy.TryGetComponent(out ImpAction _)) newEnemy.transform.position -= new Vector3(0f, 0.5f, 0f);
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

    void UpdateLights()
    {
        if (door != null)
        {
            if (roomBehavior.x == roomBehavior.boardSize.x && roomBehavior.y == roomBehavior.boardSize.y && !LayerManager.Instance.showroom)
            {
                foreach(GameObject l in roomBehavior.lights)
                {
                    l.GetComponentInChildren<Light>().color = mainPathColor;
                    l.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor",mainPathColor);;
                }
                return;
            }

            GameObject[] rooms = LayerManager.Instance.rooms;

            foreach (GameObject r in rooms)
            {
                var doorMods = door.name switch
                {
                    upDoorString => new Vector2(roomBehavior.x, roomBehavior.y - 1),
                    downDoorString => new Vector2(roomBehavior.x, roomBehavior.y + 1),
                    rightDoorString => new Vector2(roomBehavior.x + 1, roomBehavior.y),
                    leftDoorString => new Vector2(roomBehavior.x - 1, roomBehavior.y),

                    _ => throw new System.Exception("Door name was not recognized.")
                };

                if (r.GetComponent<RoomBehavior>().x == doorMods.x && r.GetComponent<RoomBehavior>().y == doorMods.y)
                {
                    for (int i = 0; i < roomBehavior.lights.Length; i++)
                        if (r.GetComponent<RoomBehavior>().mainPath && roomBehavior.doors[i] == door)
                        {
                            roomBehavior.lights[i].GetComponentInChildren<Light>().color = mainPathColor;
                            roomBehavior.lights[i].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", mainPathColor);
                        }
                }

                if (secondDoor != null)
                {
                    var secondDoorMods = secondDoor.name switch
                    {
                        upDoorString => new Vector2(roomBehavior.x, roomBehavior.y - 1),
                        downDoorString => new Vector2(roomBehavior.x, roomBehavior.y + 1),
                        rightDoorString => new Vector2(roomBehavior.x + 1, roomBehavior.y),
                        leftDoorString => new Vector2(roomBehavior.x - 1, roomBehavior.y),

                        _ => throw new System.Exception("Door name was not recognized.")
                    };

                    if (r.GetComponent<RoomBehavior>().x == secondDoorMods.x && r.GetComponent<RoomBehavior>().y == secondDoorMods.y)
                    {
                        for (int i = 0; i < roomBehavior.lights.Length; i++)
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

    #endregion

    #region General Methods

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.CompareTag("Player")) inArea = true;
    }
    void OnTriggerExit(Collider coll)
    {
        if (coll.transform.CompareTag("Player")) inArea = false;
    }

    #endregion
}
