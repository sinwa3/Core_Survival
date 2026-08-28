using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    #region 인스펙터
    [Header("플레이어 스크립트")]
    [SerializeField] private Player _player;

    [Header("플레이어 레벨 스크립트")]
    [SerializeField] private PlayerLevel _playerLevel;

    [Header("바")]
    [Description("경험치 바")]
    [SerializeField] private Image _expBar;

    [Description("체력 바")]
    [SerializeField] private Image _hpBar;

    [Header("레벨")]
    [SerializeField] private TMP_Text _levelText;

    [Header("게임 매니저")]
    [SerializeField] private GameManager _gameManager;
    #endregion

    #region 내부 변수
    private int _previousLevel;
    private CanvasGroup _canvasGroup;
    #endregion

    private void OnEnable()
    {
        if (_gameManager != null)
        {
            _gameManager.OnStateChanged += SetAlpha;
        }
    }

    private void OnDisable()
    {
        _gameManager.OnStateChanged -= SetAlpha;
    }


    private void Awake()
    {
        if (_player == null)
        {
            Debug.LogWarning("플레이어 스크립트 연결 안됨 / 인스펙터 확인");

            return;
        }

        if (_playerLevel == null)
        {
            Debug.LogWarning("플레이어 레벨 스크립트 연결 안됨 / 인스펙터 확인");

            return;
        }

        if (_hpBar == null)
        {
            Debug.LogWarning("경험치 바 이미지 없음");

            return;
        }

        if (_expBar == null)
        {
            Debug.LogWarning("경험치 바 이미지 없음");

            return;
        }

        if (_levelText == null)
        {
            Debug.LogWarning("레벨 텍스트 연결 안됨 / 인스펙터 확인");

            return;
        }

        _canvasGroup = GetComponent<CanvasGroup>();

        if (_canvasGroup == null)
        {
            Debug.LogWarning("캔버스 그룹 null / 확인 요망");

            return;
        }
    }

    void Start()
    {
        _expBar.fillAmount = _playerLevel.ExpRatio;
        _hpBar.fillAmount = _player.HpRatio;

        _previousLevel = _playerLevel.Level;
    }

    void Update()
    {
        _expBar.fillAmount = Mathf.Lerp(_expBar.fillAmount, _playerLevel.ExpRatio, 4.0f * Time.deltaTime);

        if (Mathf.Abs(_playerLevel.ExpRatio - _expBar.fillAmount) < 0.001f)
        {
            _expBar.fillAmount = _playerLevel.ExpRatio;
        }

        _hpBar.fillAmount = Mathf.Lerp(_hpBar.fillAmount, _player.HpRatio, 4.0f * Time.deltaTime);

        if (Mathf.Abs(_player.HpRatio - _hpBar.fillAmount) < 0.001f)
        {
            _hpBar.fillAmount = _player.HpRatio;
        }

        if (_playerLevel.Level > _previousLevel)
        {
            ApplyLevel();
        }
        
    }

    private void ApplyLevel()
    {
        _levelText.text = _playerLevel.Level.ToString();
        _previousLevel = _playerLevel.Level;
    }

    private void SetAlpha(EGameState state)
    {
        switch (state)
        {
            case EGameState.Playing:
                _canvasGroup.alpha = 1.0f;
                break;
            default:
                _canvasGroup.alpha = 0.0f;
                break;
        }
    }


}
