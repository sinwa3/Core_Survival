using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultButtonControl : MonoBehaviour
{
    public void OnClickRestartGame()
    {
        if (SceneflowManager.instance == null)
        {
            Debug.LogWarning("씬 플로우 매니저 인스턴스 null / 확인 요망");
            
            return;
        }

        SceneflowManager.instance.ReLoadScene();
    }

    public void OnClickReturnToTitle()
    {
        if (SceneflowManager.instance == null)
        {
            Debug.LogWarning("씬 플로우 매니저 인스턴스 null / 확인 요망");

            return;
        }

        SceneflowManager.instance.LoadScene(ESceneID.Title);
    }
}
