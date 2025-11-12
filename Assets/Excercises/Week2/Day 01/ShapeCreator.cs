using Unity.VisualScripting;
using UnityEngine;

public class ShapeCreator : MonoBehaviour
{
    public int SizeOfGrid = 128;

    [Range(1, 6)]
    public int levelOfDetail = 1;
    public AnimationCurve AnimationCurve;
    public float heightMult = 10f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshFilter mf = this.AddComponent<MeshFilter>();
        MeshRenderer mr = this.AddComponent<MeshRenderer>();

        Material mat = new Material(Shader.Find("Unlit/Texture"));
      

        float[,] noiseMap = NoiseMapGenerator.GenerateNoiseMap(SizeOfGrid, SizeOfGrid, 10, 1, 5, 1, 0, Vector2.zero);

       MeshData md =  MeshGenerator.GenerateTerrainMesh(noiseMap, heightMult, AnimationCurve,levelOfDetail);
        mf.sharedMesh = md.CreateMesh();

        mat.mainTexture = ProcGenTools.RenderNoiseAsGreyTexture(noiseMap);
        mr.material = mat;
        // then worry about slapping onthe texture.
    }
   



}
