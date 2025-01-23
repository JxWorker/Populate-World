using System.Collections.Generic;

public class Point
{
    public int xCoordinate;
    public int yCoordinate;
    public float cost = 0;
    public Point previous;

    public Point(int xCoordinate, int yCoordinate, Point previous)
    {
        this.xCoordinate = xCoordinate;
        this.yCoordinate = yCoordinate;
        this.previous = previous;
    }

    public Point Offset(int offsetX, int offsetY)
    {
        return new Point(xCoordinate + offsetX, yCoordinate + offsetY, this);
    }

    public override bool Equals(object obj)
    {
        Point point = (Point)obj;
        return xCoordinate == point.xCoordinate && yCoordinate == point.yCoordinate;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public (int x, int y) DirectionDifferenceBetweenPoints(Point point)
    {
        return (xCoordinate - point.xCoordinate, yCoordinate - point.yCoordinate);
    }

    public void CalculateCost(int xPositionBound, int xNegativeBound, int yPositionBound, int yNegativeBound, float[,] villageGrid)
    {
        if (xCoordinate < xNegativeBound || xCoordinate > xPositionBound)
        {
            cost = -100;
            return;
        }

        if (yCoordinate < yNegativeBound || yCoordinate > yPositionBound)
        {
            cost = -100;
            return;
        }

        if (villageGrid[xCoordinate, yCoordinate] == 2f || villageGrid[xCoordinate, yCoordinate] == 1f)
        {
            cost = -100;
            return;
        }

        if (villageGrid[xCoordinate, yCoordinate] == 3f)
        {
            cost = 0;
            return;
        }

        cost = 1;
    }

    public bool PointInList(List<Point> list)
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
}