using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LayerGenerator : MonoBehaviour
{
    #region SubClass

    public class Cell
    {
        #region Cell Variables

        [Header("Bools")]
        public bool visited = false;
        public bool mainPath = false;

        [Header("Arrays")]
        public bool[] status = new bool[4];
        public bool[] doors = new bool[4];
        public bool[] backDoors = new bool[4];

        #endregion Cell Variables
    }

    #endregion

    #region Variables

    [Header("Generation Options")]
    public InteriorChoice iChoice;
    [Range(0, 20)]
    public int internalMaxHoles = 7;
    [Range(1, 99)]
    public int mazeChance;
    [Range(0, 100)]
    public int basicDemonChance;
    [Range(0, 100)]
    public int impChance;
    public bool fillOut;
    public Vector2 size;

    [Header("Ints")]
    public int startPos = 0;

    [Header("Bools")]
    [HideInInspector]
    public bool layerGenerated;
    public bool printPops;
    bool doInterior;
    public bool playerPathfinder;

    [Header("GameObjects")]
    public GameObject internalCell;
    public GameObject enemySpawnPoint;

    [Header("Array")]
    public GameObject[] rooms;

    [Header("Lists")]
    List<Cell> board;

    [Header("Vector2s")]
    public Vector2 offset;

    [Header("Components")]
    LayerManager layerManager;
    [SerializeField] PlayerSouls playerSouls;
    SaveLoadManager slManager;


    #endregion

    #region StartUpdate Methods

    void Start()
    {
        var managers = GameObject.FindWithTag("Managers");

        layerManager = managers.GetComponent<LayerManager>();
        slManager = managers.GetComponent<SaveLoadManager>();
    }

    void Update()
    {
        if (slManager.ready && !layerGenerated)
        {
            if (!layerManager.showroom)
            {
                playerSouls = GameObject.FindWithTag("Player").GetComponent<PlayerSouls>();
                playerPathfinder = playerSouls.playerPathfinder;
            }

            Generator();
        }
    }

    #endregion

    #region General Methods

    void GenerateLayer()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];
                if (currentCell.visited)
                {
                    int randomRoom = Random.Range(1 , rooms.Length);
                    if (iChoice == InteriorChoice.NoMaze)
                    {
                        doInterior = false;
                    }
                    else if (iChoice == InteriorChoice.SomeMaze)
                    {
                        int interior = Random.Range(1, 101);
                        doInterior = interior <= mazeChance;
                    }
                    else if (iChoice == InteriorChoice.FullMaze)
                    {
                        doInterior = true;
                    }

                    if (doInterior)
                    {
                        randomRoom = 0;
                    }

                    var newRoom = Instantiate(rooms[randomRoom], new Vector3(i * offset.x, 0, -j * offset.y), 
                        Quaternion.identity, transform).GetComponentInChildren<RoomBehavior>();
                    newRoom.UpdateDoors(board[Mathf.FloorToInt(i + j * size.x)].doors);
                    newRoom.UpdateBackDoors(board[Mathf.FloorToInt(i + j * size.x)].backDoors);

                    newRoom.name += " " + i + " - " + j;
                    newRoom.boardSize = new Vector2(size.x - 1, size.y - 1);
                    newRoom.x = i;
                    newRoom.y = j;
                    newRoom.index = Mathf.FloorToInt(i + j * size.x);
                    newRoom.doInterior = doInterior;
                    newRoom.mainPath = currentCell.mainPath;

                    newRoom.playerPathfinder = playerPathfinder || layerManager.showroom;

                    if (!doInterior && newRoom.interior != null)
                    {
                        int randRot = Random.Range(0, 2);
                        newRoom.interior.transform.rotation = Quaternion.Euler(0f, randRot * 90f, 0f);
                        newRoom.transform.Find("Floor").rotation = Quaternion.Euler(0f, randRot * 90f, 0f);
                    }

                    if (doInterior)
                    {
                        var maze = newRoom.gameObject.AddComponent<InternalMaze>();

                        maze.internalCell = internalCell;
                        maze.enemySpawnPoint = enemySpawnPoint;
                        maze.roomBehavior = newRoom;
                        maze.size = new Vector2(7f, 7f);
                        maze.offset = new Vector2(2f, 2f);
                        maze.roomOffset = offset;
                        maze.maxHoles = internalMaxHoles;

                        maze.MazeGenerator();
                    }
                    
                    newRoom.UpdateRoom(board[Mathf.FloorToInt(i + j * size.x)].status);
                }
            }
        }
    }

    void  Generator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new();

        int k = 0;

        while(k < 1000)
        {
            k++;

            board[currentCell].visited = true;
            board[currentCell].mainPath = true;

            if (currentCell == board.Count - 1  && !fillOut)
            {
                if (!layerManager.showroom)
                {
                    board[currentCell].status[1] = true;
                    board[currentCell].doors[1] = true;
                }
                layerGenerated = true;
                break;
            }
            if (currentCell == startPos)
            {
                board[currentCell].status[0] = true;
                board[currentCell].backDoors[0] = true;
            }

            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    board[currentCell].mainPath = false;
                    currentCell = path.Pop();
                    if (printPops)
                    {
                        print("Path popped");
                    }
                    
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        board[currentCell].doors[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                        board[currentCell].backDoors[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        board[currentCell].doors[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                        board[currentCell].backDoors[0] = true;
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        board[currentCell].doors[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                        board[currentCell].backDoors[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        board[currentCell].doors[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                        board[currentCell].backDoors[1] = true;
                    }
                }
            }
        }
        GenerateLayer();
        layerGenerated = true;
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new();

        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - size.x));
        }

        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + size.x));
        }

        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }

        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbors;
    }

    #endregion

    #region Enums

    public enum InteriorChoice
    {
        NoMaze,
        SomeMaze,
        FullMaze
    }

    #endregion
}
