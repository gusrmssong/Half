using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class GridData
{
    public int width;
    public int height;
    public CellData[] cells;

    public GridData(int w, int h)
    {
        width = w;
        height = h;
        cells = new CellData[w * h];

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = new CellData(Unit.None);
        }
    }

    private int Idx(int x, int y)
    {
        return y * width + x;
    }

    public CellData Get(int x, int y)
    {
        return cells[Idx(x, y)];
    }
    public void Set(int x, int y, CellData data)
    {
        cells[Idx(x, y)] = data;
    }
   
}