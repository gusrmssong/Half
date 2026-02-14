using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Board playerBoard;
    [SerializeField] private Board enemyBoard;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;

    private int nextPlacementId = 0;
    private readonly List<Cell> selectedCells = new List<Cell>();

    private readonly List<Cell> previewCells = new List<Cell>();
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuildSettingGrid();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            BuildGameGrid(playerBoard, GameManager.Instance.playerGridData);
            BuildGameGrid(enemyBoard, GameManager.Instance.enemyGridData);
        }
    }
    public void Select()
    {
        GameManager.Instance.Select();

    }

    public void GameStartAlpha()
    {
        GameManager.Instance.playerBoard = playerBoard;
        GameManager.Instance.enemyBoard = enemyBoard;

        GameManager.Instance.GameStart();
    }
    public void BuildSettingGrid() // 준비 화면 그리드 만들기
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell cell = Instantiate(cellPrefab, playerBoard.transform);
                cell.Init(x, y, Unit.None, playerBoard, this, true);
                
                playerBoard.cells[x, y] = cell;
            }
        }
    }
    public void BuildGameGrid(Board targetBoard, GridData data) // 게임 화면 그리드 만들기
    {
        for (int y = 0; y < data.height; y++)
        {
            for (int x = 0; x < data.width; x++)
            {
                Cell cell = Instantiate(cellPrefab, targetBoard.transform);

                CellData cellData = data.Get(x, y); // 플레이어 또는 적의 GridData에서 cellData 추출

                cell.Init(x, y, cellData.unit, targetBoard, this, false);

                targetBoard.cells[x, y] = cell;
            }
        }
    }

    private void SaveBoardToGameData()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameData가 씬에 없습니다");
            return;
        }
        GameManager.Instance.playerGridData = new GridData(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell cell = playerBoard.cells[x, y];

                GameManager.Instance.playerGridData.Set(x, y, cell.cellData);
                
            }
        }
        Debug.Log("보드 저장 완료");
    }
    public void OnClickStartGame()
    {
        SaveBoardToGameData();
        HalfSceneManager.Instance.SceneGame();
    }

    public bool TryPlaceUnit(Board targetBoard, Vector2Int origin, Vector2Int size, Unit unitType)
    {
        // 1) 범위 체크 + 겹침 체크
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                int gx = origin.x + x;
                int gy = origin.y + y;

                if (gx < 0 || gx >= width || gy < 0 || gy >= height)
                {
                    Debug.Log($"배치 실패(범위 밖): {unitType} origin={origin} size={size} failCell=({gx},{gy})");
                    return false;
                }

                var cell = targetBoard.cells[gx, gy];
                if (cell == null || cell.cellData == null)
                {
                    Debug.Log($"배치 실패(셀/데이터 null): {unitType} at ({gx},{gy})");
                    return false;
                }

                if (cell.cellData.unit != Unit.None)
                {
                    Debug.Log($"배치 실패(겹침): {unitType} at ({gx},{gy}) already={cell.cellData.unit}");
                    return false;
                }
            }
        }
        int pid = nextPlacementId++;
        // 2) 실제로 기록(확정)
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                int gx = origin.x + x;
                int gy = origin.y + y;

                targetBoard.cells[gx, gy].cellData.unit = unitType;
                targetBoard.cells[gx, gy].cellData.placementId = pid;
                Debug.Log($"[WRITE] ({gx},{gy}) = {unitType}");
                targetBoard.cells[gx, gy].UpdateCellSprite();
            }
        }

        Debug.Log($"배치 성공: {unitType} origin={origin} size={size}");
        return true;
    }

    public void ClearPreview()
    {
        for (int i = 0; i < previewCells.Count; i++)
        {
            if (previewCells[i] != null)
                previewCells[i].SetPreviewHighlight(false, true);
        }
        previewCells.Clear();
    }

    public void ShowPreview(Board targetBoard, Vector2Int origin, Vector2Int size)
    {
        ClearPreview();

        bool canPlace = CanPlaceUnit(targetBoard, origin, size);

        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                int gx = origin.x + x;
                int gy = origin.y + y;

                if (gx < 0 || gx >= width || gy < 0 || gy >= height)
                    continue;

                var cell = targetBoard.cells[gx, gy];
                if (cell == null) continue;

                cell.SetPreviewHighlight(true, canPlace);
                previewCells.Add(cell);
            }
        }
    }

    public bool CanPlaceUnit(Board targetBoard, Vector2Int origin, Vector2Int size)
    {
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                int gx = origin.x + x;
                int gy = origin.y + y;

                // 범위 밖이면 불가능
                if (gx < 0 || gx >= width || gy < 0 || gy >= height)
                    return false;

                var cell = targetBoard.cells[gx, gy];
                if (cell == null || cell.cellData == null)
                    return false;

                // 겹치면 불가능
                if (cell.cellData.unit != Unit.None)
                    return false;
            }
        }

        return true;
    }
    public void ClearSelection()
    {
        for (int i = 0; i < selectedCells.Count; i++)
        {
            if (selectedCells[i] != null)
                selectedCells[i].SetHighlight(false);
        }
        selectedCells.Clear();
    }
    public void SelectPlacement(Board targetBoard, int placementId)
    {
        ClearSelection();

        if (placementId < 0) return;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = targetBoard.cells[x, y];
                if (cell == null || cell.cellData == null) continue;

                if (cell.cellData.placementId == placementId)
                {
                    cell.SetHighlight(true);
                    selectedCells.Add(cell);
                }
            }
        }
    }

}


