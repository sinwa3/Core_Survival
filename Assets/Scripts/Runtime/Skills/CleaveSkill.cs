using UnityEngine;

public class CleaveSkill : SkillsBase
{
    public override SkillID SkillID => SkillID.Cleave;
    public CleaveSkill(float skillCool, SkillEffectBase prefab, SkillEffectPooling pool) : base (skillCool, prefab, pool)
    {

    }

    protected override void UseSkill(Transform player, Transform target)
    {
        Vector3 dir = (target.transform.position - player.transform.position).normalized;
        dir.y = 0.0f;

        Quaternion skillRot = Quaternion.LookRotation(dir, Vector3.up);
        skillEffectPool.GetEffect(SkillID, skillPrefab, player.position, skillRot);

        Debug.Log("가시 스킬 발동 성공");
    }
}
