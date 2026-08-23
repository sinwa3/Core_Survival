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
    #endregion

    public Transform GetNearest()
    {
        _enemyNearList.RemoveAll(enemy => enemy == null);

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

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(other.tag) && other.CompareTag(_enemyTag))
        {
            if (!_enemyNearList.Contains(other.transform))
            {
                _enemyNearList.Add(other.transform);
                Debug.Log($"주변 적에 {other.name} 추가");

                return;
            }

            Debug.LogWarning("이미 추가됨");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!string.IsNullOrEmpty(other.tag) && other.CompareTag(_enemyTag))
        {
            if (_enemyNearList.Contains(other.transform))
            {
                _enemyNearList.Remove(other.transform);
                Debug.Log($"주변 적에 {other.name} 삭제");

                return;
            }

            Debug.LogWarning("리스트에 적 없음");
        }
    }
}
