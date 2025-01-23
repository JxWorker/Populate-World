using Sirenix.OdinInspector;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [TabGroup("Terrain Parameter")]
    public float amplitude_Terrain = 0.4f;
    [TabGroup("Terrain Parameter")]
    public float frequency_Terrain = 0.02f;
    [TabGroup("Terrain Parameter")]
    public int octaves_Terrain = 3;
    [TabGroup("Terrain Parameter")]
    public float lacunarity_Terrain = 2.0f;
    [TabGroup("Terrain Parameter")]
    public float persistence_Terrain = 0.5f;
    [TabGroup("Terrain Parameter")]
    public int seed_Terrain = 0;


    [TabGroup("Water Parameter")]
    public float amplitude_Water = 0.43f;
    [TabGroup("Water Parameter")]
    public float frequency_Water = 0.005f;
    [TabGroup("Water Parameter")]
    public int octaves_Water = 3;
    [TabGroup("Water Parameter")]
    public float lacunarity_Water = 2.0f;
    [TabGroup("Water Parameter")]
    public float persistence_Water = 0.5f;
    [TabGroup("Water Parameter")]
    public int seed_Water = 0;


    public float[,] WorldGrid;
    private float[,] TerrainGrid;
    private float[,] WaterGird;

    public void GenerateWorldNosie(int worldSize)
    {
        GenerateTerrainNoise(worldSize);
        GenerateWaterNoise(worldSize);

        var grid = new float[worldSize, worldSize];

        for (int i = 0; i < WaterGird.GetLength(0); i++)
        {
            for (int j = 0; j < WaterGird.GetLength(1); j++)
            {
                if (WaterGird[i, j] >= 0f && WaterGird[i, j] <= 0.3f)
                {
                    grid[i, j] = -1f;
                }
                else
                {
                    grid[i, j] = TerrainGrid[i, j];
                }
            }
        }

        WorldGrid = grid;
    }

    private void GenerateTerrainNoise(int worldSize)
    {
        seed_Terrain = RandomSeed();
        var grid = new float[worldSize, worldSize];

        for (int i = 0; i < worldSize; i++)
        {
            for (int j = 0; j < worldSize; j++)
            {
                var elevation = 0f;
                var Frequency = frequency_Terrain;
                var Amplitude = amplitude_Terrain;

                for (int k = 0; k < octaves_Terrain; k++)
                {
                    var x = i * Frequency + seed_Terrain;
                    var y = j * Frequency + seed_Terrain;

                    elevation += Mathf.PerlinNoise(x, y) * Amplitude;

                    Frequency *= lacunarity_Terrain;
                    Amplitude *= persistence_Terrain;
                }

                grid[i, j] = elevation;
            }
        }

        TerrainGrid = grid;
    }

    private void GenerateWaterNoise(int worldSize)
    {
        seed_Water = RandomSeed();
        var grid = new float[worldSize, worldSize];

        for (int i = 0; i < worldSize; i++)
        {
            for (int j = 0; j < worldSize; j++)
            {
                var elevation = 0f;
                var Frequency = frequency_Water;
                var Amplitude = amplitude_Water;

                for (int k = 0; k < octaves_Water; k++)
                {
                    var x = i * Frequency + seed_Water;
                    var y = j * Frequency + seed_Water;

                    elevation += Mathf.PerlinNoise(x, y) * Amplitude;

                    Frequency *= lacunarity_Water;
                    Amplitude *= persistence_Water;
                }

                grid[i, j] = elevation;
            }
        }

        WaterGird = grid;
    }

    private int RandomSeed()
    {
        return Random.Range(-10000, 10000);
    }
}
