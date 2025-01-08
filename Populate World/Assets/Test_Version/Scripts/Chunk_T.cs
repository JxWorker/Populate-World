using System.Linq;

public class Chunk_T
{
    public int XCoordinate { get; set; }
    public int YCoordinate { get; set; }
    public float[,] TileValues = new float[16,16]; //TODO: Die größe sollte auch über den Konstruktor gesetz werden können. 
    
    public Chunk_T(int xCoordinate, int yCoordiante, float[,] TileValues)
    {
        XCoordinate = xCoordinate;
        YCoordinate = yCoordiante;
        this.TileValues = TileValues;
    }

    public Chunk_T(int xCoordinate, int yCoordiante)
    {
        XCoordinate = xCoordinate;
        YCoordinate = yCoordiante;
    }

    public override string ToString()
    {
        string str = "[ ";

        for (int x = 0; x < TileValues.GetLength(0); x++)
        {
            for (int y = 0; y < TileValues.GetLength(1); y++)
            {
                str = string.Concat(str + TileValues[x,y] + ", ");
            }

            if (x == TileValues.GetLength(0) - 1)
            {
                str = string.Concat(str + "]");
                continue;
            }

            str = string.Concat(str + "] \n [ ");
        }

        return str;
    }
}
