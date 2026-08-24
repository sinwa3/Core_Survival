using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStats
{
    public float maxHP;
    public float currentHP;
    public float attack;
}

public class TempEnemy : MonoBehaviour
{
    #region 인스펙터
    [Header("플레이어")]
    [SerializeField] private Transform _playerTransform;

    [Header("이동 / 회전")]
    [SerializeField] private float _moveSpeed = 10.0f;
    [SerializeField] private float _rotateSpeed = 5.0f;

    [Header("스텟")]
    [SerializeField] private EnemyStats _stats;
    #endregion

    private void Awake()
    {
        if (_playerTransform == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");

            if (p != null)
            {
                _playerTransform = p.transform;
            }
            else
            {
                Debug.LogWarning("Player 태그 오브젝트 찾을 수 없음");
            }
        }
    }

    void Update()
    {
        Vector3 toPlayer = (_playerTransform.position - transform.position);

        FollowPlayer(toPlayer);
        TickRotate(toPlayer);
    }

    public void TakeDamage(float damage)
    {
        _stats.currentHP -= damage;

        if (_stats.currentHP <= 0)
        {
            DestroyThis();
        }
    }

    public void ResetStats()
    {
        _stats.currentHP = _stats.maxHP;
    }

    private void FollowPlayer(Vector3 toPlayer)
    {
        if (toPlayer.magnitude <= 2.0f)
        {
            return;
        }

        transform.position += toPlayer.normalized * _moveSpeed * Time.deltaTime;
    }

    private void TickRotate(Vector3 toPlayer)
    {
        if (toPlayer.sqrMagnitude < 0.0001f)
        {
            return;
        }

        Quaternion rot = Quaternion.LookRotation(toPlayer);

        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1.0f - Mathf.Exp(-_rotateSpeed * Time.deltaTime));
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2.0f);
    }
}
