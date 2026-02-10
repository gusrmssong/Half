using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGridPreset : MonoBehaviour
{
    public static GridData CreatePresetA()
    {
        GridData g = new GridData(10, 10);

        g.Set(2, 3, new CellData(Unit.UnitB));
        g.Set(3, 3, new CellData(Unit.UnitB));
        g.Set(6, 6, new CellData(Unit.UnitC));

        return g;
    }


}
