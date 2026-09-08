using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonControl : MonoBehaviour
{
    public void OnClickStartGame()
    {
        if (SceneflowManager.instance == null)
        {
            Debug.LogWarning("씬 플로우 매니저 인스턴스 null / 확인 요망");

            return;
        }

        SceneflowManager.instance.LoadNextScene();
    }

    public void OnClickOption()
    {
        Debug.Log("옵션 버튼 눌림");
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
