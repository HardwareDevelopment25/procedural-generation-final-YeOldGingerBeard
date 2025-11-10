using Unity.VisualScripting;
using UnityEngine;

public class ShapeCreator : MonoBehaviour
{
    public int SizeOfGrid = 128;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshFilter mf = this.AddComponent<MeshFilter>();
        MeshRenderer mr = this.AddComponent<MeshRenderer>();

        Material mat = new Material(Shader.Find("Unlit/Texture"));
      

        float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap(SizeOfGrid, SizeOfGrid, 10, 1, 5, 1, 0, Vector2.zero);

       MeshData md =  MeshGenerator.GenerateTerrain(noiseMap, 10f);
        mf.sharedMesh = md.CreateMesh();

        mat.mainTexture = ProcGenTools.RenderNoiseAsGreyTexture(noiseMap);
        mr.material = mat;
        // then worry about slapping onthe texture.
    }
   



}
