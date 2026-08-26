using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame_PauseMenu : MonoBehaviour
{
    #region 인스펙터
    [Header("애니메이션 설정")]
    [SerializeField] private float _duration = 0.3f;

    [Header("일시정지 메뉴")]
    [SerializeField] RectTransform _rectTransform;
    #endregion


    #region 내부 변수
    private float _unscaledTimer;
    private Vector2 _startPosition;
    private Vector2 _centerPosition;
    private bool _isPlaying = false;
    #endregion

    private void OnEnable()
    {
        _rectTransform.anchoredPosition = _startPosition;
        _unscaledTimer = 0.0f;
        _isPlaying = true;
    }

    private void Awake()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        _centerPosition = _rectTransform.anchoredPosition;
        Vector2 offSet = GetOffset();
        _startPosition = _centerPosition + offSet;
    }

    void Start()
    {

    }

    void Update()
    {
        if (_isPlaying)
        {
            _unscaledTimer += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(_unscaledTimer / _duration);
            float lerp = Mathf.SmoothStep(0.0f, 1.0f, t);

            _rectTransform.anchoredPosition = Vector2.Lerp(_startPosition, _centerPosition, lerp);

            if (t >= 1.0f)
            {
                _isPlaying = false;
                _rectTransform.anchoredPosition = _centerPosition;
            }
        }
    }

    private Vector2 GetOffset()
    {
        Vector2 offset = Vector2.zero;

        offset = new Vector2(-_rectTransform.rect.size.x, 0.0f);


        return offset;
    }
}
