using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VillageGenerator : MonoBehaviour
{
    public float[,] villageGrid;
    private List<Village> villageList;
    private Village currentVillage;

    public void GenerateVillages(float[,] world, int worldSize)
    {
        villageGrid = new float[worldSize, worldSize];
        villageList = new List<Village>();
        int numberOfVillages;

        if (worldSize < 100)
        {
            numberOfVillages = 1;
        }
        else if (worldSize < 1000)
        {
            numberOfVillages = worldSize / 100;
        }
        else
        {
            numberOfVillages = (int)(worldSize / 100 * 1.5f);
        }

        Debug.Log(numberOfVillages);

        for (int i = 0; i < numberOfVillages; i++)
        {
            GenerateVillage(worldSize);
        }


        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                if (world[x, y] == -1f)
                {
                    villageGrid[x, y] = 0f;
                    continue;
                }
            }
        }
    }

    private void GenerateVillage(int worldSize)
    {
        int[,] directions = { { +1, +1 }, { +1, -1 }, { -1, +1 }, { -1, -1 } };

        int villageRadius = Random.Range(5, 21);
        int xCenter = Random.Range(villageRadius + 1, worldSize - villageRadius);
        int yCenter = Random.Range(villageRadius + 1, worldSize - villageRadius);
        int numberOfHouses = Random.Range(villageRadius * 2 / 3, villageRadius - 1);

        currentVillage = new Village(xCenter, yCenter, numberOfHouses + 1);

        CenterPathCross(xCenter, yCenter, villageRadius, currentVillage);

        var count = 0;

        while (count < numberOfHouses)
        {
            var xdirection = directions[count % 4, 0];
            var ydirection = directions[count % 4, 1];
            var x = Random.Range(1, villageRadius + 1);
            var y = Random.Range(1, villageRadius + 1);

            var xWorld = xCenter + x * xdirection;
            var yWorld = yCenter + y * ydirection;

            if (!IsHouseInRadius(villageRadius, x, y) || xWorld > worldSize || yWorld > worldSize || xWorld < 0 || yWorld < 0)
            {
                continue;
            }

            if (OnCenterPathCross(xWorld, yWorld))
            {
                continue;
            }

            if (HasDirectNeighbour(xWorld, yWorld, count))
            {
                continue;
            }

            var houseValue = RandomHouse();
            currentVillage.listOfStructures[count] = new Structure(xWorld, yWorld, houseValue);
            villageGrid[xWorld, yWorld] = houseValue;
            count++;
        }

        var centerValue = RandomCenter();
        currentVillage.listOfStructures[numberOfHouses] = new Structure(xCenter, yCenter, centerValue);

        var structureCount = 0;
        foreach (var structure in currentVillage.listOfStructures)
        {
            if (structure.xCoordinate == xCenter && structure.yCoordinate == yCenter)
            {
                continue;
            }

            var path = FindPath(new Point(xCenter, yCenter, null), new Point(structure.xCoordinate, structure.yCoordinate, null));

            foreach (var point in path)
            {
                villageGrid[point.xCoordinate, point.yCoordinate] = 3f;
            }

            currentVillage.listOfPaths.Add(("Structure " + structureCount, path));
            structureCount++;
        }

        villageGrid[xCenter, yCenter] = centerValue;
        villageList.Add(currentVillage);
    }

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
            if (new Point(xWorld, yWorld, null).PointInList(pathTuple.path))
            {
                return true;
            }
        }

        return false;
    }

    private void CenterPathCross(int xCenter, int yCenter, int radius, Village village)
    {
        List<Point> path = new List<Point>();

        var xStart = xCenter - radius;
        var yStart = yCenter - radius;
        var xEnd = xCenter + radius + 1;
        var yEnd = yCenter + radius + 1;

        for (int x = xStart; x < xEnd; x++)
        {
            if (x == xCenter)
            {
                village.listOfPaths.Add(("Left", path));
                path = new List<Point>();
                continue;
            }

            villageGrid[x, yCenter] = 3f;
            path.Add(new Point(x, yCenter, null));
        }

        village.listOfPaths.Add(("Right", path));
        path = new List<Point>();

        for (int y = yStart; y < yEnd; y++)
        {
            if (y == yCenter)
            {
                village.listOfPaths.Add(("Down", path));
                path = new List<Point>();
                continue;
            }

            villageGrid[xCenter, y] = 3f;
            path.Add(new Point(xCenter, y, null));
        }

        village.listOfPaths.Add(("Up", path));
    }

    private bool IsHouseInRadius(int radius, int xPosition, int yPosition)
    {
        return radius * radius >= xPosition * xPosition + yPosition * yPosition;
    }

    private List<Point> FindPath(Point start, Point end)
    {
        Point newStart;
        Point newEnd;
        var usedList = new List<Point>();

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
        }

        var xPositionBound = directionDifferences.x == 0 ? newStart.xCoordinate : directionDifferences.x > 0 ? newStart.xCoordinate : newEnd.xCoordinate;
        var xNegativeBound = directionDifferences.x == 0 ? newStart.xCoordinate : directionDifferences.x > 0 ? newEnd.xCoordinate : newStart.xCoordinate;
        var yPositionBound = directionDifferences.y == 0 ? newStart.yCoordinate : directionDifferences.y > 0 ? newStart.yCoordinate : newEnd.yCoordinate;
        var yNegativeBound = directionDifferences.y == 0 ? newStart.yCoordinate : directionDifferences.y > 0 ? newEnd.yCoordinate : newStart.yCoordinate;

        usedList.Add(start);
        usedList.Add(newStart);
        var neighbours = FindNeighbours(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, directionDifferences.x, directionDifferences.y, newStart);


        while (neighbours.Count != 0)
        {
            var point = GetCheapestPoint(neighbours, directionDifferences.x, directionDifferences.y);
            usedList.Add(point);
            neighbours = FindNeighbours(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, directionDifferences.x, directionDifferences.y, point);
        }

        return usedList;
    }

    private Point GetCheapestPoint(List<Point> listOfPoints, int xDirectionDifference, int yDirectionDifference)
    {
        Point cheapest = listOfPoints.First();

        foreach (var point in listOfPoints)
        {
            if (cheapest.cost == point.cost)
            {
                if (Mathf.Abs(xDirectionDifference) > Mathf.Abs(yDirectionDifference))
                {
                    cheapest = point;
                }

                if (Mathf.Abs(xDirectionDifference) < Mathf.Abs(yDirectionDifference))
                {
                    cheapest = point;
                }
            }

            if (cheapest.cost > point.cost)
            {
                cheapest = point;
            }
        }

        return cheapest;
    }

    private List<Point> FindNeighbours(int xPositionBound, int xNegativeBound, int yPositionBound, int yNegativeBound, int xDirectionDifference, int yDirectionDifference, Point point)
    {
        var up = point.Offset(0, 1);
        var down = point.Offset(0, -1);
        var right = point.Offset(1, 0);
        var left = point.Offset(-1, 0);

        up.CalculateCost(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, villageGrid);
        down.CalculateCost(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, villageGrid);
        right.CalculateCost(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, villageGrid);
        left.CalculateCost(xPositionBound, xNegativeBound, yPositionBound, yNegativeBound, villageGrid);

        var neighbourPoints = new List<Point>();

        if (xDirectionDifference < 0 && right.cost >= 0)
        {
            neighbourPoints.Add(right);
        }

        if (xDirectionDifference > 0 && left.cost >= 0)
        {
            neighbourPoints.Add(left);
        }

        if (yDirectionDifference < 0 && up.cost >= 0)
        {
            neighbourPoints.Add(up);
        }

        if (yDirectionDifference > 0 && down.cost >= 0)
        {
            neighbourPoints.Add(down);
        }

        return neighbourPoints;
    }

    private float RandomHouse()
    {
        int houseVariant = Random.Range(1, 3);
        float value;

        switch (houseVariant)
        {
            case 1:
                value = 2.1f;
                break;
            case 2:
                value = 2.2f;
                break;
            default:
                value = 2.1f;
                break;
        }

        return value;
    }

    private float RandomCenter()
    {
        int centerVariant = Random.Range(1, 6);
        float value;

        switch (centerVariant)
        {
            case 1:
                value = 1.1f;
                break;
            case 2:
                value = 1.2f;
                break;
            case 3:
                value = 1.3f;
                break;
            case 4:
                value = 1.4f;
                break;
            case 5:
                value = 1.5f;
                break;
            default:
                value = 1.1f;
                break;
        }

        return value;
    }
}
