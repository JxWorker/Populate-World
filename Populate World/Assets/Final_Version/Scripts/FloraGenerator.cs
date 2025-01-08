using Sirenix.OdinInspector;
using UnityEngine;

public class FloraGenerator : MonoBehaviour
{
    [TabGroup("Random Generation Parameter")]
    public int plantChance = 50;
    [TabGroup("Random Generation Parameter")]
    public int stoneChance = 50;
    [TabGroup("Random Generation Parameter")]
    public int snowmanChance = 20;


    [TabGroup("Forest Parameter")]
    public float amplitude_Forest = 0.6f;
    [TabGroup("Forest Parameter")]
    public float frequency_Forest = 0.03f;
    [TabGroup("Forest Parameter")]
    public int octaves_Forest = 3;
    [TabGroup("Forest Parameter")]
    public float lacunarity_Forest = 2.0f;
    [TabGroup("Forest Parameter")]
    public float persistence_Forest = 0.5f;
    [TabGroup("Forest Parameter")]
    public int seed_Forest = 0;

    public float[,] FloraGrid;
    public void GenerateFlora(float[,] world)
    {
        var forestValues = GenerateForestNoise(world);
        FloraGrid = GenerateRandomFlora(world, forestValues);
    }

    private float[,] GenerateForestNoise(float[,] world)
    {
        var width = world.GetLength(0);
        var height = world.GetLength(1);
        var grid = new float[width, height];
        seed_Forest = RandomSeed();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var elevation = 0f;
                var Frequency = frequency_Forest;
                var Amplitude = amplitude_Forest;

                for (int k = 0; k < octaves_Forest; k++)
                {
                    var x = i * Frequency + seed_Forest;
                    var y = j * Frequency + seed_Forest;


                    elevation += Mathf.PerlinNoise(x, y) * Amplitude;

                    Frequency *= lacunarity_Forest;
                    Amplitude *= persistence_Forest;
                }

                grid[i, j] = elevation;
            }
        }

        var tempGrid = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (world[x, y] == -1f)
                {
                    tempGrid[x, y] = 0f;
                    continue;
                }

                if (world[x, y] > 0.4f)
                {
                    tempGrid[x, y] = 0f;
                    continue;
                }

                if (grid[x, y] > 0.4f)
                {
                    tempGrid[x, y] = 0f;
                }
                if (grid[x, y] <= 0.4f)
                {
                    tempGrid[x, y] = 1f;
                }
            }
        }

        grid = tempGrid;


        var neighbourGrid = new float[width, height];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == 0f)
                {
                    continue;
                }

                neighbourGrid[x, y] = FirstOuterForestLayer(x, y, grid);
            }
        }

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == 0f)
                {
                    continue;
                }
                grid[x, y] = neighbourGrid[x, y];
            }
        }

        neighbourGrid = new float[width, height];

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == 0f)
                {
                    continue;
                }

                neighbourGrid[x, y] = SecondOuterForestLayer(x, y, grid);
            }
        }

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                if (grid[x, y] == 3f || grid[x, y] == 0f)
                {
                    continue;
                }
                grid[x, y] = neighbourGrid[x, y];
            }
        }

        return grid;
    }

    private float FirstOuterForestLayer(int xPosition, int yPosition, float[,] grid)
    {
        var neighbours = 0;
        float value = 1f;

        int[] xDirection = { -1, -1, -1, 0, 0, 1, 1, 1 };
        int[] yDirection = { -1, 0, 1, -1, 1, -1, 0, 1 };

        for (int dir = 0; dir < xDirection.Length; dir++)
        {
            var x = xPosition + xDirection[dir];
            var y = yPosition + yDirection[dir];

            if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1))
            {
                neighbours++;
                continue;
            }

            if (grid[x, y] == 0f)
            {
                neighbours++;
            }
        }

        if (neighbours >= 2)
        {
            value = 3.0f;
        }

        return value;
    }

    private float SecondOuterForestLayer(int xPosition, int yPosition, float[,] grid)
    {
        var neighbours = 0;
        var directionCount_NESWC = new int[5];
        float value = 1f;

        int[] xDirection = { -2, -2, -2, -2, -2, -1, -1, 0, 0, 1, 1, 2, 2, 2, 2, 2 };
        int[] yDirection = { -2, -1, 0, 1, 2, -2, 2, -2, 2, -2, 2, -2, -1, 0, 1, 2 };

        for (int dir = 0; dir < xDirection.Length; dir++)
        {
            var x = xPosition + xDirection[dir];
            var y = yPosition + yDirection[dir];

            if (x < 0 && y < 0)
            {
                directionCount_NESWC[4]++;
            }
            else if (x < 0 && y >= grid.GetLength(1))
            {
                directionCount_NESWC[4]++;
            }
            else if (x >= grid.GetLength(0) && y < 0)
            {
                directionCount_NESWC[4]++;
            }
            else if (x >= grid.GetLength(0) && y >= grid.GetLength(1))
            {
                directionCount_NESWC[4]++;
            }
            else if (x < 0)
            {
                directionCount_NESWC[3]++;
            }
            else if (x >= grid.GetLength(0))
            {
                directionCount_NESWC[1]++;
            }
            else if (y < 0)
            {
                directionCount_NESWC[2]++;
            }
            else if (y >= grid.GetLength(1))
            {
                directionCount_NESWC[0]++;
            }

            if (x < 0 || x >= grid.GetLength(0) || y < 0 || y >= grid.GetLength(1))
            {
                neighbours++;
                continue;
            }



            if (grid[x, y] == 0f)
            {
                neighbours++;
            }

            if (Mathf.Abs(xDirection[dir]) == Mathf.Abs(yDirection[dir]) && grid[x, y] == 0f)
            {
                directionCount_NESWC[4]++;
            }
            else if (yDirection[dir] == 2 && grid[x, y] == 0f)
            {
                directionCount_NESWC[0]++;
            }
            else if (xDirection[dir] == 2 && grid[x, y] == 0f)
            {
                directionCount_NESWC[1]++;
            }
            else if (yDirection[dir] == -2 && grid[x, y] == 0f)
            {
                directionCount_NESWC[2]++;
            }
            else if (xDirection[dir] == -2 && grid[x, y] == 0f)
            {
                directionCount_NESWC[3]++;
            }
        }

        if (neighbours <= 2)
        {
            return value;
        }

        var direction = CompareDirectionCount(directionCount_NESWC[3], directionCount_NESWC[2], directionCount_NESWC[0], directionCount_NESWC[1]);

        switch (direction.dir)
        {
            case "n":
                value = 2.1f;
                break;
            case "e":
                value = 2.2f;
                break;
            case "s":
                value = 2.3f;
                break;
            case "w":
                value = 2.0f;
                break;
            default:
                value = 1f;
                break;
        }

        return value;
    }

    private (string dir, int count) CompareDirectionCount(int xNegativ, int yNegativ, int yPositive, int xPositive)
    {
        if (xNegativ > xPositive && xNegativ > yPositive && xNegativ > yNegativ)
        {
            return ("w", xNegativ);
        }
        else if (yNegativ > yPositive && yNegativ > xPositive && yNegativ > xNegativ)
        {
            return ("s", yNegativ);
        }
        else if (xPositive > xNegativ && xPositive > yNegativ && xPositive > yPositive)
        {
            return ("e", xPositive);
        }
        else if (yPositive > yNegativ && yPositive > xNegativ && yPositive > xPositive)
        {
            return ("n", yPositive);
        }

        return ("0", 0);
    }

    private float[,] GenerateRandomFlora(float[,] world, float[,] forest)
    {
        var width = world.GetLength(0);
        var height = world.GetLength(1);

        var tempGrid = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (world[i, j] == -1f)
                {
                    tempGrid[i, j] = 0f;
                }
                else if (world[i, j] > 0.2f && world[i, j] <= 0.4f && forest[i, j] == 0f)
                {
                    tempGrid[i, j] = RandomPlant();
                }
                else if (world[i, j] > 0.4f && world[i, j] <= 0.5f)
                {
                    tempGrid[i, j] = RandomStone();
                }
                else if (world[i, j] > 0.5f)
                {
                    if (1 == Random.Range(1, snowmanChance))
                    {
                        tempGrid[i, j] = 9f;
                    }
                    else
                    {
                        tempGrid[i, j] = 0f;
                    }
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                switch (tempGrid[x, y])
                {
                    case 4.1f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                    case 4.2f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.1f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.2f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.3f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.4f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.5f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.6f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                    case 6.1f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                    case 6.2f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                    case 6.3f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                    case 9f:
                        forest[x, y] = tempGrid[x, y];
                        break;
                }
            }
        }

        return forest;
    }

    private float RandomPlant()
    {
        int plantType = Random.Range(1, plantChance);
        float value;

        switch (plantType)
        {
            case 1:
                int bushVariant = Random.Range(1, 7);

                value = bushVariant == 1 ? 5.1f : bushVariant == 2 ? 5.2f : bushVariant == 3 ? 5.3f : bushVariant == 4 ? 5.4f : bushVariant == 5 ? 5.5f : 5.6f;
                break;
            case 2:
                int flowerVariant = Random.Range(1, 4);

                value = flowerVariant == 1 ? 6.1f : flowerVariant == 2 ? 6.2f : 6.3f;
                break;
            default:
                value = 0f;
                break;
        }

        return value;
    }

    private float RandomStone()
    {
        int stoneVariant = Random.Range(1, stoneChance);
        float value;

        switch (stoneVariant)
        {
            case 1:
                value = 4.1f;
                break;
            case 2:
                value = 4.2f;
                break;
            default:
                value = 0f;
                break;
        }

        return value;
    }

    private int RandomSeed()
    {
        return Random.Range(-10000, 10000);
    }
}
