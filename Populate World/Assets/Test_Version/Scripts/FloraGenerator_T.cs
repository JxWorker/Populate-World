using System.IO;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloraGenerator_T : MonoBehaviour
{
    public float[,] floraGrid;
    private int width;
    private int height;
    [TabGroup("Debuging & Testing")]
    // [Header("DebugUI")]
    public Canvas canvas;
    [TabGroup("Debuging & Testing")]
    // [Header("Noise Map")]
    public Renderer textureRender;

    [TabGroup("Parameter Random Generation")]
    // [Header("Spawn Chance")]
    public int plantChance = 9;
    [TabGroup("Parameter Random Generation")]
    public int stoneChance = 9;
    [TabGroup("Parameter Random Generation")]
    public int snowmanChance = 9;

    [TabGroup("Parameter Forest Noise")]
    // [Header("Forest Noise Parameter")]
    public int width_Forest = 10;
    [TabGroup("Parameter Forest Noise")]
    public int height_Forest = 10;
    [TabGroup("Parameter Forest Noise")]
    public float amplitude_Forest = 0.4f;
    [TabGroup("Parameter Forest Noise")]
    public float frequency_Forest = 0.02f;
    [TabGroup("Parameter Forest Noise")]
    public int octaves_Forest = 3;
    [TabGroup("Parameter Forest Noise")]
    public float lacunarity_Forest = 2.0f;
    [TabGroup("Parameter Forest Noise")]
    public float persistence_Forest = 0.5f;
    [TabGroup("Parameter Forest Noise")]
    public bool randomSeed = false;
    [TabGroup("Parameter Forest Noise")]
    public int seed_Forest = 9574;

    [TabGroup("Parameter Forest Noise")]
    // [Space]
    // [InlineButton("ClearFloraMap")]
    // [InlineButton("GenerateForestNoise")]
    // [InlineButton("DrawForest_1")]
    // [InlineButton("DrawForest_2")]
    // [ShowInInspector]
    private float[,] generatedGrid_Forest;
    // [TabGroup("Parameter Randamon Generation")]
    // [InlineButton("ClearFloraMap")]
    // [InlineButton("GenerateFlora_1")]
    // [InlineButton("DrawFlora")]
    // [Header("Tiles")]

    private readonly int[] xDirection = { -2, -2, -2, -2, -2, -1, -1, -1, -1, -1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2 };
    private readonly int[] yDirection = { -2, -1, 0, 1, 2, -2, -1, 0, 1, 2, -2, -1, 1, 2, -2, -1, 0, 1, 2, -2, -1, 0, 1, 2 };

    [TabGroup("Tiles")]
    public Tilemap folraMap;

    [TabGroup("Tiles")]
    public Tile[] tree = new Tile[15];
    [TabGroup("Tiles")]
    public Tile[] bush = new Tile[6];
    [TabGroup("Tiles")]
    public Tile[] flower = new Tile[3];
    [TabGroup("Tiles")]
    public Tile[] stone = new Tile[2];
    [TabGroup("Tiles")]
    public Tile snowman;


    #region random gen
    [TabGroup("Parameter Random Generation")]
    [Button("Generate Flora 1")]
    public void GenerateFlora_1()
    {
        WorldGenerator_T world = FindAnyObjectByType<WorldGenerator_T>();
        world.GenerateNoise_LW();

        width = world.generatedGrid_LW.GetLength(0);
        height = world.generatedGrid_LW.GetLength(1);

        floraGrid = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (world.generatedGrid_LW[i, j] <= 0.2f)
                {
                    floraGrid[i, j] = 0f;
                }
                else if (world.generatedGrid_LW[i, j] > 0.2f && world.generatedGrid_LW[i, j] <= 0.4f)
                {
                    RandomPlant(i, j);
                }
                else if (world.generatedGrid_LW[i, j] > 0.4f && world.generatedGrid_LW[i, j] <= 0.5f)
                {
                    RandomStone(i, j);
                }
                else if (world.generatedGrid_LW[i, j] > 0.5f)
                {
                    if (1 == Random.Range(1, snowmanChance))
                    {
                        floraGrid[i, j] = 5f;
                    }
                    else
                    {
                        floraGrid[i, j] = 0f;
                    }
                }
            }
        }
    }

    private void RandomPlant(int x, int y)
    {
        int plantType = Random.Range(1, plantChance);

        switch (plantType)
        {
            case 1:
                int treeVariant = Random.Range(1, 4);

                floraGrid[x, y] = treeVariant == 1 ? 1.1f : treeVariant == 2 ? 1.2f : 1.3f;
                break;
            case 2:
                int bushVariant = Random.Range(1, 7);

                floraGrid[x, y] = bushVariant == 1 ? 2.1f : bushVariant == 2 ? 2.2f : bushVariant == 3 ? 2.3f : bushVariant == 4 ? 2.4f : bushVariant == 5 ? 2.5f : 2.6f;
                break;
            case 3:
                int flowerVariant = Random.Range(1, 4);

                floraGrid[x, y] = flowerVariant == 1 ? 3.1f : flowerVariant == 2 ? 3.2f : 3.3f;
                break;
            default:
                floraGrid[x, y] = 0f;
                break;
        }
    }

    private void RandomStone(int x, int y)
    {
        int stoneVariant = Random.Range(1, stoneChance);

        switch (stoneVariant)
        {
            case 1:
                floraGrid[x, y] = 4.1f;
                break;
            case 2:
                floraGrid[x, y] = 4.2f;
                break;
            default:
                floraGrid[x, y] = 0f;
                break;
        }
    }

    [TabGroup("Parameter Random Generation")]
    [Button("Draw Flora 1")]
    public void DrawFlora_1()
    {
        for (int i = 0; i < floraGrid.GetLength(0); i++)
        {
            for (int j = 0; j < floraGrid.GetLength(1); j++)
            {
                switch (floraGrid[i, j])
                {
                    case 1.1f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), tree[0]);
                        break;
                    case 1.2f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), tree[1]);
                        break;
                    case 1.3f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), tree[2]);
                        break;
                    case 2.1f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush[0]);
                        break;
                    case 2.2f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush[1]);
                        break;
                    case 2.3f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush[2]);
                        break;
                    case 2.4f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush[3]);
                        break;
                    case 2.5f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush[4]);
                        break;
                    case 2.6f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush[5]);
                        break;
                    case 3.1f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), flower[0]);
                        break;
                    case 3.2f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), flower[1]);
                        break;
                    case 3.3f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), flower[2]);
                        break;
                    case 4.1f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), stone[0]);
                        break;
                    case 4.2f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), stone[1]);
                        break;
                    case 5f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), snowman);
                        break;
                }
            }
        }
    }
    #endregion

    #region Perlin Forest
    [TabGroup("Parameter Forest Noise")]
    [Button("Generarte Forest Noise")]
    public void GenerateForestNoise()
    {
        if (randomSeed)
        {
            seed_Forest = RandomSeed();
        }

        var grid = new float[width_Forest, height_Forest];

        for (int i = 0; i < width_Forest; i++)
        {
            for (int j = 0; j < height_Forest; j++)
            {
                var elevation = 0f;
                var tempFrequency = frequency_Forest;
                var tempAmplitude = amplitude_Forest;

                for (int k = 0; k < octaves_Forest; k++)
                {
                    var x = i * tempFrequency + seed_Forest;
                    var y = j * tempFrequency + seed_Forest;


                    elevation += Mathf.PerlinNoise(x, y) * tempAmplitude;

                    tempFrequency *= lacunarity_Forest;
                    tempAmplitude *= persistence_Forest;
                }

                grid[i, j] = elevation;
            }
        }

        generatedGrid_Forest = grid;
    }

    [TabGroup("Parameter Forest Noise")]
    [Button("Draw Forest 1")]
    public void DrawForest_1()
    {
        for (int x = 0; x < generatedGrid_Forest.GetLength(0); x++)
        {
            for (int y = 0; y < generatedGrid_Forest.GetLength(1); y++)
            {
                if (generatedGrid_Forest[x, y] >= 0.45f)
                {
                    folraMap.SetTile(new Vector3Int(x, y, 1), tree[1]);
                }
                else if (generatedGrid_Forest[x, y] >= 0.4f)
                {
                    folraMap.SetTile(new Vector3Int(x, y, 1), tree[2]);
                }
            }
        }
    }

    [TabGroup("Parameter Forest Noise")]
    [Button("Draw Forest 2")]
    public void DrawForest_2()
    {
        for (int x = 0; x < generatedGrid_Forest.GetLength(0); x++)
        {
            for (int y = 0; y < generatedGrid_Forest.GetLength(1); y++)
            {
                if (generatedGrid_Forest[x, y] <= 0.36f)
                {
                    folraMap.SetTile(new Vector3Int(x, y, 1), tree[1]);
                }
                else if (generatedGrid_Forest[x, y] <= 0.4f)
                {
                    folraMap.SetTile(new Vector3Int(x, y, 1), tree[2]);
                }
            }
        }
    }

    [TabGroup("Parameter Forest Noise")]
    [Button("Draw Forest 3 Polished Gird")]
    public void DrawForest_3()
    {
        for (int x = 0; x < generatedGrid_Forest.GetLength(0); x++)
        {
            for (int y = 0; y < generatedGrid_Forest.GetLength(1); y++)
            {
                switch (generatedGrid_Forest[x, y])
                {
                    case 1f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[1]);
                        break;
                    case 2.0f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[9]);
                        break;
                    case 2.1f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[7]);
                        break;
                    case 2.2f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[10]);
                        break;
                    case 2.3f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[8]);
                        break;
                    case 3.0f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[2]);
                        break;
                    case 3.1f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[2]);
                        break;
                    case 3.2f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[2]);
                        break;
                    case 3.3f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[2]);
                        break;
                    case 9f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), snowman);
                        break;
                        // default:
                        // break;
                }
            }
        }
    }

    [TabGroup("Parameter Forest Noise")]
    [Button("Polish generated grid")]
    public void PolishGeneratetGrid()
    {
        var grid = new float[generatedGrid_Forest.GetLength(0), generatedGrid_Forest.GetLength(1)];

        for (int x = 0; x < generatedGrid_Forest.GetLength(0); x++)
        {
            for (int y = 0; y < generatedGrid_Forest.GetLength(1); y++)
            {
                if (generatedGrid_Forest[x, y] > 0.4f)
                {
                    grid[x, y] = 0f;
                }
                if (generatedGrid_Forest[x, y] <= 0.4f)
                {
                    grid[x, y] = 1f;
                }
            }
        }

        var neighbourGrid = new float[generatedGrid_Forest.GetLength(0), generatedGrid_Forest.GetLength(1)];

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

        PrintGrid(grid, "Noise");
        PrintGrid(neighbourGrid, "Neighbour-1");

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

        // -------------

        neighbourGrid = new float[generatedGrid_Forest.GetLength(0), generatedGrid_Forest.GetLength(1)];

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

        PrintGrid(grid, "Noise");
        PrintGrid(neighbourGrid, "Neighbour-2");

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

        generatedGrid_Forest = grid;
    }

    private float NeighboursAndDirection(int xPosition, int yPosition, float[,] grid)
    {
        var xNeighbours = new int[4];
        var yNeighbours = new int[4];

        for (int dir = 0; dir < xDirection.Length; dir++)
        {
            var x = xPosition + xDirection[dir];
            var y = yPosition + yDirection[dir];

            if (x < 0 || x >= generatedGrid_Forest.GetLength(0) || y < 0 || y >= generatedGrid_Forest.GetLength(1))
            {
                continue;
            }

            if (grid[x, y] == 1f)
            {
                continue;
            }

            switch (xDirection[dir])
            {
                case -2:
                    xNeighbours[0]++;
                    break;
                case -1:
                    xNeighbours[1]++;
                    break;
                case 1:
                    xNeighbours[2]++;
                    break;
                case 2:
                    xNeighbours[3]++;
                    break;
            }

            switch (yDirection[dir])
            {
                case -2:
                    yNeighbours[0]++;
                    break;
                case -1:
                    yNeighbours[1]++;
                    break;
                case 1:
                    yNeighbours[2]++;
                    break;
                case 2:
                    yNeighbours[3]++;
                    break;
            }
        }

        float value = 9;

        // if ((xNeighbours[1] + xNeighbours[2] + yNeighbours[1] + yNeighbours[2]) >= 10)
        // {
        //     value = 3.0f;
        // }

        // if (xNeighbours[0] + xNeighbours[1] + xNeighbours[2] + xNeighbours[3] + yNeighbours[0] + yNeighbours[1] + yNeighbours[1] + yNeighbours[3] >= 20)
        // {
        //     // generatedGrid_Forest[xPosition, yPosition] = 1f;
        //     return 1f;
        // }


        var innerNeighbourDirection = CompareDirectionCount(xNeighbours[1], yNeighbours[1], yNeighbours[2], xNeighbours[2]);
        var outerNeighbourDirection = CompareDirectionCount(xNeighbours[0], yNeighbours[0], yNeighbours[3], xNeighbours[3]);


        if (innerNeighbourDirection.count > 3)
        {
            switch (innerNeighbourDirection.dir)
            {
                case "w":
                    // generatedGrid_Forest[xPosition, yPosition] = 3.0f;
                    value = 3f;
                    break;
                case "s":
                    // generatedGrid_Forest[xPosition, yPosition] = 3.1f;
                    value = 3.1f;
                    break;
                case "e":
                    // generatedGrid_Forest[xPosition, yPosition] = 3.2f;
                    value = 3.2f;
                    break;
                case "n":
                    // generatedGrid_Forest[xPosition, yPosition] = 3.3f;
                    value = 3.3f;
                    break;
                    // default:
                    //     generatedGrid_Forest[xPosition, yPosition] = 9f;
                    //     break;
            }
        }
        else
        {
            switch (outerNeighbourDirection.dir)
            {
                case "e":
                    // generatedGrid_Forest[xPosition, yPosition] = 2.0f;
                    value = 2f;
                    break;
                case "s":
                    // generatedGrid_Forest[xPosition, yPosition] = 2.1f;
                    value = 2.1f;
                    break;
                case "w":
                    // generatedGrid_Forest[xPosition, yPosition] = 2.2f;
                    value = 2.2f;
                    break;
                case "n":
                    // generatedGrid_Forest[xPosition, yPosition] = 2.3f;
                    value = 2.3f;
                    break;
                    // default:
                    //     generatedGrid_Forest[xPosition, yPosition] = 9f;
                    //     break;
            }
        }


        return value;
        //TODO: Determin when a Forest-Corner-Tile needs to be set
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
                // continue;
            }
            else if (x < 0 && y >= grid.GetLength(1))
            {
                directionCount_NESWC[4]++;
                // continue;
            }
            else if (x >= grid.GetLength(0) && y < 0)
            {
                directionCount_NESWC[4]++;
                // continue;
            }
            else if (x >= grid.GetLength(0) && y >= grid.GetLength(1))
            {
                directionCount_NESWC[4]++;
                // continue;
            }
            else if (x < 0)
            {
                directionCount_NESWC[3]++;
                // continue;
            }
            else if (x >= grid.GetLength(0))
            {
                directionCount_NESWC[1]++;
                // continue;
            }
            else if (y < 0)
            {
                directionCount_NESWC[2]++;
                // continue;
            }
            else if (y >= grid.GetLength(1))
            {
                directionCount_NESWC[0]++;
                // continue;
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

    [TabGroup("Parameter Forest Noise")]
    [Button("Generarte Polished Forest Noise Depending on LW")]
    public void GeneratePolishedForestNoise_DependLW()
    {
        WorldGenerator_T world = FindAnyObjectByType<WorldGenerator_T>();
        world.GenerateNoise_LW();
        var width = world.generatedGrid_LW.GetLength(0);
        var height = world.generatedGrid_LW.GetLength(1);

        if (randomSeed)
        {
            seed_Forest = RandomSeed();
        }

        var grid = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var elevation = 0f;
                var tempFrequency = frequency_Forest;
                var tempAmplitude = amplitude_Forest;

                for (int k = 0; k < octaves_Forest; k++)
                {
                    var x = i * tempFrequency + seed_Forest;
                    var y = j * tempFrequency + seed_Forest;


                    elevation += Mathf.PerlinNoise(x, y) * tempAmplitude;

                    tempFrequency *= lacunarity_Forest;
                    tempAmplitude *= persistence_Forest;
                }

                grid[i, j] = elevation;
            }
        }

        var tempGrid = new float[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (world.generatedGrid_LW[x, y] == -1f)
                {
                    tempGrid[x, y] = 0f;
                    continue;
                }

                if (world.generatedGrid_LW[x, y] > 0.4f)
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

        generatedGrid_Forest = grid;
    }

    // ----------------------------------------

    [TabGroup("Parameter Forest Noise")]
    [Button("Generarte Forest Noise Depending on LW")]
    public void GenerateForestNoise_DependLW()
    {
        WorldGenerator_T world = FindAnyObjectByType<WorldGenerator_T>();
        world.GenerateNoise_LW();
        var width = world.generatedGrid_LW.GetLength(0);
        var height = world.generatedGrid_LW.GetLength(1);

        if (randomSeed)
        {
            seed_Forest = RandomSeed();
        }

        var grid = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var elevation = 0f;
                var tempFrequency = frequency_Forest;
                var tempAmplitude = amplitude_Forest;

                for (int k = 0; k < octaves_Forest; k++)
                {
                    var x = i * tempFrequency + seed_Forest;
                    var y = j * tempFrequency + seed_Forest;


                    elevation += Mathf.PerlinNoise(x, y) * tempAmplitude;

                    tempFrequency *= lacunarity_Forest;
                    tempAmplitude *= persistence_Forest;
                }

                grid[i, j] = elevation;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (world.generatedGrid_LW[x, y] == -1f)
                {
                    grid[x, y] = 0f;
                    continue;
                }

                if (grid[x, y] <= 0.36f && grid[x, y] > 0f)
                {
                    grid[x, y] = 1f;
                }
                else if (grid[x, y] <= 0.4f)
                {
                    grid[x, y] = 2f;
                }
                else
                {
                    grid[x, y] = 0f;
                }
            }
        }

        generatedGrid_Forest = grid;
    }

    [TabGroup("Parameter Forest Noise")]
    [Button("Draw Forest 4")]
    public void DrawForest_4()
    {
        for (int x = 0; x < generatedGrid_Forest.GetLength(0); x++)
        {
            for (int y = 0; y < generatedGrid_Forest.GetLength(1); y++)
            {
                if (generatedGrid_Forest[x, y] == 1f)
                {
                    folraMap.SetTile(new Vector3Int(x, y, 1), tree[1]);
                }
                else if (generatedGrid_Forest[x, y] == 2f)
                {
                    folraMap.SetTile(new Vector3Int(x, y, 1), tree[2]);
                }
            }
        }
    }
    #endregion

    [TabGroup("Parameter Random Generation")]
    [TabGroup("Parameter Forest Noise")]
    [TabGroup("Finale Verison")]
    [Button("Clear Floar Map")]
    public void ClearFloraMap()
    {
        folraMap.ClearAllTiles();
    }

    public int RandomSeed()
    {
        return Random.Range(-10000, 10000);
    }

    #region Debug & Test
    [TabGroup("Debuging & Testing")]
    [Button("Draw Nosie")]
    public void DrawNoiseMap()
    {
        int width = generatedGrid_Forest.GetLength(0);
        int height = generatedGrid_Forest.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colourMap = new Color[width * height];
        // for (int y = 0; y < height; y++)
        // {
        // 	for (int x = 0; x < width; x++)
        // 	{
        // 		colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, generatedGrid_Forest[x, y]);
        // 	}
        // }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                colourMap[x * height + y] = Color.Lerp(Color.black, Color.white, generatedGrid_Forest[x, y]);
            }
        }

        texture.SetPixels(colourMap);
        texture.Apply();

        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(width, 1, height);
    }

    [TabGroup("Debuging & Testing")]
    [Button("Draw Values")]
    public void DrawValues(/* float[,] grid, bool autoUpdate */)
    {
        // if ((grid.GetLength(0) > 15 && grid.GetLength(1) > 15) || autoUpdate == true)
        // {
        // 	return;
        // }

        for (int i = 0; i < generatedGrid_Forest.GetLength(0); i++)
        {
            for (int j = 0; j < generatedGrid_Forest.GetLength(1); j++)
            {
                GameObject textObject = new GameObject("Text" + i + j);
                textObject.transform.SetParent(canvas.transform);
                TextMeshPro textcomponent = textObject.AddComponent<TextMeshPro>();
                textcomponent.text = "" + Mathf.Round(generatedGrid_Forest[i, j] * 10000f) / 10000f;
                RectTransform rectTransform = textObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(i * 20, j * 20);
            }
        }
    }

    [TabGroup("Debuging & Testing")]
    [Button("Print Values")]
    public void PrintGrid()
    {
        string docPath = "/home/michel/Repos/HHN/Game Engineering/Populate-World/Populate World/Assets/DebugFiles/DebugFlora.txt";

        string print = "[ ";

        for (int i = 0; i < generatedGrid_Forest.GetLength(0); i++)
        {
            for (int j = 0; j < generatedGrid_Forest.GetLength(1); j++)
            {
                print = string.Concat(print + generatedGrid_Forest[i, j] + ", ");
            }

            print = string.Concat(print + "]\n[ ");
        }

        Debug.Log(print);

        using (StreamWriter outputFile = new StreamWriter(docPath))
        {
            outputFile.Write(print);
        }
    }

    private void PrintGrid(float[,] grid, string name)
    {
        string docPath = $"/home/michel/Repos/HHN/Game Engineering/Populate-World/Populate World/Assets/DebugFiles/DebugFlora_{name}.txt";

        string print = "[ ";

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                print = string.Concat(print + grid[i, j] + ", ");
            }

            print = string.Concat(print + "]\n[ ");
        }

        using (StreamWriter outputFile = new StreamWriter(docPath))
        {
            outputFile.Write(print);
        }
    }
    #endregion

    #region FinalVersion
    [TabGroup("Finale Verison")]
    [Button("Generate and Draw World and Flora")]
    public void FinaleFlora()
    {
        WorldGenerator_T world = FindAnyObjectByType<WorldGenerator_T>();
        world.GenerateNoise_LW();

        FinaleGenerateForestNoise(world.generatedGrid_LW);
        FinaleGenerateFlora(world.generatedGrid_LW);

        NoiseDisplay_T display = FindAnyObjectByType<NoiseDisplay_T>();
        display.DrawTiles_LW_1(world.generatedGrid_LW);
        FinaleDrawFlora();
    }

    private void FinaleGenerateForestNoise(float[,] world)
    {
        var width = world.GetLength(0);
        var height = world.GetLength(1);

        if (randomSeed)
        {
            seed_Forest = RandomSeed();
        }

        var grid = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var elevation = 0f;
                var tempFrequency = frequency_Forest;
                var tempAmplitude = amplitude_Forest;

                for (int k = 0; k < octaves_Forest; k++)
                {
                    var x = i * tempFrequency + seed_Forest;
                    var y = j * tempFrequency + seed_Forest;


                    elevation += Mathf.PerlinNoise(x, y) * tempAmplitude;

                    tempFrequency *= lacunarity_Forest;
                    tempAmplitude *= persistence_Forest;
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

        generatedGrid_Forest = grid;
    }

    private void FinaleGenerateFlora(float[,] world)
    {
        width = world.GetLength(0);
        height = world.GetLength(1);

        var tempGrid = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (world[i, j] == -1f)
                {
                    tempGrid[i, j] = 0f;
                }
                else if (world[i, j] > 0.2f && world[i, j] <= 0.4f && generatedGrid_Forest[i, j] == 0f)
                {
                    tempGrid[i, j] = FinaleRandomPlant(i, j);
                }
                else if (world[i, j] > 0.4f && world[i, j] <= 0.5f)
                {
                    tempGrid[i, j] = FinaleRandomStone(i, j);
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
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                    case 4.2f:
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.1f:
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.2f:
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.3f:
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.4f:
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.5f:
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                    case 5.6f:
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                    case 6.1f:
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                    case 6.2f:
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                    case 6.3f:
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                    case 9f:
                        generatedGrid_Forest[x, y] = tempGrid[x, y];
                        break;
                }
            }
        }
    }

    private float FinaleRandomPlant(int x, int y)
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

    private float FinaleRandomStone(int x, int y)
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

    private void FinaleDrawFlora()
    {
        for (int x = 0; x < generatedGrid_Forest.GetLength(0); x++)
        {
            for (int y = 0; y < generatedGrid_Forest.GetLength(1); y++)
            {
                switch (generatedGrid_Forest[x, y])
                {
                    case 1f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[1]);
                        break;
                    case 2.0f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[9]);
                        break;
                    case 2.1f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[7]);
                        break;
                    case 2.2f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[10]);
                        break;
                    case 2.3f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[8]);
                        break;
                    case 3.0f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[2]);
                        break;
                    case 3.1f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[2]);
                        break;
                    case 3.2f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[2]);
                        break;
                    case 3.3f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), tree[2]);
                        break;
                    case 4.1f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), stone[0]);
                        break;
                    case 4.2f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), stone[1]);
                        break;
                    case 5.1f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), bush[0]);
                        break;
                    case 5.2f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), bush[1]);
                        break;
                    case 5.3f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), bush[2]);
                        break;
                    case 5.4f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), bush[3]);
                        break;
                    case 5.5f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), bush[4]);
                        break;
                    case 5.6f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), bush[5]);
                        break;
                    case 6.1f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), flower[0]);
                        break;
                    case 6.2f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), flower[1]);
                        break;
                    case 6.3f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), flower[2]);
                        break;
                    case 9f:
                        folraMap.SetTile(new Vector3Int(x, y, 1), snowman);
                        break;
                }
            }
        }
    }

    #endregion
}
