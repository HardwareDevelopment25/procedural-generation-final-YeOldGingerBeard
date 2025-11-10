using TreeEditor;
using UnityEngine;
using System;
public class TextureLogicalAnd : MonoBehaviour
{
    public int imageSize = 64;
    public Texture2D texture;
    public float scaler = 1.0f;
    public int seed = 0, lac = 1, oct = 4;
    System.Random rnd;
    public Vector2 off = Vector2.zero;
    public AnimationCurve curve;

    private void Start(){
      rnd = new System.Random(seed);
        texture = new Texture2D(imageSize, imageSize);
        createPattern();
        Material m = GetComponent<MeshRenderer>().material;

        m.mainTexture = texture;
        GetComponent<MeshRenderer>().material = m;
    }
    int l = 0;
    private void Update()
    {
       if(l>400)
        {
            createPattern();
            l = 0;
        }
        l++;
        
    }
    public void createPattern(){
        {
            float[,] nm1 = NoiseMapGenerator.GenerateNoiseMap(imageSize, imageSize, scaler, lac,oct,1, rnd.Next(), off);
            float[,] nm = NoiseMapGenerator.GenerateFallOffMap(imageSize, curve);



            // draw it
            for (int i = 0; i < nm.GetLength(0); i++)
                for (int j = 0; j < nm.GetLength(1); j++)
                {
                    float combined = Mathf.Clamp01(nm1[i, j] - nm[i, j]);
                    texture.SetPixel(i, j, new Color(combined, combined,combined));
                }
        }
        texture.Apply();
    }
}
