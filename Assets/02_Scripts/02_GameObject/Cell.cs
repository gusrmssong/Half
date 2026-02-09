using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public int x;
    [SerializeField] public int y;

    [SerializeField] public bool isObstacle = false;
    [SerializeField] public bool isRevealed = false;
    [SerializeField] public bool isDestroyed = false;
    [SerializeField] public bool isHighlight = false;
    [SerializeField] public Unit unit;

    [SerializeField] public Image unitSprite;
    [SerializeField] public Image destroySprite;
    [SerializeField] public Image revealSprite;
    [SerializeField] public Image outlineSprite;

    [SerializeField] public Sprite[] unitSprites;

    [SerializeField] public GridManager gridManager;
    [SerializeField] public Board board;
    public void Init(int x, int y, Unit unit, Board board, GridManager gridManager)
    {
        this.x = x;
        this.y = y;
        this.unit = unit;
        gameObject.name = $"Cell_{x}_{y}";
        this.board = board; 
        this.gridManager = gridManager;
        UpdateCellSprite();
    }

    private void Awake()
    {
        unitSprite = GetComponent<Image>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            Reveal();
        }
    }

    public void Reveal()
    {
        isRevealed = !isRevealed;
        UpdateCellSprite();
    }
    public void Reveal(bool isRevealed)
    {
        this.isRevealed = isRevealed;
        UpdateCellSprite();
    }
    public void Destory()
    {
        isDestroyed = !isDestroyed;
        UpdateCellSprite();
    }
    public void SetUnit(int input)
    {
        unit = (Unit)input;
        UpdateCellSprite();
    }
    public void SetHighlight(bool selected)
    {
        isHighlight = selected;
        UpdateCellSprite();
    }

    public void UpdateCellSprite()
    {
        int num = (int)unit;
        if (num >= 0 && num < unitSprites.Length)
        {
            unitSprite.sprite = unitSprites[num];
        }
        else
        {
            Debug.Log("해당하는 유닛 스프라이트가 없습니다");
        }
        revealSprite.gameObject.SetActive(!isRevealed);
        destroySprite.gameObject.SetActive(isDestroyed);
        outlineSprite.gameObject.SetActive(isHighlight);

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Player.Instance.CellSelect(this);


    }


}
