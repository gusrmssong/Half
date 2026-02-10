using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] public Cell currentCell;
    public Enemy enemy;
    public Cell enemyCell;
    public UIManager uiManager;
    [SerializeField] public Board currentBoard;

    [SerializeField] public Board playerBoard;
    [SerializeField] public Board enemyBoard;

    public GridData playerGridData;
    public GridData enemyGridData;

    public Turn nowTurn;
    public int turnCount;
    private void Awake()
    {
        if( Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        
    }
    private void Update()
    {
        CellUpdate();

        if (Input.GetKeyDown(KeyCode.N))
        {
            SetEnemyGridData();
        }
    }

    #region 게임
    public enum Turn
    {
        None,
        Player,
        Enemy
    }

    public void GameStart()
    {
        if(playerBoard == null)
        {
            Debug.Log("플레이어 보드 주소를 받아오지 못함, 게임 시작 불가");
            return;
        }
        Debug.Log("그리드 매니저에서 보드 주소를 주입 받아, 게임이 시작됨");
        turnCount = 1;
        uiManager.TextUpdate(playerBoard.UpdateCount(), enemyBoard.UpdateCount());
        TurnChange();
    }
    public void Endgame()
    {
        Debug.Log("게임 종료");

        if(playerBoard.UpdateCount() != 0)
        {
            Debug.Log("플레이어 승리");
        }
        else
        {
            Debug.Log("플레이어 패배");
        }
    }

    public void TurnChange()
    {
        if (turnCount % 2 == 1)
        {
            nowTurn = Turn.Player;
            Debug.Log($"플레이어의 턴, 현재 턴: {turnCount}");

        }
        else
        {
            nowTurn = Turn.Enemy;
            Debug.Log($"적의 턴, 현재 턴: {turnCount}");

            enemy.StartAttack();
        }
                
    }
    public void Select()
    {
        if(nowTurn == Turn.Enemy)
        {
            Debug.Log("적의 턴임,   ");
            return;
        }
        if(currentCell == null)
        {
            Debug.Log("선택된 셀이 없음");
            return;
        }
        if(currentBoard == playerBoard)
        {
            Debug.Log("적의 보드만 선택해야 함");
            return;
        }
        if(currentCell.cellData.isRevealed == true)
        {
            Debug.Log("밝혀지지 않은 셀을 선택해야 함");
            return;
        }
        Debug.Log($"선택된 셀은 {currentCell.name}");
        Check(currentCell);
    }
    public void EnemySelect()
    {
        if (enemyCell == null)
        {
            Debug.Log("선택된 셀이 없음");
            return;
        }
        if (enemyCell.cellData.isRevealed == true)
        {
            Debug.Log("밝혀지지 않은 셀을 선택해야 함");
            return;
        }
        Debug.Log($"선택된 셀은 {enemyCell.name}");
        Check(enemyCell);
    }


    public void Check(Cell cell)
    {
        if(cell.cellData.unit != Unit.None)
        {
            cell.Reveal(true);
            cell.Destory(true);
            Debug.Log($"셀의 유닛은 {cell.cellData.unit}, 파괴됨");
        }
        else
        {
            cell.Reveal(true);
            Debug.Log("셀에는 아무것도 들어있지 않았다");
        }
        turnCount++;
        if(CheckUnitCount() == true)
        {
            Debug.Log($"플레이어 남은 유닛: {playerBoard.UpdateCount()} 적의 남은 유닛 : {enemyBoard.UpdateCount()}");
            Endgame();
            return;
        }
        
        TurnChange();
    }
    public bool CheckUnitCount()
    {
        if (playerBoard == null)
        {
            return false;
        }
        if (enemyBoard == null)
        {
            return false;
        }
        int a = playerBoard.UpdateCount();
        int b = enemyBoard.UpdateCount();
        if (a <= 0)
        {
            return true;
        }
        if(b <= 0)
        {
            return true;
        }
        uiManager.TextUpdate(a, b);
        return false;
    }

    #endregion

    private void SetEnemyGridData()
    {
        Instance.enemyGridData = EnemyGridPreset.CreatePresetA();
        Debug.Log("적 그리드 프리셋 주입 완료");

    }
    #region 셀 상호작용

    public void CellSelect(Cell cell)
    {
        if (currentCell == cell)
        {
            currentCell.SetHighlight(false);
            currentCell = null;
            return;
        }
        if (currentCell != null)
        {
            currentCell.SetHighlight(false);
        }
        currentCell = cell;
        if (currentCell != null)
        {
            currentCell.SetHighlight(true);
            currentBoard = cell.board;
        }
        else
        {
            currentBoard = null;
        }
    }
    public void ClearCellSelection()
    {
        CellSelect(null);
    }
    public void CellSelectVertical(float v)
    {
        if (currentBoard.cells[0, 0] == null)
        {
            return;
        }
        if (currentCell == null)
        {
            CellSelect(currentBoard.cells[4, 4]);
            return;
        }
        int x = currentCell.cellData.x;
        int y = currentCell.cellData.y;

        if (v > 0)
        {
            if (y >= 9)
            {
                return;
            }
            CellSelect(currentBoard.cells[x, y + 1]);
        }
        if (v < 0)
        {
            if (y <= 0)
            {
                return;
            }
            CellSelect(currentBoard.cells[x, y - 1]);
        }
    }

    public void CellSelectHorizontal(float h)
    {
        if (currentBoard.cells[0, 0] == null)
        {
            return;
        }
        if (currentCell == null)
        {
            CellSelect(currentBoard.cells[4, 4]);
        }
        int x = currentCell.cellData.x;
        int y = currentCell.cellData.y;

        if (h > 0)
        {
            if (x >= 9)
            {
                return;
            }
            CellSelect(currentBoard.cells[x + 1, y]);
        }
        if (h < 0)
        {
            if (x <= 0)
            {
                return;
            }
            CellSelect(currentBoard.cells[x - 1, y]);
        }
    }
    bool isMoving = false;
    public void StartMove(float h, float v)
    {
        if (isMoving) return;
        StartCoroutine(MoveRoutine());

        if (h != 0 || v != 0)
        {
            CellSelectVertical(v);
            CellSelectHorizontal(h);
        }

    }
    IEnumerator MoveRoutine()
    {
        isMoving = true;
        yield return new WaitForSeconds(0.15f);
        isMoving = false;
    }
    void CellUpdate()
    {
        if (currentCell != null)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                currentCell.Reveal();
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                currentCell.Destory();
            }
            if (Input.GetKey(KeyCode.C))
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    currentCell.SetUnit(0);
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    currentCell.SetUnit(1);
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    currentCell.SetUnit(2);
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    currentCell.SetUnit(3);
                }
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                currentCell.SetHighlight(!currentCell.isHighlight);
            }
        }
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h != 0 || v != 0)
        {
            StartMove(h, v);
        }

    }
    #endregion

    
}
