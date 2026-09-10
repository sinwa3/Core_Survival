using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPooling : MonoBehaviour
{
    #region 인스펙터
    [Header("풀링 옵션")]
    [SerializeField] private EnemyRangeAttackEffect _attackPrefab;
    [SerializeField] private int _prewarmCount = 50;

    [Header("측정용")]
    [SerializeField] private bool _usePool = true;
    #endregion

    #region 내부 변수
    private Queue<EnemyRangeAttackEffect> _attackPool = new Queue<EnemyRangeAttackEffect>();
    #endregion

    private void Awake()
    {
        if (_attackPrefab == null)
        {
            Debug.LogWarning("프리팹 null (EnemyAttackPooling)");
            enabled = false;

            return;
        }

        if (_usePool)
        {
            EnemyPrewarm();
        }
    }

    // 프리웜
    private void EnemyPrewarm()
    {
        if (_attackPrefab == null)
        {
            Debug.LogWarning("프리웜 불가 (EnemyAttackPooling)/ 프리팹 없음");

            return;
        }

        for (int i = 0; i < _prewarmCount; i++)
        {
            _attackPool.Enqueue(CreateAttack());
        }

        Debug.Log("공격 프리웜 성공 / 큐 생성 완료");
    }

    private EnemyRangeAttackEffect CreateAttack()
    {
        EnemyRangeAttackEffect attack = Instantiate(_attackPrefab, transform);

        attack.SetOwnerPool(this);
        attack.gameObject.SetActive(false);

        return attack;
    }

    public EnemyRangeAttackEffect GetAttack(Vector3 pos, Quaternion rot)
    {
        if (_attackPrefab == null)
        {
            Debug.LogWarning("공격 생성 불가 (EnemyAttackPooling) / 프리팹 없음");

            return null;
        }

        EnemyRangeAttackEffect attack;

        attack = (_attackPool.Count > 0 && _usePool) ? _attackPool.Dequeue() : CreateAttack();

        attack.transform.SetPositionAndRotation(pos, rot);
        attack.gameObject.SetActive(true);
        attack.OnSpawn();

        return attack;
    }

    // 공격 반납
    public void ReturnAttack(EnemyRangeAttackEffect attack)
    {
        if (attack == null)
        {
            Debug.LogWarning("공격 반환 불가 (EnemyAttackPooling) / null인 공격");

            return;
        }

        if (!attack.IsActive)
        {
            Debug.LogWarning("공격 반환 불가 (EnemyAttackPooling) / 중복 반환");

            return;
        }

        if (!_usePool)
        {
            Destroy(attack.gameObject);

            return;
        }

        attack.OnDespawn();
        attack.gameObject.SetActive(false);
        _attackPool.Enqueue(attack);
    }


}

