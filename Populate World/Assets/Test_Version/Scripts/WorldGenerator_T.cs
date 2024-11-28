using System;
using UnityEngine;

public class WorldGenerator_T : MonoBehaviour
{
    public bool autoUpdate;
    public bool randomSeed;
    public float[,] generatedGrid_Land;
    public float[,] generatedGrid_Water;
    public float[,] generatedGrid_LW;
    // public float scale;

    [Header("World Size")]
    public int width = 10;
    public int height = 10;

    [Header("Land Parameter")]
    public float amplitude_Land = 0.4f;
    public float frequency_Land = 0.02f;
    public int octaves_Land = 3;
    public float lacunarity_Land = 2.0f;
    public float persistence_Land = 0.5f;
    public int seed_Land = 1;

    [Header("Water Parameter")]
    public float amplitude_Water = 0.43f;
    public float frequency_Water = 0.005f;
    public int octaves_Water = 3;
    public float lacunarity_Water = 2.0f;
    public float persistence_Water = 0.5f;
    public int seed_Water = 1;

    public void GenerateNoise_Land()
    {
        /*  if (scale <= 0)
         {
             scale = 0.0001f;   
         } */

        if (randomSeed)
        {
            seed_Land = RandomSeed();
        }

        var grid = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var elevation = 0f;
                var tempFrequency = frequency_Land;
                var tempAmplitude = amplitude_Land;

                for (int k = 0; k < octaves_Land; k++)
                {
                    var x = i /* / scale */ * tempFrequency + seed_Land;
                    var y = j /* / scale */ * tempFrequency + seed_Land;

                    //var perlinNoise = Mathf.PerlinNoise(x, y) * 2 - 1;

                    elevation += Mathf.PerlinNoise(x, y) * tempAmplitude;

                    tempFrequency *= lacunarity_Land;
                    tempAmplitude *= persistence_Land;
                }

                grid[i, j] = elevation;
            }
        }
        // PrintGrid(grid);

        NoiseDisplay_T display = FindAnyObjectByType<NoiseDisplay_T>();
        //display.DrawNoiseMap(grid);
        //display.DrawTiles_4(grid);
        //display.DrawValues(grid, autoUpdate);

        generatedGrid_Land = grid;
    }

    public void GenerateNoise_Water()
    {
        if (randomSeed)
        {
            seed_Water = RandomSeed();
        }

        var grid = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var elevation = 0f;
                var tempFrequency = frequency_Water;
                var tempAmplitude = amplitude_Water;

                for (int k = 0; k < octaves_Water; k++)
                {
                    var x = i * tempFrequency + seed_Water;
                    var y = j * tempFrequency + seed_Water;

                    elevation += Mathf.PerlinNoise(x, y) * tempAmplitude;

                    tempFrequency *= lacunarity_Water;
                    tempAmplitude *= persistence_Water;
                }

                grid[i, j] = elevation;
            }
        }
        // PrintGrid(grid);

        generatedGrid_Water = grid;
    }

    public void GenerateNoise_LW()
    {
        GenerateNoise_Land();
        GenerateNoise_Water();

        var grid = new float[width, height];

        for (int i = 0; i < generatedGrid_Water.GetLength(0); i++)
        {
            for (int j = 0; j < generatedGrid_Water.GetLength(1); j++)
            {
                if (generatedGrid_Water[i, j] >= 0f && generatedGrid_Water[i, j] <= 0.3f)
                {
                    grid[i, j] = -1f;
                }
                else
                {
                    grid[i, j] = generatedGrid_Land[i, j];
                }
            }
        }

        generatedGrid_LW = grid;
    }

    public int RandomSeed()
    {
        return UnityEngine.Random.Range(-10000, 10000);
    }

    public void PrintGrid(float[,] gird)
    {
        string print = "[ ";

        for (int i = 0; i < gird.GetLength(0); i++)
        {
            for (int j = 0; j < gird.GetLength(1); j++)
            {
                print = string.Concat(print + gird[i, j] + ", ");
            }

            print = string.Concat(print + "]\n[ ");
        }

        Console.Write(print);
        Debug.Log(print);
    }
}
