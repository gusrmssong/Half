using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public Board playerBoard = null;

    public static Player Instance;

    #region ¼¿ »óÈ£ÀÛ¿ë

    [SerializeField] public Cell currentCell;
    [SerializeField] public Board currentBoard;
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
        if (currentCell != null)
        {
            currentCell.SetHighlight(false);
        }
        currentCell = null;
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
        // ÇöÀç ¼¿ÀÇ ÁÂÇ¥°ª
        int x = currentCell.x;
        int y = currentCell.y;

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
        int x = currentCell.x;
        int y = currentCell.y;

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
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        CellUpdate();
    }
}
