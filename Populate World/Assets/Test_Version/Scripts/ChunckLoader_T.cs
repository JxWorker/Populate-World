using System.IO;
using Sirenix.OdinInspector;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChunckLoader_T : MonoBehaviour
{
	[TabGroup("Offset Test", "Draw")]
	public Renderer textureRender_1;
    [TabGroup("Offset Test", "Draw")]
    public Renderer textureRender_2;

    [TabGroup("Offset Test", "Draw")]
	public void DrawNoiseMap(float[,] noiseMap, Renderer textureRender)
	{
		int width = noiseMap.GetLength(0);
		int height = noiseMap.GetLength(1);

		Texture2D texture = new Texture2D(width, height);

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				colourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
			}
		}
		texture.SetPixels(colourMap);
		texture.Apply();

		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3(width, 1, height);
	}

    [TabGroup("Offset Test", "Draw")]
	public Tilemap Grid_Map01_02;
    [TabGroup("Offset Test", "Draw")]
	public Tilemap Grid_Map02_03;
    [TabGroup("Offset Test", "Draw")]
	public Tilemap Grid_Map03_04;
    [TabGroup("Offset Test", "Draw")]
	public Tilemap Grid_Map04_05;
    [TabGroup("Offset Test", "Draw")]
	public Tilemap Grid_Map05_06;

    [TabGroup("Offset Test", "Draw")]
	public Tilemap Test_Map01_02;
    [TabGroup("Offset Test", "Draw")]
	public Tilemap Test_Map02_03;
    [TabGroup("Offset Test", "Draw")]
	public Tilemap Test_Map03_04;
    [TabGroup("Offset Test", "Draw")]
	public Tilemap Test_Map04_05;
    [TabGroup("Offset Test", "Draw")]
	public Tilemap Test_Map05_06;

    [TabGroup("Offset Test", "Draw")]
	public RuleTile Water;
    [TabGroup("Offset Test", "Draw")]
	public RuleTile DirtGrass;
    [TabGroup("Offset Test", "Draw")]
	public RuleTile DirtDirt;
    [TabGroup("Offset Test", "Draw")]
	public RuleTile DirtSnow;
    public void DrawTiles(float[,] grid, Tilemap Map01_02, Tilemap Map02_03, Tilemap Map03_04, Tilemap Map04_05, Tilemap Map05_06)
	{
		ClearTilemaps(Map01_02, Map02_03, Map03_04, Map04_05, Map05_06);

		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i, j] >= 0f && grid[i, j] <= 0.2f)
				{
					Map01_02.SetTile(new Vector3Int(i, j, 0), Water);
				}
				else if (grid[i, j] > 0.2f && grid[i, j] <= 0.3f)
				{
					Map02_03.SetTile(new Vector3Int(i, j, 0), DirtGrass);
				}
				else if (grid[i, j] > 0.3f && grid[i, j] <= 0.4f)
				{
					Map03_04.SetTile(new Vector3Int(i, j, 0), DirtGrass);
				}
				else if (grid[i, j] > 0.4f && grid[i, j] <= 0.5f)
				{
					Map04_05.SetTile(new Vector3Int(i, j, 0), DirtDirt);
				}
				else if (grid[i, j] > 0.5f)
				{
					Map05_06.SetTile(new Vector3Int(i, j, 0), DirtSnow);
				}
			}
		}
	}

    public void ClearTilemaps(Tilemap Map01_02, Tilemap Map02_03, Tilemap Map03_04, Tilemap Map04_05, Tilemap Map05_06)
	{
		Map01_02.ClearAllTiles();
		Map02_03.ClearAllTiles();
		Map03_04.ClearAllTiles();
		Map04_05.ClearAllTiles();
		Map05_06.ClearAllTiles();
	}

    [TabGroup("Offset Test", "Generate")]
    public bool randomSeed;
    [TabGroup("Offset Test", "Generate")]
    public float[,] generatedGrid_With_Offset;
    [TabGroup("Offset Test", "Generate")]
    public float[,] generatedGrid_Without_Offset;
    [TabGroup("Offset Test", "Generate")]
    public int width = 10;
    [TabGroup("Offset Test", "Generate")]
    public int height = 10;
    [TabGroup("Offset Test", "Generate")]
    public float amplitude = 0.4f;
    [TabGroup("Offset Test", "Generate")]
    public float frequency = 0.02f;
    [TabGroup("Offset Test", "Generate")]
    public int octaves = 3;
    [TabGroup("Offset Test", "Generate")]
    public float lacunarity = 2.0f;
    [TabGroup("Offset Test", "Generate")]
    public float persistence = 0.5f;
    [TabGroup("Offset Test", "Generate")]
    public int seed = 1;
    [TabGroup("Offset Test", "Generate")]
    public int spawn_offset_x = 0;
    [TabGroup("Offset Test", "Generate")]
    public int spawn_offset_y = 0;

    public void GenerateNoise_With_Offset()
    {
        if (randomSeed)
        {
            seed = RandomSeed();
        }

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
                    var x = i * tempFrequency + seed / spawn_offset_x;
                    var y = j * tempFrequency + seed / spawn_offset_y;

                    elevation += Mathf.PerlinNoise(x, y) * tempAmplitude;

                    tempFrequency *= lacunarity;
                    tempAmplitude *= persistence;
                }

                grid[i, j] = elevation;
            }
        }

        generatedGrid_With_Offset = grid;

        // DrawNoiseMap(generatedGrid_With_Offset, textureRender_1);
    }

    public void GenerateNoise_Without_Offset()
    {
        int width = this.width*4;
        int height = this.height*4;

        if (randomSeed)
        {
            seed = RandomSeed();
        }

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
                    var x = i * tempFrequency + seed;
                    var y = j * tempFrequency + seed;

                    elevation += Mathf.PerlinNoise(x, y) * tempAmplitude;

                    tempFrequency *= lacunarity;
                    tempAmplitude *= persistence;
                }

                grid[i, j] = elevation;
            }
        }

        generatedGrid_Without_Offset = grid;

        // DrawNoiseMap(generatedGrid_Without_Offset, textureRender_2);
    }

    public int RandomSeed()
    {
        return Random.Range(-10000, 10000);
    }

    [TabGroup("Offset Test", "Draw")]
    [Button("Generate and Draw Texture")]
    public void GenerateAndDraw(){
        GenerateNoise_With_Offset();
        GenerateNoise_Without_Offset();
    }

    [TabGroup("Offset Test", "Draw")]
    [Button("Generate and Draw Tiles")]
    public void GenerateAndDraw_2(){
        GenerateNoise_With_Offset();
        GenerateNoise_Without_Offset();

        DrawTiles(generatedGrid_With_Offset, Grid_Map01_02, Grid_Map02_03, Grid_Map03_04, Grid_Map04_05, Grid_Map05_06);
        DrawTiles(generatedGrid_Without_Offset, Test_Map01_02, Test_Map02_03, Test_Map03_04, Test_Map04_05, Test_Map05_06);
    }

    // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    [TabGroup("Chunk Loader", "Loader 1")]
    public Chunk_T[,] chunkGrid;
    [TabGroup("Chunk Loader", "Loader 1")]
    public int chunkSize = 16;
    [TabGroup("Chunk Loader", "Loader 1")]
    public int chunkMultiplier = 1001;
    [TabGroup("Chunk Loader", "Loader 1")]
    public int chunkIteration = 0;
    [TabGroup("Chunk Loader", "Loader 1")]
    public int chunkIterationIncrease = 4;
    [TabGroup("Chunk Loader", "Loader 1")]
    public float[,] generatedGrid_For_ChunkSpliter;
    [TabGroup("Chunk Loader", "Loader 1")]
    public int gridMiddleChunk;

    [TabGroup("Chunk Loader", "Loader 1")]
    [Button("Split Gird in Chunks")]
    public void SplitGridInChunks(){
        chunkGrid = new Chunk_T[chunkMultiplier, chunkMultiplier];

        width = chunkSize * chunkMultiplier;
        height = width;

        GenerateNoise_For_ChunkSpliter();

        // float[,] grid = new float[chunkSize, chunkSize];

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
                        
                        if (x < 0 || y < 0 || x >= chunkMultiplier || y >= chunkMultiplier)
                        {
                            continue;
                        }

                        // grid[xCS,yCS] = generatedGrid_For_ChunkSpliter[x, y];
                        Debug.Log("X: " + xCM + "\nY: " + yCM);
                        Debug.Log("Grid Value: " + generatedGrid_For_ChunkSpliter[x,y]);
                        // Debug.Log("Chunk Value: " + grid[xCS,yCS]);

                        chunkGrid[xCM, yCM].TileValues[xCS, yCS] = generatedGrid_For_ChunkSpliter[x, y];

                        Debug.Log("Chunk Value: " + chunkGrid[xCM, yCM].TileValues[xCS, yCS]);
                    }
                }


                // grid = new float[chunkSize, chunkSize];
            }
        }

        // PrintGrid(generatedGrid_For_ChunkSpliter, "GridForSpliting");
        // PrintGrid(chunkGrid, "Chunks");
    }

    #region Load Chunk Methods
    [TabGroup("Chunk Loader", "Loader 1")]
    [Button("Load Chunk")]
    public void LoadChunk(){
        int[] xDirection = { 1, -1, -1,  1};
        int[] yDirection = {-1, -1,  1,  1};

        gridMiddleChunk = (chunkMultiplier-1) / 2;

        if (chunkIteration == 0)
        {
            DrawChunkTiles(chunkGrid[gridMiddleChunk, gridMiddleChunk], Grid_Map01_02, Grid_Map02_03, Grid_Map03_04, Grid_Map04_05, Grid_Map05_06);
        }

        for (int i = 0; i < chunkIteration; i++)
        {
            var directionPosition = i % chunkIterationIncrease;
            var directionMultiplier = chunkIteration / chunkIterationIncrease;

            var x = gridMiddleChunk + xDirection[directionPosition] * directionMultiplier;
            var y = gridMiddleChunk + yDirection[directionPosition] * directionMultiplier;

            if (x < 0 || y < 0 || x > chunkMultiplier || y > chunkMultiplier)
            {
                continue;
            }

            DrawChunkTiles(chunkGrid[x,y], Grid_Map01_02, Grid_Map02_03, Grid_Map03_04, Grid_Map04_05, Grid_Map05_06);
        }

        chunkIteration += chunkIterationIncrease;
    }

    [TabGroup("Chunk Loader", "Loader 1")]
    [Button("Load Chunk 1")]
    public void LoadChunk_1(){
        int[] xDirection = { 1, -1, -1,  1};
        int[] yDirection = {-1, -1,  1,  1};

        gridMiddleChunk = (chunkMultiplier-1) / 2;
        Debug.Log("Grid Middle Chunk: " + gridMiddleChunk);

        if (chunkIteration == 0)
        {
            Debug.Log("Chunk in Loader: \n" + chunkGrid[gridMiddleChunk, gridMiddleChunk].ToString());
            DrawChunkTiles(chunkGrid[gridMiddleChunk, gridMiddleChunk], Grid_Map01_02, Grid_Map02_03, Grid_Map03_04, Grid_Map04_05, Grid_Map05_06);
        }

        var x = gridMiddleChunk;
        var y = gridMiddleChunk + chunkIteration/chunkIterationIncrease;
        // Debug.Log("X Start: " + x);
        // Debug.Log("Y Start: " + y);
        // Debug.Log("-------------------------------");
        // var directionMultiplier = chunkIteration / chunkIterationIncrease;
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
            // var directionPosition = i % chunkIterationIncrease;

            // x += xDirection[directionPosition] * directionMultiplier;
            // y += yDirection[directionPosition] * directionMultiplier;

            x += xDirection[directionPosition];
            y += yDirection[directionPosition];
            directionCount++;
            // Debug.Log("X: " + x);
            // Debug.Log("Y: " + y);
            // Debug.Log("~~~~~~~~~~~~~~");

            if (x < 0 || y < 0 || x >= chunkMultiplier || y >= chunkMultiplier)
            {
                continue;
            }

            Debug.Log("Chunk in Loader: \n" + chunkGrid[x, y].ToString());

            DrawChunkTiles(chunkGrid[x,y], Grid_Map01_02, Grid_Map02_03, Grid_Map03_04, Grid_Map04_05, Grid_Map05_06);
        }

        chunkIteration += chunkIterationIncrease;
    }
    #endregion

    [TabGroup("Chunk Loader", "Loader 1")]
    public void DrawChunkTiles(Chunk_T chunk, Tilemap Map01_02, Tilemap Map02_03, Tilemap Map03_04, Tilemap Map04_05, Tilemap Map05_06)
	{
        var grid = chunk.TileValues;

		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
                var xCoordinate = x + chunk.XCoordinate * chunkSize;
                var yCoordinate = y + chunk.YCoordinate * chunkSize;

                // Debug.Log("Chunk x: " + chunk.XCoordinate);
                // Debug.Log("Chunk y: " + chunk.YCoordinate);
                // Debug.Log("Grid width: " + grid.GetLength(0));
                // Debug.Log("Grid height: " + grid.GetLength(1));
                // Debug.Log("x: " + xCoordinate);
                // Debug.Log("y: " + yCoordinate);
                // Debug.Log("Chunk Value: " + grid[x,y]);
                // Debug.Log("Chunk Value: " + chunk.TileValues[x,y]);
                
				if (grid[x, y] >= 0f && grid[x, y] <= 0.2f)
				{
                    // Debug.Log("Set Water");
					Grid_Map01_02.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Water);
				}
				else if (grid[x, y] > 0.2f && grid[x, y] <= 0.3f)
				{
                    // Debug.Log("Set Grass");
					Grid_Map02_03.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtGrass);
				}
				else if (grid[x, y] > 0.3f && grid[x, y] <= 0.4f)
				{
                    // Debug.Log("Set Grass");
					Grid_Map03_04.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtGrass);
				}
				else if (grid[x, y] > 0.4f && grid[x, y] <= 0.5f)
				{
                    // Debug.Log("Set Stone");
					Grid_Map04_05.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtDirt);
				}
				else if (grid[x, y] > 0.5f)
				{
                    // Debug.Log("Set Snow");
					Grid_Map05_06.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtSnow);
				}
			}
		}
	}

    [TabGroup("Chunk Loader", "Loader 1")]
    public void GenerateNoise_For_ChunkSpliter()
    {
        if (randomSeed)
        {
            seed = RandomSeed();
        }

        // var grid = new float[width, height];
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

                // grid[i, j] = elevation;
                generatedGrid_For_ChunkSpliter[i, j] = elevation;
            }
        }

        // generatedGrid_For_ChunkSpliter = grid;
        // PrintGrid(generatedGrid_For_ChunkSpliter, "generatedGrid_For_ChunkSpliter");
    }

    [TabGroup("Chunk Loader", "Loader 1")]
    [TabGroup("Chunk Loader", "Loader 2")]
    [Button("Clear Gird")]
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

    [TabGroup("Chunk Loader", "Debug")]
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

    [TabGroup("Chunk Loader", "Debug")]
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

    #region Chunk Loader 2 (Generat while going)
    [TabGroup("Chunk Loader", "Loader 2")]
    [InlineButton("Random_Seed_L2", "RNG")]
    public int seed_L2 = 3456;
    [TabGroup("Chunk Loader", "Loader 2")]
    public int chunk_size_L2 = 16;
    [TabGroup("Chunk Loader", "Loader 2")]
    public float seed_muiltiplier_L2 = 0.5f;
    [TabGroup("Chunk Loader", "Loader 2")]
    [InlineButton("Increase_X_L2", "+")]
    [InlineButton("Decrease_X_L2", "-")]
    public int x_L2 = 0;
    [TabGroup("Chunk Loader", "Loader 2")]
    [InlineButton("Increase_Y_L2", "+")]
    [InlineButton("Decrease_Y_L2", "-")]
    public int y_L2 = 0;
    // [TabGroup("Chunk Loader", "Loader 2")]
    // public int iterationIncrease_L2 = 8;
    // [TabGroup("Chunk Loader", "Loader 2")]
    // public int iteration_L2 = 0;

    [TabGroup("Chunk Loader", "Loader 2")]
    // [Button("Seamless Chunk Generater")]
    public Chunk_T SeamlessChunkGenerater(int xStart, int yStart){

        var chunkseed = seed_L2 + (xStart * seed_muiltiplier_L2 + yStart);

        var grid = new float[chunk_size_L2, chunk_size_L2];

        for (int i = 0; i < chunk_size_L2; i++)
        {
            for (int j = 0; j < chunk_size_L2; j++)
            {
                var elevation = 0f;
                var tempFrequency = frequency;
                var tempAmplitude = amplitude;

                for (int k = 0; k < octaves; k++)
                {
                    var x = i * tempFrequency + chunkseed;
                    var y = j * tempFrequency;

                    elevation += Mathf.PerlinNoise(x, y) * tempAmplitude;

                    tempFrequency *= lacunarity;
                    tempAmplitude *= persistence;
                }

                grid[i, j] = elevation;
            }
        }

        return new Chunk_T(xStart, yStart, grid);
    }

    [TabGroup("Chunk Loader", "Loader 2")]
    [Button("Load Chunk")]
    public void LoadChunk_Loader2(){
        var chunk = SeamlessChunkGenerater(x_L2, y_L2);
        var grid = chunk.TileValues;

		for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
                var xCoordinate = x + chunk.XCoordinate * chunk_size_L2;
                var yCoordinate = y + chunk.YCoordinate * chunk_size_L2;
                
				// if (grid[x, y] >= 0f && grid[x, y] <= 0.2f)
				// {
				// 	Grid_Map01_02.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Water);
				// }
				// else if (grid[x, y] > 0.2f && grid[x, y] <= 0.3f)
				// {
				// 	Grid_Map02_03.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtGrass);
				// }
				// else if (grid[x, y] > 0.3f && grid[x, y] <= 0.4f)
				// {
				// 	Grid_Map03_04.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtGrass);
				// }
				// else if (grid[x, y] > 0.4f && grid[x, y] <= 0.5f)
				// {
				// 	Grid_Map04_05.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtDirt);
				// }
				// else if (grid[x, y] > 0.5f)
				// {
				// 	Grid_Map05_06.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtSnow);
				// }

                if (grid[x, y] >= 0f && grid[x, y] <= 0.3f)
				{
					Grid_Map01_02.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtGrass);
				}
				else if (grid[x, y] > 0.3f && grid[x, y] <= 0.4f)
				{
					Grid_Map04_05.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtGrass);
				}
				else if (grid[x, y] > 0.4f && grid[x, y] <= 0.5f)
				{
					Grid_Map05_06.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtDirt);
				}
				else if (grid[x, y] > 0.5f)
				{
					Grid_Map05_06.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), DirtSnow);
				}
			}
		}
    }

    
    public void Increase_Y_L2(){
        y_L2++;
    }
    
    public void Decrease_Y_L2(){
        y_L2--;
    }
    
    public void Increase_X_L2(){
        x_L2++;
    }
    [InlineButton("Decrease_X_L2")]
    public void Decrease_X_L2(){
        x_L2--;
    }
    public void Random_Seed_L2(){
        seed_L2 = Random.Range(-10000,10000);
    }
    #endregion
}
