using UnityEngine;

public class Cleave : MonoBehaviour
{
    #region 인스펙터
    [Header("스킬 옵션")]
    [SerializeField] private float _damage = 20.0f;
    [SerializeField] private float _lifeTime = 0.9f;

    [Header("충돌 태그")]
    [SerializeField] private string _enemyTag = "Enemy";
    #endregion
    void Start()
    {
        Destroy(this.gameObject, _lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(other.tag) && other.CompareTag(_enemyTag))
        {
            tempEnemy enemy = other.GetComponent<tempEnemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
                Debug.Log($"{other.name}에게 {_damage}의 데미지");
            }
        }
    }
}
