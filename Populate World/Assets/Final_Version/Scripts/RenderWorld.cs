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
    [TabGroup("Variables", "Tilemaps")]
    public Tilemap Path_Layer;
    [TabGroup("Variables", "Tilemaps")]
    public Tilemap Village_Layer;


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
    [TabGroup("Variables", "RuleTiles")]
    public RuleTile Path;
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
    public Tile[] VillageHouses = new Tile[2];
    [TabGroup("Variables", "Tiles")]
    public Tile[] VillageCenter = new Tile[5];
    [TabGroup("Variables", "Tiles")]
    public Tile Snowman;


    [TabGroup("Variables", "World Size"), ShowInInspector]
    public int ChunkSize
    {
        get {return chunkSize; }
    }
    [TabGroup("Variables", "World Size")]
    public int chunkMultiplier = 101;
    [TabGroup("Variables", "World Size"), ShowInInspector]
    public int WorldSize
    {
        get {return worldSize; }
    }

    private int chunkSize = 16;
    private int worldSize;


    private WorldGenerator worldGenerator;
    private VillageGenerator villageGenerator;
    private FloraGenerator floraGenerator;
    private ChunkLoader chunkLoader;


    [TabGroup("Buttons")]
    [Button("Render World")]
    public void RenderTheWorld()
    {
        worldSize = chunkSize * chunkMultiplier;

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

        worldGenerator = FindAnyObjectByType<WorldGenerator>();
        worldGenerator.GenerateWorldNosie(worldSize);

        villageGenerator = FindAnyObjectByType<VillageGenerator>();
        villageGenerator.GenerateVillages(worldGenerator.WorldGrid, worldSize);

        floraGenerator = FindAnyObjectByType<FloraGenerator>();
        floraGenerator.GenerateFlora(worldGenerator.WorldGrid, villageGenerator.villageGrid);

        chunkLoader = FindAnyObjectByType<ChunkLoader>();
        chunkLoader.SplitGridInChunks(worldGenerator.WorldGrid, villageGenerator.villageGrid, floraGenerator.FloraGrid, chunkMultiplier, chunkSize);
        chunkLoader.LoadChunk(chunkMultiplier);
    }

    [TabGroup("Buttons")]
    [Button("Render Chuncks")]
    public void RenderChunks()
    {
        chunkLoader.LoadChunk(chunkMultiplier);
    }

    public void RenderTerrain(Chunk chunk)
    {
        var grid = chunk.terrainTileValues;

        for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
                var xCoordinate = x + chunk.XCoordinate * chunkSize;
                var yCoordinate = y + chunk.YCoordinate * chunkSize;

				if (grid[x, y] == -1f)
				{
					Water_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Water);
				}
				else if (grid[x, y] >= 0f && grid[x, y] <= 0.3f)
				{
					Terrain_Layer_1.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Grass);
				}
				else if (grid[x, y] > 0.3f && grid[x, y] <= 0.4f)
				{
					Terrain_Layer_2.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Grass);
				}
				else if (grid[x, y] > 0.4f && grid[x, y] <= 0.5f)
				{
					Terrain_Layer_3.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Stone);
				}
				else if (grid[x, y] > 0.5f)
				{
					Terrain_Layer_4.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Snow);
				}
			}
		}
    }

    public void RenderVillage(Chunk chunk)
    {
        var grid = chunk.villageTileValues;

        for (int x = 0; x < grid.GetLength(0); x++)
		{
			for (int y = 0; y < grid.GetLength(1); y++)
			{
                var xCoordinate = x + chunk.XCoordinate * chunkSize;
                var yCoordinate = y + chunk.YCoordinate * chunkSize;

				if (grid[x, y] == 1.1f)
				{
					Village_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), VillageCenter[0]);

                    Path_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Path);
				}
                else if (grid[x, y] == 1.2f)
				{
					Village_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), VillageCenter[1]);

                    Path_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Path);
				}
                else if (grid[x, y] == 1.3f)
				{
					Village_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), VillageCenter[2]);

                    Path_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Path);
				}
                else if (grid[x, y] == 1.4f)
				{
					Village_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), VillageCenter[3]);

                    Path_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Path);
				}
                else if (grid[x, y] == 1.5f)
				{
					Village_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), VillageCenter[4]);

                    Path_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Path);
				}
				else if (grid[x, y] == 2.1f)
				{
					Village_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), VillageHouses[0]);
				}
                else if (grid[x, y] == 2.2f)
				{
					Village_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), VillageHouses[1]);
				}
				else if (grid[x, y] == 3f)
				{
					Path_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 0), Path);
				}
			}
		}
    }

    public void RenderFlora(Chunk chunk)
    {
        var grid = chunk.floraTileValues;

        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                var xCoordinate = x + chunk.XCoordinate * chunkSize;
                var yCoordinate = y + chunk.YCoordinate * chunkSize;

                switch (grid[x, y])
                {
                    case 1f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Trees[1]);
                        break;
                    case 2.0f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Trees[9]);
                        break;
                    case 2.1f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Trees[7]);
                        break;
                    case 2.2f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Trees[10]);
                        break;
                    case 2.3f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Trees[8]);
                        break;
                    case 3.0f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Trees[2]);
                        break;
                    case 3.1f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Trees[2]);
                        break;
                    case 3.2f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Trees[2]);
                        break;
                    case 3.3f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Trees[2]);
                        break;
                    case 4.1f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Stones[0]);
                        break;
                    case 4.2f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Stones[1]);
                        break;
                    case 5.1f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Bushes[0]);
                        break;
                    case 5.2f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Bushes[1]);
                        break;
                    case 5.3f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Bushes[2]);
                        break;
                    case 5.4f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Bushes[3]);
                        break;
                    case 5.5f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Bushes[4]);
                        break;
                    case 5.6f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Bushes[5]);
                        break;
                    case 6.1f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Flowers[0]);
                        break;
                    case 6.2f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Flowers[1]);
                        break;
                    case 6.3f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Flowers[2]);
                        break;
                    case 9f:
                        Flora_Layer.SetTile(new Vector3Int(xCoordinate, yCoordinate, 1), Snowman);
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
        Path_Layer.ClearAllTiles();
        Village_Layer.ClearAllTiles();
    }
}
