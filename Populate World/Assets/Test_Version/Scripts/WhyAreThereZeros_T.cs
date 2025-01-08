using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WhyAreThereZeros_T : MonoBehaviour
{
    [TabGroup("Tile")]
	public Tilemap Grid_Map01_02;
    [TabGroup("Tile")]
	public Tilemap Grid_Map02_03;
    [TabGroup("Tile")]
	public Tilemap Grid_Map03_04;
    [TabGroup("Tile")]
	public Tilemap Grid_Map04_05;
    [TabGroup("Tile")]
	public Tilemap Grid_Map05_06;

    [TabGroup("Tile")]
	public RuleTile Water;
    [TabGroup("Tile")]
	public RuleTile DirtGrass;
    [TabGroup("Tile")]
	public RuleTile DirtDirt;
    [TabGroup("Tile")]
	public RuleTile DirtSnow;


    
    [TabGroup("Chunk Loader")]
    public int width = 0;
    [TabGroup("Chunk Loader")]
    public int height = 0;
    [TabGroup("Chunk Loader")]
    public float amplitude = 0.4f;
    [TabGroup("Chunk Loader")]
    public float frequency = 0.02f;
    [TabGroup("Chunk Loader")]
    public int octaves = 3;
    [TabGroup("Chunk Loader")]
    public float lacunarity = 2.0f;
    [TabGroup("Chunk Loader")]
    public float persistence = 0.5f;
    [TabGroup("Chunk Loader")]
    public int seed = 1;

    [TabGroup("Chunk Loader")]
    public Chunk_T[,] chunkGrid;
    [TabGroup("Chunk Loader")]
    public int chunkSize = 16;
    [TabGroup("Chunk Loader")]
    public int chunkMultiplier = 101;
    [TabGroup("Chunk Loader")]
    public int chunkIteration = 0;
    [TabGroup("Chunk Loader")]
    public int chunkIterationIncrease = 4;
    [TabGroup("Chunk Loader")]
    public float[,] generatedGrid_For_ChunkSpliter;
    [TabGroup("Chunk Loader")]
    public int gridMiddleChunk;


    public int RandomSeed()
    {
        return Random.Range(-10000, 10000);
    }

    [TabGroup("Chunk Loader")]
    public void GenerateNoise_For_ChunkSpliter()
    {
        seed = RandomSeed();
        generatedGrid_For_ChunkSpliter = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var elevation = 0f;
                var tempFrequency = frequency;
                var tempAmplitude = amplitude;

                for (int k = 0; k < octaves; k++)
                {
                    var x = i * tempFrequency + seed;
                    var y = j * tempFrequency + seed;

                    elevation += Mathf.PerlinNoise(x, y) * tempAmplitude;

                    tempFrequency *= lacunarity;
                    tempAmplitude *= persistence;
                }

                generatedGrid_For_ChunkSpliter[i, j] = elevation;
            }
        }
    }

    [TabGroup("Chunk Loader")]
    [Button("Split")]
    public void SplitGridInChunks(){
        chunkGrid = new Chunk_T[chunkMultiplier, chunkMultiplier];

        width = chunkSize * chunkMultiplier;
        height = width;

        GenerateNoise_For_ChunkSpliter();

        for (int xCM = 0; xCM < chunkMultiplier; xCM++)
        {
            for (int yCM = 0; yCM < chunkMultiplier; yCM++)
            {
                chunkGrid[xCM, yCM] = new Chunk_T(xCM, yCM);
                
                for (int xCS = 0; xCS < chunkSize; xCS++)
                {
                    for (int yCS = 0; yCS < chunkSize; yCS++)
                    {
                        var x = (xCM * 16) + xCS;
                        var y = (yCM * 16) + yCS;
                        
                        if (x < 0 || y < 0 || x > chunkMultiplier*chunkSize || y > chunkMultiplier*chunkSize)
                        {
                            continue;
                        }

                        chunkGrid[xCM, yCM].TileValues[xCS, yCS] = generatedGrid_For_ChunkSpliter[x, y];
                    }
                }
            }
        }
    }
    
    [TabGroup("Chunk Loader")]
    public void DrawChunkTiles(Chunk_T chunk, Tilemap Map01_02, Tilemap Map02_03, Tilemap Map03_04, Tilemap Map04_05, Tilemap Map05_06)
	{
        var grid = chunk.TileValues;

		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
                var xCoordinate = x + chunk.XCoordinate * chunkSize;
                var yCoordinate = y + chunk.YCoordinate * chunkSize;
                
				if (grid[x, y] >= 0f && grid[x, y] <= 0.2f)
				{
					Map01_02.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Water);
				}
				else if (grid[x, y] > 0.2f && grid[x, y] <= 0.3f)
				{
					Map02_03.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtGrass);
				}
				else if (grid[x, y] > 0.3f && grid[x, y] <= 0.4f)
				{
					Map03_04.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtGrass);
				}
				else if (grid[x, y] > 0.4f && grid[x, y] <= 0.5f)
				{
					Map04_05.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtDirt);
				}
				else if (grid[x, y] > 0.5f)
				{
					Map05_06.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtSnow);
				}
			}
		}
	}

    [TabGroup("Chunk Loader")]
    [Button("Load")]
    public void LoadChunk_1(){
        int[] xDirection = { 1, -1, -1,  1};
        int[] yDirection = {-1, -1,  1,  1};

        gridMiddleChunk = (chunkMultiplier-1) / 2;

        if (chunkIteration == 0)
        {
            DrawChunkTiles(chunkGrid[gridMiddleChunk, gridMiddleChunk], Grid_Map01_02, Grid_Map02_03, Grid_Map03_04, Grid_Map04_05, Grid_Map05_06);
        }

        var x = gridMiddleChunk;
        var y = gridMiddleChunk + chunkIteration/chunkIterationIncrease;
        var directionPosition = 0;
        var directionCount = 0;
        var countOfDirectionPosition = chunkIteration / chunkIterationIncrease;

        for (int i = 0; i < chunkIteration; i++)
        {
            if (directionCount == countOfDirectionPosition)
            {
                directionPosition++;
                directionCount = 0;
            }

            x += xDirection[directionPosition];
            y += yDirection[directionPosition];
            directionCount++;

            if (x < 0 || y < 0 || x >= chunkMultiplier || y >= chunkMultiplier)
            {
                continue;
            }

            DrawChunkTiles(chunkGrid[x,y], Grid_Map01_02, Grid_Map02_03, Grid_Map03_04, Grid_Map04_05, Grid_Map05_06);
        }

        chunkIteration += chunkIterationIncrease;
    }

    [TabGroup("Chunk Loader")]
    [Button("Clear")]
    public void ClearTilemaps()
	{
		Grid_Map01_02.ClearAllTiles();
		Grid_Map02_03.ClearAllTiles();
		Grid_Map03_04.ClearAllTiles();
		Grid_Map04_05.ClearAllTiles();
		Grid_Map05_06.ClearAllTiles();

        chunkIteration = 0;
        gridMiddleChunk = 0;
	}


    private void PrintGrid(float[,] grid, string name)
    {
        string docPath = $"/home/michel/Repos/HHN/Game Engineering/Populate-World/Populate World/Assets/Test_Version/DebugFiles/Debug_{name}.txt";

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

    private void PrintGrid(Chunk_T[,] grid, string name)
    {
        string docPath = $"/home/michel/Repos/HHN/Game Engineering/Populate-World/Populate World/Assets/Test_Version/DebugFiles/Debug_{name}.txt";

        string print = "";

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                print = string.Concat(print + "Chunk: " + grid[i,j].XCoordinate + " | " + grid[i,j].YCoordinate + " \n");

                print = string.Concat(print + grid[i,j].ToString());
            }

            print = string.Concat(print + "\n");
        }

        using (StreamWriter outputFile = new StreamWriter(docPath))
        {
            outputFile.Write(print);
        }
    }

    [TabGroup("Debug")]
    [Button("Print Grid")]
    public void PrintGridButton(){
        string docPath = $"/home/michel/Repos/HHN/Game Engineering/Populate-World/Populate World/Assets/Test_Version/DebugFiles/Debug_GridForSpliting_Button.txt";

        string print = "[ ";

        for (int i = 0; i < generatedGrid_For_ChunkSpliter.GetLength(0); i++)
        {
            for (int j = 0; j < generatedGrid_For_ChunkSpliter.GetLength(1); j++)
            {
                print = string.Concat(print + generatedGrid_For_ChunkSpliter[i, j] + ", ");
            }

            print = string.Concat(print + "]\n[ ");
        }

        using (StreamWriter outputFile = new StreamWriter(docPath))
        {
            outputFile.Write(print);
        }
    }

    [TabGroup("Debug")]
    [Button("Print Chunks")]
    public void PrintChunksButton(){
        string docPath = $"/home/michel/Repos/HHN/Game Engineering/Populate-World/Populate World/Assets/Test_Version/DebugFiles/Debug_ChunkArray_Button.txt";

        string print = " ";

        for (int i = 0; i < chunkGrid.GetLength(0); i++)
        {
            for (int j = 0; j < chunkGrid.GetLength(1); j++)
            {
                print = string.Concat(print + chunkGrid[i,j].XCoordinate + ", " + chunkGrid[i,j].YCoordinate + "\n " + chunkGrid[i, j].ToString() + "\n ");
            }
        }

        using (StreamWriter outputFile = new StreamWriter(docPath))
        {
            outputFile.Write(print);
        }
    }
}
