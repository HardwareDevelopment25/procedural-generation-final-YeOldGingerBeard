using UnityEngine;


// I want to create a bag of tools for future use
public static class ProcGenTools
{

    public static Mesh makeTriangle(float SizeOfTriangle)
    {
        Mesh triangle = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            new Vector3 (0, 0, 0),
            new Vector3 (SizeOfTriangle, 0, 0),
            new Vector3 (SizeOfTriangle/2, SizeOfTriangle, 0)
        };

        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0.5f,1)
        };

        int[] triangles = new int[]
        {
            0,1,2
        };

        triangle.vertices = vertices;
        triangle.uv = uvs;
        triangle.triangles = triangles;

        return triangle; 

    }


    public static Texture2D IntToBoolTexture(int[,] grid)
    {
        Texture2D texture2D = new Texture2D(grid.GetLength(0), grid.GetLength(1));
        // Do some conversion and ship the texture2d off to who needs it.

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y]==0)
                {
                    texture2D.SetPixel(x, y, Color.white);
                }
                else if (grid[x, y]==1) 
                {
                    texture2D.SetPixel(x, y, Color.black); // vlassing black as wall
                }
                else
                {
                    texture2D.SetPixel(x, y, Color.blue);
                }
            }
        }

        texture2D.Apply();
        texture2D.filterMode = FilterMode.Point;
        texture2D.wrapMode = TextureWrapMode.Clamp;

        return texture2D;
    }
    public static Texture2D RenderBoolArrayAsTexture(bool[,] maze) //
    {
        Texture2D texture2D = new Texture2D(maze.GetLength(0), maze.GetLength(1));
        // Do some conversion and ship the texture2d off to who needs it.

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {       
                if (maze[x, y])
                {
                    texture2D.SetPixel(x, y, Color.white);
                }
                else
                {
                    texture2D.SetPixel(x, y, Color.black);
                }
            }
        }

        texture2D.Apply();
        texture2D.filterMode = FilterMode.Point;
        texture2D.wrapMode = TextureWrapMode.Clamp;
                
        return texture2D;
    }
    public static Texture2D RenderNoiseAsGreyTexture(float[,] maze) //
    {
        Texture2D texture2D = new Texture2D(maze.GetLength(0), maze.GetLength(1));
        // Do some conversion and ship the texture2d off to who needs it.

        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                Color gradiant = new Color(maze[x, y], maze[x, y], maze[x, y]);
                    texture2D.SetPixel(x, y, gradiant);
            }
        }

        texture2D.Apply();
        texture2D.filterMode = FilterMode.Point;
        texture2D.wrapMode = TextureWrapMode.Clamp;

        return texture2D;
    }

    public static int[,] BorderMe (int[,] gridToBorder)
    {
        for(int x = 0;x < gridToBorder.GetLength(0);x++)
            for(int y = 0;y < gridToBorder.GetLength(1);y++)
            {
              // all 0 and heights/widths are set to walls  
               if(x==0 || x == gridToBorder.GetLength(0)-1 
                 || y == 0 || y == gridToBorder.GetLength(1)-1 )
                {
                    gridToBorder[x, y] = 1;
                }
            }  
        return gridToBorder;
    }

   

 
}
