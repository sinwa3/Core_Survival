using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WaveData
{
    public int minEnemy;
    public float spawnInterval;
    public float hpMulti = 1.0f;
    public float attackMulti = 1.0f;
}

public class EnemySpawner : MonoBehaviour
{
    #region 인스펙터
    [Header("풀 연결")]
    [SerializeField] private EnemyPooling _pool;

    [Header("플레이어")]
    [SerializeField] private Transform _playerTr;

    [Header("옵션")]
    [SerializeField] private float _minRange = 10.0f;
    [SerializeField] private float _maxRange = 20.0f;

    [Header("웨이브 정보")]
    [SerializeField] private List<WaveData> _waveData;
    [SerializeField] private float _waveDuration = 20.0f;
    [SerializeField] private int _maxEnemy = 100;

    [Header("게임 매니저")]
    [SerializeField] private GameManager _gameManager;
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
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
            {
                Debug.LogWarning("플레이어 없음 / 확인 요망");
                enabled=false;

                return;
            }

            _playerTr = player.transform;
        }

        if (_gameManager == null)
        {
            Debug.LogWarning("게임 매니저 연결 안됨 / 인스펙터 확인");
            enabled = false;

            return;
        }
    }

    void Start()
    {
        StartCoroutine(Co_WaveSpawn());
    }

    private IEnumerator Co_WaveSpawn()
    {
        while (true)
        {
            WaveData currentWave = GetWaveData();

            if (currentWave == null)
            {
                Debug.LogWarning("웨이브 데이터 없음 / 확인 요망");

                yield break;
            }

            int targetCount = Mathf.Min(currentWave.minEnemy, _maxEnemy);

            while (_pool.ActiveEnemy.Count < targetCount)
            {
                SpawnEnemy(currentWave);

                yield return null;
            }

            yield return new WaitForSeconds(currentWave.spawnInterval);
        }
    }

    private WaveData GetWaveData()
    {
        if (_waveData == null || _waveData.Count == 0)
        {
            return null;
        }

        int index = (int)(_gameManager.PlayTime / _waveDuration);

        // waveDuration이 0일 경우 방지
        index = Mathf.Clamp(index, 0, _waveData.Count - 1);

        return _waveData[index];
    }

    // 적 스폰
    private void SpawnEnemy(WaveData currentWave)
    {
        Vector2 pos = Random.insideUnitCircle.normalized * Random.Range(_minRange, _maxRange);
        Vector3 spawnPos = _playerTr.position + new Vector3(pos.x, 0.0f, pos.y);

        TempEnemy enemy = _pool.GetEnemy(spawnPos, Quaternion.LookRotation(spawnPos - _playerTr.position));
        enemy.ApplyStatMultiple(currentWave.hpMulti, currentWave.attackMulti);
    }
}
