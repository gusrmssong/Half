using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPanelSpawnerA : MonoBehaviour
{
    public class UnitDef
    {
        public string id;
        public Vector2Int size;
        public Sprite icon;
        public Unit unitType;
    }

    private Transform unitListParent;
    private GameObject unitCardPrefab;

    private List<UnitDef> unitsToPlace = new List<UnitDef>();

    void Spawn()
    {
        for (int i = unitListParent.childCount - 1; i >= 0; i--)
        {
            Destroy(unitListParent.GetChild(i).gameObject);
        }

        foreach(var def in unitsToPlace)
        {
            var go = Instantiate(unitCardPrefab, unitListParent);
            
            var card = go.GetComponent<UnitCardUI>();
            if(card != null )
            {
                card.Setup(def.id, def.size, def.icon, Unit.None);
            }
        }
    }



}
