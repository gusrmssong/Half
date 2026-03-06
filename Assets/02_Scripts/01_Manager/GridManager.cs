using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Board playerBoard;
    [SerializeField] private Board enemyBoard;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;

    [SerializeField] private GameObject unitCardPrefab;   // 이동용 카드 생성에 사용
    [SerializeField] private Transform dragRoot;          // 보통 Canvas(또는 Canvas 하위 DragLayer)

    private UnitCardUI movingCard = null;
    private Cell hoverCell = null;

    private bool isBuilt = false;

    private int nextPlacementId = 0;
    private readonly List<Cell> selectedCells = new List<Cell>();

    private readonly List<Cell> previewCells = new List<Cell>();
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            BuildSettingGridOnce();
        }

    }
    public void GameStartA()
    {

        GameManager.Instance.playerBoard = playerBoard;
        GameManager.Instance.enemyBoard = enemyBoard;

        GameManager.Instance.SetEnemyGridData();

        BuildGameGrid(playerBoard, GameManager.Instance.playerGridData);
        BuildGameGrid(enemyBoard, GameManager.Instance.enemyGridData);

        GameManager.Instance.GameStart();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            BuildGameGrid(playerBoard, GameManager.Instance.playerGridData);
            BuildGameGrid(enemyBoard, GameManager.Instance.enemyGridData);
        }
        if (movingCard == null) return;

        // 1) 이동용 카드가 마우스를 따라가게
        movingCard.transform.position = Input.mousePosition;

        // 2) 현재 포인터 아래 셀 찾기 (UI 레이캐스트)
        var cell = FindCellUnderPointer();  // 아래 4)에서 추가할 함수
        if (cell != hoverCell)
        {
            hoverCell = cell;
            if (hoverCell != null)
                ShowPreview(hoverCell.board, new Vector2Int(hoverCell.cellData.x, hoverCell.cellData.y), movingCard.CurrentSize);
            else
                ClearPreview();
        }

        // 3) 마우스 버튼을 떼면 배치 시도
        if (Input.GetMouseButtonUp(0))
        {
            if (hoverCell != null)
            {
                bool placed = TryPlaceUnit(
                    hoverCell.board,
                    new Vector2Int(hoverCell.cellData.x, hoverCell.cellData.y),
                    movingCard.CurrentSize,
                    movingCard.UnitType
                );

                if (placed)
                {
                    movingCard.EndExternalDrag();
                    Destroy(movingCard.gameObject);
                    movingCard = null;
                    ClearPreview();
                    hoverCell = null;
                }
                // 실패면 계속 들고 있게 둠(유저가 다른 곳으로 옮기면 됨)
            }
        }
    }

    private void StartGameScene()
    {
        // 보드 주입
        GameManager.Instance.playerBoard = playerBoard;
        GameManager.Instance.enemyBoard = enemyBoard;

        // 플레이어 데이터 없으면 시작 불가
        if (GameManager.Instance.playerGridData == null)
        {
            Debug.LogError("playerGridData가 null이라 게임 시작 불가");
            return;
        }

        // 적 데이터 없으면 프리셋 주입
        if (GameManager.Instance.enemyGridData == null)
        {
            Debug.Log("enemyGridData가 null이라 프리셋 주입");
            GameManager.Instance.SetEnemyGridData();
        }

        Debug.Log($"enemyGridData null? {GameManager.Instance.enemyGridData == null}");

        // 보드 생성
        BuildGameGrid(playerBoard, GameManager.Instance.playerGridData);
        BuildGameGrid(enemyBoard, GameManager.Instance.enemyGridData);

        // 시작
        GameManager.Instance.GameStart();
    }
    private void BuildSettingGridOnce()
    {
        if (isBuilt) return;
        isBuilt = true;

        BuildSettingGrid();
    }

    public void Select()
    {
        GameManager.Instance.Select();

    }

    public void GameStartAlpha()
    {
        GameManager.Instance.playerBoard = playerBoard;
        GameManager.Instance.enemyBoard = enemyBoard;
        GameManager.Instance.SetEnemyGridData();
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

    private bool TryGetPlacementInfo(Board targetBoard, int pid, out Unit unitType, out Vector2Int origin, out Vector2Int size)
    {
        unitType = Unit.None;
        origin = Vector2Int.zero;
        size = Vector2Int.zero;

        int minX = int.MaxValue, minY = int.MaxValue;
        int maxX = int.MinValue, maxY = int.MinValue;
        bool found = false;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = targetBoard.cells[x, y];
                if (cell == null || cell.cellData == null) continue;

                if (cell.cellData.placementId != pid) continue;

                if (!found)
                {
                    unitType = cell.cellData.unit; // 같은 pid면 unitType도 동일하다고 가정
                    found = true;
                }

                minX = Mathf.Min(minX, x);
                minY = Mathf.Min(minY, y);
                maxX = Mathf.Max(maxX, x);
                maxY = Mathf.Max(maxY, y);
            }
        }

        if (!found) return false;

        origin = new Vector2Int(minX, minY);
        size = new Vector2Int(maxX - minX + 1, maxY - minY + 1);
        return true;
    }
    private void ClearPlacement(Board targetBoard, int pid)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var cell = targetBoard.cells[x, y];
                if (cell == null || cell.cellData == null) continue;

                if (cell.cellData.placementId != pid) continue;

                cell.cellData.unit = Unit.None;
                cell.cellData.placementId = -1;
                cell.UpdateCellSprite(); // 표시가 아직 약해도 호출은 유지
            }
        }
    }
    public void BeginMovePlacement(Board targetBoard, int pid, Sprite icon = null, string id = "")
    {
        // 이미 이동 중이면 무시
        if (movingCard != null) return;

        if (pid < 0) return;

        if (!TryGetPlacementInfo(targetBoard, pid, out var unitType, out var oldOrigin, out var size))
            return;

        // 1) 먼저 보드에서 제거(집기)
        ClearPlacement(targetBoard, pid);

        // 2) 이동용 카드 생성
        var go = Instantiate(unitCardPrefab, dragRoot);
        movingCard = go.GetComponent<UnitCardUI>();

        // icon/id는 지금 단계에서는 없어도 되지만, 나중에 패널/아이콘 복구에 유용함
        if (movingCard != null)
        {
            movingCard.Setup(id, size, icon, unitType);
            movingCard.BeginExternalDrag();
            movingCard.transform.position = Input.mousePosition;
        }

        // 선택/프리뷰 상태 정리
        ClearSelection();
        ClearPreview();
        hoverCell = null;
    }
    private Cell FindCellUnderPointer()
    {
        if (EventSystem.current == null) return null;

        var data = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        for (int i = 0; i < results.Count; i++)
        {
            var cell = results[i].gameObject.GetComponentInParent<Cell>();
            if (cell != null) return cell;
        }

        return null;
    }


    public void WinGame()
    {
        HalfSceneManager.Instance.SceneEnding();
    }
    public void LoseGame()
    {
        HalfSceneManager.Instance.SceneMain();
    }
}


