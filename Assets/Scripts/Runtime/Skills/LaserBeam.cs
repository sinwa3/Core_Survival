using PolygonArsenal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : SkillEffectBase
{
    public override SkillID SkillID => SkillID.LaserBeam;

    #region 인스펙터
    [Header("스킬 옵션")]
    [SerializeField] private float _damage = 5.0f;
    [SerializeField] private float _lifeTime = 3.0f;

    [Header("충돌 태그")]
    [SerializeField] private string _enemyTag = "Enemy";

    [Header("플레이어")]
    [SerializeField] private Transform _playerTransform;

    [Header("데미지 간격")]
    [SerializeField] private float _damageInterval = 0.3f;

    [Header("스페어캐스트 넓이")]
    [SerializeField] private float _sphereCastRadius = 0.3f;
    #endregion

    #region 내부 변수
    private PolygonBeamStatic _beamStatic;
    private float _timer = 0.0f;
    private bool _hasHit;
    #endregion

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("플레이어 태그 찾을 수 없음 / 확인 요망");
            enabled = false;

            return;
        }

        _playerTransform = player.transform;

        _beamStatic = GetComponent<PolygonBeamStatic>();
    }

    void Start()
    {

    }

    void Update()
    {
        transform.position = _playerTransform.position;

        Tick();
    }

    public override void OnSpawn()
    {
        StartCoroutine(Co_Life(_lifeTime));
    }

    public void Tick()
    {
        if (_timer < _damageInterval)
        {
            _timer += Time.deltaTime;

            return;
        }

        _timer = 0.0f;

        _hasHit = TrySpherecast(out RaycastHit hit);

       

        if (_hasHit && hit.collider.CompareTag(_enemyTag))
        {
            TempEnemy enemy = hit.collider.GetComponent<TempEnemy>();

            if (enemy == null)
            {
                Debug.LogWarning("적 컴포넌트 null / 확인요망");

                return;
            }

            enemy.TakeDamage(_damage);
            Debug.Log($"{SkillID} 스킬로 {hit.collider.name}에게 {_damage}의 데미지");
        }

    }

    private bool TrySpherecast(out RaycastHit hit)
    {
        Vector3 origin = transform.position + transform.forward * 0.5f;
        Debug.DrawLine(origin, origin + transform.forward * _beamStatic.beamLength, Color.red, 0.6f);

        return Physics.SphereCast(origin, _sphereCastRadius, transform.forward, out hit, _beamStatic.beamLength);
    }
}
