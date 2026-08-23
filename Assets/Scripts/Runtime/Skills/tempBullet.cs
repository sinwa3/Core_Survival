using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempBullet : MonoBehaviour
{
    #region 인스펙터
    [Header("프리팹")]
    [SerializeField] private GameObject _prefab;

    [Header("주변 적")]
    [SerializeField] private List<Transform> _enemyNearList = new List<Transform>();

    [Header("감지 태그")]
    [SerializeField] private string _enemyTag = "Enemy";
    #endregion

    #region 내부 변수
    private Transform _parent;
    private Transform _nearestTr;
    private float _attackTimer = 0.0f;
    private float _attackCool = 1.0f;

    #endregion

    void Start()
    {
        SetParent();
    }

    void Update()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer < _attackCool)
        {
            return;
        }

        _nearestTr = GetNearest();

        if (_nearestTr != null)
        {
            ShotBullet();
            _attackTimer = 0.0f;
        }
    }

    private Transform GetNearest()
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

    private void ShotBullet()
    {
        GameObject go = Instantiate(_prefab, _parent);
        go.transform.position = transform.position;

        Bullet bullet = go.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.GetTarget(_nearestTr);
        }

        Debug.Log("쏘기 성공");
    }

    private void SetParent()
    {
        if (_parent != null)
        {
            return;
        }

        GameObject parent = new GameObject("Bullet_Parent");
        _parent = parent.transform;

        Debug.Log("총알 부모 생성 성공");
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
