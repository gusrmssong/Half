using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            BuildOneGrid();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuildGrid();
        }
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
            }
        }
    }

}
