using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PausePanel : MenuPanelBase
{
    protected override EGameState TargetState => EGameState.Paused;

    #region 인스펙터
    [Header("텍스트")]
    [SerializeField] private TMP_Text _health;
    [SerializeField] private TMP_Text _attackRatio;
    [SerializeField] private TMP_Text _speedRatio;

    [Header("플레이어 스탯")]
    [SerializeField] private Player _player;
    #endregion

    #region 내부 변수
    #endregion

    protected override void Awake()
    {
        base.Awake();

        if (_player == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                _player = player.GetComponent<Player>();
            }
            else
            {
                Debug.LogWarning("Player 태그 오브젝트 찾을 수 없음");
                enabled = false;

                return;
            }
        }
    }

    protected override void Show()
    {
        base.Show();

        _health.text = $"{_player.PlayerStats.maxHP : 0}";
        _attackRatio.text = $"{_player.PlayerStats.attack * 100:0}%";
        _speedRatio.text = $"{_player.PlayerStats.speed * 100:0}%";
    }
}
