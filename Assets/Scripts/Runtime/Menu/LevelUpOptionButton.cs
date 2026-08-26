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
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private Image _buttonIcon;
    #endregion

    #region 내부 변수
    private Button _button;
    private Action<LevelUpOptionButton> _onClicked;
    #endregion

    private void Awake()
    {
        _button = GetComponent<Button>();

        if (_button != null)
        {
            _button.onClick.AddListener(Clicked);
        }
    }

    public void Setup(string text, Sprite icon, Action<LevelUpOptionButton> onClicked)
    {
        _buttonText.text = text;
        _buttonIcon.sprite = icon;
        _onClicked = onClicked;
    }

    private void Clicked()
    {
        _onClicked?.Invoke(this);
    }

}
