using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InternalMaze : MonoBehaviour
{
    #region SubClass

    public class Cell
    {
        #region Cell Variables

        [Header("Bools")]
        public bool visited = false;
        public bool unVisitable = false;

        [Header("Arrays")]
        public bool[] status = new bool[4];
        public bool[] doors = new bool[4];
        public bool[] backDoors = new bool[4];

        #endregion Cell Variables
    }

    #endregion

    #region Variables

    [Header("Ints")]
    public int startPos = 0;
    public int maxHoles;

    [Header("GameObjects")]
    public GameObject internalCell;
    public GameObject floorRemover;

    [Header("Vector2s")]
    public Vector2 size;
    public Vector2 offset;
    public Vector2 roomOffset;

    [Header("Lists")]
    List<Cell> board;

    [Header("Components")]
    public RoomBehavior roomBehavior;

    #endregion

    #region General Methods

    void GenerateMazeCell()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[Mathf.FloorToInt(i + j * size.x)];
                if (currentCell.visited)
                {
                    var newCell = Instantiate(internalCell, new Vector3(i * offset.x + roomBehavior.x * roomOffset.x - size.x + 1, 0, 
                        -j * offset.y - roomBehavior.y * roomOffset.y + size.y - 1), 
                        Quaternion.identity, transform.Find("InternalMaze")).GetComponent<InternalCellBehaiviour>();
                    
                    newCell.UpdateCell(board[Mathf.FloorToInt(i + j * size.x)].status);
                    newCell.cellNumber = Mathf.FloorToInt(i + j * size.x);
                    newCell.name += " " + i + " - " + j;
                }
                else if (!currentCell.visited)
                {
                    var newRemover = Instantiate(floorRemover, new Vector3(i * offset.x + roomBehavior.x * roomOffset.x - size.x + 1, 0, 
                        -j * offset.y - roomBehavior.y * roomOffset.y + size.y - 1),
                        Quaternion.identity, transform.Find("InternalMaze"));
                    
                    Destroy(newRemover, 2f);
                }
            }
        }
    }

    public void MazeGenerator()
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

        int l = Random.Range(0, maxHoles);
        while (l > 0)
        {
            int cellRemover = Random.Range(0, board.Count);

            if (cellRemover > 6 && cellRemover < 42 && 
                cellRemover != 13 && cellRemover != 20 && cellRemover != 27 && cellRemover != 34 && cellRemover != 41 && 
                cellRemover != 0 && cellRemover != 7 && cellRemover != 14 && cellRemover != 21 && cellRemover != 28 && cellRemover != 35 &&
                !board[cellRemover].unVisitable)
            {
                board[cellRemover].unVisitable = true;
                l--;
            }
            continue;
        }

        int k = 0;

        while(k < 1000)
        {
            k++;

            if (!board[currentCell].unVisitable)
            {
                board[currentCell].visited = true;
            }
            else
            {
                currentCell++;
                continue;
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
                    currentCell = path.Pop();
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
                        // new cell is to the Right of current cell
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        // new cell is Bellow of current cell
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        // new cell is to the Left of current cell
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        // new cell is Above of current cell
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }
        GenerateMazeCell();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new();

        // Upward
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited && 
            !board[Mathf.FloorToInt(cell - size.x)].unVisitable)
        {
            neighbors.Add(Mathf.FloorToInt(cell - size.x));
        }

        // Downward
        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited && 
            !board[Mathf.FloorToInt(cell + size.x)].unVisitable)
        {
            neighbors.Add(Mathf.FloorToInt(cell + size.x));
        }

        // Right
        if ((cell + 1) % size.x != 0 && !board[Mathf.FloorToInt(cell + 1)].visited && 
            !board[Mathf.FloorToInt(cell + 1)].unVisitable)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }

        // Left
        if (cell % size.x != 0 && !board[Mathf.FloorToInt(cell - 1)].visited && 
            !board[Mathf.FloorToInt(cell - 1)].unVisitable)
        {
            neighbors.Add(Mathf.FloorToInt(cell - 1));
        }

        return neighbors;
    }

    #endregion
}
