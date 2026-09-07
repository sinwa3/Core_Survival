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
    public float critChance;
}

public class Player : MonoBehaviour, IDamageable
{
    #region 인스펙터 
    [Header("스텟")]
    [SerializeField] private PlayerStats _playerStats;

    [Header("무적")]
    [SerializeField] private float _invincibleDuration = 0.4f;

    [Header("레벨업")]
    [SerializeField] private PlayerLevel _playerLevel;

    [Header("치명타")]
    [SerializeField] private float _critMulti = 1.5f;

    [Header("회복량")]
    [SerializeField] private float _healAmount = 10.0f;
    #endregion

    #region 내부 변수
    private float _invincibleUntil;
    #endregion

    public event Action OnPlayerDead;
    public event Action OnDamaged;
    public bool IsInvincible => Time.time < _invincibleUntil;

    public float CritChance => _playerStats.critChance;
    public float CritMulti => _critMulti;

    public bool IsAlive
    {
        get; private set;
    } = true;

    public PlayerStats PlayerStats => _playerStats;
    public float HpRatio => _playerStats.currentHP / _playerStats.maxHP;

    private void OnEnable()
    {
        if (_playerLevel != null)
        {
            _playerLevel.OnLevelUp += HealHP;
        }
    }

    private void OnDisable()
    {
        if (_playerLevel != null)
        {
            _playerLevel.OnLevelUp -= HealHP;
        }
    }


    private void Awake()
    {
        _playerStats.currentHP = _playerStats.maxHP;

        if (_playerLevel == null)
        {
            Debug.LogWarning("플레이어 레벨 null (Player) / 인스펙터 확인");

            return;
        }
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

    public void IncreaseAttack(float amount)
    {
        _playerStats.attack += amount;
    }

    public void IncreaseCritChance(float amount)
    {
        _playerStats.critChance = Mathf.Clamp01(_playerStats.critChance + amount);
    }

    public void HealHP()
    {
        _playerStats.currentHP = Mathf.Min(_playerStats.currentHP + _healAmount, _playerStats.maxHP);
    }
}
