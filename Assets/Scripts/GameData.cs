using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class GameData
{
    public int Width { get; set; }
    public int Height { get; set; }

    /// <summary>
    /// Array representing the number of bomb a cell of board contains
    /// -1: the cell contains a bomb ; X: number of bomb around the cell
    /// </summary>
    public int[,] bombNumberCells;

    /// <summary>
    /// Array representing the state of a cell
    /// 1: covered ; 2: discovered ; 3: flagged
    /// </summary>
    public int[,] stateCells;

    public GameData(int width, int height)
    {
        Height = height;
        Width = width;

        bombNumberCells = new int[Height, Width];
        stateCells = new int[Height, Width];

        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                bombNumberCells[row, col] = 0;
                stateCells[row, col] = 1;
            }
        }
        
    }
}
