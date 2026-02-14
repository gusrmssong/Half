using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellA : MonoBehaviour, IPointerClickHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
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

    }

    public void OnDrop(PointerEventData eventData)
    {
        gridManager.ClearPreview();
        UnitCardUI card = eventData.pointerDrag != null ? eventData.pointerDrag.GetComponent<UnitCardUI>() : null;
        if (card == null) return;

        bool placed = gridManager.TryPlaceUnit(board, new Vector2Int(cellData.x, cellData.y), card.Size, card.UnitType);

        if(placed)
        {
            card.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        UnitCardUI card = eventData.pointerDrag.GetComponent<UnitCardUI>();
        if(card == null) return;

        gridManager.ShowPreview(board, new Vector2Int(cellData.x, cellData.y), card.Size);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gridManager.ClearPreview();
    }
}
