using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneflowManager : MonoBehaviour
{
    #region 인스펙터
    [Header("씬 스크립트")]
    [SerializeField] private SceneTransition _transition;
    [SerializeField] private SceneCatalog _catalog;
    #endregion

    #region 내부 변수
    public static SceneflowManager instance;
    private int _currentSceneIndex;
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
            Debug.LogWarning("씬 인스턴스 중복 / 새 인스턴스 생성 불가");
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(this.gameObject);

        _catalog.SettingDictionary();

        SetCurrentSceneIndex();
    }

    void Start()
    {
        if (_transition != null)
        {
            _transition.InitFadeGroup();
        }
    }

    void Update()
    {

    }

    private void SetCurrentSceneIndex()
    {
        IReadOnlyList<SceneInfo> scenes = _catalog.Scenes;
        string sceneName = SceneManager.GetActiveScene().name;

        for (int i = 0; i < scenes.Count; i++)
        {
            if (sceneName == scenes[i].sceneName)
            {
                _currentSceneIndex = i;

                return;
            }
        }

        Debug.LogWarning("현재 씬 인덱스 찾지 못함");
        _currentSceneIndex = 0;
    }

    public void LoadScene(ESceneID id)
    {
        if (!_catalog.TryGetSceneName(id, out string name))
        {
            Debug.LogWarning("잘못된 ID / 씬 전환 불가");

            return;
        }

        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("씬 이름 없음 / 씬 전환 불가");

            return;
        }

        SceneManager.LoadScene(name);
    }
}
