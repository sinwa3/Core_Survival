using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectPooling : MonoBehaviour
{

    #region 내부 변수
    private Dictionary<SkillID, Queue<SkillEffectBase>> _skillPools = new Dictionary<SkillID, Queue<SkillEffectBase>>();
    #endregion

    public void ReturnSkillEffectPool(SkillEffectBase skill)
    {
        if (skill == null)
        {
            Debug.LogWarning("스킬 반환 불가 / null인 스킬");

            return;
        }

        if (skill.IsPooled)
        {
            Debug.LogWarning("스킬 반환 불가 / 중복 반환");

            return;
        }

        if (!_skillPools.TryGetValue(skill.SkillID, out Queue<SkillEffectBase> pool))
        {
            Debug.LogWarning($"스킬 반환 불가 / 스킬 ID {skill.SkillID}에 대한 풀을 찾을 수 없음");

            return;
        }

        skill.OnDespawn();
        skill.gameObject.SetActive(false);
        skill.transform.SetParent(transform);

        pool.Enqueue(skill);
    }

    public SkillEffectBase GetEffect(SkillID id, SkillEffectBase prefab, Vector3 pos, Quaternion rot)
    {
        Queue<SkillEffectBase> pool;
        SkillEffectBase skillEffect = null;

        if (!_skillPools.TryGetValue(id, out pool))
        {
            pool = new Queue<SkillEffectBase>();
            _skillPools.Add(id, pool);
        }

        skillEffect = (pool.Count > 0) ? pool.Dequeue() : Instantiate(prefab, transform);

        skillEffect.transform.SetPositionAndRotation(pos, rot);
        skillEffect.SetOwnerPool(this);
        skillEffect.gameObject.SetActive(true);
        skillEffect.OnSpawn();

        return skillEffect;
    }
}
