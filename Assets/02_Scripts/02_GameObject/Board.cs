using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Cell[,] cells = new Cell[10,10];

    public int UpdateCount()
    {
        if (cells == null)
        {
            
            return -1;
        }
       int count = 0;
       for (int y = 0; y < 10; y++)
       {
           for( int x = 0; x < 10; x++)
           {
                Cell cell = cells[x, y];

                if (cell == null) continue;
                if (cell.cellData == null) continue;

               if (cell.cellData.unit != Unit.None)
               {
                    count++;
               }
               if (cell.cellData.isDestroyed == true)
               {
                    count--;
               }
           }
       }
        return count;
    }



}
