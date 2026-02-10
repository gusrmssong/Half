using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CellData
{
    public Unit unit = Unit.None;
    public int x;
    public int y;

    public bool isRevealed = false;
    public bool isDestroyed = false;

    public CellData(Unit unit)
    {
        this.unit = unit;

    }


}
