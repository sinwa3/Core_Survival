using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PausePanel : MenuPanelBase
{
    protected override EGameState TargetState => EGameState.Paused;

    #region 인스펙터
    [Header("스텟 강화")]
    [SerializeField] private TMP_Text _health;
    [SerializeField] private TMP_Text _attackRatio;
    [SerializeField] private TMP_Text _speedRatio;

    [Header("플레이어 스탯")]
    [SerializeField] private Player _player;

    [Header("스킬 강화")]
    [SerializeField] private SkillStatUI[] _skillStats;
    [SerializeField] private SkillManager _skillManager;
    #endregion

    #region 내부 변수
    #endregion

    protected override void Awake()
    {
        base.Awake();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (_player == null)
        {
            if (player == null)
            {
                Debug.LogWarning("Player 태그 오브젝트 찾을 수 없음 (PausePanel)");
                enabled = false;

                return;
            }

            _player = player.GetComponent<Player>();
        }


        if (_skillManager == null)
        {
            if (player == null)
            {
                Debug.LogWarning("Player 태그 오브젝트 찾을 수 없음 (PausePanel)");
                enabled = false;

                return;
            }

            _skillManager = player.GetComponent<SkillManager>();
        }

    }

    protected override void Show()
    {
        base.Show();

        _health.text = $"{_player.PlayerStats.maxHP:0}";
        _attackRatio.text = $"{_player.PlayerStats.attack * 100:0}%";
        _speedRatio.text = $"{_player.PlayerStats.speed * 100:0}%";

        if (_skillManager == null)
        {
            Debug.LogWarning("스킬 매니저 null (PausePanel) / 인스펙터 확인");

            return;
        }

        int index = 0;

        foreach (var skill in _skillManager.Skills)
        {
            SkillDataSO data = _skillManager.GetSkillData(skill.Key);

            if (_skillStats.Length <= index)
            {
                Debug.LogWarning("슬롯 부족");

                break;
            }

            _skillStats[index].Setup(data, skill.Value);

            index++;
        }

        for (int i = index; i < _skillStats.Length; i++)
        {
            _skillStats[i].Hide();
        }
    }
}
