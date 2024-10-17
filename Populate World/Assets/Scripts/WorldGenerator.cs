using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public bool autoUpdate;
    // public float scale;

    [Header("World Size")]
    public int width = 10;
    public int height = 10;

    [Header("Parameter")]
    public float amplitude = 0.4f;
    public float frequency = 0.05f;
    public int octaves = 3;
    public float lacunarity = 2.0f;
    public float persistence = 0.5f;
    public int seed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateNoise(){
       /*  if (scale <= 0)
        {
            scale = 0.0001f;   
        } */

        var grid = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var elevation = 0f;
                var tempFrequency = frequency;
                var tempAmplitude = amplitude;

                for (int k = 0; k < octaves; k++)
                {
                    var x = i /* / scale */ * tempFrequency + seed;
                    var y = j /* / scale */ * tempFrequency + seed;

                    //var perlinNoise = Mathf.PerlinNoise(x, y) * 2 - 1;

                    elevation += Mathf.PerlinNoise(x, y) * tempAmplitude;

                    tempFrequency *= lacunarity;
                    tempAmplitude *= persistence;
                }

                grid[i, j] = elevation;
            }
        }
        // PrintGrid(grid);

        NoiseDisplay display = FindAnyObjectByType<NoiseDisplay>();
        // display.DrawNoiseMap(grid);
        display.DrawTiles(grid);
        display.DrawValues(grid, autoUpdate);
    }

    public void PrintGrid(float[,] gird){
        string print = "[ ";

        for (int i = 0; i < gird.GetLength(0); i++)
        {
            for (int j = 0; j < gird.GetLength(1); j++)
            {
                print = string.Concat(print + gird[i,j] + ", ");
            }

            print = string.Concat(print + "]\n[ ");
        }

        Console.Write(print);
        Debug.Log(print);
    }
}
