using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManagerA : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Board playerBoard;
    [SerializeField] private Board enemyBoard;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;

    private readonly List<Cell> previewCells = new List<Cell>();

    private void SaveBoardToGameData()
    {
        if (GameManager.Instance == null)
        {
            return;
        }
        GameManager.Instance.playerGridData = new GridData(10, 10);

        for (int y = 0; y < 10; y++) 
        {
            for (int x = 0; x < 10; x++)
            {
                Cell cell = playerBoard.cells[x, y];

                GameManager.Instance.playerGridData.Set(x, y, cell.cellData);
            }
        }
    }

    public bool TryPlaceUnit(Board targetBoard, Vector2Int origin, Vector2Int size, Unit unitType)
    {
        for(int y = 0; y <size.y; y++)
        {
            for(int x = 0; x < size.x; x++)
            {
                int gx = origin.x + x;
                int gy = origin.y + y;

                if(gx < 0 || gx >= width || gy < 0 || gy >= height)
                {
                    return false;
                }

                Cell cell = targetBoard.cells[gx, gy];
                if(cell == null || cell.cellData == null)
                {
                    return false;
                }

                if( cell.cellData.unit != Unit.None)
                {
                    return false;
                }

            }
        }

        for (int y = 0; y<size.y; y++)
        {
            for (int x = 0;x < size.x; x++)
            {
                int gx = origin.x + x;
                int gy = origin.y + y;

                targetBoard.cells[gx, gy].cellData.unit = unitType;
            }
        }
        return true;
    }

    void ClearPreview()
    {
        for (int i = 0; i < previewCells.Count; i++)
        {
            if (previewCells[i] != null)
            {
                previewCells[i].SetHighlight(false);
            }
        }
        previewCells.Clear();
    }

    void ShowPreview(Board targetBoard, Vector2Int origin, Vector2Int size)
    {
        ClearPreview();

        for(int y = 0; y<size.y;y++)
        {
            for(int x = 0; x < size.x; x++)
            {
                int gx = origin.x + x;
                int gy = origin.y + y;

                if(gx < 0 || gx >= width ||  gy < 0 || gy >= height) continue;

                Cell cell = targetBoard.cells[gx, gy];
                if (cell == null) continue;
                
                cell.SetHighlight(true);
                previewCells.Add(cell);
            }
        }
    }

}
