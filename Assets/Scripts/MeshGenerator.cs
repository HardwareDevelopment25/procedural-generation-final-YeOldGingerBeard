using UnityEngine;
using UnityEngine.Rendering.Universal;

public static class MeshGenerator
{
	public static MeshData GenerateTerrain(float[,] heightMap, float heightMultiplier )
	{
		int height = heightMap.GetLength( 0 );
		int width = heightMap.GetLength( 1 );

		float topLeftX = (width - 1) / -2f;
		float topLeftZ = (height - 1) / 2f;

		MeshData meshdata = new MeshData(width, height);
		int vertexIndex = 0;

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				meshdata.vertices[vertexIndex] =
				new Vector3(topLeftX + x, heightMap[x, y] * heightMultiplier, topLeftZ - y);

				meshdata.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

				// finally lets create those triangle faces

				if(x<width-1 && y< height -1)// We are making a square of 2x Tris
				{
					meshdata.AddTriangle(vertexIndex,vertexIndex+width+1,vertexIndex+width);
                    meshdata.AddTriangle(vertexIndex + width +1, vertexIndex, vertexIndex+1);
                }
				vertexIndex++;
			}
		}
		return meshdata;
	}
}

public class MeshData
{
	public Vector3[] vertices;
	public int[] triangles;

	int trianglesIndex = 0;
	public Vector2[] uvs;

	public MeshData(int meshWidth,int meshHeight)
	{
		vertices = new Vector3[meshWidth * meshHeight];
		//I dont want to fall off the grid into invalid space
		triangles = new int[(meshWidth-1) * (meshHeight-1)];
		uvs = new Vector2[meshWidth * meshHeight];
	}
	public void AddTriangle(int a, int b , int c)
	{
		triangles[trianglesIndex] = a;
		triangles[trianglesIndex+1] = b;
		triangles[trianglesIndex+2] = c;
		trianglesIndex+=3;
	}

	public Mesh CreateMesh() // here we make the mesh
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		return mesh;
	}
}
