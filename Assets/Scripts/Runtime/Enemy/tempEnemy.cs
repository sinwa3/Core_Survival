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

    #region 내부 변수
    private EnemyPooling _ownerPool;
    private Rigidbody _rb;
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

        _rb = GetComponent<Rigidbody>();

        if (_rb == null)
        {
            Debug.LogWarning("적 리지드바디 null 확인 요망");

            return;
        }
    }

    void Update()
    {
        Vector3 toPlayer = (_playerTransform.position - transform.position);

        FollowPlayer(toPlayer);
        TickRotate(toPlayer);
    }

    // 데미지 계산
    public void TakeDamage(float damage)
    {
        _stats.currentHP -= damage;

        if (_stats.currentHP <= 0)
        {
            DestroyThis();
        }
    }

    public void OnSpawn()
    {
        _stats.currentHP = _stats.maxHP;
    }

    public void OnDespawn()
    {

    }

    // 죽었을 때
    public void DestroyThis()
    {
        if (_ownerPool != null)
        {
            _ownerPool.ReturnEnemy(this);

            return;
        }

        // 풀 미연결 시 파괴
        Destroy(gameObject);
    }

    // 풀 종류 설정
    public void SetOwnerPool(EnemyPooling pool)
    {
        _ownerPool = pool;
    }

    // 플레이어 따라가기
    private void FollowPlayer(Vector3 toPlayer)
    {
        if (toPlayer.magnitude <= 2.0f)
        {
            return;
        }
        
        Vector3 movePos = transform.position + toPlayer.normalized * _moveSpeed * Time.deltaTime;

        _rb.MovePosition(movePos);
    }

    // 플레이어쪽으로 돌기
    private void TickRotate(Vector3 toPlayer)
    {
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
