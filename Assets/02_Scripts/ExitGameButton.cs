using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    // 버튼 OnClick에 연결할 함수
    public void ExitGame()
    {
        // 에디터에서는 종료가 안 되니 로그로 확인
#if UNITY_EDITOR
        Debug.Log("ExitGame() 호출됨 (에디터에서는 Application.Quit가 동작하지 않음)");
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 플레이 종료
#else
        Application.Quit(); // 빌드된 게임 종료
#endif
    }
}