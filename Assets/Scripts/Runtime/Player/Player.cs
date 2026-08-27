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

    public event Action OnPlayerDead;

    public bool IsAlive
    {
        get; private set;
    } = true;

    public PlayerStats PlayerStats => _playerStats;

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
            OnPlayerDead?.Invoke();
        }

        Debug.Log($"플레이어 체력 {_playerStats.currentHP} / {_playerStats.maxHP}");
    }

    public void IncreaseHP(float amount)
    {
        _playerStats.maxHP += amount;
        _playerStats.currentHP += amount;
    }

    public void IncreaseSpeed(float amount)
    {
        _playerStats.speed += amount;
    }
}
