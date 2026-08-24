using UnityEngine;

public abstract class SkillsBase
{
    public abstract SkillID SkillID
    { 
        get;
    }

    public float skillCooldown;
    protected float skillTimer = 0.0f;
    public SkillEffectBase skillPrefab;
    protected SkillEffectPooling skillEffectPool;

    public SkillsBase(float skillCool, SkillEffectBase prefab, SkillEffectPooling effectPool)
    {
        skillCooldown = skillCool;
        skillPrefab = prefab;
        skillEffectPool = effectPool;
    }

    public void TickCooltime(Transform player, Transform target)
    {
        if (skillTimer < skillCooldown)
        {
            skillTimer += Time.deltaTime;

            return;
        }

        if (target == null)
        {
            return;
        }

        UseSkill(player, target);
        skillTimer = 0.0f;
    }

    protected abstract void UseSkill(Transform player, Transform target);
}
