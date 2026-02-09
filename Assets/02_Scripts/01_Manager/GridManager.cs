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
 
    private void BuildAlphaGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell cell = Instantiate(cellPrefab, playerBoard.transform);
                cell.Init(x, y, Unit.None, playerBoard, this);
                cell.Reveal(true);
                playerBoard.cells[x, y] = cell;
            }
        }
    }
    private void BuildBetaGrid(Board targetBoard, GridData data)
    {
        for (int y = 0; y < data.height; y++)
        {
            for (int x = 0; x < data.width; x++)
            {
                Cell cell = Instantiate(cellPrefab, targetBoard.transform);

                CellData cd = data.Get(x, y);

                cell.Init(x, y, cd.unit, targetBoard, this);

                cell.isRevealed = false;
                cell.isDestroyed = false;
                cell.isHighlight = false;

                cell.gridManager = this;
                targetBoard.cells[x, y] = cell;
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
    private void EnsureEnemyGrid()
    {
        if (GameData.gameData == null)
        {
            Debug.LogError("GameData가 없습니다. 준비 씬에 GameData 오브젝트가 있어야 합니다.");
            return;
        }

        GameData.gameData.enemyGridData = EnemyGridPreset.CreatePresetA();
        Debug.Log("적 그리드 프리셋 주입 완료");

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
            BuildAlphaGrid();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            BuildBetaGrid(playerBoard, GameData.gameData.playerGridData);
            BuildBetaGrid(enemyBoard, GameData.gameData.enemyGridData);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            EnsureEnemyGrid();
        }



    }
}


