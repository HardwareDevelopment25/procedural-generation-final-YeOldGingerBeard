using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class Lsystem : MonoBehaviour
{
    public string axion = "F";

    public float angle = 45f;
    public int iterations = 3;
    public string[] laws;

    private string currentString;

    [SerializeReference]
    private Dictionary<char, string> rules = new Dictionary<char, string>();

    private void Awake()
    {
        foreach (var law in laws)
        {
            string[] l= law.Split("->");
            rules.Add(l[0][0], l[1]);// Takes first character, in a string and what ever is after 
            
        }
        currentString = axion;
        GenerateLSystemString();
    }

    private void GenerateLSystemString()
    {
        for (int i = 0; i < iterations; i++)// for each iteraton
        {
            StringBuilder sb = new StringBuilder();

            foreach (char c in currentString)
            {
                sb.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());

            }
            currentString = sb.ToString();

        }
        Debug.Log(currentString);
    }
}
