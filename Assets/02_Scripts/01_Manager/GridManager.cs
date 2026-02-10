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
}


