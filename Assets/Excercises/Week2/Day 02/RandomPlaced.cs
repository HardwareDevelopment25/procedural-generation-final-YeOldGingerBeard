using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class RandomPlaced : MonoBehaviour
{
    public int numPoints = 50; // This must have a finaite set of points
    public int numberCandidates = 5; // accuracy
    public float maxWidth = 10f;
    [SerializeReference]
    List<Vector2> points = new List<Vector2>();
    

    void GeneratePoints()
    {
        // Generate points using Mitchell's Best-Candidate Algorithm
        while (points.Count < numPoints)
        {
            Vector2 bestCandidate = Vector2.zero;
            float bestDistance = 0;
            for (int i = 0; i < numberCandidates; i++)
            {
                // Generate a random candidate
                Vector2 candidate = new Vector2(
                    Random.Range(0f, 10f),
                    Random.Range(0f, 10f)
                );
                // Find the distance to the nearest point in the list
                float nearestDistance = float.MaxValue;
                foreach (Vector2 point in points)
                {
                    float distance = Vector2.Distance(candidate, point);
                    nearestDistance = Mathf.Min(nearestDistance, distance);
                }
                // Update the best candidate if this one is farther
                if (nearestDistance > bestDistance)
                {
                    bestCandidate = candidate;
                    bestDistance = nearestDistance;
                }
            }
            // Add the best candidate to the points list
            points.Add(bestCandidate);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GeneratePoints();
        RenderPoints();
    }

    private void RenderPoints()
    {
        // Generate points
        foreach (var point in points)
        {
          var v =   GameObject.CreatePrimitive(PrimitiveType.Sphere);
            v.transform.parent = this.transform;
            v.transform.localScale = Vector3.one * .2f;
            v.transform.position = point;
        }
    }





    /* IEnumerator SpawnPoints()
     {
         while (totalSpheres > 0)
         {
             var point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
             point.transform.position = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100),0f );

             yield return new WaitForSeconds(0.1f);
             totalSpheres--;
         }
     }*/
    // Update is called once per frame

}
