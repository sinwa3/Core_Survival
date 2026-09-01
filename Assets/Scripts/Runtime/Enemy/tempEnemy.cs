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

    [Header("땅")]
    [SerializeField] private LayerMask _groundMask = 1 << 7;
    [SerializeField] private float _groundCheckHeight = 2.0f;
    [SerializeField] private float _groundOffset = 0.5f;

    [Header("겹침 방지")]
    [SerializeField] private LayerMask _enemyMask = 1 << 6;
    [SerializeField] private float _pushDistance = 0.9f;
    [SerializeField] private float _pushForce = 1.0f;

    [Header("애니메이션")]
    [SerializeField] private Animator _animator;
    [SerializeField] private string _paramRun = "bRun";
    [SerializeField] private string _paramAttack = "tAttack";
    [SerializeField] private string _paramDead = "tDead";

    [SerializeField] private float _deadDuration = 0.9f;
    #endregion

    #region 내부 변수
    private EnemyPooling _ownerPool;
    private Rigidbody _rb;
    private Collider _myCollider;
    private Collider[] _nearEnemy = new Collider[16];

    private float _attackTimer = 0.0f;
    private int _hashRun;
    private int _hashAttack;
    private int _hashDead;
    private bool _isAttacking;

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

    public bool IsDead
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

        _myCollider = GetComponent<Collider>();

        if (_myCollider == null)
        {
            Debug.LogWarning("적 콜라이더 null (TempEnemy) / 확인 요망");

            return;
        }

        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
        {
            Debug.LogWarning("적 리지드바디 null 확인 요망");

            return;
        }

        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }

        _baseHP = _stats.maxHP;
        _baseAttack = _stats.attack;

        _hashRun = Animator.StringToHash(_paramRun);
        _hashAttack = Animator.StringToHash(_paramAttack);
        _hashDead = Animator.StringToHash(_paramDead);
    }

    void Update()
    {
        if (IsDead)
        {
            return;
        }

        Vector3 toPlayer = (_playerTransform.position - transform.position);
        toPlayer.y = 0.0f;

        TickAttack(toPlayer);
        TickMove(toPlayer);
        TickRotate(toPlayer);
    }

    // 공격
    private void TickAttack(Vector3 toPlayer)
    {
        if (_isAttacking)
        {
            return;
        }

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
        _isAttacking = true;
        _animator.SetTrigger(_hashAttack);
    }

    private Vector3 GetPushVector()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, _pushDistance, _nearEnemy, _enemyMask);
        Vector3 push = Vector3.zero;

        for (int i = 0; i < count; i++)
        {
            Vector3 dir = transform.position - _nearEnemy[i].transform.position;
            dir.y = 0.0f;

            float mag = dir.magnitude;

            if (mag < 0.0001f)
            {
                continue;
            }

            float overlap = _pushDistance - mag;

            push += (dir / mag) * overlap;
        }

        return push;
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
        IsDead = false;
        _stats.currentHP = _stats.maxHP;
        _isAttacking = false;
        _attackTimer = 0.0f;
        _myCollider.enabled = true;
    }

    public void OnDespawn()
    {
        IsActive = false;
        IsDead = true;
        _myCollider.enabled = false;
    }
    
    public void OnRecycle()
    {
        IsDead = false;
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
        if (_ownerPool == null)
        {
            // 풀 미연결 시 파괴
            IsActive = false;
            Destroy(gameObject);

            return;
        }

        _animator.SetTrigger(_hashDead);
        _ownerPool.DespawnEnemy(this);

        StartCoroutine(Co_Dead());
    }

    private IEnumerator Co_Dead()
    {
        yield return new WaitForSeconds(_deadDuration);
        _ownerPool.RecycleEnemy(this);
    }

    // 풀 종류 설정
    public void SetOwnerPool(EnemyPooling pool)
    {
        _ownerPool = pool;
    }

    // 애니메이터 이벤트용
    public void OnAttackHit()
    {
        if (_damageable == null)
        {
            Debug.LogWarning("데미지 인터페이스 null (TempEnemy) / 확인 요망");

            return;
        }

        if (_playerTransform == null)
        {
            Debug.LogWarning("플레이어 트랜스폼 null (TempEnemy) / 확인 요망");

            return;
        }

        Vector3 dir = _playerTransform.position - transform.position;
        dir.y = 0.0f;

        if (dir.sqrMagnitude > _attackRange * _attackRange)
        {
            return;
        }

        _damageable.TakeDamage(_stats.attack);
    }

    // 애니메이터 이벤트용
    public void OnAttackEnd()
    {
        _isAttacking = false;
    }

    private float GroundY(Vector3 movePos, float groundHeight)
    {
        Vector3 origin = movePos + Vector3.up * _groundCheckHeight;

        if (!Physics.Raycast(origin, Vector3.down, out RaycastHit hit, _groundCheckHeight * 2.0f, _groundMask))
        {
            return groundHeight;
        }

        return hit.point.y + _groundOffset;
    }

    // 플레이어 따라가기
    private void TickMove(Vector3 toPlayer)
    {
        if (toPlayer.magnitude <= _attackRange || _isAttacking)
        {
            toPlayer = Vector3.zero;
        }

        bool isRunning = (toPlayer.sqrMagnitude > 0.001f);
        _animator.SetBool(_hashRun, isRunning);

        Vector3 push = GetPushVector();

        Vector3 movePos = transform.position + toPlayer.normalized * _moveSpeed * Time.deltaTime + push * _pushForce * Time.deltaTime;
        float y = GroundY(movePos, transform.position.y);
        movePos.y = Mathf.Lerp(transform.position.y, y, 1.0f - Mathf.Exp(-10.0f * Time.deltaTime));

        _rb.MovePosition(movePos);
    }

    // 플레이어쪽으로 돌기
    private void TickRotate(Vector3 toPlayer)
    {
        if (_isAttacking)
        {
            return;
        }

        if (toPlayer.sqrMagnitude < 0.0001f)
        {
            return;
        }

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
