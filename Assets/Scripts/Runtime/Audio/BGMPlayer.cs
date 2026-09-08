using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    #region 인스펙터
    [SerializeField] private GameManager _gameManager;
    #endregion

    #region 내부 변수
    private AudioSource _audioSource;
    #endregion

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogWarning("AudioSource 컴포넌트 null (BGMPlayer) / 확인 요망");
            enabled = false;

            return;
        }

        _audioSource.ignoreListenerPause = true;
    }

    private void OnEnable()
    {
        if (_gameManager != null)
        {
            _gameManager.OnStateChanged += StateChanged;
        }
    }

    private void OnDisable()
    {
        if(_gameManager != null)
        {
            _gameManager.OnStateChanged -= StateChanged;
        }
    }

    private void StateChanged(EGameState state)
    {
        switch (state)
        {
            case EGameState.Paused:
                _audioSource.Pause();
                break;
            case EGameState.GameOver:
                _audioSource.Stop();
                break;
            case EGameState.Playing:
                _audioSource.UnPause();
                break;
            case EGameState.LevelUp:
                break;
        }
    }
}
