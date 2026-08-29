using System.Collections.Generic;
using UnityEngine;

public class LevelUpPanel : MenuPanelBase
{
    protected override EGameState TargetState => EGameState.LevelUp;

    #region 인스펙터
    [Header("스킬 버튼")]
    [SerializeField] private LevelUpOptionButton[] _options;

    [Header("선택지")]
    [SerializeField] private List<LevelUpOptionSO> _allOptions;

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
        List<LevelUpOptionSO> options = GetOptions();

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
        LevelUpOptionSO option = button.LevelUpOption;

        option.Apply(_skillManager, _player);

        _playerLevel.UseLevelBuffer();

        if (_playerLevel.LevelBuffer > 0)
        {
            Show();

            return;
        }

        _gameManager.ChangeGameState(EGameState.Playing);
    }


    private List<LevelUpOptionSO> GetOptions()
    {
        List<LevelUpOptionSO> options = new List<LevelUpOptionSO>();

        for (int i = 0; i < _allOptions.Count; i++)
        {
            LevelUpOptionSO option = _allOptions[i];

            if (option == null)
            {
                Debug.LogWarning($"선택지 {i}번 null / LevelUpPanel 인스펙터 확인");

                continue;
            }

            if (!option.IsAvailable(_skillManager))
            {
                continue;
            }

            options.Add(option);
        }

        return options;
    }
}
