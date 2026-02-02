using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CellData
{
    public int x;
    public int y;

    public bool isObstacle;
    public bool isRevealed;
    public Units unit;
    public int X
    {
        get { return x; }
        set { x = value; }
    }
    public int Y
    {
        get { return y; }
        set { y = value; }
    }

}
