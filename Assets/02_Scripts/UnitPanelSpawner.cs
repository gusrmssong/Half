using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitPanelSpawner : MonoBehaviour
{
    [System.Serializable]
    public class UnitDef
    {
        public string id;           // "A", "B" 같은 식별자
        public Vector2Int size;     // (width, height) 예: 1x4, 2x2
        public Sprite icon;         // 패널에 보일 이미지
        public Unit unitType;
    }

    [Header("References")]
    [SerializeField] private Transform unitListParent; // UnitList
    [SerializeField] private GameObject unitCardPrefab; // UnitCardPrefab

    [Header("Units to Spawn")]
    [SerializeField] private List<UnitDef> unitsToPlace = new List<UnitDef>();

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        // 기존 카드 정리(테스트용)
        for (int i = unitListParent.childCount - 1; i >= 0; i--)
        {
            Destroy(unitListParent.GetChild(i).gameObject);
        }

        // 카드 생성
        foreach (var def in unitsToPlace)
        {
            var go = Instantiate(unitCardPrefab, unitListParent);

            var card = go.GetComponent<UnitCardUI>();
            if (card != null)
            {
                card.Setup(def.id, def.size, def.icon, def.unitType);
            }
        }
    }
}