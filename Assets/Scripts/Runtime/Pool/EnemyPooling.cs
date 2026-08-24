using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour
{
    #region 인스펙터
    [Header("풀링 옵션")]
    [SerializeField] private TempEnemy _enemyPrefab;
    [SerializeField] private int _prewarmCount = 20;
    #endregion

    #region 내부 변수
    private Queue<TempEnemy> _enemyPool = new Queue<TempEnemy>();
    #endregion

    public event Action<TempEnemy> OnEnemyReturn;

    private void Awake()
    {
        EnemyPrewarm();
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
            TempEnemy enemy = Instantiate(_enemyPrefab, transform);
            enemy.gameObject.SetActive(false);

            _enemyPool.Enqueue(enemy);
        }

        Debug.Log("적 프리웜 성공 / 큐 생성 완료");
    }

    // 적 꺼내기
    public TempEnemy GetEnemy(Vector3 pos, Quaternion rot)
    {
        TempEnemy enemy = null;

        enemy = (_enemyPool.Count > 0) ? _enemyPool.Dequeue() : Instantiate(_enemyPrefab, transform);
        enemy.transform.SetPositionAndRotation(pos, rot);
        enemy.ResetStats();
        enemy.SetOwnerPool(this);
        enemy.gameObject.SetActive(true);

        return enemy;
    }

    // 적 반납
    public void ReturnEnemy(TempEnemy enemy)
    {
        if (enemy == null)
        {
            Debug.LogWarning("적 반환 불가 / null인 적");

            return;
        }

        if (_enemyPool.Contains(enemy))
        {
            Debug.LogWarning("적 반환 불가 / 중복 반환");

            return;
        }

        enemy.gameObject.SetActive(false);
        _enemyPool.Enqueue(enemy);
        OnEnemyReturn?.Invoke(enemy);

        Debug.Log("적 반환 성공");
    }


}
