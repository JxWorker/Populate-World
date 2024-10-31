using UnityEngine;
using UnityEngine.Tilemaps;

public class FloraGenerator : MonoBehaviour
{
    public float[,] floraGrid;
    private int width;
    private int height;

    [Header("Spawn Chance")]
    public int plantChance = 9;
    public int stoneChance = 9;
    public int snowmanChance = 9;

    [Header("Tiles")]
    public Tilemap folraMap;
    public Tile tree1;
    public Tile tree2;
    public Tile tree3;
    public Tile bush1;
    public Tile bush2;
    public Tile bush3;
    public Tile bush4;
    public Tile bush5;
    public Tile bush6;
    public Tile flower1;
    public Tile flower2;
    public Tile flower3;
    public Tile stone1;
    public Tile stone2;
    public Tile snowman;

    public void GenerateFlora_1(float[,] worldGrid)
    {
        width = worldGrid.GetLength(0);
        height = worldGrid.GetLength(1);

        floraGrid = new float[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (worldGrid[i, j] <= 0.2f)
                {
                    floraGrid[i, j] = 0f;
                }
                else if (worldGrid[i, j] > 0.2f && worldGrid[i, j] <= 0.4f)
                {
                    RandomPlant(i, j);
                }
                else if (worldGrid[i, j] > 0.4f && worldGrid[i, j] <= 0.5f)
                {
                    RandomStone(i, j);
                }
                else if (worldGrid[i, j] > 0.5f)
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
                int treeVariant = Random.Range(1, 3);

                floraGrid[x, y] = treeVariant == 1 ? 1.1f : treeVariant == 2 ? 1.2f : 1.3f;
                break;
            case 2:
                int bushVariant = Random.Range(1, 6);

                floraGrid[x, y] = bushVariant == 1 ? 2.1f : bushVariant == 2 ? 2.2f : bushVariant == 3 ? 2.3f : bushVariant == 4 ? 2.4f : bushVariant == 5 ? 2.5f : 2.6f; 
                break;
            case 3:
                int flowerVariant = Random.Range(1, 3);

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

    public void DrawFlora()
    {
        for (int i = 0; i < floraGrid.GetLength(0); i++)
        {
            for (int j = 0; j < floraGrid.GetLength(1); j++)
            {
                switch (floraGrid[i, j])
                {
                    case 1.1f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), tree1);
                        break;
                    case 1.2f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), tree2);
                        break;
                    case 1.3f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), tree3);
                        break;
                    case 2.1f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush1);
                        break;
                    case 2.2f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush2);
                        break;
                    case 2.3f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush3);
                        break;
                    case 2.4f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush4);
                        break;
                    case 2.5f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush5);
                        break;
                    case 2.6f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), bush6);
                        break;
                    case 3.1f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), flower1);
                        break;
                    case 3.2f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), flower2);
                        break;
                    case 3.3f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), flower3);
                        break;
                    case 4.1f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), stone1);
                        break;
                    case 4.2f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), stone2);
                        break;
                    case 5f:
                        folraMap.SetTile(new Vector3Int(i, j, 1), snowman);
                        break;
                }
            }
        }
    }

    public void ClearFloraMap()
    {
        folraMap.ClearAllTiles();
    }
}
