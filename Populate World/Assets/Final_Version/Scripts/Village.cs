using System.Collections.Generic;

public class Village
{
    public int xCenterCoordinate;
    public int yCenterCoordinate;
    public Structure[] listOfStructures;
    public List<(string name, List<Point> path)> listOfPaths = new List<(string name, List<Point> path)>();

    public Village(int xCenterCoordinate, int yCenterCoordinate, int numberOfStructures){
        this.xCenterCoordinate = xCenterCoordinate;
        this.yCenterCoordinate =yCenterCoordinate;
        listOfStructures = new Structure[numberOfStructures];
    }
}
