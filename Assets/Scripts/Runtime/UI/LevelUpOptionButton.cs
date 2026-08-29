using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpOptionButton : MonoBehaviour
{
    #region 인스펙터
    [Header("버튼")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _descText;
    [SerializeField] private Image _buttonIcon;
    #endregion

    #region 내부 변수
    private Button _button;
    private Action<LevelUpOptionButton> _onClicked;
    private LevelUpOptionSO _levelUpOption;
    #endregion

    public LevelUpOptionSO LevelUpOption => _levelUpOption;


    private void Awake()
    {
        _button = GetComponent<Button>();

        if (_button != null)
        {
            _button.onClick.AddListener(Clicked);
        }
    }

    public void Setup(LevelUpOptionSO option, Action<LevelUpOptionButton> onClicked)
    {
        _levelUpOption = option;
        _onClicked = onClicked;

        gameObject.SetActive(true);

        _nameText.text = option.OptionName;
        _descText.text = option.Description;
        _buttonIcon.sprite = option.Icon;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Clicked()
    {
        _onClicked?.Invoke(this);
    }


}
