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


	public void DrawNoiseMap(float[,] noiseMap) {
		int width = noiseMap.GetLength (0);
		int height = noiseMap.GetLength (1);

		Texture2D texture = new Texture2D (width, height);

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, noiseMap [x, y]);
			}
		}
		texture.SetPixels (colourMap);
		texture.Apply ();

		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3 (width, 1, height);
	}

    public void DrawTiles(float[,] grid){
		ClearTilemaps();

        for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i,j] >= 0f /* 0.1f */ && grid[i,j] <= 0.2f)
				{
					Map01_02.SetTile(new Vector3Int(i,j,0), Water);
				}
				else if (grid[i,j] > 0.2f && grid[i,j] <= 0.3f)
				{
					Map02_03.SetTile(new Vector3Int(i,j,0), DirtGrass);
				}
				else if (grid[i,j] > 0.3f && grid[i,j] <= 0.4f)
				{
					Map03_04.SetTile(new Vector3Int(i,j,0), DirtGrass);
				}
				else if (grid[i,j] > 0.4f && grid[i,j] <= 0.5f)
				{
					Map04_05.SetTile(new Vector3Int(i,j,0), DirtDirt);
				}
				else if (grid[i,j] > 0.5f /* && grid[i,j] <= 0.6f */)
				{
					Map05_06.SetTile(new Vector3Int(i,j,0), DirtSnow);
				}
			}
		}
    }

	public void ClearTilemaps(){
		Map01_02.ClearAllTiles();
		Map02_03.ClearAllTiles();
		Map03_04.ClearAllTiles();
		Map04_05.ClearAllTiles();
		Map05_06.ClearAllTiles();
	}

	public void DrawValues(float[,] grid, bool autoUpdate){
		if ((grid.GetLength(0) > 15 && grid.GetLength(1) > 15) || autoUpdate == true)
		{
			return;
		}

		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				GameObject textObject = new GameObject("Text"+i+j);
				textObject.transform.SetParent(canvas.transform);
				TextMeshPro textcomponent = textObject.AddComponent<TextMeshPro>();
				textcomponent.text = ""+Mathf.Round(grid[i,j]*10000f)/10000f;
				RectTransform rectTransform = textObject.GetComponent<RectTransform>();
				rectTransform.anchoredPosition = new Vector2(i*20,j*20);
			}
		}
	}
}
