using System;
using UnityEngine;
using GameEngineProfiler;

public class Grid : ILoggable
{
    public readonly int Width;
    public readonly int Length;
    private int[,] _cells;
    private int _cellSize;
    private int _groundHeight;
    public readonly float _gridYOffset;

    public int[,] Cells
    {
        get { return _cells; }
        set { _cells = value; }
    }

    public int CellSize
    {
        get { return _cellSize; }
        set { _cellSize = value; }
    }

    public int GroundHeight
    {
        get { return _groundHeight; }
        set { _groundHeight = value; }
    }

    /// <summary>
    /// Parametrized constructor that also starts the Debug.DrawLine process for visualizing the grid.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="length"></param>
    /// <param name="cellSize"></param>
    /// <param name="groundHeight"></param>
    public Grid(int width, int length, int cellSize, int groundHeight)
    {
        this.Width = width;
        this.Length = length;
        _cellSize = cellSize;
        _cells = new int[width, length];
        _groundHeight = groundHeight;
        _gridYOffset = 0.01f;
        
        int wLen = _cells.GetLength(0);
        int lLen = _cells.GetLength(1);
        
        for (int x = -wLen; x < wLen; x++) // e.g., from -5, 5 world pos
        {
            for (int z = -lLen; z < lLen; z++)
            {
                Vector3 thisCell = new Vector3(x * _cellSize, z * _cellSize, _groundHeight);
                Vector3 westCell = new Vector3(x-1 * _cellSize, z * _cellSize, _groundHeight);
                Vector3 eastCell = new Vector3(x+1 * _cellSize, z * _cellSize, _groundHeight);
                Vector3 northCell = new Vector3(x * _cellSize, z + 1 * _cellSize, _groundHeight);
                Vector3 southCell = new Vector3(x * _cellSize, z - 1 * _cellSize, _groundHeight);
                Debug.DrawLine(thisCell, westCell, Color.red, Single.PositiveInfinity, false);
                Debug.DrawLine(thisCell, eastCell, Color.red, Single.PositiveInfinity, false);
                Debug.DrawLine(thisCell, northCell, Color.red, Single.PositiveInfinity, false);
                Debug.DrawLine(thisCell, southCell, Color.red, Single.PositiveInfinity, false);
            }
        }
        Debug.Log("Created grid!");
    }

    /// <summary>
    /// For the logger interface.
    /// </summary>
    /// <returns></returns>
    public string Log()
    {
        return $"Grid width: " + this.Width + " length: " + this.Length;
    }
}
