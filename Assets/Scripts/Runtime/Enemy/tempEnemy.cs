using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    public float maxHP;
    public float currentHP;
    public float attack;
    public float exp;
}

public class TempEnemy : MonoBehaviour, IDamageable
{
    #region 인스펙터
    [Header("플레이어")]
    [SerializeField] private Transform _playerTransform;

    [Header("이동 / 회전")]
    [SerializeField] private float _moveSpeed = 10.0f;
    [SerializeField] private float _rotateSpeed = 5.0f;

    [Header("스텟")]
    [SerializeField] private EnemyStats _stats;

    [Header("공격")]
    [SerializeField] private float _attackRange = 2.0f;
    [SerializeField] private float _attackInterval = 1.0f;
    #endregion

    #region 내부 변수
    private EnemyPooling _ownerPool;
    private Rigidbody _rb;
    private float _attackTimer = 0.0f;

    private float _baseHP;
    private float _baseAttack;
    IDamageable _damageable;
    #endregion

    public static event Action<TempEnemy> OnEnemyDead;

    public EnemyStats Stats => _stats;

    // 스폰 상태 판별
    public bool IsActive
    {
        get; private set;
    }

    private void Awake()
    {
        if (_playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                _playerTransform = player.transform;
                _damageable = player.GetComponent<IDamageable>();
            }
            else
            {
                Debug.LogWarning("Player 태그 오브젝트 찾을 수 없음");
            }
        }

        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
        {
            Debug.LogWarning("적 리지드바디 null 확인 요망");

            return;
        }

        _baseHP = _stats.maxHP;
        _baseAttack = _stats.attack;
    }

    void Update()
    {
        Vector3 toPlayer = (_playerTransform.position - transform.position);

        FollowPlayer(toPlayer);
        TickRotate(toPlayer);
        TickAttack(toPlayer);
    }

    // 공격
    private void TickAttack(Vector3 toPlayer)
    {
        float sqrRange = _attackRange * _attackRange;

        if (toPlayer.sqrMagnitude >= sqrRange)
        {
            return;
        }

        if (_attackTimer < _attackInterval)
        {
            _attackTimer += Time.deltaTime;

            return;
        }

        _attackTimer = 0.0f;

        _damageable.TakeDamage(_stats.attack);
    }

    // 받는 데미지 계산
    public void TakeDamage(float damage)
    {
        if (!IsActive)
        {
            return;
        }

        _stats.currentHP -= damage;

        if (_stats.currentHP <= 0)
        {
            OnEnemyDead?.Invoke(this);
            DestroyThis();
        }
    }

    public void OnSpawn()
    {
        IsActive = true;
        _stats.currentHP = _stats.maxHP;
    }

    public void OnDespawn()
    {
        IsActive = false;
    }

    public void ApplyStatMultiple(float hpMulti, float attackMulti)
    {
        _stats.maxHP = _baseHP * hpMulti;
        _stats.attack = _baseAttack * attackMulti;
        _stats.currentHP = _stats.maxHP;
    }

    // 죽었을 때
    public void DestroyThis()
    {
        if (_ownerPool != null)
        {
            _ownerPool.ReturnEnemy(this);

            return;
        }

        // 풀 미연결 시 파괴
        IsActive = false;
        Destroy(gameObject);
    }

    // 풀 종류 설정
    public void SetOwnerPool(EnemyPooling pool)
    {
        _ownerPool = pool;
    }

    // 플레이어 따라가기
    private void FollowPlayer(Vector3 toPlayer)
    {
        if (toPlayer.magnitude <= _attackRange)
        {
            return;
        }
        
        Vector3 movePos = transform.position + toPlayer.normalized * _moveSpeed * Time.deltaTime;

        _rb.MovePosition(movePos);
    }

    // 플레이어쪽으로 돌기
    private void TickRotate(Vector3 toPlayer)
    {
        if (toPlayer.sqrMagnitude < 0.0001f)
        {
            return;
        }

        toPlayer.y = 0.0f;
        Quaternion rot = Quaternion.LookRotation(toPlayer);
        Quaternion moveRot = Quaternion.Slerp(transform.rotation, rot, 1.0f - Mathf.Exp(-_rotateSpeed * Time.deltaTime));

        _rb.MoveRotation(moveRot);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2.0f);
    }
}
