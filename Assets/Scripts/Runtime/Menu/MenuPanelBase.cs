using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MenuPanelBase : MonoBehaviour
{
    #region 인스펙터
    [Header("애니메이션 설정")]
    [SerializeField] private float _duration = 0.3f;

    [Header("패널")]
    [SerializeField] RectTransform _rectTransform;
    [SerializeField] protected CanvasGroup _canvasGroup;

    [Header("게임 매니저")]
    [SerializeField] protected GameManager _gameManager;

    #endregion

    #region 내부 변수
    private float _unscaledTimer;
    private Vector2 _startPosition;
    private Vector2 _centerPosition;
    private bool _isPlaying;
    #endregion

    protected abstract EGameState TargetState 
    { 
        get;
    }

    protected virtual void OnEnable()
    {
        if (_gameManager != null)
        {
            _gameManager.OnStateChanged += StateChanged;
        }
    }

    protected virtual void OnDisable()
    {
        if (_gameManager != null)
        {
            _gameManager.OnStateChanged -= StateChanged;
        }
    }

    protected virtual void Awake()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        _centerPosition = _rectTransform.anchoredPosition;
        Vector2 offSet = GetOffset();
        _startPosition = _centerPosition + offSet;
    }

    protected virtual void Start()
    {
        Hide();
    }

    protected virtual void Update()
    {
        if (_isPlaying)
        {
            _unscaledTimer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(_unscaledTimer / _duration);
            float lerp = Mathf.SmoothStep(0.0f, 1.0f, t);

            _rectTransform.anchoredPosition = Vector2.Lerp(_startPosition, _centerPosition, lerp);

            if (t >= 1.0f)
            {
                _rectTransform.anchoredPosition = _centerPosition;
                _isPlaying = false;
            }
        }
    }

    private void StateChanged(EGameState gameState)
    {
        if (gameState == TargetState)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    protected virtual void Show()
    {
        _rectTransform.anchoredPosition = _startPosition;
        _isPlaying = true;
        _unscaledTimer = 0.0f;
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    private void Hide()
    {
        _isPlaying = false;
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    private Vector2 GetOffset()
    {
        Vector2 offset = Vector2.zero;

        offset = new Vector2(-_rectTransform.rect.size.x, 0.0f);


        return offset;
    }
}
