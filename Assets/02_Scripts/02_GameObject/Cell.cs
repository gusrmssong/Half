using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class Cell : MonoBehaviour
{
    [SerializeField] public int x;
    [SerializeField] public int y;

    [SerializeField] public bool isObstacle = false;
    [SerializeField] public bool isRevealed = false;
    [SerializeField] public bool isDestroyed = false;
    [SerializeField] public Unit unit;

    [SerializeField] public Image unitSprite;
    [SerializeField] public Image destroySprite;
    [SerializeField] public Image revealSprite;

    [SerializeField] public Sprite[] unitSprites;
    public void Init(int x, int y, Unit unit)
    {
        this.x = x;
        this.y = y;
        this.unit = unit;
        gameObject.name = $"Cell_{x}_{y}";
        UpdateCellSprite();
    }

    private void Awake()
    {
        unitSprite = GetComponent<Image>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Reveal();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Destory();
        }
        if (Input.GetKey(KeyCode.C))
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                SetUnit(0);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                SetUnit(1);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                SetUnit(2);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                SetUnit(3);
            }
        }
    }

    public void Reveal()
    {
        isRevealed = !isRevealed;
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
    }

}
