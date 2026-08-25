using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStats
{
    public float maxHP;
    public float currentHP;
    public float attack;
    public float speed;
}

public class Player : MonoBehaviour, IDamageable
{
    #region 인스펙터 
    [Header("스텟")]
    [SerializeField] private PlayerStats _playerStats;
    #endregion

    public bool IsAlive
    {
        get; private set;
    } = true;

    private void Awake()
    {
        _playerStats.currentHP = _playerStats.maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive)
        {
            return;
        }

        _playerStats.currentHP -= damage;

        if (_playerStats.currentHP <= 0)
        {
            IsAlive = false;
            // 죽음 함수
            // 예비용 게임 멈추기
            Time.timeScale = 0.0f;
        }

        Debug.Log($"플레이어 체력 {_playerStats.currentHP} / {_playerStats.maxHP}");
    }
}
