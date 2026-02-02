using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public CellData cellData;


    public void Init(int x, int y)
    {
        cellData.X = x;
        cellData.Y = y;
        gameObject.name = $"Cell_{cellData.X}_{cellData.Y}";
    }
}
