using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance = null;

    [SerializeField] private Cell cellPrefab;
    [SerializeField] private Board playerBoard;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
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
    private void Start()
    {

    }
  

    private void BuildGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell cell = Instantiate(cellPrefab, playerBoard.transform);
                cell.Init(x, y, Unit.None, playerBoard);
                cell.Reveal(true);
                playerBoard.cells[x, y] = cell;
            }
        }
    }
    private void BuildGrid(GridData data)
    {
        for (int y = 0; y < data.height; y++)
        {
            for (int x = 0; x < data.width; x++)
            {
                Cell cell = Instantiate(cellPrefab, playerBoard.transform);

                CellData cd = data.Get(x, y);

                cell.Init(x, y, cd.unit, playerBoard);

                cell.isRevealed = false;
                cell.isDestroyed = false;
                cell.isHighlight = false;

                cell.gridManager = this;
                playerBoard.cells[x, y] = cell;
            }
        }
    }

    private void SaveBoardToGameData()
    {
        GameData.gameData.playerGridData = new GridData(width, height);

        if (GameData.gameData == null)
        {
            Debug.LogError("GameData가 씬에 없습니다");
            return;
        }
        if (GameData.gameData.playerGridData == null)
        {
            GameData.gameData.InitNewGrid(width, height);
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell cell = playerBoard.cells[x, y];

                GameData.gameData.playerGridData.Set(x, y, new CellData { unit = cell.unit });
                
            }
        }
        Debug.Log("보드 저장 완료: GameData.playerGridData에 복사됨");
    }

    public void OnClickStartGame()
    {
        SaveBoardToGameData();
        HalfSceneManager.Instance.SceneGame();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuildGrid();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            BuildGrid();
        }


    }
}


