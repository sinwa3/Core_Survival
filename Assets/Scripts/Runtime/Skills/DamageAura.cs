using UnityEngine;

public class DamageAura : SkillEffectBase
{
    public override SkillID SkillID => SkillID.DamageAura;

    #region 인스펙터
    [Header("스킬 옵션")]
    [SerializeField] private float _damage = 5.0f;
    [SerializeField] private float _lifeTime = 5.0f;

    [Header("충돌 태그")]
    [SerializeField] private string _enemyTag = "Enemy";

    [Header("데미지 간격")]
    [SerializeField] private float _damageInterval = 0.3f;

    [Header("플레이어")]
    [SerializeField] private Transform _playerTransform;
    #endregion

    #region 내부 변수
    private float _timer = 0.0f;
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

    public override void OnSpawn()
    {
        StartCoroutine(Co_Life(_lifeTime));

        transform.SetParent(_playerTransform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(other.tag) && other.CompareTag(_enemyTag))
        {
            TempEnemy enemy = other.GetComponent<TempEnemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
                Debug.Log($"{SkillID} 스킬로 {other.name}에게 {_damage}의 데미지");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_timer < _damageInterval)
        {
            _timer += Time.deltaTime;

            return;
        }

        _timer = 0.0f;

        if (!string.IsNullOrEmpty(other.tag) && other.CompareTag(_enemyTag))
        {
            TempEnemy enemy = other.GetComponent<TempEnemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
                Debug.Log($"{SkillID} 스킬로 {other.name}에게 {_damage}의 데미지");
            }
        }
    }
}
