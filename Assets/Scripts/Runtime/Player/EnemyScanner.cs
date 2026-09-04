using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScanner : MonoBehaviour
{
    #region 인스펙터
    [Header("주변 적")]
    [SerializeField] private List<Transform> _enemyNearList = new List<Transform>();

    [Header("감지 태그")]
    [SerializeField] private string _enemyTag = "Enemy";

    [Header("풀 참조")]
    [SerializeField] private EnemyPooling _enemyPool;
    #endregion

    #region 내부 변수
    private SphereCollider _scanCollider;
    #endregion

    private void Awake()
    {
        _scanCollider = GetComponent<SphereCollider>();

        if (_scanCollider == null)
        {
            Debug.LogWarning("콜라이더 null / 확인 요망");

            return;
        }
    }

    private void OnEnable()
    {
        if (_enemyPool != null)
        {
            _enemyPool.OnEnemyReturn += RemoveList;
        }
    }

    private void OnDisable()
    {
        if (_enemyPool != null)
        {
            _enemyPool.OnEnemyReturn -= RemoveList;
        }
    }

    // 가까운 적 반환
    public Transform GetNearest()
    {
        float range = _scanCollider.radius + 1.0f;

        _enemyNearList.RemoveAll(enemy => enemy == null || Vector3.Distance(enemy.position, transform.position) > range);

        if (_enemyNearList.Count == 0)
        {
            return null;
        }

        Transform nearest = null;
        float nearDist = float.MaxValue;
        float distance;

        for (int i = 0; i < _enemyNearList.Count; i++)
        {
            distance = Vector3.Distance(_enemyNearList[i].position, transform.position);

            if (nearDist > distance)
            {
                nearDist = distance;
                nearest = _enemyNearList[i];
            }
        }

        return nearest;
    }

    // 리스트에서 지우기
    public void RemoveList(TempEnemy enemy)
    {
        if (enemy == null)
        {
            Debug.LogWarning("리스트 지우기 불가 / 적 null"); 

            return;
        }

        _enemyNearList.Remove(enemy.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(other.tag) && other.CompareTag(_enemyTag))
        {
            if (!_enemyNearList.Contains(other.transform))
            {
                _enemyNearList.Add(other.transform);
#if UNITY_EDITOR
                Debug.Log($"주변 적에 {other.name} 추가 / 현재 주변 적 {_enemyNearList.Count}");
#endif
            }
        }
    }
}
