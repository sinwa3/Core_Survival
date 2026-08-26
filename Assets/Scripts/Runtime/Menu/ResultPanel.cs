using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPanel : MenuPanelBase
{
    protected override EGameState TargetState => EGameState.GameOver;

    [Header("플레이어 레벨")]
    [SerializeField] private PlayerLevel _playerLevel;

    [Header("텍스트")]
    [SerializeField] private TMP_Text _playTime;
    [SerializeField] private TMP_Text _lastLevel;
    [SerializeField] private TMP_Text _killCount;

    protected override void Awake()
    {
        base.Awake();
        
        if (_playerLevel == null)
        {
            Debug.LogWarning("플레이어 레벨 null / 인스펙터 확인");
            enabled = false;

            return;
        }
        
    }

    protected override void Show()
    {
        base.Show();

        float time = _gameManager.PlayTime;
        float minute = Mathf.FloorToInt(time / 60);
        float second = Mathf.FloorToInt(time % 60);

        _playTime.text = $"{minute}분 {second}초";
        _lastLevel.text = _playerLevel.Level.ToString();
        _killCount.text = _gameManager.KillCount.ToString();
    }
}
