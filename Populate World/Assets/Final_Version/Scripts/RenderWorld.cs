using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RenderWorld : MonoBehaviour
{
    [TabGroup("Variables", "Tilemaps")]
    public Tilemap Water_Layer;
    [TabGroup("Variables", "Tilemaps")]
    public Tilemap Terrain_Layer_1;
    [TabGroup("Variables", "Tilemaps")]
    public Tilemap Terrain_Layer_2;
    [TabGroup("Variables", "Tilemaps")]
    public Tilemap Terrain_Layer_3;
    [TabGroup("Variables", "Tilemaps")]
    public Tilemap Terrain_Layer_4;
    [TabGroup("Variables", "Tilemaps")]
    public Tilemap Flora_Layer;


    [TabGroup("Variables", "RuleTiles")]
    public bool isBrownStone = false;
    [TabGroup("Variables", "RuleTiles")]
    public RuleTile Water;
    [TabGroup("Variables", "RuleTiles")]
    public RuleTile Grass;
    [TabGroup("Variables", "RuleTiles")]
    public RuleTile BrownStone;
    [TabGroup("Variables", "RuleTiles")]
    public RuleTile GrayStone;
    [TabGroup("Variables", "RuleTiles")]
    public RuleTile SnowBrownStone;
    [TabGroup("Variables", "RuleTiles")]
    public RuleTile SnowGrayStone;
    private RuleTile Stone;
    private RuleTile Snow;


    [TabGroup("Variables", "Tiles")]
    public Tile[] Trees = new Tile[11];
    [TabGroup("Variables", "Tiles")]
    public Tile[] Bushes = new Tile[6];
    [TabGroup("Variables", "Tiles")]
    public Tile[] Flowers = new Tile[3];
    [TabGroup("Variables", "Tiles")]
    public Tile[] Stones = new Tile[2];
    [TabGroup("Variables", "Tiles")]
    public Tile Snowman;



    [TabGroup("Buttons")]
    [Button("Render World")]
    public void RenderTheWorld()
    {
        if (isBrownStone)
        {
            Stone = BrownStone;
            Snow = SnowBrownStone;
        }
        else
        {
            Stone = GrayStone;
            Snow = SnowGrayStone;
        }

        WorldGenerator worldGenerator = FindAnyObjectByType<WorldGenerator>();
        worldGenerator.GenerateWorldNosie();
        RenderTerrain(worldGenerator.WorldGrid);

        FloraGenerator floraGenerator = FindAnyObjectByType<FloraGenerator>();
        floraGenerator.GenerateFlora(worldGenerator.WorldGrid);
        RenderFlora(floraGenerator.FloraGrid);
    }

    private void RenderTerrain(float[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i, j] == -1f)
				{
					Water_Layer.SetTile(new Vector3Int(i, j, 0), Water);
				}
				else if (grid[i, j] >= 0f && grid[i, j] <= 0.3f)
				{
					Terrain_Layer_1.SetTile(new Vector3Int(i, j, 0), Grass);
				}
				else if (grid[i, j] > 0.3f && grid[i, j] <= 0.4f)
				{
					Terrain_Layer_2.SetTile(new Vector3Int(i, j, 0), Grass);
				}
				else if (grid[i, j] > 0.4f && grid[i, j] <= 0.5f)
				{
					Terrain_Layer_3.SetTile(new Vector3Int(i, j, 0), Stone);
				}
				else if (grid[i, j] > 0.5f)
				{
					Terrain_Layer_4.SetTile(new Vector3Int(i, j, 0), Snow);
				}
			}
		}
    }

    private void RenderFlora(float[,] grid)
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                switch (grid[x, y])
                {
                    case 1f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Trees[1]);
                        break;
                    case 2.0f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Trees[9]);
                        break;
                    case 2.1f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Trees[7]);
                        break;
                    case 2.2f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Trees[10]);
                        break;
                    case 2.3f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Trees[8]);
                        break;
                    case 3.0f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Trees[2]);
                        break;
                    case 3.1f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Trees[2]);
                        break;
                    case 3.2f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Trees[2]);
                        break;
                    case 3.3f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Trees[2]);
                        break;
                    case 4.1f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Stones[0]);
                        break;
                    case 4.2f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Stones[1]);
                        break;
                    case 5.1f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Bushes[0]);
                        break;
                    case 5.2f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Bushes[1]);
                        break;
                    case 5.3f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Bushes[2]);
                        break;
                    case 5.4f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Bushes[3]);
                        break;
                    case 5.5f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Bushes[4]);
                        break;
                    case 5.6f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Bushes[5]);
                        break;
                    case 6.1f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Flowers[0]);
                        break;
                    case 6.2f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Flowers[1]);
                        break;
                    case 6.3f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Flowers[2]);
                        break;
                    case 9f:
                        Flora_Layer.SetTile(new Vector3Int(x, y, 1), Snowman);
                        break;
                }
            }
        }
    }

    [TabGroup("Buttons")]
    [Button("Clear World")]
    public void ClearWorld(){
        Water_Layer.ClearAllTiles();
        Terrain_Layer_1.ClearAllTiles();
        Terrain_Layer_2.ClearAllTiles();
        Terrain_Layer_3.ClearAllTiles();
        Terrain_Layer_4.ClearAllTiles();
        Flora_Layer.ClearAllTiles();
    }
}
