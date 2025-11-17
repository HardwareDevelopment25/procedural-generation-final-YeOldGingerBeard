using System;
using System.Collections;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class W02D02S01CA : MonoBehaviour
{
    public int gridSize = 64;
    public float percentageOfOnes = .4f;
    public int seed = 0;

    public GameObject wall, grass, spikes;

    public MeshRenderer planeRenderer;
    private int[,] intGrid;

    public float speed;

    public int smoothingIterations = 1;
    public GameObject cellPrefab;
    public Sprite[] marchinSprites;

    private void Awake()
    {
 
        intGrid = new int[gridSize,gridSize];
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        System.Random rnd = new System.Random(seed);

        for (int i = 0; i < gridSize; i++)
            for (int j = 0; j < gridSize; j++)
            {
                double randomFloat = rnd.NextDouble();
                if( randomFloat < percentageOfOnes)
                {
                    intGrid[i, j] = 0;
                }
                else
                {
                    intGrid[i, j] = 1;
                }
            }

        planeRenderer =  GameObject.CreatePrimitive(PrimitiveType.Plane).GetComponent<MeshRenderer>();
       intGrid= ProcGenTools.BorderMe(intGrid);

        // WolframsRule();
        SmoothMap(); // Makes the Cave

        ApplyGrassAndSpikes();


        planeRenderer.material.mainTexture = ProcGenTools.IntToBoolTexture(intGrid); // Rendering to a texture


        // add up all the neightbours to calculate which tile to show.



       // Render3D(intGrid);


        GenerateMarchinSquares();



    }

    void GenerateMarchinSquares()
    {   
        for(int x = 1;x < intGrid.GetLength(0)-1; x++)
            for (int y = 1; y < intGrid.GetLength(1)-1; y++)
            {

                PlaceCell(x, y, GetConfigIndex(x, y));
            }
    }

    void PlaceCell(int x, int y , int configIndex)
    {
        Vector3 pos = new Vector3(x, 0, y);
        GameObject cell = Instantiate(cellPrefab, pos, Quaternion.identity);
        cell.transform.rotation = Quaternion.Euler(90,0,0);
        SpriteRenderer spriteRenderer = cell.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = marchinSprites[configIndex];    


        // You now need to take the index of sprite 0 - 16 and load that into the
        // texture of the plan created.

    }
    int GetConfigIndex(int x, int y)
    {
        int configIndex = 0;

        if (intGrid[x, y] == 1) configIndex |= 1;
        if (intGrid[x+1, y] == 1) configIndex |= 2;
        if (intGrid[x + 1, y+ 1] == 1) configIndex |= 4;
        if (intGrid[x, y+1] == 1) configIndex |= 8;
        return configIndex;
    }



    private void ApplyGrassAndSpikes()
    {

        for (int x = 1; x < intGrid.GetLength(0)-1; x++)
        {
            for (int y = 1; y < intGrid.GetLength(1)-1; y++)
            {
                if(intGrid[x, y] ==1)
                {
                    if(intGrid[x, y+1] == 0)
                    {
                        intGrid[x, y] =2;
                    }
                    if(intGrid[x, y-1]==0)
                    {
                        intGrid[x, y] = 3;
                    }
                }
            }
        }
    }


    public void Render3D(int[,] grid)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                switch(grid[x,y]) // 1 wall, 2 grass, 3 spikes ....  .. 
                {
                    case 0: 
                    break;
                    case 1:GameObject.Instantiate(wall,new Vector3(x,0,y), quaternion.identity, this.transform);
                    break;
                    case 2:
                        GameObject.Instantiate(grass, new Vector3(x, 0, y), quaternion.identity, this.transform);
                    break;

                    case 3:
                        GameObject.Instantiate(spikes, new Vector3(x, 0, y), quaternion.identity, this.transform);
                    break;
                    default: Debug.LogError("UNKNOWN TILE TYPE");
                        break;
                        
                }
            }
        }
    }

    IEnumerator animator()
    {
        while (true)
        {
            //WolframsRule();
            SmoothMap();
           planeRenderer.material.mainTexture = ProcGenTools.IntToBoolTexture(intGrid);
            yield return new WaitForSeconds(speed);
        }

    }

    void SmoothMap()
    {
        for(int s = 0; s< smoothingIterations;s++)
        for(int i = 0;i < gridSize;i++)
        for(int j = 0;j < gridSize;j++)
            {
                int nei = GetNeighours(i,j);
                if (nei > 4) intGrid[i, j] = 1;
                else if (nei < 4) intGrid[i, j] = 0;

            }
    }

    void WolframsRule()
    {
      for(int x=0;x<gridSize;x++)
            for(int y=0;y<gridSize;y++)
            {
                int neighbours = GetNeighours(x, y);
                Debug.Log(neighbours);
                // Add old wolfie here
                if (intGrid[x,y]==1) // for space that is population
                {
                    if (neighbours < 2) intGrid[x, y] = 0;
                    else if (neighbours >= 4) intGrid[x, y] = 0;
                    else if (neighbours == 2 || neighbours == 3) intGrid[x, y] = 1;
                }
                else
                {
                    // if this grid is dead, a single rule brings to life
                    if(neighbours ==3) intGrid[x, y] = 1;
                }

            }
    }
    public int GetNeighours(int gridX, int gridY )
    {
        int totalNeighbouts = 0;
        for(int x= -1; x<=1; x++)
            for (int y = -1; y <= 1; y++)
                if(isMapInRange(gridX+x, gridY+y))
                {
                    if (intGrid[gridX + x, gridY + y] == 1) totalNeighbouts++;
                }
        if (intGrid[gridX, gridY] == 1) totalNeighbouts -= 1;// we remove our selfs if it was counted.

        return totalNeighbouts;
    }

    // just saying I hate this as it has limited usibilty scope.
    bool isMapInRange(int x, int y)=> x>=0 && x< gridSize && y>=0 && y< gridSize;


}
