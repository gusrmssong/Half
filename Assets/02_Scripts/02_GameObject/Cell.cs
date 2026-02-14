using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class Cell : MonoBehaviour, IPointerClickHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
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
        this.cellData.placementId = -1;
        
        UpdateCellSprite();
    }

    private void Awake()
    {

        if (cellData == null)
        {
            cellData = new CellData(Unit.None);
        }
    }

    private void Update()
    {
        
    }

    #region 셀 설정

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

    #endregion
    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.CellSelect(this);
        Debug.Log($"[CELL CLICK] ({cellData.x},{cellData.y}) unit={cellData.unit} pid={cellData.placementId}");
        gridManager.SelectPlacement(board, cellData.placementId);

    }

    public void OnDrop(PointerEventData eventData)
    {
        gridManager.ClearPreview();
        // 드롭한 대상이 UnitCardUI인지 확인
        var card = eventData.pointerDrag != null ? eventData.pointerDrag.GetComponent<UnitCardUI>() : null;
        if (card == null) return;

        // 준비 화면은 playerBoard에 배치한다고 가정
        bool placed = gridManager.TryPlaceUnit(board, new Vector2Int(cellData.x, cellData.y), card.CurrentSize, card.UnitType);

        if (placed)
        {
            // 성공하면 패널에서 사라지게(지금 단계 목표)
            card.gameObject.SetActive(false);
        }
        // 실패하면 UnitCardUI의 OnEndDrag가 원래 자리로 되돌려줌(지금 구조 그대로)


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        var card = eventData.pointerDrag.GetComponent<UnitCardUI>();
        if (card == null) return;

        gridManager.ShowPreview(board, new Vector2Int(cellData.x, cellData.y), card.CurrentSize);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gridManager.ClearPreview();
    }

    public void SetPreviewHighlight(bool on, bool canPlace)
    {
        isHighlight = on;

        if (outlineSprite != null)
        {
            outlineSprite.gameObject.SetActive(on);
            outlineSprite.color = canPlace ? Color.green : Color.red;
        }
    }
}
