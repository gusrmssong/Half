using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitCardUIA : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string UnitId { get; private set; }
    public Vector2Int Size {  get; private set; }
    public Unit UnitType { get; private set; } = Unit.None;

    private Image iconImage;
    private TextMeshProUGUI sizeLabel;

    private CanvasGroup canvasGroup;

    private Transform originalParent;
    private Vector3 originalPosition;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();
    }

    void Setup(string  unitId, Vector2Int size, Sprite icon, Unit unitType)
    {
        UnitId = unitId;
        UnitType = unitType;
        Size = size;

        if(sizeLabel != null)
        {
            sizeLabel.text = $"{size.x} x {size.y}";
        }    
        if(iconImage != null && icon != null)
        {
            iconImage.sprite = icon;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;

        if(canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
        transform.SetParent(originalParent.root, false);

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent, true);
        transform.position = originalPosition;
    
        canvasGroup.blocksRaycasts = true;
    }

}
