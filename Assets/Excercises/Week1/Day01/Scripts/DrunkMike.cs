using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DrunkMike : MonoBehaviour
{
    public GameObject wallPrefab, floorPrefab;
    public Vector2Int gridSize = Vector2Int.one;
    public Vector2Int startPos = Vector2Int.zero;
    public int seed = 0;
    private bool[,] Maze;

    public int steps = 30;
    System.Random rand;

    private void Awake()
    {
     rand = new System.Random(seed);
    }

    private void Start()
    {
        DrawMaze();
        DrawGrid();
    }

    // think about repeatability

    // get a directions list e.g up , down ,left
    public static List<Vector2Int> DirectionsList = new List<Vector2Int>
    {
        Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right
    };

    
    // get a random direction
    private Vector2Int GetRandomDirection()=> DirectionsList[ rand.Next(0, DirectionsList.Count)];




    private void DrawMaze()
    {
        Maze = new bool[gridSize.x, gridSize.y];// so it resets each time

        Maze[startPos.x,startPos.y] = true; // So mark my start area as a floor
        Vector2Int currentPosition = startPos;

        while(steps>0)
        {
            steps --;
            var direction = currentPosition + GetRandomDirection(); // where I am plus a random direction.
            // do some bound checking
            if (!IsInBounds(direction, gridSize)) continue;

            Maze[currentPosition.x, currentPosition.y] = true;
            currentPosition = direction;

        }

    }
    // check bounds 
    public bool IsInBounds(Vector2Int pos, Vector2Int matrixSize)
    {

        if(pos.x < 0 || pos.x >=matrixSize.x || pos.y < 0 || pos.y >=matrixSize.y) return false;
        return true;
    }
    // render the maze, So I can see.

    public void DrawGrid()
    {
        for (int y = 0; y < gridSize.x; y++)
        {
            for (int x = 0; x < gridSize.y; x++)
            {
                if (Maze[x, y])
                {
                    GameObject.Instantiate(floorPrefab, new Vector3(x, 0, y), Quaternion.identity);
                }
                else
                {
                    GameObject.Instantiate(wallPrefab, new Vector3(x, 0, y), Quaternion.identity);
                }
            }
        }
    }
   
}
