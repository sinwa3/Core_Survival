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

public class tempEnemy : MonoBehaviour
{
    #region 인스펙터
    [Header("플레이어")]
    [SerializeField] private Transform _playerTransform;

    [Header("이동 / 회전")]
    [SerializeField] private float _moveSpeed = 10.0f;
    [SerializeField] private float _rotateSpeed = 5.0f;

    [Header("충돌 태그")]
    [SerializeField] private string _tag = "Skill";

    [Header("스텟")]
    [SerializeField] private EnemyStats _stats;
    #endregion

    private void Awake()
    {
        if (_playerTransform == null)
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    private void OnEnable()
    {
        _stats.currentHP = _stats.maxHP;
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
