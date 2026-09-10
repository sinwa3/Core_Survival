using System.Collections;
using UnityEngine;

public class EnemyRangeAttackEffect : MonoBehaviour
{
    #region 인스펙터
    [Header("옵션")]
    [SerializeField] private float _lifeTime = 4.0f;
    [SerializeField] private float _moveSpeed = 15.0f;

    [Header("충돌 태그")]
    [SerializeField] private string _playerTag = "Player";
    #endregion

    #region 내부 변수
    private EnemyAttackPooling _ownerPool;
    private float _damage;
    #endregion

    public bool IsActive
    {
        get; private set;
    }


    public void OnSpawn()
    {
        IsActive = true;
        StartCoroutine(Co_Life(_lifeTime));
    }

    public void OnDespawn()
    {
        IsActive = false;
    }

    public void SetOwnerPool(EnemyAttackPooling pool)
    {
        _ownerPool = pool;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _moveSpeed);
    }

    private IEnumerator Co_Life(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (_ownerPool == null)
        {
            Debug.LogWarning("풀 null (EnemyRangeAttackEffect) / 반환 불가");

            return;
        }

        _ownerPool.ReturnAttack(this);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
        {
            Debug.LogWarning("콜라이더 null");

            return;
        }

        if (!string.IsNullOrEmpty(other.tag) && other.CompareTag(_playerTag))
        {
            IDamageable player = other.GetComponent<IDamageable>();

            if (player != null)
            {
                player.TakeDamage(_damage);
            }

            ReturnToPool();
        }
    }
}
