using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneflowManager : MonoBehaviour
{
    #region 인스펙터
    [Header("씬 스크립트")]
    [SerializeField] private SceneTransition _transition;
    [SerializeField] private SceneCatalog _catalog;
    #endregion

    #region 내부 변수
    public static SceneflowManager instance;
    #endregion

    private void Awake()
    {
        if (_catalog == null)
        {
            Debug.LogError("카탈로그 null / 확인 요망");
            Destroy(this.gameObject);

            return;
        }

        if (_transition == null)
        {
            Debug.LogWarning("트랜지션 스크립트 null / 확인 요망");
        }

        if (instance != null && instance != this)
        {
            Debug.LogWarning("인스턴스 중복 / 새 인스턴스 생성 불가");
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);

        _catalog.SettingDictionary();
    }

    void Start()
    {
        _transition.InitFadeGroup();
    }

    void Update()
    {
        
    }
}
