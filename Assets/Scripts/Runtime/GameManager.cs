using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    Playing,
    Paused,
    LevelUp,
    GameOver
}

public class GameManager : MonoBehaviour
{
    #region 인스펙터
    [Header("게임 상태")]
    [SerializeField] private EGameState _gameState;

    [Header("키 설정")]
    [SerializeField] private KeyCode _pauseKey = KeyCode.Escape;

    [SerializeField] private Player _player;
    [SerializeField] private PlayerLevel _playerLevel;
    #endregion

    #region 내부 변수
    private int _killCount;
    private float _playTime;
    #endregion

    public int KillCount => _killCount;
    public float PlayTime => _playTime;
    public event Action<EGameState> OnStateChanged;
    private void OnEnable()
    {
        TempEnemy.OnEnemyDead += AddKillCount;

        if (_player != null)
        {
            _player.OnPlayerDead += PlayerDead;
        }

        if (_playerLevel != null)
        {
            _playerLevel.OnLevelUp += PlayerLevelUp;
        }
    }

    private void OnDisable()
    {
        TempEnemy.OnEnemyDead -= AddKillCount;

        if (_player != null)
        {
            _player.OnPlayerDead -= PlayerDead;
        }

        if (_playerLevel != null)
        {
            _playerLevel.OnLevelUp -= PlayerLevelUp;
        }
    }

    private void Awake()
    {
        if (_player == null)
        {
            Debug.LogWarning("플레이어 컴포넌트 null / 인스펙터 확인");

            return;
        }

        if (_playerLevel == null)
        {
            Debug.LogWarning("플레이어 레벨 컴포넌트 null / 인스펙터 확인");

            return;
        }
    }

    void Start()
    {
        Time.timeScale = 1.0f;
        _killCount = 0;
    }

    void Update()
    {
        _playTime += Time.deltaTime;

        if (Input.GetKeyDown(_pauseKey))
        {
            switch (_gameState)
            {
                case EGameState.Playing:
                    ChangeGameState(EGameState.Paused);
                    break;
                case EGameState.Paused:
                    ChangeGameState(EGameState.Playing);
                    break;
            }
        }
    }

    private void PlayerDead()
    {
        ChangeGameState(EGameState.GameOver);
    }

    private void PlayerLevelUp()
    {
        ChangeGameState(EGameState.LevelUp);
    }

    public void ChangeGameState(EGameState gameState)
    {
        if (_gameState == gameState)
        {
            Debug.Log("상태 변경 불가 / 상태 같음");

            return;
        }

        _gameState = gameState;

        switch (_gameState)
        {
            case EGameState.Playing:
                Time.timeScale = 1.0f;
                break;
            default:
                Time.timeScale = 0.0f;
                break;
        }

        OnStateChanged?.Invoke(_gameState);
    }

    private void AddKillCount(TempEnemy enemy)
    {
        _killCount++;
    }
}
