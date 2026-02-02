using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] public int x;
    [SerializeField] public int y;

    [SerializeField] public bool isObstacle;
    [SerializeField] public bool isRevealed;
    [SerializeField] public Unit unit;

    public void Init(int x, int y, bool isRevealed, Unit unit)
    {
        this.x = x;
        this.y = y;
        this.isRevealed = isRevealed;
        this.unit = unit;
        gameObject.name = $"Cell_{x}_{y}";
    }

    public void UpdateCell()
    {
        if(isRevealed)
        {

        }
        else
        {

        }

        if(unit!=Unit.None)
        {

        }
        else
        {

        }

    }

}
