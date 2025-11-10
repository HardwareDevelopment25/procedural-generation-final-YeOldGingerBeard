using Unity.VisualScripting;
using UnityEngine;

public class ShapeCreator : MonoBehaviour
{
    public float sizeOfShape = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MeshFilter mf = this.AddComponent<MeshFilter>();
        MeshRenderer mr = this.AddComponent<MeshRenderer>();

        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mr.material = mat;
        mf.mesh = ProcGenTools.makeTriangle(sizeOfShape);
    }


}
