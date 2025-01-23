using UnityEngine;

public class ChunkLoader : MonoBehaviour
{
    private Chunk[,] chunkGrid;
    public int chunkIteration = 0;
    private int gridMiddleChunk;
    private readonly int chunkIterationIncrease = 4;
    private readonly int[] xDirection = { 1, -1, -1, 1 };
    private readonly int[] yDirection = { -1, -1, 1, 1 };

    private RenderWorld renderWorld;

    public void SplitGridInChunks(float[,] worldGrid, float[,] villageGrid, float[,] floraGrid, int chunkMultiplier, int chunkSize)
    {
        chunkIteration = 0;

        renderWorld = FindAnyObjectByType<RenderWorld>();
        chunkGrid = new Chunk[chunkMultiplier, chunkMultiplier];

        for (int xCM = 0; xCM < chunkMultiplier; xCM++)
        {
            for (int yCM = 0; yCM < chunkMultiplier; yCM++)
            {
                chunkGrid[xCM, yCM] = new Chunk(xCM, yCM, chunkSize);

                for (int xCS = 0; xCS < chunkSize; xCS++)
                {
                    for (int yCS = 0; yCS < chunkSize; yCS++)
                    {
                        var x = (xCM * 16) + xCS;
                        var y = (yCM * 16) + yCS;

                        if (x < 0 || y < 0 || x > chunkMultiplier * chunkSize || y > chunkMultiplier * chunkSize)
                        {
                            continue;
                        }

                        chunkGrid[xCM, yCM].terrainTileValues[xCS, yCS] = worldGrid[x, y];
                        chunkGrid[xCM, yCM].villageTileValues[xCS, yCS] = villageGrid[x, y];
                        chunkGrid[xCM, yCM].floraTileValues[xCS, yCS] = floraGrid[x, y];
                    }
                }
            }
        }
    }

    public void LoadChunk(int chunkMultiplier)
    {
        gridMiddleChunk = (chunkMultiplier - 1) / 2;

        if (chunkIteration == 0)
        {
            renderWorld.RenderTerrain(chunkGrid[gridMiddleChunk, gridMiddleChunk]);
            renderWorld.RenderVillage(chunkGrid[gridMiddleChunk, gridMiddleChunk]);
            renderWorld.RenderFlora(chunkGrid[gridMiddleChunk, gridMiddleChunk]);
            Debug.Log("X: " + gridMiddleChunk + ", Y: " + gridMiddleChunk);
        }

        var x = gridMiddleChunk;
        var y = gridMiddleChunk + chunkIteration / chunkIterationIncrease;
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

            renderWorld.RenderTerrain(chunkGrid[x, y]);
            renderWorld.RenderVillage(chunkGrid[x, y]);
            renderWorld.RenderFlora(chunkGrid[x, y]);
        }

        chunkIteration += chunkIterationIncrease;
    }
}
