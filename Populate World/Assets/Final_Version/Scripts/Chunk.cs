public class Chunk
{
    public int XCoordinate { get; set; }
    public int YCoordinate { get; set; }
    public float[,] terrainTileValues;
    public float[,] floraTileValues;
    public float[,] villageTileValues;
    
    public Chunk(int xCoordinate, int yCoordiante, float[,] terrainTileValues, float[,] floraTileValues, float[,] villageTileValues)
    {
        XCoordinate = xCoordinate;
        YCoordinate = yCoordiante;
        this.terrainTileValues = terrainTileValues;
        this.floraTileValues = floraTileValues;
        this.villageTileValues = villageTileValues;
    }

    public Chunk(int xCoordinate, int yCoordiante, int size)
    {
        XCoordinate = xCoordinate;
        YCoordinate = yCoordiante;
        terrainTileValues = new float[size, size];
        floraTileValues = new float[size, size];
        villageTileValues = new float[size, size];
    }
}
