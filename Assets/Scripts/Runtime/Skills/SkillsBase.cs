using UnityEngine;

public abstract class SkillsBase
{
    public float skillCooldown;
    protected float skillTimer = 0.0f;
    public GameObject skillPrefab;

    public SkillsBase(float skillCool, GameObject prefab)
    {
        skillCooldown = skillCool;
        skillPrefab = prefab;
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
