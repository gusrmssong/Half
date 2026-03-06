using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text playerCount;
    public TMP_Text enemyCount;

    [SerializeField] private GameObject endPanel; // 종료 패널
    [SerializeField] private GameObject winPanel; // 종료 패널

    private void Awake()
    {
        GameManager.Instance.uiManager = this;
        if (endPanel != null) endPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
    }
    public void TextUpdate(int a, int b)
    {
        playerCount.text = $"플레이어 유닛 : {a}";
        enemyCount.text = $"적 유닛 : {b}";
    }

    public void ShowEndPanel()
    {
        if (endPanel == null) return;

        endPanel.SetActive(true);

    }
    public void ShowWinPanel()
    {
        if (winPanel == null) return;

        winPanel.SetActive(true);

    }


}
