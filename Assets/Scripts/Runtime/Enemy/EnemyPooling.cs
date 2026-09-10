using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    #region 인스펙터
    [Header("풀링 옵션")]
    [SerializeField] private TempEnemy _enemyPrefab;
    [SerializeField] private int _prewarmCount = 100;

    [Header("측정용")]
    [SerializeField] private bool _usePool = true;

    [Header("데미지 텍스트")]
    [SerializeField] private DamageTextPooling _damageTextPool;

    [Header("적 공격 풀")]
    [Tooltip("원거리 적에게 붙이기")]
    [SerializeField] private EnemyAttackPooling _attackPool;
    #endregion

    #region 내부 변수
    private Queue<TempEnemy> _enemyPool = new Queue<TempEnemy>();

    // 활성 적 추적
    private List<TempEnemy> _activeEnemy = new List<TempEnemy>();
    #endregion

    public IReadOnlyList<TempEnemy> ActiveEnemy => _activeEnemy;
    public event Action<TempEnemy> OnEnemyReturn;

    private void Awake()
    {
        if (_usePool)
        {
            EnemyPrewarm();
        }

        if (_damageTextPool == null)
        {
            Debug.LogWarning("데미지 풀 null (EnemyPooling)/ 데미지 표시 불가 / 인스펙터 확인");
        }

        if (_enemyPrefab != null && _enemyPrefab.AttackType == EEnemyAttackType.Ranged && _attackPool == null)
        {
            Debug.LogWarning("원거리 적 공격 풀 null (EnemyPooling) / 공격 불가");
        }
    }

    // 프리웜
    private void EnemyPrewarm()
    {
        if (_enemyPrefab == null)
        {
            Debug.LogWarning("프리웜 불가 / 적 프리팹 없음");

            return;
        }

        for (int i = 0; i < _prewarmCount; i++)
        {
            _enemyPool.Enqueue(CreateEnemy());
        }

        Debug.Log("적 프리웜 성공 / 큐 생성 완료");
    }

    private TempEnemy CreateEnemy()
    {
        TempEnemy enemy = Instantiate(_enemyPrefab, transform);

        enemy.SetOwnerPool(this);
        enemy.SetDamageTextPool(_damageTextPool);
        enemy.SetAttackPool(_attackPool);
        enemy.gameObject.SetActive(false);

        return enemy;
    }

    // 적 꺼내기
    public TempEnemy GetEnemy(Vector3 pos, Quaternion rot)
    {
        if (_enemyPrefab == null)
        {
            Debug.LogWarning("적 생성 불가 / 프리팹 없음");

            return null;
        }

        TempEnemy enemy;

        enemy = (_enemyPool.Count > 0 && _usePool) ? _enemyPool.Dequeue() : CreateEnemy();

        enemy.transform.SetPositionAndRotation(pos, rot);
        enemy.gameObject.SetActive(true);

        _activeEnemy.Add(enemy);

        enemy.OnSpawn();

        return enemy;
    }

    // 적 반납
    public void DespawnEnemy(TempEnemy enemy)
    {
        if (enemy == null)
        {
            Debug.LogWarning("적 반환 불가 / null인 적");

            return;
        }

        if (!enemy.IsActive)
        {
            Debug.LogWarning("적 반환 불가 / 중복 반환");

            return;
        }

        enemy.OnDespawn();
        OnEnemyReturn?.Invoke(enemy);
        _activeEnemy.Remove(enemy);
    }

    public void RecycleEnemy(TempEnemy enemy)
    {
        if (enemy == null)
        {
            Debug.LogWarning("적 반환 불가 / null인 적");

            return;
        }

        if (!enemy.IsDead)
        {
            return;
        }

        if (!_usePool)
        {
            Destroy(enemy.gameObject);

            return;
        }

        enemy.OnRecycle();
        enemy.gameObject.SetActive(false);
        _enemyPool.Enqueue(enemy);
    }


}
