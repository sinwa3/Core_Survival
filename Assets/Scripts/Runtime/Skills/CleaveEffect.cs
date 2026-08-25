using UnityEngine;

public class CleaveEffect : SkillEffectBase
{
    public override SkillID SkillID => SkillID.Cleave;

    #region 인스펙터
    [Header("스킬 옵션")]
    [SerializeField] private float _damage = 20.0f;
    [SerializeField] private float _lifeTime = 0.9f;

    [Header("충돌 태그")]
    [SerializeField] private string _enemyTag = "Enemy";
    #endregion

    public override void OnSpawn()
    {
        StartCoroutine(Co_Life(_lifeTime));
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
}
