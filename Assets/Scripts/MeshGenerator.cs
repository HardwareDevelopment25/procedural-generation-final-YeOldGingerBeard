using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve curveModifier, int levelOfDetail)
    {

        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        // Force this to be centered on the screen X   -1   0   1
        float topLeftX = (width - 1) / -2f;//-1 to 1
        float topLeftZ = (height - 1) / 2f; //-1 to 1

        int simplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;

        int verticesPerLine = levelOfDetail;

        MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);

        // keep track on what vertex were on in the 1d array
        int vertexIndex = 0;

        // we want to loop through our heightmap
        for (int y = 0; y < height-simplificationIncrement; y += simplificationIncrement)
        {
            for (int x = 0; x < width- simplificationIncrement; x += simplificationIncrement)
            {
                // start creating our verticles
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, curveModifier.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);

                // final part, add the Uvs which is vertexIndex [ x / width, u / height ] 
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)// Dont be confused my vertix index its 1D to whole width needed to get next row down, check slide for details.
                {
                    //                        i            i+w+1                i+1 
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    //                          i+w+1                    i             i+1
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }


                vertexIndex++;
            }
        }

        // why return the mesh data instead of the mesh itsself?, 
        // unity prevents creation of Mesh from within a thread.

        return meshData;
    }


}



public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    // keep track on which triangle were on in AddTriangle 
    int trianglesIndex = 0;
    public Vector2[] uvs;  // Now lets add UVs so we can add textures to it

    public MeshData(int meshWidth, int meshHeight)
    {
        // Check slide for explaination 
        vertices = new Vector3[meshWidth * meshHeight];
        triangles = new int[((meshWidth - 1) * (meshHeight - 1)) * 6];
        uvs = new Vector2[meshWidth * meshHeight];
    }

    // easier way to add triangles 3 at a time for conveniance.
    public void AddTriangle(int a, int b, int c)
    {

        triangles[trianglesIndex] = a;
        triangles[trianglesIndex + 1] = b;
        triangles[trianglesIndex + 2] = c;
        trianglesIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals(); // fixes light issues
        return mesh;
    }
}


/*

    public static MeshData GenerateTerrainMesh1( float[,] heightMap, float heightMultiplier, AnimationCurve curveModifier,int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        
        // Force this to be centered on the screen X   -1   0   1
        float topLeftX = (width - 1) / -2f;//-1 to 1
        float topLeftZ = (height - 1) / 2f; //-1 to 1
        
        MeshData meshData = new MeshData(width, height);
        
        // keep track on what vertex were on in the 1d array
        int vertexIndex=0;
        
        // we want to loop through our heightmap
        for(int y=0; y<height; y++)
        {
            for(int x=0; x<width; x++)
            {
                // start creating our verticles
                meshData.vertices[vertexIndex]=new Vector3(topLeftX + x,0,topLeftZ-y);
                
                // final part, add the Uvs which is vertexIndex [ x / width, u / height ] 
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                
                if (x < width - 1 && y < height - 1)// Dont be confused my vertix index its 1D to whole width needed to get next row down, check slide for details.
                { 
                  meshData.AddTriangle(vertexIndex,vertexIndex+width+1,vertexIndex+width);
                  meshData.AddTriangle(vertexIndex + width +1 , vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
        return meshData;
    }*/