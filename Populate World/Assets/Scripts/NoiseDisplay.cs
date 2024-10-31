using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NoiseDisplay : MonoBehaviour
{
	[Header("DebugUI")]
	public Canvas canvas;

	[Header("Noise Map")]
	public Renderer textureRender;

	[Header("Tiles")]
	public Tilemap Map01_02;
	public Tilemap Map02_03;
	public Tilemap Map03_04;
	public Tilemap Map04_05;
	public Tilemap Map05_06;
	public RuleTile Water;
	public RuleTile DirtGrass;
	public RuleTile DirtDirt;
	public RuleTile DirtSnow;

	[Header("Chunk Loader")]
	public int chunkWidth;
	public int chunkHeight;
	private int xLastChunk = 0;
	private int yLastChunk = 0;
	private float[,] noiseMap;


	public void DrawNoiseMap(float[,] noiseMap)
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

	public void DrawTiles(float[,] grid)
	{
		ClearTilemaps();

		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i, j] >= 0f /* 0.1f */ && grid[i, j] <= 0.2f)
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
				else if (grid[i, j] > 0.5f /* && grid[i,j] <= 0.6f */)
				{
					Map05_06.SetTile(new Vector3Int(i, j, 0), DirtSnow);
				}
			}
		}
	}

	public void DrawTiles_2(float[,] grid)
	{
		ClearTilemaps();

		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i, j] >= 0f && grid[i, j] <= 0.3f)
				{
					Map02_03.SetTile(new Vector3Int(i, j, 0), DirtGrass);
				}
				else if (grid[i, j] > 0.3f && grid[i, j] <= 0.4f)
				{
					Map03_04.SetTile(new Vector3Int(i, j, 0), DirtDirt);
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

	public void DrawTiles_3(float[,] grid)
	{
		ClearTilemaps();

		int widthStart = 0;
		int heightStart = 0;

		int widthEnd = grid.GetLength(0);
		int heightEnd = grid.GetLength(1);

		if (grid.GetLength(0) > 300 && grid.GetLength(1) > 300)
		{
			widthStart = grid.GetLength(0) / 2;
			heightStart = grid.GetLength(1) / 2;

			widthEnd = widthStart + widthStart / 2;
			heightEnd = heightStart + heightStart / 2;
		}

		for (int i = widthStart; i < widthEnd; i++)
		{
			for (int j = heightStart; j < heightEnd; j++)
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

	public void DrawTiles_4(float[,] grid)
	{
		Debug.Log("DrawTiles_4: Beginning");
		noiseMap = grid.Clone() as float[,];
		Debug.Log("ChunkWidth:" + chunkWidth);

		for (int i = 0; i < chunkWidth; i++)
		{
			for (int j = 0; j < chunkHeight; j++)
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

		Debug.Log("DrawTiles_4: End");
		xLastChunk = chunkWidth;
		yLastChunk = chunkHeight;
	}

	public void LoadChunks()
	{
		int widthStart = 0;
		int heightStart = 0;
		int widthEnd = 0;
		int heightEnd = 0;

		if (yLastChunk == noiseMap.GetLength(1) && xLastChunk == noiseMap.GetLength(0))
		{
			return;
		}

		if (yLastChunk == noiseMap.GetLength(1))
		{
			widthStart = xLastChunk;
			heightStart = 0;
			widthEnd = widthStart + chunkWidth;
			heightEnd = heightStart + chunkHeight;

			xLastChunk += chunkWidth;
			yLastChunk = 0;
		}
		else
		{
			widthStart = xLastChunk - chunkWidth;
			heightStart = yLastChunk;
			widthEnd = widthStart + chunkWidth;
			heightEnd = heightStart + chunkHeight;

			yLastChunk += chunkHeight;
		}


		for (int i = widthStart; i < widthEnd; i++)
		{
			for (int j = heightStart; j < heightEnd; j++)
			{
				if (noiseMap[i, j] >= 0f && noiseMap[i, j] <= 0.2f)
				{
					Map01_02.SetTile(new Vector3Int(i, j, 0), Water);
				}
				else if (noiseMap[i, j] > 0.2f && noiseMap[i, j] <= 0.3f)
				{
					Map02_03.SetTile(new Vector3Int(i, j, 0), DirtGrass);
				}
				else if (noiseMap[i, j] > 0.3f && noiseMap[i, j] <= 0.4f)
				{
					Map03_04.SetTile(new Vector3Int(i, j, 0), DirtGrass);
				}
				else if (noiseMap[i, j] > 0.4f && noiseMap[i, j] <= 0.5f)
				{
					Map04_05.SetTile(new Vector3Int(i, j, 0), DirtDirt);
				}
				else if (noiseMap[i, j] > 0.5f)
				{
					Map05_06.SetTile(new Vector3Int(i, j, 0), DirtSnow);
				}
			}
		}

		// xLastChunk += chunkWidth;
		// yLastChunk += chunkHeight;
	}

	public void DrawTiles_5(float[,] grid)
	{
		// ClearTilemaps();

		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i, j] >= 0f && grid[i, j] <= 0.3f)
				{
					Map02_03.SetTile(new Vector3Int(i, j, 0), DirtGrass);
				}
				// else if (grid[i,j] > 0.2f && grid[i,j] <= 0.3f)
				// {
				// 	Map02_03.SetTile(new Vector3Int(i,j,0), DirtGrass);
				// }
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


	public void DrawTiles_Water_1(float[,] grid)
	{
		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i, j] >= 0f && grid[i, j] <= 0.3f)
				{
					Map01_02.SetTile(new Vector3Int(i, j, 0), Water);
				}
			}
		}
	}

	public void DrawTiles_Water_2(float[,] grid)
	{
		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i, j] >= 0.3f && grid[i, j] <= 0.7f)
				{
					Map01_02.SetTile(new Vector3Int(i, j, 0), Water);
				}
			}
		}
	}

	public void DrawTiles_LW_1(float[,] grid)
	{
		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i, j] == -1f)
				{
					Map01_02.SetTile(new Vector3Int(i, j, 0), Water);
				}
				else if (grid[i, j] >= 0f && grid[i, j] <= 0.3f)
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

	public void ClearTilemaps()
	{
		Map01_02.ClearAllTiles();
		Map02_03.ClearAllTiles();
		Map03_04.ClearAllTiles();
		Map04_05.ClearAllTiles();
		Map05_06.ClearAllTiles();
	}

	public void DrawValues(float[,] grid, bool autoUpdate)
	{
		if ((grid.GetLength(0) > 15 && grid.GetLength(1) > 15) || autoUpdate == true)
		{
			return;
		}

		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				GameObject textObject = new GameObject("Text" + i + j);
				textObject.transform.SetParent(canvas.transform);
				TextMeshPro textcomponent = textObject.AddComponent<TextMeshPro>();
				textcomponent.text = "" + Mathf.Round(grid[i, j] * 10000f) / 10000f;
				RectTransform rectTransform = textObject.GetComponent<RectTransform>();
				rectTransform.anchoredPosition = new Vector2(i * 20, j * 20);
			}
		}
	}
}
