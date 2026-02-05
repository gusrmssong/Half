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
    }

    private int Idx(int x, int y) => y * width + x;
    public CellData Get(int x, int y) => cells[Idx(x, y)];
    public void Set(int x, int y, CellData data) => cells[Idx(x, y)] = data;
   
}

[System.Serializable]
public struct CellData
{
    public Unit unit;



}