using UnityEngine;

public class DamageAuraEffect : SkillEffectBase
{
    public override SkillID SkillID => SkillID.DamageAura;

    #region 인스펙터
    [Header("스킬 옵션")]
    [SerializeField] private float _lifeTime = 5.0f;

    [Header("충돌 태그")]
    [SerializeField] private string _enemyTag = "Enemy";

    [Header("데미지 간격")]
    [SerializeField] private float _damageInterval = 0.3f;

    [Header("적 레이어")]
    [SerializeField] private LayerMask _enemyMask = 1 << 6;

    [Header("플레이어")]
    [SerializeField] private Transform _playerTransform;
    #endregion

    #region 내부 변수
    private SphereCollider _sphereCollider;
    private Collider[] _hitBuffer = new Collider[128];
    private float _timer = 0.0f;
    #endregion

    private float HitRadius => _sphereCollider.radius * transform.localScale.x;

    protected override void Awake()
    {
        base.Awake();

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

        _sphereCollider = GetComponent<SphereCollider>();

        if (_sphereCollider == null)
        {
            Debug.LogWarning("콜라이더 null (DamageAuraEffect)");
            
            return;
        }
    }

    
    public override void OnSpawn()
    {
        base.OnSpawn();

        _timer = _damageInterval;
        StartCoroutine(Co_Life(_lifeTime));

        // 플레이어 따라가기
        transform.SetParent(_playerTransform);
    }

    private void Update()
    {
        if (_timer < _damageInterval)
        {
            _timer += Time.deltaTime;

            return;
        }

        _timer = 0.0f;

        TickDamage();
    }

    private void TickDamage()
    {
        if (_sphereCollider == null)
        {
            return;
        }

        int count = Physics.OverlapSphereNonAlloc(transform.position, HitRadius, _hitBuffer, _enemyMask);

        if (count == _hitBuffer.Length)
        {
            Debug.LogWarning("적 감지 최대 수에 도달 (DamageAuraEffect) / 이 이상은 피해 안받음");
        }

        for (int i = 0; i < count; i++)
        {
            if (_hitBuffer[i].CompareTag(_enemyTag))
            {
                IDamageable enemy = _hitBuffer[i].GetComponent<IDamageable>();

                if (enemy != null)
                {
                    enemy.TakeDamage(SkillDamage);

                    if (printLog)
                    {
                        Debug.Log($"{SkillID} 스킬로 {_hitBuffer[i].name}에게 {SkillDamage}의 데미지");
                    }
                }
            }
        }
    }
}
