using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HalfSceneManager : MonoBehaviour
{
    public static HalfSceneManager Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SceneMain();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SceneGameReady();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SceneGame();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SceneShop();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SceneEnding();
        }
    }
    
    public void SceneMain()
    {
        SceneManager.LoadScene(0);
    }
    public void SceneGameReady()
    {
        SceneManager.LoadScene(1);
    }
    public void SceneGame()
    {
        SceneManager.LoadScene(2);
    }
    public void SceneShop()
    {
        SceneManager.LoadScene(3);
    }
    public void SceneEnding()
    {
        SceneManager.LoadScene(4);
    }



}
