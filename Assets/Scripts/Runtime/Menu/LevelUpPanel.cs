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
    #endregion

    protected override void Show()
    {
        base.Show();

        for (int i = 0; i < _options.Length; i++)
        {
            _options[i].Setup($"임시 선택지 {i + 1}", null, OptionClicked);
        }
    }

    private void OptionClicked(LevelUpOptionButton option)
    {
        // 선택한 효과를 여기서 적용
        _gameManager.ChangeGameState(EGameState.Playing);
    }
}
