using System.Collections.Generic;

public class Village_T
{
    public int xCenterCoordinate;
    public int yCenterCoordinate;
    public Structure_T[] listOfStructures;
    public List<(string name, List<Point_T> path)> listOfPaths = new List<(string name, List<Point_T> path)>();

    public Village_T(int xCenterCoordinate, int yCenterCoordinate, int numberOfStructures){
        this.xCenterCoordinate = xCenterCoordinate;
        this.yCenterCoordinate =yCenterCoordinate;
        listOfStructures = new Structure_T[numberOfStructures];
    }

    public override string ToString()
    {
        var str = "Center: ["+xCenterCoordinate+", "+yCenterCoordinate+"]\n";

        for (int i = 0; i < listOfStructures.Length; i++)
        {
            str = string.Concat(str, "Structure ", i.ToString(), ": ", listOfStructures[i].ToString(), "\n");   
        }

        str = string.Concat(str, "\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");

        // var count = 0;

        foreach (var path in listOfPaths)
        {
            var temp = "["+path.name+"]"+"{";

            foreach (var point in path.path)
            {
                temp = string.Concat(temp, point.ToString(), ", ");
            }

            temp = string.Concat(temp, "}\n");

            str = string.Concat(str, temp);
            // count++;
        }

        return str;
    }
}
