using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelUpPanel : MenuPanelBase
{
    protected override EGameState TargetState => EGameState.LevelUp;

    #region 인스펙터
    [Header("스킬 버튼")]
    [SerializeField] private LevelUpOptionButton[] _options;

    [Header("선택지")]
    [SerializeField] private List<LevelUpOption> _allOptions;

    [Header("스킬 매니저")]
    [SerializeField] private SkillManager _skillManager;

    [Header("레벨 동시 업")]
    [SerializeField] private PlayerLevel _playerLevel;

    [Header("플레이어")]
    [SerializeField] private Player _player;
    #endregion

    protected override void Show()
    {
        base.Show();
        List<LevelUpOption> options = GetOptions();

        if (options.Count == 0)
        {
            Debug.Log("옵션이 0개 / 게임으로 돌아갑니다.");
            _playerLevel.ResetLevelBuffer();
            _gameManager.ChangeGameState(EGameState.Playing);

            return;
        }

        for (int i = 0; i < _options.Length; i++)
        {
            if (options.Count == 0)
            {
                _options[i].Hide();

                continue;
            }

            int index = Random.Range(0, options.Count);

            _options[i].Setup(options[index], OptionClicked);
            options.RemoveAt(index);
        }
    }

    private void OptionClicked(LevelUpOptionButton button)
    {
        LevelUpOption option = button.LevelUpOption;

        switch (option.type)
        {
            case EOptionType.LearnSkill:
                _skillManager.LearnSkill(option.skillId);
                break;
            case EOptionType.UpgradeStat:
                StatUpgrade(option);
                break;
        }

        _playerLevel.UseLevelBuffer();

        if (_playerLevel.LevelBuffer > 0)
        {
            Show();

            return;
        }

        _gameManager.ChangeGameState(EGameState.Playing);
    }


    private List<LevelUpOption> GetOptions()
    {
        List<LevelUpOption> options = new List<LevelUpOption>();

        for (int i = 0; i < _allOptions.Count; i++)
        {
            LevelUpOption option = _allOptions[i];

            if (option.type == EOptionType.LearnSkill && _skillManager.HasSkill(option.skillId))
            {
                continue;
            }

            options.Add(option);
        }

        return options;
    }

    private void StatUpgrade(LevelUpOption option)
    {
        if (_player == null)
        {
            Debug.LogWarning("플레이어 컴포넌트 null / 인스펙터 확인");

            return;
        }

        if (option == null)
        {
            Debug.LogWarning("레벨 업 옵션 null / 확인 요망"); 
            
            return;
        }

        switch (option.statType)
        {
            case EStatType.MaxHP:
                _player.IncreaseHP(option.statAmount);
                break;
            case EStatType.Speed:
                _player.IncreaseSpeed(option.statAmount);
                break;
            default:
                Debug.LogWarning($"스탯 타입 미설정 / {option.optionName} 확인 요망");
                break;
        }
    }

}
