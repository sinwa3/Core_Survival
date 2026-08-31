using UnityEngine;

public abstract class SkillsBase
{
    public abstract SkillID SkillID
    {
        get;
    }

    // 스킬 자체 정보
    public float skillCooldown;
    protected float skillTimer = 0.0f;

    // 스킬 프리팹
    public SkillEffectBase skillPrefab;
    // 스킬 풀
    protected SkillEffectPooling skillEffectPool;

    // 타겟 유무
    public virtual bool NeedTarget => true;

    public float CooldownRemainRatio
    {
        get
        {
            if (skillCooldown == 0.0f)
            {
                return 0.0f;
            }

            float ratio = Mathf.Clamp01(skillTimer / skillCooldown);

            return 1.0f - ratio;
        }
    }

    public SkillsBase(float skillCool, SkillEffectBase prefab, SkillEffectPooling effectPool)
    {
        skillCooldown = skillCool;
        skillPrefab = prefab;
        skillEffectPool = effectPool;
    }

    // 업데이트에서 스킬 쿨타임을 돌리고 사용
    public void TickCooltime(Transform player, Transform target)
    {
        if (skillTimer < skillCooldown)
        {
            skillTimer += Time.deltaTime;

            return;
        }

        if (NeedTarget && target == null)
        {
            return;
        }

        UseSkill(player, target);
        skillTimer = 0.0f;
    }

    // 실제 사용 로직
    protected abstract void UseSkill(Transform player, Transform target);
}
