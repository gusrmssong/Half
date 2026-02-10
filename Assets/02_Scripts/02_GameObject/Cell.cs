using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class Cell : MonoBehaviour, IPointerClickHandler
{

    public CellData cellData;

    [SerializeField] public bool isHighlight = false;

    [SerializeField] public Image unitSprite;
    [SerializeField] public Image destroySprite;
    [SerializeField] public Image revealSprite;
    [SerializeField] public Image outlineSprite;

    [SerializeField] public Sprite[] unitSprites;

    [SerializeField] public GridManager gridManager;
    [SerializeField] public Board board;

    public void Init(int x, int y, Unit unit, Board board, GridManager gridManager, bool isRevealed)
    {
        if (cellData == null)
            cellData = new CellData(Unit.None);

        this.cellData.x = x;
        this.cellData.y = y;
        this.cellData.unit = unit;
        gameObject.name = $"Cell_{x}_{y}";
        this.board = board;
        this.cellData.isRevealed = isRevealed;
        this.gridManager = gridManager;
        
        UpdateCellSprite();
    }

    private void Awake()
    {
        unitSprite = GetComponent<Image>();

        if (cellData == null)
        {
            cellData = new CellData(Unit.None);
        }
    }

    private void Update()
    {
        
    }

    public void Reveal()
    {
        cellData.isRevealed = !cellData.isRevealed;
        UpdateCellSprite();
    }
    public void Reveal(bool isRevealed)
    {
        this.cellData.isRevealed = isRevealed;
        UpdateCellSprite();
    }
    public void Destory()
    {
        cellData.isDestroyed = !cellData.isDestroyed;
        UpdateCellSprite();
    }
    public void Destory(bool isDestroyed)
    {
        cellData.isDestroyed = isDestroyed;
        UpdateCellSprite();
    }
    public void SetUnit(int input)
    {
        cellData.unit = (Unit)input;
        UpdateCellSprite();
    }
    public void SetHighlight(bool selected)
    {
        isHighlight = selected;
        UpdateCellSprite();
    }
    public void SetHighlight()
    {
        isHighlight = !isHighlight;
        UpdateCellSprite();
    }

    public void UpdateCellSprite()
    {
        int num = (int)cellData.unit;
        if (num >= 0 && num < unitSprites.Length)
        {
            unitSprite.sprite = unitSprites[num];
        }
        else
        {
            Debug.Log("해당하는 유닛 스프라이트가 없습니다");
        }
        revealSprite.gameObject.SetActive(!cellData.isRevealed);
        destroySprite.gameObject.SetActive(cellData.isDestroyed);
        outlineSprite.gameObject.SetActive(isHighlight);

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.CellSelect(this);


    }


}
