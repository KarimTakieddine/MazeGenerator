using System;
using UnityEngine;

[Flags]
public enum Walls
{
    NONE    = 0x00000000,
    TOP     = 0x00000001,
    RIGHT   = 0x00000002,
    BOTTOM  = 0x00000004,
    LEFT    = 0x00000008
};

[Flags]
public enum VisitedFlags
{
    NONE    = 0x00000000,
    VISITED = 0x00000001
};

public class Cell
{
    public Color        WallColor           { get; set; }
    public Walls        CurrentWalls        { get; set; }
    public VisitedFlags CurrentVisitedFlags { get; set; }
    public int          Row                 { get; set; }
    public int          Column              { get; set; }

    public Cell(
        int row,
        int column,
        Color wallColor
    )
    {
        WallColor           = wallColor;
        CurrentWalls        = Walls.NONE;
        CurrentVisitedFlags = VisitedFlags.NONE;
        Row                 = row;
        Column              = column;
    }

    public bool IsVisited()
    {
        return (CurrentVisitedFlags & VisitedFlags.VISITED) == VisitedFlags.VISITED;
    }

    public void SetVisited()
    {
        CurrentVisitedFlags |= VisitedFlags.VISITED;
    }

    public void AddTopWall()
    {
        CurrentWalls |= Walls.TOP;
    }

    public void AddRightWall()
    {
        CurrentWalls |= Walls.RIGHT;
    }

    public void AddBottomWall()
    {
        CurrentWalls |= Walls.BOTTOM;
    }

    public void AddLeftWall()
    {
        CurrentWalls |= Walls.LEFT;
    }

    public void AddAllWalls()
    {
        AddTopWall();
        AddRightWall();
        AddBottomWall();
        AddLeftWall();
    }

    public void RemoveTopWall()
    {
        CurrentWalls &= ~Walls.TOP;
    }

    public void RemoveRightWall()
    {
        CurrentWalls &= ~Walls.RIGHT;
    }

    public void RemoveBottomWall()
    {
        CurrentWalls &= ~Walls.BOTTOM;
    }

    public void RemoveLeftWall()
    {
        CurrentWalls &= ~Walls.LEFT;
    }

    public void RemoveAllWalls()
    {
        RemoveTopWall();
        RemoveRightWall();
        RemoveBottomWall();
        RemoveLeftWall();
    }

    public void Render()
    {
        if ((CurrentWalls & Walls.TOP) == Walls.TOP)
        {
            Debug.DrawLine(new Vector3(Column, Row + 1), new Vector3(Column + 1, Row + 1), WallColor);
        }

        if ((CurrentWalls & Walls.RIGHT) == Walls.RIGHT)
        {
            Debug.DrawLine(new Vector3(Column + 1, Row + 1), new Vector3(Column + 1, Row), WallColor);
        }

        if ((CurrentWalls & Walls.BOTTOM) == Walls.BOTTOM)
        {
            Debug.DrawLine(new Vector3(Column + 1, Row), new Vector3(Column, Row), WallColor);
        }

        if ((CurrentWalls & Walls.LEFT) == Walls.LEFT)
        {
            Debug.DrawLine(new Vector3(Column, Row), new Vector3(Column, Row + 1), WallColor);
        }
    }
};