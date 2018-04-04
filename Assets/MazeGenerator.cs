using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public Cell[][]         CellMatrix      { get; private set; }
    public Stack<Cell>      BacktrackStack  { get; private set; }
    public Cell             CurrentCell     { get; private set; }
    private System.Random   randomizer;

    public Color    MainCellColor, CurrentCellColor, VisitedCellColor;
    public int      Rows, Columns;

    void AllocateCellMatrix()
    {
        if (Rows <= 0 || Columns <= 0)
        {
            return;
        }

        randomizer  = new System.Random();
        CellMatrix  = new Cell[Rows][];

        for (int i = 0; i < Rows; ++i)
        {
            Cell[] cellRow = new Cell[Columns];

            for (int j = 0; j < Columns; ++j)
            {
                Cell cell = new Cell(i, j, MainCellColor);
                cell.AddAllWalls();
                cellRow[j] = cell;
            }

            CellMatrix[i] = cellRow;
        }
    }

    public bool MatrixContainsUnvisitedCells()
    {
        for (int i = 0; i < Rows; ++i)
        {
            for (int j = 0; j < Columns; ++j)
            {
                if (!CellMatrix[i][j].IsVisited())
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void LoadNeighbors(
        Cell cell,
        List<Cell> neighbors
    )
    {
        int cellRow             = cell.Row;
        int cellColumn          = cell.Column;
        int topNeighborRow      = cellRow + 1;
        int bottomNeighborRow   = cellRow - 1;
        int leftNeighborColumn  = cellColumn - 1;
        int rightNeighborColumn = cellColumn + 1;

        if (topNeighborRow < Rows)
        {
            neighbors.Add(CellMatrix[topNeighborRow][cellColumn]);
        }

        if (rightNeighborColumn < Columns)
        {
            neighbors.Add(CellMatrix[cellRow][rightNeighborColumn]);
        }

        if (bottomNeighborRow >= 0)
        {
            neighbors.Add(CellMatrix[bottomNeighborRow][cellColumn]);
        }

        if (leftNeighborColumn >= 0)
        {
            neighbors.Add(CellMatrix[cellRow][leftNeighborColumn]);
        }
    }

    public static bool ContainsUnvisitedCells(List<Cell> cells)
    {
        for (int i = 0; i < cells.Count; ++i)
        {
            if (!cells[i].IsVisited())
            {
                return true;
            }
        }

        return false;
    }

    public static void RemoveWallsBetween(
        Cell first,
        Cell second
    )
    {
        int rowDifference       = second.Row - first.Row;
        int columnDifference    = second.Column - first.Column;

        if (rowDifference > 0)
        {
            first.RemoveTopWall();
            second.RemoveBottomWall();
        }
        else if (rowDifference < 0)
        {
            first.RemoveBottomWall();
            second.RemoveTopWall();
        }
        else if (columnDifference > 0)
        {
            first.RemoveRightWall();
            second.RemoveLeftWall();
        }
        else if (columnDifference < 0)
        {
            first.RemoveLeftWall();
            second.RemoveRightWall();
        }
    }

	void Awake ()
    {
        AllocateCellMatrix();
        BacktrackStack  = new Stack<Cell>();
        CurrentCell     = CellMatrix[0][0];
        CurrentCell.SetVisited();
    }
	
	void Update ()
    {
        if (MatrixContainsUnvisitedCells())
        {
            List<Cell> currentNeighbors = new List<Cell>();
            LoadNeighbors(CurrentCell, currentNeighbors);

            if (ContainsUnvisitedCells(currentNeighbors))
            {
                int neighborCount   = currentNeighbors.Count;
                Cell neighbor       = currentNeighbors[randomizer.Next(0, neighborCount)];

                BacktrackStack.Push(CurrentCell);

                RemoveWallsBetween(CurrentCell, neighbor);

                CurrentCell = neighbor;
                CurrentCell.SetVisited();
            }
            else if (BacktrackStack.Count > 0)
            {
                Cell topCell    = BacktrackStack.Pop();
                CurrentCell     = topCell;
            }
        }

        for (int i = 0; i < Rows; ++i)
        {
            for (int j = 0; j < Columns; ++j)
            {
                Cell cell       = CellMatrix[i][j];
                cell.WallColor  = cell == CurrentCell ? CurrentCellColor : (cell.IsVisited() ? VisitedCellColor : MainCellColor);
                cell.Render();
            }
        }
    }
};