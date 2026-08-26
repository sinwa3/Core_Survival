using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    #region 인스펙터
    [Header("레벨")]
    [Tooltip("기준 경험치")][SerializeField] private float _baseExp = 100.0f;
    [Tooltip("증가 경험치 배수")][SerializeField] private float _expMultiplier = 1.05f;
    #endregion

    #region 내수 변수
    private int _level;
    private float _currentExp;
    private float _requiredExp;
    #endregion

    public int Level => _level;

    // 임시 GUI용 스타일
    private GUIStyle _style;

    private void OnEnable()
    {
        TempEnemy.OnEnemyDead += AddExp;
    }

    private void OnDisable()
    {
        TempEnemy.OnEnemyDead -= AddExp;
    }

    void Start()
    {
        _level = 1;
        _currentExp = 0;
        _requiredExp = GetRequiredExp();

        // 임시 GUI용 스타일
        _style = new GUIStyle();
        _style.fontSize = 40;
        _style.normal.textColor = Color.red;
        _style.fontStyle = FontStyle.Bold;
    }

    private void AddExp(TempEnemy enemy)
    {
        _currentExp += enemy.Stats.exp;

        while (_requiredExp > 0.0f && _currentExp >= _requiredExp)
        {
            _currentExp -= _requiredExp;
            _level++;
            _requiredExp = GetRequiredExp();
        }
    }

    private float GetRequiredExp()
    {
        return _baseExp * Mathf.Pow(_expMultiplier, _level - 1);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(1000, 10, 300, 100), $"레벨 {_level}, 경험치 {_currentExp} / {_requiredExp}", _style);
    }
}
