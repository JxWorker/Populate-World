using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VillageGenerator_T : MonoBehaviour
{
    [TabGroup("Tile")]
    public Tilemap Grid_Village;
    [TabGroup("Tile")]
    public Tilemap Grid_Path;
    [TabGroup("Tile")]
    public Tilemap Grid_Test;
    [TabGroup("Tile")]
    public Tile TestTile_1;
    [TabGroup("Tile")]
    public Tile TestTile_2;
    [TabGroup("Tile")]
    public Tile TestTile_3;

    public int WorldSize;

    public float[,] villageGrid;

    private List<Village_T> villageList = new List<Village_T>();
    private Village_T currentVillage;

    [TabGroup("Button")]
    [Button("Generate")]
    public void GenerateVillage()
    {
        villageGrid = new float[WorldSize, WorldSize];

        int[,] directions = { { +1, +1 }, { +1, -1 }, { -1, +1 }, { -1, -1 } };

        int villageRadius = Random.Range(5, 21);
        int xCenter = Random.Range(villageRadius + 1, WorldSize - villageRadius);
        int yCenter = Random.Range(villageRadius + 1, WorldSize - villageRadius);
        // int numberOfHouses = Random.Range(villageRadius / 3 * 2, villageRadius + 1);
        // int numberOfHouses = Random.Range(villageRadius, villageRadius * 2 / 3);
        int numberOfHouses = Random.Range(villageRadius * 2 / 3, villageRadius - 1);

        // ShowSquare(xCenter, yCenter, villageRadius);

        print("X: " + xCenter + ", Y: " + yCenter);
        print("Radius: " + villageRadius);
        print("Number of Houses: " + numberOfHouses);

        // var village = new Village_T(xCenter, yCenter, numberOfHouses + 1);
        currentVillage = new Village_T(xCenter, yCenter, numberOfHouses + 1);

        CenterPathCross(xCenter, yCenter, villageRadius, currentVillage);

        var count = 0;

        while (count < numberOfHouses)
        {
            // var isAboveOrBelow = false;
            var xdirection = directions[count % 4, 0];
            var ydirection = directions[count % 4, 1];
            var x = Random.Range(1, villageRadius + 1);
            var y = Random.Range(1, villageRadius + 1);

            var xWorld = xCenter + x * xdirection;
            var yWorld = yCenter + y * ydirection;

            if (!IsHouseInRadius(villageRadius, x, y) || xWorld > WorldSize || yWorld > WorldSize || xWorld < 0 || yWorld < 0)
            {
                continue;
            }

            // if (currentVillage.listOfStructures.Length != 0)
            // {
            //     for (int i = 0; i < count; i++)
            //     {
            //         if (currentVillage.listOfStructures[i].yCoordinate == yWorld - 1 || currentVillage.listOfStructures[i].yCoordinate == yWorld + 1)
            //         {
            //             isAboveOrBelow = true;
            //         }
            //     }
            // }
            // var positivBound = yCenter + villageRadius;
            // var negativeBound = yCenter - villageRadius;

            if (OnCenterPathCross(xWorld, yWorld))
            {
                continue;
            }

            if (HasDirectNeighbour(xWorld, yWorld, count))
            {   
                continue;
            }

            // if (HasStructureInColumn(yCenter, count))
            // {
            //     continue;
            // }

            // if (HasStructureToRightOrLeft(xWorld, count))
            // {
            //     continue;
            // }

            currentVillage.listOfStructures[count] = new Structure_T(xWorld, yWorld, 2f);
            villageGrid[xWorld, yWorld] = 2f;
            count++;
        }

        currentVillage.listOfStructures[numberOfHouses] = new Structure_T(xCenter, yCenter, 1f);
        villageGrid[xCenter, yCenter] = 1f;

        // print(village.listOfStructures.Length);
        // print(village.ToString());

        villageList.Add(currentVillage);


        // CenterPathCross(xCenter, yCenter, villageRadius, currentVillage);

        var structureCount = 0;
        foreach (var structure in currentVillage.listOfStructures)
        {
            // Debug.Log("Entered ForEach");
            if (structure.xCoordinate == xCenter && structure.yCoordinate == yCenter)
            {
                continue;
            }

            var path = FindPath(new Point_T(xCenter, yCenter, null), new Point_T(structure.xCoordinate, structure.yCoordinate, null));

            foreach (var point in path)
            {
                villageGrid[point.xCoordinate, point.yCoordinate] = 3f;
            }

            currentVillage.listOfPaths.Add(("Structure " + structureCount, path));
            structureCount++;
        }

        print(currentVillage.ToString());
    }

    // private bool HasStructureInColumn(int yCenter, int currentStructureCount)
    // {
    //     // var loopStart = 0;
    //     // var loopEnd = 0;

    //     if (currentVillage.listOfStructures.Length == 0)
    //     {
    //         return false;
    //     }

    //     // if (yWorld > yCenter)
    //     // {
    //     // loopStart = yCenter;
    //     // loopEnd = yWorld;

    //     // for (int i = yCenter; i < yWorld; i++)
    //     // {
    //     //     for (int j = 0; j < currentStructureCount; j++)
    //     //     {
    //     //         if (currentVillage.listOfStructures[j].tileValue == 2f)
    //     //         {
    //     //             return true;
    //     //         }
    //     //     }
    //     // }

    //     for (int i = 0; i < currentStructureCount; i++)
    //     {
    //         // Debug.Log("yCoordinate: " + currentVillage.listOfStructures[i].yCoordinate);
    //         // Debug.Log("yCenter: " + yCenter);
    //         // Debug.Log("yWorld: " + radius);
    //         // Debug.Log("+Y: " + (currentVillage.listOfStructures[i].yCoordinate > yCenter && currentVillage.listOfStructures[i].yCoordinate < radius));
    //         // Debug.Log("-Y: " + (currentVillage.listOfStructures[i].yCoordinate < yCenter && currentVillage.listOfStructures[i].yCoordinate > radius));
    //         // Debug.Log("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

    //         // var temp1 = yCenter + radius;
    //         // var temp2 = yCenter - radius;

    //         // if (currentVillage.listOfStructures[i].yCoordinate > yCenter && currentVillage.listOfStructures[i].yCoordinate < positivBound || currentVillage.listOfStructures[i].yCoordinate < yCenter && currentVillage.listOfStructures[i].yCoordinate > negativeBound)
    //         // {
    //         //     return true;
    //         // }

    //         if (currentVillage.listOfStructures[i].yCoordinate > yCenter || currentVillage.listOfStructures[i].yCoordinate < yCenter)
    //         {
    //             return true;
    //         }
    //     }
    //     // }

    //     // if (yWorld < yCenter)
    //     // {
    //     // loopStart = yWorld;
    //     // loopEnd = yCenter;

    //     //     for (int i = yWorld; i < yCenter; i++)
    //     //     {
    //     //         for (int j = 0; j < currentStructureCount; j++)
    //     //         {
    //     //             if (currentVillage.listOfStructures[j].tileValue == 2f)
    //     //             {
    //     //                 return true;
    //     //             }
    //     //         }
    //     //     }
    //     // }

    //     // for (int i = loopStart; i < loopEnd; i++)
    //     // {
    //     //     for (int j = 0; j < currentStructureCount; j++)
    //     //     {
    //     //         if (currentVillage.listOfStructures[j].tileValue == 2f)
    //     //         {
    //     //             return true;
    //     //         }
    //     //     }
    //     // }

    //     return false;
    // }

    // private bool HasStructureToRightOrLeft(int xWorld, int currentStructureCount)
    // {
    //     if (currentVillage.listOfStructures.Length == 0)
    //     {
    //         return false;
    //     }

    //     for (int i = 0; i < currentStructureCount; i++)
    //     {
    //         if (currentVillage.listOfStructures[i].xCoordinate == xWorld - 1 || currentVillage.listOfStructures[i].xCoordinate == xWorld + 1)
    //         {
    //             return true;
    //         }
    //     }

    //     return false;
    // }

    private bool HasDirectNeighbour(int xWorld, int yWorld, int currentStructureCount)
    {
        if (currentVillage.listOfStructures.Length == 0)
        {
            return false;
        }

        for (int i = 0; i < currentStructureCount; i++)
        {
            if (currentVillage.listOfStructures[i].xCoordinate == xWorld - 1 || currentVillage.listOfStructures[i].xCoordinate == xWorld + 1 || currentVillage.listOfStructures[i].yCoordinate == yWorld - 1 || currentVillage.listOfStructures[i].yCoordinate == yWorld + 1)
            {
                return true;
            }
        }

        return false;
    }

    private bool OnCenterPathCross(int xWorld, int yWorld)
    {
        foreach (var pathTuple in currentVillage.listOfPaths)
        {
            if (new Point_T(xWorld, yWorld, null).PointInList(pathTuple.path))
            {
                return true;
            }
        }

        return false;
    }

    private void CenterPathCross(int xCenter, int yCenter, int radius, Village_T village)
    {
        List<Point_T> path = new List<Point_T>();
        var xStart = xCenter - radius;
        var yStart = yCenter - radius;
        var xEnd = xCenter + radius + 1;
        var yEnd = yCenter + radius + 1;

        // foreach (var structure in village.listOfStructures)
        // {
        //     if (structure.xCoordinate >= xStart && structure.xCoordinate < xCenter)
        //     {
        //         xStart = structure.xCoordinate + 1;
        //     }

        //     if (structure.xCoordinate <= xEnd && structure.xCoordinate > xCenter)
        //     {
        //         xEnd = structure.xCoordinate - 1;
        //     }

        //     if (structure.yCoordinate >= yStart && structure.yCoordinate < yCenter)
        //     {
        //         yStart = structure.yCoordinate + 1;
        //     }

        //     if (structure.yCoordinate <= yEnd && structure.yCoordinate > yCenter)
        //     {
        //         yEnd = structure.yCoordinate - 1;
        //     }
        // }

        for (int x = xStart; x < xEnd; x++)
        {
            if (x == xCenter)
            {
                village.listOfPaths.Add(("Left", path));
                path = new List<Point_T>();
                continue;
            }

            villageGrid[x, yCenter] = 3f;
            path.Add(new Point_T(x, yCenter, null));
        }

        village.listOfPaths.Add(("Right", path));
        path = new List<Point_T>();

        for (int y = yStart; y < yEnd; y++)
        {
            if (y == yCenter)
            {
                village.listOfPaths.Add(("Down", path));
                path = new List<Point_T>();
                continue;
            }

            villageGrid[xCenter, y] = 3f;
            path.Add(new Point_T(xCenter, y, null));
        }

        village.listOfPaths.Add(("Up", path));
    }

    [TabGroup("Button")]
    [Button("Draw Village")]
    public void DrawVillage()
    {
        foreach (var village in villageList)
        {
            foreach (var structure in village.listOfStructures)
            {
                switch (structure.tileValue)
                {
                    case 1f:
                        Grid_Village.SetTile(new Vector3Int(structure.xCoordinate, structure.yCoordinate, 0), TestTile_1);
                        break;
                    case 2f:
                        Grid_Village.SetTile(new Vector3Int(structure.xCoordinate, structure.yCoordinate, 0), TestTile_2);
                        break;
                }
            }

            foreach (var path in village.listOfPaths)
            {
                foreach (var point in path.path)
                {
                    Grid_Path.SetTile(new Vector3Int(point.xCoordinate, point.yCoordinate, 0), TestTile_3);
                }
            }
        }
    }

    [TabGroup("Button")]
    [Button("Draw Center Path Cross")]
    public void DrawCenterPathCross(int xCenter, int yCenter, int radius)
    {
        var xStart = xCenter - radius;
        var yStart = yCenter - radius;
        var xEnd = xCenter + radius + 1;
        var yEnd = yCenter + radius + 1;

        for (int x = xStart; x < xEnd; x++)
        {
            if (x == xCenter)
            {
                continue;
            }

            Grid_Path.SetTile(new Vector3Int(x, yCenter, 0), TestTile_3);
        }

        for (int y = yStart; y < yEnd; y++)
        {
            if (y == yCenter)
            {
                continue;
            }

            Grid_Path.SetTile(new Vector3Int(xCenter, y, 0), TestTile_3);
        }
    }

    [TabGroup("Button")]
    [Button("Clear")]
    public void ClearTileMaps()
    {
        Grid_Village.ClearAllTiles();
        Grid_Path.ClearAllTiles();
        Grid_Test.ClearAllTiles();

        villageList = new List<Village_T>();
    }

    private bool IsHouseInRadius(int radius, int xPosition, int yPosition)
    {
        return radius * radius >= xPosition * xPosition + yPosition * yPosition;
    }

    // private void ShowSquare(int xCenter, int yCenter, int radius)
    // {
    //     print("Show Square");
    //     var xStart = xCenter - radius;
    //     var yStart = yCenter - radius;
    //     var xEnd = xCenter + radius + 1;
    //     var yEnd = yCenter + radius + 1;
    //     print("X: " + xStart + ", Y: " + yStart + ", End X: " + xEnd + ", End Y: " + yEnd);
    //     for (int x = xStart; x < xEnd; x++)
    //     {
    //         for (int y = yStart; y < yEnd; y++)
    //         {
    //             if (x == xCenter && y == yCenter)
    //             {
    //                 Grid_Test.SetTile(new Vector3Int(x, y, 0), TestTile_1);
    //             }
    //             else
    //             {
    //                 Grid_Test.SetTile(new Vector3Int(x, y, 0), TestTile_3);
    //             }
    //             print("Tile drawn");
    //         }
    //     }
    // }


    private List<Point_T> FindPath(Point_T start, Point_T end)
    {
        // Debug.Log("Entered FindPaht");
        Point_T newStart;
        Point_T newEnd;
        var usedList = new List<Point_T>();

        var directionDifferences = start.DirectionDifferenceBetweenPoints(end);

        if (directionDifferences.y > 0)
        {
            newEnd = end.Offset(0, -1);
        }
        else if (directionDifferences.y < 0)
        {
            newEnd = end.Offset(0, -1);
        }
        else if (directionDifferences.y == 0 && directionDifferences.x > 0)
        {
            newEnd = end.Offset(+1, 0);
        }
        else if (directionDifferences.y == 0 && directionDifferences.x < 0)
        {
            newEnd = end.Offset(-1, 0);
        }
        else
        {
            newEnd = end;
            Debug.Log("Why is it End?");
        }

        if (directionDifferences.x > 0)
        {
            newStart = start.Offset(-1, 0);
        }
        else if (directionDifferences.x < 0)
        {
            newStart = start.Offset(+1, 0);
        }
        else if (directionDifferences.x == 0 && directionDifferences.y > 0)
        {
            newStart = start.Offset(0, -1);
        }
        else if (directionDifferences.x == 0 && directionDifferences.y < 0)
        {
            newStart = start.Offset(0, +1);
        }
        else
        {
            newStart = start;
            Debug.Log("Why is it Start?");
        }

        // Debug.Log("New Start: " + newStart);
        // Debug.Log("New End: " + newEnd);

        var xPositionBound = directionDifferences.x == 0 ? newStart.xCoordinate : directionDifferences.x > 0 ? newStart.xCoordinate : newEnd.xCoordinate;
        var xNegativeBound = directionDifferences.x == 0 ? newStart.xCoordinate : directionDifferences.x > 0 ? newEnd.xCoordinate : newStart.xCoordinate;
        var yPositionBound = directionDifferences.y == 0 ? newStart.yCoordinate : directionDifferences.y > 0 ? newStart.yCoordinate : newEnd.yCoordinate;
        var yNegativeBound = directionDifferences.y == 0 ? newStart.yCoordinate : directionDifferences.y > 0 ? newEnd.yCoordinate : newStart.yCoordinate;

        usedList.Add(start);
        usedList.Add(newStart);
        var neighbours = FindNeighbours(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, directionDifferences.x, directionDifferences.y, newStart);

        // Debug.Log("Neighbour Conut-1: " + neighbours.Count);

        while (neighbours.Count != 0)
        {
            var point = GetCheapestPoint(neighbours, directionDifferences.x, directionDifferences.y);
            usedList.Add(point);
            neighbours = FindNeighbours(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, directionDifferences.x, directionDifferences.y, point);
            // Debug.Log("Neighbour Conut: " + neighbours.Count);
        }

        // usedList.Add(end);
        return usedList;
    }

    private Point_T GetCheapestPoint(List<Point_T> listOfPoints, int xDirectionDifference, int yDirectionDifference)
    {
        Point_T cheapest = listOfPoints.First();

        // foreach (var point in listOfPoints)
        // {
        //     // if (cheapest.cost == point.cost)
        //     // {
        //     //     if (Mathf.Abs(xDirectionDifference) > Mathf.Abs(yDirectionDifference))
        //     //     {
        //     //         // && (point.PointInList(currentVillage.listOfPaths.Find(item => item.name.Equals("Left")).path) || point.PointInList(currentVillage.listOfPaths.Find(item => item.name.Equals("Right")).path))
        //     //         cheapest = point;
        //     //     }

        //     //     if (Mathf.Abs(xDirectionDifference) < Mathf.Abs(yDirectionDifference))
        //     //     {
        //     //         // && (point.PointInList(currentVillage.listOfPaths.Find(item => item.name.Equals("Down")).path) || point.PointInList(currentVillage.listOfPaths.Find(item => item.name.Equals("Up")).path))
        //     //         cheapest = point;
        //     //     }
        //     // }

        //     if (cheapest.cost > point.cost)
        //     {
        //         cheapest = point;
        //     }
        // }

        return cheapest;
    }


    // (int elementCount, List<string> directions, List<Point_T> neighbours)
    private List<Point_T> FindNeighbours(int xPositionBound, int xNegativeBound, int yPositionBound, int yNegativeBound, int xDirectionDifference, int yDirectionDifference, Point_T point)
    {
        // Debug.Log("Enterd FindNeighbour");
        var up = point.Offset(0, 1);
        var down = point.Offset(0, -1);
        var right = point.Offset(1, 0);
        var left = point.Offset(-1, 0);

        up.CalculateCost(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, xDirectionDifference, yDirectionDifference, "Up", villageGrid);
        down.CalculateCost(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, xDirectionDifference, yDirectionDifference, "Down", villageGrid);
        right.CalculateCost(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, xDirectionDifference, yDirectionDifference, "Right", villageGrid);
        left.CalculateCost(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, xDirectionDifference, yDirectionDifference, "Left", villageGrid);

        // Debug.Log("Given Point: " + point.ToString());
        // Debug.Log("x Difference: " + xDirectionDifference);
        // Debug.Log("y Difference: " + yDirectionDifference);

        // Debug.Log("Up: " + up.ToString());
        // Debug.Log("Down: " + down.ToString());
        // Debug.Log("Right: " + right.ToString());
        // Debug.Log("Left: " + left.ToString());

        // var neighbourPoints = new List<Point_T>{up, down, right, left};

        var neighbourPoints = new List<Point_T>();
        // var neighbourDirections = new List<string>();

        if (xDirectionDifference < 0 && right.cost >= 0)
        {
            // Debug.Log("Add Right Neighbour");
            neighbourPoints.Add(right);
        }

        if (xDirectionDifference > 0 && left.cost >= 0)
        {
            // Debug.Log("Add Left Neighbour");
            neighbourPoints.Add(left);
        }

        if (yDirectionDifference < 0 && up.cost >= 0)
        {
            // Debug.Log("Add Up Neighbour");
            neighbourPoints.Add(up);
        }

        // if (yDirectionDifference > 0 && down.cost >= 0)
        // {
        //     // Debug.Log("Add Down Neighbour");
        //     neighbourPoints.Add(down);
        // }

        // if (right.cost >= 0)
        // {
        //     neighbourPoints.Add(right);
        // }

        // if (left.cost >= 0)
        // {
        //     neighbourPoints.Add(left);
        // }

        // if (up.cost >= 0)
        // {
        //     neighbourPoints.Add(up);
        // }

        if (down.cost >= 0)
        {
            neighbourPoints.Add(down);
        }

        // neighbourPoints.Add(up);
        // neighbourPoints.Add(down);
        // neighbourPoints.Add(right);
        // neighbourPoints.Add(left);

        // switch (xDirectionDifference)
        // {
        //     case var _ when xDirectionDifference > 0 && right.cost >= 0:
        //         if (IsWalkable(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, right))
        //         {
        //             neighbours.Add(right);
        //         }
        //         break;
        //     case var _ when xDirectionDifference < 0:
        //         if (IsWalkable(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, left))
        //         {
        //             neighbours.Add(left);
        //         }
        //         break;
        // }

        // switch (yDirectionDifference)
        // {
        //     case var _ when yDirectionDifference > 0:
        //         if (IsWalkable(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, up))
        //         {
        //             neighbours.Add(up);
        //         }
        //         break;
        //     case var _ when yDirectionDifference < 0:
        //         if (IsWalkable(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, down))
        //         {
        //             neighbours.Add(down);
        //         }
        //         break;
        // }

        // foreach (var item in neighbourPoints)
        // {
        //     Debug.Log(item.ToString());
        // }

        // return (neighbourPoints.Count, neighbourDirections, neighbourPoints);
        return neighbourPoints;
    }

    // private bool IsWalkable(int xPositionBound, int xNegativeBound, int yPositionBound, int yNegativeBound, Point_T point)
    // {
    //     if (point.xCoordinate < xNegativeBound || point.xCoordinate > xPositionBound)
    //     {
    //         return false;
    //     }

    //     if (point.yCoordinate < yNegativeBound || point.yCoordinate > yPositionBound)
    //     {
    //         return false;
    //     }

    //     if (villageGrid[point.xCoordinate, point.yCoordinate] == 2f || villageGrid[point.xCoordinate, point.yCoordinate] == 1f)
    //     {
    //         return false;
    //     }

    //     //TODO: Check the Map (World and Flora)
    //     return true;
    // }

    // private void Dijkstra(int xCenter, int yCenter, int radius){
    //     var visitedList = new List<int[]>();
    //     var nodeList = GetAllNodes(xCenter, yCenter, radius);

    //     costTable.Add(new Point_T(xCenter, yCenter, 0, xCenter, yCenter));
    //     visitedList.Add(new int[]{xCenter, yCenter, 0});

    //     foreach (var node in nodeList)
    //     {

    //     }

    // }

    // private List<int[]> GetAllNodes(int xCenter, int yCenter, int radius){
    //     var list = new List<int[]>();

    //     var xStart = xCenter - radius;
    //     var yStart = yCenter - radius;
    //     var xEnd = xCenter + radius + 1;
    //     var yEnd = yCenter + radius + 1;

    //     for (int x = xStart; x < xEnd; x++)
    //     {
    //         for (int y = yStart; y < yEnd; y++)
    //         {
    //             if (x == xCenter && y == yCenter)
    //             {
    //                 continue;
    //             }

    //             list.Add(new int[]{x, y, 0});
    //         }
    //     }

    //     return list;
    // }
}
