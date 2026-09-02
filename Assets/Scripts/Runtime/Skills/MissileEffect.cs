using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileEffect : SkillEffectBase
{
    public override SkillID SkillID => SkillID.Missile;

    #region 인스펙터
    [Header("스킬 옵션")]
    [SerializeField] private float _lifeTime = 3.0f;
    [SerializeField] private float _moveSpeed = 15.0f;

    [Header("충돌 태그")]
    [SerializeField] private string _enemyTag = "Enemy";
    #endregion

    public override void OnSpawn()
    {
        base.OnSpawn();
        StartCoroutine(Co_Life(_lifeTime));
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            Debug.LogWarning("콜라이더 null");

            return;
        }

        if(string.IsNullOrEmpty(other.tag) && other.CompareTag(_enemyTag))
        {
            IDamageable enemy = other.GetComponent<IDamageable>();

            if (enemy != null)
            {
                enemy.TakeDamage(SkillDamage);

                if (printLog)
                {
                    Debug.Log($"{SkillID} 스킬로 {other.name}에게 {SkillDamage}의 데미지");
                }
            }

            ReturnToPool();
        }
}
}
