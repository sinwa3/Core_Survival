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

    [Header("무적")]
    [SerializeField] private float _invincibleDuration = 0.4f;
    #endregion

    #region 내부 변수
    private float _invincibleUntil;
    #endregion

    public event Action OnPlayerDead;
    public event Action OnDamaged;
    public bool IsInvincible => Time.time < _invincibleUntil;

    public bool IsAlive
    {
        get; private set;
    } = true;

    public PlayerStats PlayerStats => _playerStats;
    public float HpRatio => _playerStats.currentHP / _playerStats.maxHP;

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

        if (IsInvincible)
        {
            return;
        }

        _invincibleUntil = Time.time + _invincibleDuration;

        _playerStats.currentHP -= damage;

        if (_playerStats.currentHP <= 0)
        {
            IsAlive = false;
            OnPlayerDead?.Invoke();

            return;
        }

        OnDamaged?.Invoke();
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
