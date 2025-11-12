using UnityEngine;
using System;
using UnityEditor.UI;
using System.Collections;
using Unity.Mathematics;
public class W02D02S01CA : MonoBehaviour
{
    public int gridSize = 64;
    public float percentageOfOnes = .4f;
    public int seed = 0;

    public MeshRenderer planeRenderer;
    private int[,] intGrid;

    public float speed;

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
                    intGrid[i, j] = 1;
                }
                else
                {
                    intGrid[i, j] = 0;
                }
            }

        planeRenderer =  GameObject.CreatePrimitive(PrimitiveType.Plane).GetComponent<MeshRenderer>();
       intGrid= ProcGenTools.BorderMe(intGrid);

        // WolframsRule();
        SmoothMap();
        planeRenderer.material.mainTexture = ProcGenTools.IntToBoolTexture(intGrid);
    
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
