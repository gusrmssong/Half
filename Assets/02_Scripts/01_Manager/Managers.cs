using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log($"{name} Áßº¹ÀÌ¶ó ÆÄ±«µÊ. scene={gameObject.scene.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log($"{name} À¯ÁöµÊ. scene={gameObject.scene.name}");
    }
}
