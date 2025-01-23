using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    public void CalculateCost(int xPositionBound, int xNegativeBound, int yPositionBound, int yNegativeBound, int xDirectionDifference, int yDirectionDifference, string directionOfNeighbour, float[,] villageGrid)
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
            cost += 0;
            return;
        }

        if (directionOfNeighbour.Equals("Up"))
        {
            if (yDirectionDifference < 0 && Mathf.Abs(xDirectionDifference) < Mathf.Abs(yDirectionDifference))
            {
                cost += 0.5f;
            }
            else if (yDirectionDifference < 0)
            {
                cost += 2;
            }
            else
            {
                cost = -100;
                return;
            }
        }

        if (directionOfNeighbour.Equals("Down"))
        {
            if (yDirectionDifference > 0 && Mathf.Abs(xDirectionDifference) < Mathf.Abs(yDirectionDifference))
            {
                cost += 0.5f;
            }
            else if (yDirectionDifference > 0)
            {
                cost += 2;
            }
            else
            {
                cost = -100;
                return;
            }
        }

        if (directionOfNeighbour.Equals("Right"))
        {
            if (xDirectionDifference < 0 && Mathf.Abs(xDirectionDifference) > Mathf.Abs(yDirectionDifference))
            {
                cost += 0.5f;
            }
            else if (xDirectionDifference < 0)
            {
                cost += 1;
            }
            else
            {
                cost += 5;
            }
        }

        if (directionOfNeighbour.Equals("Left"))
        {
            if (xDirectionDifference > 0 && Mathf.Abs(xDirectionDifference) > Mathf.Abs(yDirectionDifference))
            {
                cost += 0.5f;
            }
            else if (xDirectionDifference > 0)
            {
                cost += 1;
            }
            else
            {
                cost += 5;
            }
        }

        cost += 1;
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