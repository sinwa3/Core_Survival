using UnityEngine;
using UnityEngine.UIElements;

public class CleaveEffect : SkillEffectBase
{
    public override SkillID SkillID => SkillID.Cleave;

    #region 인스펙터
    [Header("스킬 옵션")]
    [SerializeField] private float _lifeTime = 0.9f;

    [Header("충돌 태그")]
    [SerializeField] private string _enemyTag = "Enemy";

    [Header("판정")]
    [SerializeField] private float _hitTime = 0.3f;
    [Tooltip("판정 범위")]
    [SerializeField] private float _hitRange = 5.2f;
    #endregion

    #region 내부 변수
    private BoxCollider _boxCollider;
    private float _hitTimer;
    private bool _isAdvancing;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        _boxCollider = GetComponent<BoxCollider>();

        if (_boxCollider == null)
        {
            Debug.LogWarning("BoxCollider 컴포넌트 null (CleaveEffect) / 확인 요망");

            return;
        }
    }

    public override void OnSpawn()
    {
        base.OnSpawn();

        _hitTimer = 0.0f;
        _isAdvancing = false;

        if (_boxCollider != null)
        {
            SetHitPosition(_boxCollider.size.z * 0.5f);
            _isAdvancing = true;
        }

        StartCoroutine(Co_Life(_lifeTime));
    }

    private void FixedUpdate()
    {
        if (!_isAdvancing)
        {
            return;
        }

        if (_hitTimer >= _hitTime)
        {
            _isAdvancing = false;

            return;
        }

        _hitTimer += Time.fixedDeltaTime;

        float advanceRatio = Mathf.Clamp01(_hitTimer / _hitTime);
        float half = _boxCollider.size.z * 0.5f;

        SetHitPosition(Mathf.Lerp(half, _hitRange - half, advanceRatio));
    }

    private void SetHitPosition(float z)
    {
        Vector3 centerPos = _boxCollider.center;
        centerPos.z = z;
        _boxCollider.center = centerPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isAdvancing)
        {
            return;
        }

        if (!string.IsNullOrEmpty(other.tag) && other.CompareTag(_enemyTag))
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
