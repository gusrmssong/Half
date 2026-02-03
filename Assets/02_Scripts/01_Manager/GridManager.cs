using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Board playerBoard;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;

    [SerializeField] public Cell currentCell;

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

        BuildGrid();
    }
    private void Start()
    {

    }
    private void BuildOneGrid()
    {
        Cell cell = Instantiate(cellPrefab, playerBoard.transform);
    }

    private void BuildGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell cell = Instantiate(cellPrefab, playerBoard.transform);
                cell.Init(x, y, Unit.UnitA);
                cell.gridManager = this;
                playerBoard.cells[x, y] = cell;
            }
        }
    }

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
        if (playerBoard.cells[0,0] == null)
        {
            return;
        }
        if (currentCell == null)
        {
            CellSelect(playerBoard.cells[4, 4]);
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
            CellSelect(playerBoard.cells[x, y + 1]);
        }
        if (v < 0)
        {
            if (y <= 0)
            {
                return;
            }
            CellSelect(playerBoard.cells[x, y - 1]);
        }
    }

    public void CellSelectHorizontal(float h)
    {
        if (playerBoard.cells[0, 0] == null)
        {
            return;
        }
        if (currentCell == null)
        {
            CellSelect(playerBoard.cells[4, 4]);
        }
        int x = currentCell.x;
        int y = currentCell.y;

        if (h > 0)
        {
            if (x >= 9)
            {
                return;
            }
            CellSelect(playerBoard.cells[x + 1, y]);
        }
        if (h < 0)
        {
            if (x <= 0)
            {
                return;
            }
            CellSelect(playerBoard.cells[x - 1, y]);
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

    public void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BuildOneGrid();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuildGrid();
        }

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
}


