using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text playerCount;
    public TMP_Text enemyCount;

    private void Awake()
    {
        GameManager.Instance.uiManager = this;
    }
    public void TextUpdate(int a, int b)
    {
        playerCount.text = $"ÇÃ·¹ÀÌ¾î À¯´Ö : {a}";
        enemyCount.text = $"Àû À¯´Ö : {b}";
    }


}
