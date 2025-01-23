using System.Collections.Generic;
using UnityEditor;

public class Point_T
{
    public int xCoordinate;
    public int yCoordinate;
    public float cost = 0;
    public Point_T previous;

    public Point_T(int xCoordinate, int yCoordinate, Point_T previous)
    {
        this.xCoordinate = xCoordinate;
        this.yCoordinate = yCoordinate;
        this.previous = previous;
    }

    public Point_T Offset(int offsetX, int offsetY)
    {
        return new Point_T(xCoordinate + offsetX, yCoordinate + offsetY, this);
    }

    public override bool Equals(object obj)
    {
        Point_T point = (Point_T)obj;
        return xCoordinate == point.xCoordinate && yCoordinate == point.yCoordinate;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public (int x, int y) DirectionDifferenceBetweenPoints(Point_T point)
    {
        return (xCoordinate - point.xCoordinate, yCoordinate - point.yCoordinate);
    }

    public void CalculateCost(int xPositionBound, int xNegativeBound, int yPositionBound, int yNegativeBound, float[,] villageGrid)
    {
        if (villageGrid[xCoordinate, yCoordinate] == 2f || villageGrid[xCoordinate, yCoordinate] == 1f)
        {
            cost = -1;
            return;
        }

        if (xCoordinate < xNegativeBound || xCoordinate > xPositionBound)
        {
            cost = -1;
            return;
        }

        if (yCoordinate < yNegativeBound || yCoordinate > yPositionBound)
        {
            cost = -1;
            return;
        }

        if (villageGrid[xCoordinate, yCoordinate] == 3f)
        {
            cost = 0;
            return;
        }

        cost = 1;
    }

    public bool PointInList(List<Point_T> list)
    {
        foreach (var point in list)
        {
            if (Equals(point))
            {
                return true;
            }
        }

        return false;
    }

    public override string ToString()
    {
        return "[X: " + xCoordinate + ", Y: " + yCoordinate + ", Cost: " + cost + "]";
    }
}