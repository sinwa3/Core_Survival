using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    void Start()
    {
        if (SceneflowManager.instance == null)
        {
            Debug.LogWarning("씬 플로우 매니저 인스턴스 없음 / 확인 요망");

            return;
        }

        SceneflowManager.instance.LoadSceneInstant(ESceneID.Title);
    }
}
