using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UnitCardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // 이 카드가 어떤 유닛인지 저장하는 데이터
    public string UnitId { get; private set; }
    public Vector2Int Size { get; private set; }
    public Unit UnitType { get; private set; } = Unit.None;
    public bool IsRotated { get; private set; } = false;
    private bool isDragging = false;

    [Header("UI References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI sizeLabel;

    [Header("Drag")]
    [SerializeField] private CanvasGroup canvasGroup;

    private Transform originalParent;
    private Vector3 originalPosition;

    private void Awake()
    {
        // 인스펙터 연결을 안 해도 자동으로 찾게 안전장치
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Update()
    {
        if (!isDragging) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Rotate90();
        }
    }

    public Vector2Int CurrentSize
    {
        get
        {
            // 회전이면 (h,w), 아니면 (w,h)
            return IsRotated ? new Vector2Int(Size.y, Size.x) : Size;
        }
    }
    public void Rotate90()
    {
        IsRotated = !IsRotated;

        // 라벨도 현재 크기로 갱신
        if (sizeLabel != null)
            sizeLabel.text = $"{CurrentSize.x}x{CurrentSize.y}";
        RefreshPreviewIfHoveringCell();
    }

    public void Setup(string unitId, Vector2Int size, Sprite icon, Unit unitType)
    {
        UnitId = unitId;
        Size = size;
        UnitType = unitType;

        // UI 표시 업데이트
        if (sizeLabel != null)
            sizeLabel.text = $"{size.x}x{size.y}";

        if (iconImage != null && icon != null)
            iconImage.sprite = icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 1) 원래 자리 기억
        originalParent = transform.parent;
        originalPosition = transform.position;

        // 2) 드래그 중엔 "레이캐스트(클릭 판정)"를 꺼서
        //    아래 있는 UI(나중엔 셀)로 이벤트가 전달될 수 있게 준비
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;

        // 3) 화면에서 항상 위에 보이게 Canvas 최상단으로 올림
        transform.SetParent(originalParent.root, true);

        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 마우스를 따라 움직이게 함
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 1) 드래그가 끝나면 일단 원래 자리로 되돌림 (지금 단계 목표)
        transform.SetParent(originalParent, true);
        transform.position = originalPosition;

        // 2) 레이캐스트 다시 켬
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;

        isDragging = false;
    }
    private Cell FindCellUnderPointer()
    {
        if (EventSystem.current == null) return null;

        var data = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        for (int i = 0; i < results.Count; i++)
        {
            var cell = results[i].gameObject.GetComponentInParent<Cell>();
            if (cell != null) return cell;
        }

        return null;
    }

    private void RefreshPreviewIfHoveringCell()
    {
        var cell = FindCellUnderPointer();
        if (cell == null) return;

        // cell 안에 이미 board/gridManager가 연결되어 있으니 그대로 사용 가능
        cell.gridManager.ShowPreview(cell.board, new Vector2Int(cell.cellData.x, cell.cellData.y), CurrentSize);
    }

    public void BeginExternalDrag()
    {
        isDragging = true;

        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;
    }

    public void EndExternalDrag()
    {
        isDragging = false;

        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;
    }
}
