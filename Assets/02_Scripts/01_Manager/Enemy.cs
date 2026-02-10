using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Board playerBoard;
    public GridManager gridManager;

    public Cell currentCell;
    public List<int> random;

    private void Awake()
    {
        GameManager.Instance.enemy = this;
        SetRandom();
    }
    public void SetRandom()
    { 
        random = new List<int>();
        for(int i = 1; i <= 100; i++)
        {
            random.Add(i);
        }
    }
    public int MakeRandom()
    {
        if(random == null || random.Count <= 0)
        {
            Debug.Log("랜덤 숫자 생성 불가");
            return -1;
        }

        int r = Random.Range(0, random.Count);
        int value = random[r];
        random.RemoveAt(r);
        return value;
    }

    public Vector2Int Index(int a)
    {
        int x, y;
        x = a % 10;
        y = a / 10;
        return new Vector2Int(x, y);

    }
    public void StartAttack()
    {
        StartCoroutine(AttackCoroutine());


    }
    public IEnumerator AttackCoroutine()
    {
        // 적의 턴 시작! -> 랜덤 숫자 생성하고 ->  셀 선택해서 공격 -> 결과 출력 -> 턴 넘기기
        Debug.Log("적의 턴 시작!");

        yield return new WaitForSeconds(1f);

        Debug.Log("랜덤 좌표 생성");
        Vector2Int vector = Index(MakeRandom());

        yield return new WaitForSeconds(1f);

        int x = vector.x;
        int y = vector.y;
        Debug.Log($"생성된 좌표는 [{x},{y}]");

        yield return new WaitForSeconds(1f);

        GameManager.Instance.enemyCell = playerBoard.cells[x, y];

        yield return new WaitForSeconds(1f);

        GameManager.Instance.EnemySelect();

    }
    



}
