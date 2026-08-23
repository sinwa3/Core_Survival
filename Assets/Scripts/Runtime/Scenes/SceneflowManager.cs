using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneflowManager : MonoBehaviour
{
    #region 인스펙터
    [Header("씬 스크립트")]
    [SerializeField] private SceneTransition _transition;
    [SerializeField] private SceneCatalog _catalog;

    [Header("페이드 옵션")]
    [SerializeField] private float _fadeDuration = 0.5f;

    [Header("이미지")]
    [SerializeField] private Image _loadingBarImage;

    [Header("페이드")]
    #endregion

    #region 내부 변수
    public static SceneflowManager instance;
    private int _currentSceneIndex;
    private bool _isLoading;
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

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

        if (_loadingBarImage == null)
        {
            Debug.LogWarning("로딩 바 이미지 null / 인스펙터 확인");

            return;
        }

        _loadingBarImage.gameObject.SetActive(false);

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            LoadPrevScene();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            LoadNextScene();
        }
    }

    private void SetCurrentSceneIndex()
    {
        IReadOnlyList<SceneInfo> scenes = _catalog.Scenes;

        if (scenes == null || scenes.Count == 0)
        {
            Debug.LogWarning("씬 비어있음 확인 요망");

            return;
        }

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

    private void ReLoadScene()
    {
        string reLoadSceneName = SceneManager.GetActiveScene().name;

        if (_catalog.TryGetSceneId(reLoadSceneName, out ESceneID id))
        {
            Debug.LogWarning("씬 ID 로드 불가 / 확인 요망");

            return;
        }

        LoadScene(id);
    }
    public void LoadNextScene()
    {
        IReadOnlyList<SceneInfo> scenes = _catalog.Scenes;

        if (scenes == null || scenes.Count == 0)
        {
            Debug.LogWarning("씬 비어있음 확인 요망");

            return;
        }

        _currentSceneIndex++;

        if (_currentSceneIndex >= scenes.Count)
        {
            _currentSceneIndex = 0;
        }

        string nextSceneName = scenes[_currentSceneIndex].sceneName;

        if (!_catalog.TryGetSceneId(nextSceneName, out ESceneID id))
        {
            Debug.LogWarning("씬 ID 로드 불가 / 확인 요망");

            return;
        }

        LoadScene(id);
    }
    private void LoadPrevScene()
    {
        IReadOnlyList<SceneInfo> scenes = _catalog.Scenes;

        if (scenes == null || scenes.Count == 0)
        {
            Debug.LogWarning("씬 비어있음 확인 요망");

            return;
        }

        _currentSceneIndex--;

        if (_currentSceneIndex < 0)
        {
            _currentSceneIndex = scenes.Count - 1;
        }

        string prevSceneName = scenes[_currentSceneIndex].sceneName;

        if (!_catalog.TryGetSceneId(prevSceneName, out ESceneID id))
        {
            Debug.LogWarning("씬 ID 로드 불가 / 확인 요망");

            return;
        }

        LoadScene(id);
    }

    public void LoadScene(ESceneID id)
    {
        if (!_catalog.TryGetSceneName(id, out string sceneName))
        {
            Debug.LogWarning("잘못된 ID / 씬 전환 불가");

            return;
        }

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("씬 이름 없음 / 씬 전환 불가");

            return;
        }

        StartCoroutine(Co_LoadScene(sceneName));
        Debug.Log($"씬 {sceneName} 로드 성공");
    }

    private IEnumerator Co_LoadScene(string sceneName)
    {
        if (_isLoading)
        {
            yield break;
        }

        _isLoading = true;

        _loadingBarImage.fillAmount = 0.0f;
        _loadingBarImage.gameObject.SetActive(true);

        if (_transition == null)
        {
            Debug.LogWarning("트랜지션 스크립트 null 확인 요망");

            yield break;
        }

        yield return _transition.Co_Fade(1.0f, _fadeDuration);

        _transition.SetLoadingText("Loading...");

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);

            _loadingBarImage.fillAmount = progress;

            if (progress >= 1.0f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    op.allowSceneActivation = true;
                }
            }

            yield return null;
        }

        _loadingBarImage.gameObject.SetActive(false);
        _transition.SetLoadingText("");
        yield return _transition.Co_Fade(0.0f, _fadeDuration);

        _isLoading = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetCurrentSceneIndex();
    }

    public void LoadSceneInstant(ESceneID id)
    {
        if (!_catalog.TryGetSceneName(id, out string name))
        {
            Debug.LogWarning("없는 씬 ID / 확인 요망");

            return;
        }

        SceneManager.LoadScene(name);
    }

}
