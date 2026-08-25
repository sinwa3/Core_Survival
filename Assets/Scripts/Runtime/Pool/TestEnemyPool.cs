using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyPool : MonoBehaviour
{
    #region 인스펙터
    [Header("키 설정")]
    [SerializeField] private KeyCode _spawnKey = KeyCode.Q;

    [Header("풀 연결")]
    [SerializeField] private EnemyPooling _pool;

    [Header("플레이어")]
    [SerializeField] private Transform _playerTr;

    [Header("옵션")]
    [SerializeField] private float _minRange = 10.0f;
    [SerializeField] private float _maxRange = 20.0f;
    #endregion


    private void Awake()
    {
        if (_pool == null)
        {
            Debug.LogWarning("풀 연결 안됨 / 인스펙터 확인");
            enabled = false;

            return;
        }

        if (_playerTr == null)
        {
            _playerTr = transform;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(_spawnKey))
        {
            Vector2 pos = Random.insideUnitCircle.normalized * Random.Range(_minRange, _maxRange);

            Vector3 spawnPos = _playerTr.position + new Vector3(pos.x, 0.0f, pos.y);

            _pool.GetEnemy(spawnPos, Quaternion.LookRotation(spawnPos - _playerTr.position));
        }
    }
}
