public class Structure_T
{
    public int xCoordinate;
    public int yCoordinate;
    public float tileValue;

    public Structure_T(int xCoordinate, int yCoordinate, float tileValue){
        this.xCoordinate = xCoordinate;
        this.yCoordinate = yCoordinate;
        this.tileValue = tileValue;
    }

    public override string ToString()
    {
        return "[X: "+xCoordinate+", Y: "+yCoordinate+", Tile: "+tileValue+"]";
    }
}
