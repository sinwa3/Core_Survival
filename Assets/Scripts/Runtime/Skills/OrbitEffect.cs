using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitEffect : SkillEffectBase
{
    public override SkillID SkillID => SkillID.Orbit;

    #region 인스펙터
    [Header("스킬 옵션")]
    [SerializeField] private float _lifeTime = 6.0f;
    [SerializeField] private float _orbitRadius = 3.0f;
    [SerializeField] private float _orbitSpeed = 120.0f;

    [Header("충돌 태그")]
    [SerializeField] private string _enemyTag = "Enemy";
    #endregion

    #region 내부 변수
    private Transform _playerTransform;
    private float _currentAngle;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("플레이어 null (OrbitEffect) / 확인 요망");
            enabled = false;

            return;
        }

        _playerTransform = player.transform;
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        StartCoroutine(Co_Life(_lifeTime));
    }

    void Update()
    {
        _currentAngle += _orbitSpeed * Time.deltaTime;

        Vector3 dir = Quaternion.Euler(0, _currentAngle, 0) * Vector3.forward;

        transform.position = _playerTransform.position + dir * _orbitRadius;
    }

    public void SetStartAngle(float angle)
    {
        _currentAngle = angle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            Debug.LogWarning("콜라이더 null (OrbitEffect)");

            return;
        }

        if(!string.IsNullOrEmpty(other.tag) && other.CompareTag(_enemyTag))
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
        }
    }
}
