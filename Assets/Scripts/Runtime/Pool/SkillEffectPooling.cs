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

        if (_skillPools[skill.SkillID].Contains(skill))
        {
            Debug.LogWarning("스킬 반환 불가 / 중복 반환");

            return;
        }

        skill.OnDespawn();
        skill.gameObject.SetActive(false);
        skill.transform.SetParent(transform);
        _skillPools[skill.SkillID].Enqueue(skill);
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
