using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    #region 인스펙터
    [Header("텍스트")]
    [SerializeField] private TextMeshPro _text;

    [Header("연출")]
    [SerializeField] private float _lifeTime = 0.6f;
    [Tooltip("올라가는 속도")]
    [SerializeField] private float _riseSpeed = 1.5f;
    [Tooltip("스폰위치")]
    [SerializeField] private Vector3 _spawnOffset = new Vector3(0.0f, 2.6f, 0.0f);
    #endregion

    #region 내부 변수
    private DamageTextPooling _ownerPool;
    private float _spawnTime;
    #endregion

    public bool IsPooled
    {
        get; private set;
    } = true;

    private float LifeRatio
    {
        get
        {
            if (_lifeTime <= 0.0f)
            {
                return 1.0f;
            }

            return Mathf.Clamp01((Time.time - _spawnTime) / _lifeTime );
        }
    }

    private void Awake()
    {
        if (_text == null)
        {
            _text = GetComponent<TextMeshPro>();
        }

        if (_text == null)
        {
            Debug.LogWarning("TextMeshPro 없음 (DamageText) / 확인 요망");
            enabled = false;

            return;
        }
    }

    public void SetOwnerPool(DamageTextPooling pool)
    {
        _ownerPool = pool;
    }

    public void OnSpawn(float damage, Vector3 enemyPos)
    {
        transform.position = enemyPos + _spawnOffset;

        _text.text = $"{damage:0}";
        _text.alpha = 1.0f;

        _spawnTime = Time.time;
        IsPooled = false;
    }

    public void OnDespawn()
    {
        IsPooled = true;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * _riseSpeed * Time.deltaTime, Space.World);
        _text.alpha = Mathf.Lerp(1.0f, 0.0f, LifeRatio);

        if (LifeRatio < 1.0f)
        {
            return;
        }

        if (_ownerPool == null)
        {
            Debug.LogWarning("풀 null (DamageText) / 반환 불가");

            return;
        }

        _ownerPool.ReturnDamageText(this);
    }
}